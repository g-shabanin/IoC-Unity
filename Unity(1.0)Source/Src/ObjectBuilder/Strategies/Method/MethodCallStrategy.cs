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
    /// Represents a strategy to call method.
    /// </summary>
	public class MethodCallStrategy : BuilderStrategy
	{
        /// <summary>
        /// Called during the chain of responsibility for a build operation. Looks for a method call policy for the buildKey and uses it to call a method if found.
        /// </summary>
        /// <param name="context">The context for the operation.</param>
		public override void PreBuildUp(IBuilderContext context)
		{
			IMethodCallPolicy policy = context.Policies.Get<IMethodCallPolicy>(context.BuildKey);

			if (context.Existing != null && policy != null)
			{
			    foreach (IMethodCallInfo method in policy.Methods)
			    {
			        method.Execute(context, context.Existing, context.BuildKey);
			    }
			}
		}
	}
}
