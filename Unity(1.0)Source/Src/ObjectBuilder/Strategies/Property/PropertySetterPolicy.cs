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
	public class PropertySetterPolicy : IPropertySetterPolicy
	{
		readonly List<IPropertySetterInfo> properties = new List<IPropertySetterInfo>();

        /// <summary>
        /// Gets a collection of properties to be called on the object instance.
        /// </summary>
        /// <value>
        /// A collection of properties to be called on the object instance.
        /// </value>
		public ICollection<IPropertySetterInfo> Properties
		{
			get { return properties; }
		}
	}
}
