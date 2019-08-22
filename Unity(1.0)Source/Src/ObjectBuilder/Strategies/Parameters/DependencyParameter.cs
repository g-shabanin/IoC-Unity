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

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// Parameter that performs value retrieval depending on dependency injection attributes.
    /// </summary>
	public class DependencyParameter : IParameter
	{
		readonly object buildKey;
		readonly NotPresentBehavior notPresentBehavior;
		object value;

        /// <summary>
        /// Initialize a new instance of the <see cref="DependencyParameter"/> class with a build key and behavior.
        /// </summary>
        /// <param name="buildKey">The key of the object begin built.</param>
        /// <param name="notPresentBehavior">One of the <see cref="NotPresentBehavior"/> values.</param>
		public DependencyParameter(object buildKey,
		                           NotPresentBehavior notPresentBehavior)
		{
			this.buildKey = buildKey;
			this.notPresentBehavior = notPresentBehavior;
		}

        /// <summary>
        /// Gets the key for the object being built.
        /// </summary>
        /// <value>
        /// The key for the object being built.
        /// </value>
		public object BuildKey
		{
			get { return buildKey; }
		}

        /// <summary>
        /// Gets the behaviour for missing dependencies.
        /// </summary>
        /// <value>
        /// One of the <see cref="NotPresentBehavior"/> values.
        /// </value>
		public NotPresentBehavior NotPresentBehavior
		{
			get { return notPresentBehavior; }
		}

        /// <summary>
        /// Gets the type of the parameter value.
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <returns>The parameter's type.</returns>
		public Type GetParameterType(IBuilderContext context)
		{
			return GetValue(context).GetType();
		}

        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <returns>The parameter's value.</returns>
		public object GetValue(IBuilderContext context)
		{
			if (value == null)
				value = DependencyResolver.Resolve(context, BuildKey, NotPresentBehavior);

			return value;
		}
	}
}
