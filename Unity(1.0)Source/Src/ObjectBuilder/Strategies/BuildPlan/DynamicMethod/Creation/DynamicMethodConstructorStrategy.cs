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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Practices.ObjectBuilder2.Properties;

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// A <see cref="BuilderStrategy"/> that emits IL to call constructors
    /// as part of creating a build plan.
    /// </summary>
    public class DynamicMethodConstructorStrategy : BuilderStrategy
    {
        private static MethodInfo throwForNullExistingObject =
            typeof (DynamicMethodConstructorStrategy).GetMethod("ThrowForNullExistingObject");

        /// <summary>
        /// Called during the chain of responsibility for a build operation.
        /// </summary>
        /// <remarks>Existing object is an instance of <see cref="DynamicBuildPlanGenerationContext"/>.</remarks>
        /// <param name="context">The context for the operation.</param>
        // FxCop suppression: Validation is done by Guard class
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.ArgumentNotNull(context, "context");

            DynamicBuildPlanGenerationContext buildContext =
                (DynamicBuildPlanGenerationContext)context.Existing;

            IConstructorSelectorPolicy selector =
                context.Policies.Get<IConstructorSelectorPolicy>(context.BuildKey);

            SelectedConstructor selectedCtor = selector.SelectConstructor(context);
            // Method preamble - test if we have an existing object
            // First off, set up jump - if there's an existing object, skip us entirely
            Label existingObjectNotNull = buildContext.IL.DefineLabel();
            buildContext.EmitLoadExisting();
            buildContext.IL.Emit(OpCodes.Ldnull);
            buildContext.IL.Emit(OpCodes.Ceq);
            buildContext.IL.Emit(OpCodes.Brfalse, existingObjectNotNull);

            if (selectedCtor != null)
            {
                // Resolve parameters
                ParameterInfo[] parameters = selectedCtor.Constructor.GetParameters();

                int i = 0;
                foreach (string parameterKey in selectedCtor.GetParameterKeys())
                {
                    buildContext.EmitResolveDependency(parameters[i].ParameterType, parameterKey);
                    ++i;
                }

                // Call the constructor

                buildContext.IL.Emit(OpCodes.Newobj, selectedCtor.Constructor);
                buildContext.EmitStoreExisting();
            }
            else
            {
                // If we get here, object has no constructors. It's either
                // an interface or a primitive (like int). In this case,
                // verify that we have an Existing object, and if not,
                // throw (via helper function).
                buildContext.EmitLoadContext();
                buildContext.IL.EmitCall(OpCodes.Call, throwForNullExistingObject, null);
            }

            buildContext.IL.MarkLabel(existingObjectNotNull);
        }

        /// <summary>
        /// A helper method used by the generated IL to throw an exception if
        /// a dependency cannot be resolved.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> currently being
        /// used for the build of this object.</param>
        // FxCop suppression: Validation is done by Guard class
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static void ThrowForNullExistingObject(IBuilderContext context)
        {
            Guard.ArgumentNotNull(context, "context");
            throw new InvalidOperationException(
                string.Format(CultureInfo.CurrentCulture,
                              Resources.NoConstructorFound,
                              BuildKey.GetType(context.BuildKey).Name));
        }
    }
}
