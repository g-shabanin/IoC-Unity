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

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// Implementation of <see cref="IBuilderStrategy"/> which allows objects to be
    /// singletons.
    /// </summary>
    /// <remarks>
    /// This strategy looks for policies in the context registered under the interface type
    /// <see cref="ISingletonPolicy"/>. It uses the locator in the build context to rememeber
    /// singleton objects, and the lifetime container contained in the locator to ensure they
    /// are not garbage collected. Upon the second request for an object, it will short-circuit
    /// the strategy chain and return the singleton instance (and will not re-inject the
    /// object).
    /// </remarks>
	public class SingletonStrategy : BuilderStrategy
	{
        /// <summary>
        /// Called during the chain of responsibility for a build operation. The
        /// PreBuildUp method is called when the chain is being executed in the
        /// forward direction.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        // FxCop suppression: Validation is done by Guard class
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.ArgumentNotNull(context, "context");
            if(context.Locator != null)
            {
                if(context.Locator.Contains(context.BuildKey))
                {
                    context.Existing = context.Locator.Get(context.BuildKey);
                    context.BuildComplete = true;
                }
            }
        }

        /// <summary>
        /// Called during the chain of responsibility for a build operation. The
        /// PostBuildUp method is called when the chain has finished the PreBuildUp
        /// phase and executes in reverse order from the PreBuildUp calls.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        // FxCop suppression: Validation is done by Guard class
        public override void PostBuildUp(IBuilderContext context)
        {
            base.PostBuildUp(context);
            if(context.Locator != null && context.Lifetime != null)
            {
                ISingletonPolicy singletonPolicy = context.Policies.Get<ISingletonPolicy>(context.BuildKey);
                if(singletonPolicy != null && singletonPolicy.IsSingleton)
                {
                    context.Locator.Add(context.BuildKey, context.Existing);
                    context.Lifetime.Add(context.Existing);
                }
            }
        }
	}
}
