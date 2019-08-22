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
    /// Attribute applied to properties and constructor parameters, to describe when the dependency
    /// injection system should inject an object.
    /// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public sealed class DependencyAttribute : ParameterAttribute
	{
		readonly object buildKey;
		NotPresentBehavior notPresentBehavior = NotPresentBehavior.Build;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyAttribute"/> class with a null build key.
        /// </summary>
		public DependencyAttribute()
			: this(null) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyAttribute"/> class with a build key.
        /// </summary>
        /// <param name="buildKey">The key of the object to build.</param>
		public DependencyAttribute(object buildKey)
		{
			this.buildKey = buildKey;
		}
    
        /// <summary>
        /// Gets the key of the object to be built.
        /// </summary>
        /// <value>
        /// The key of the object to be built.
        /// </value>
		public object BuildKey
		{
			get { return buildKey; }
		}

        /// <summary>
        /// Gets or sets the behaviour when the dependecy can't be found.
        /// </summary>
		public NotPresentBehavior NotPresentBehavior
		{
			get { return notPresentBehavior; }
			set { notPresentBehavior = value; }
		}

        /// <summary>
        /// Creates a parameter for use with various <see cref="IBuilderPolicy"/> implementations 
        /// that can process <see cref="IParameter"/>s.
        /// </summary>
        /// <param name="annotatedMemberType">The type of the annotated member, such as a property or a 
        /// constructor parameter.</param>
        /// <returns>The parameter instance that knows how to retrieve a value for the dependency.</returns>
		public override IParameter CreateParameter(Type annotatedMemberType)
		{
			return new DependencyParameter(BuildKey ?? annotatedMemberType, notPresentBehavior);
		}
	}
}
