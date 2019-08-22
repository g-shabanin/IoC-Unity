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
    /// An implementation helper class for <see cref="IParameter"/> which can be used
    /// when you know the type of the parameter value ahead of time.
    /// </summary>
	public abstract class KnownTypeParameter : IParameter
	{
        /// <summary>
        /// The parameter type.
        /// </summary>
		private Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownTypeParameter"/> class
        /// using the given type.
        /// </summary>
        /// <param name="type">The parameter type.</param>
		protected KnownTypeParameter(Type type)
		{
			this.type = type;
		}

        /// <summary>
        /// Gets the type of the parameter value.
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <returns>The parameter's type.</returns>
		public Type GetParameterType(IBuilderContext context)
		{
			return type;
		}

        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <returns>The parameter's value.</returns>
		public abstract object GetValue(IBuilderContext context);
	}
}
