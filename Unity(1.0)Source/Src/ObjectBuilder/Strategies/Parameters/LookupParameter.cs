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

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// Looks up the parameter value in the build context locator.
    /// </summary>
	public class LookupParameter : IParameter
	{
		readonly object key;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupParameter"/> class.
        /// </summary>
        /// <param name="key">The key to look the object up with.</param>
		public LookupParameter(object key)
		{
			this.key = key;
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
        // FxCop suppression: validation is done by Guard class.
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public object GetValue(IBuilderContext context)
		{
            Guard.ArgumentNotNull(context, "context");
			return context.Locator.Get(key);
		}
	}
}
