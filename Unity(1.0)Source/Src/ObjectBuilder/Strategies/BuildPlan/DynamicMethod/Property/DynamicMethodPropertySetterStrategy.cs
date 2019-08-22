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

using System.Reflection.Emit;

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// A <see cref="BuilderStrategy"/> that generates IL to resolve properties
    /// on an object being built.
    /// </summary>
    public class DynamicMethodPropertySetterStrategy : BuilderStrategy
    {
        /// <summary>
        /// Called during the chain of responsibility for a build operation.
        /// </summary>
        /// <param name="context">The context for the operation.</param>
        public override void PreBuildUp(IBuilderContext context)
        {
            DynamicBuildPlanGenerationContext ilContext = (DynamicBuildPlanGenerationContext)(context.Existing);

            IPropertySelectorPolicy selector = context.Policies.Get<IPropertySelectorPolicy>(context.BuildKey);
            foreach(SelectedProperty property in selector.SelectProperties(context))
            {
                ilContext.EmitLoadExisting();
                ilContext.EmitResolveDependency(property.Property.PropertyType, property.Key);
                ilContext.IL.EmitCall(OpCodes.Callvirt, property.Property.GetSetMethod(), null);
            }
        }
    }
}
