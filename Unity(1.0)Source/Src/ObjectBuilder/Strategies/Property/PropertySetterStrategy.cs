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

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// Implementation of <see cref="IBuilderStrategy"/> which sets property values.
    /// </summary>
    /// <remarks>
    /// This strategy looks for policies in the context registered under the interface type
    /// <see cref="IPropertySetterPolicy"/>, and sets the property values. If no policy is
    /// found, the no property values are set.
    /// </remarks>
	public class PropertySetterStrategy : BuilderStrategy
	{
        /// <summary>
        /// Called during the chain of responsibility for a build operation.  Looks for the <see cref="IPropertySetterPolicy"/> and
        /// sets the value for the property if found.
        /// </summary>
        /// <param name="context">The context for the operation.</param>
		public override void PreBuildUp(IBuilderContext context)
		{
			IPropertySetterPolicy policy = context.Policies.Get<IPropertySetterPolicy>(context.BuildKey);

			if (context.Existing != null && policy != null)
			{
			    foreach (IPropertySetterInfo property in policy.Properties)
			    {
			        property.Set(context, context.Existing, context.BuildKey);
			    }
			}
		}
	}
}
