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
using Microsoft.Practices.ObjectBuilder2.Properties;

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// Represents a strategy which creates objects.
    /// </summary>
    /// <remarks>
    /// <para>This strategy looks for policies in the context registered under the interface type
    /// <see cref="ICreationPolicy"/>. If it cannot find a policy on how to create the object,
    /// it will select the first constructor that returns from reflection, and re-runs the chain
    /// to create all the objects required to fulfill the constructor's parameters.</para>
    /// <para>If the Build method is passed an object via the existing parameter, then it
    /// will do nothing (since the object already exists). This allows this strategy to be
    /// in the chain when running dependency injection on existing objects, without fear that
    /// it will attempt to re-create the object.</para>
    /// </remarks>
	public class CreationStrategy : BuilderStrategy
	{
        /// <summary>
        /// Called during the chain of responsibility for a build operation.
        /// </summary>
        /// <param name="context">The context for the operation.</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
            Justification="Validation is done via the Guard class")]
        public override void PreBuildUp(IBuilderContext context)
		{
            Guard.ArgumentNotNull(context, "context");
            if(context.Existing == null)
            {
                context.Existing = BuildUpNewObject(context);
            }
		}

		static object BuildUpNewObject(IBuilderContext context)
		{
			ICreationPolicy policy = context.Policies.Get<ICreationPolicy>(context.BuildKey);

			if (policy == null)
			    throw new InvalidOperationException(
			        string.Format(
			            CultureInfo.CurrentCulture,
			            Resources.NoCreationPolicy,
			            context.BuildKey));

			return policy.Create(context, context.BuildKey);
		}
	}
}
