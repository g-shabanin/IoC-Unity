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
    /// Indicates dependency injection members, whose value at 
    /// build time will be determined by the <see cref="IParameter"/> returned 
    /// from the attribute <see cref="CreateParameter"/> factory method.
    /// </summary>
	public abstract class ParameterAttribute : Attribute
	{
        /// <summary>
        /// Creates a parameter for use with various <see cref="IBuilderPolicy"/> implementations 
        /// that can process <see cref="IParameter"/>s.
        /// </summary>
        /// <param name="annotatedMemberType">The type of the annotated member, such as a property or a 
        /// constructor parameter.</param>
        /// <returns>The parameter instance that knows how to retrieve a value for the dependency.</returns>
		public abstract IParameter CreateParameter(Type annotatedMemberType);
	}
}
