//===============================================================================
// Microsoft patterns & practices
// Unity Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Practices.ObjectBuilder2.Properties;

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// A <see cref="BuilderStrategy"/> that generates IL to call
    /// chosen methods (as specified by the current <see cref="IMethodSelectorPolicy"/>)
    /// as part of object build up.
    /// </summary>
    public class DynamicMethodCallStrategy : BuilderStrategy
    {
        /// <summary>
        /// Called during the chain of responsibility for a build operation. The
        /// PreBuildUp method is called when the chain is being executed in the
        /// forward direction.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        // FxCop suppression: Validation is done by Guard class
        public override void PreBuildUp(IBuilderContext context)
        {
            DynamicBuildPlanGenerationContext ilContext = (DynamicBuildPlanGenerationContext)(context.Existing);
            IMethodSelectorPolicy selector = context.Policies.Get<IMethodSelectorPolicy>(context.BuildKey);

            foreach (SelectedMethod method in selector.SelectMethods(context))
            {
                GuardMethodIsNotOpenGeneric(method.Method);
                GuardMethodHasNoOutParams(method.Method);
                GuardMethodHasNoRefParams(method.Method);

                ParameterInfo[] parameters = method.Method.GetParameters();

                ilContext.EmitLoadExisting();

                int i = 0;
                foreach(string key in method.GetParameterKeys())
                {
                    ilContext.EmitResolveDependency(parameters[i].ParameterType, key);
                    ++i;
                }
                ilContext.IL.EmitCall(OpCodes.Callvirt, method.Method, null);
                if(method.Method.ReturnType != typeof(void))
                {
                    ilContext.IL.Emit(OpCodes.Pop);
                }
            }
        }

        private static void GuardMethodIsNotOpenGeneric(MethodInfo method)
        {
            if(method.IsGenericMethodDefinition)
            {
                ThrowIllegalInjectionMethod(Resources.CannotInjectOpenGenericMethod, method);
            }
        }

        private static void GuardMethodHasNoOutParams(MethodInfo method)
        {
            if(Array.Find(method.GetParameters(), 
                delegate(ParameterInfo param)
                {
                    return param.IsOut;
                }) != null)
            {
                ThrowIllegalInjectionMethod(Resources.CannotInjectMethodWithOutParam, method);
            }
        }

        private static void GuardMethodHasNoRefParams(MethodInfo method)
        {
            if (Array.Find(method.GetParameters(),
                delegate(ParameterInfo param)
                {
                    return param.ParameterType.IsByRef;
                }) != null)
            {
                ThrowIllegalInjectionMethod(Resources.CannotInjectMethodWithOutParam, method);
            }
        }

        private static void ThrowIllegalInjectionMethod(string format, MethodInfo method)
        {
                throw new IllegalInjectionMethodException(
                    string.Format(CultureInfo.CurrentCulture,
                        format,
                        method.DeclaringType.Name,
                        method.Name));
            
        }
    }
}



