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

using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// Represents a policy for <see cref="PropertySetterStrategy"/>.
    /// </summary>
	public interface IPropertySetterPolicy : IBuilderPolicy
	{
        /// <summary>
        /// Gets a collection of properties to be called on the object instance.
        /// </summary>
        /// <value>
        /// A collection of properties to be called on the object instance.
        /// </value>
		ICollection<IPropertySetterInfo> Properties { get; }
	}
}
