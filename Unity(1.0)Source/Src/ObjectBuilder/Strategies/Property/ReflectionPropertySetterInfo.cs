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

using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// Encapsulates a reflection property setting.
    /// </summary>
	public class ReflectionPropertySetterInfo : IPropertySetterInfo
	{
		readonly PropertyInfo property;
		readonly IParameter value;

        /// <summary>
        /// Initialize a new instance of the <see cref="ReflectionPropertySetterInfo"/> class with a property and paramter value.
        /// </summary>
        /// <param name="property">The property to use to set the value.</param>
        /// <param name="value">The value for the property.</param>
		public ReflectionPropertySetterInfo(PropertyInfo property,
		                                    IParameter value)
		{
			this.property = property;
			this.value = value;
		}

        /// <summary>
        /// Gets the property for the setter call.
        /// </summary>
        /// <value>
        /// The property for the setter call.
        /// </value>
		public PropertyInfo Property
		{
			get { return property; }
		}

        /// <summary>
        /// Gets the value for the property.
        /// </summary>
        /// <value>
        /// The value for the property.
        /// </value>
		public IParameter Value
		{
			get { return value; }
		}

        /// <summary>
        /// Sets the value on the property.
        /// </summary>
        /// <param name="context">The current <see cref="IBuilderContext"/>.</param>
        /// <param name="instance">The instance to use to execute the method.</param>
        /// <param name="buildKey">The key for the object being built.</param>
		public void Set(IBuilderContext context,
		                object instance,
		                object buildKey)
		{
			Property.SetValue(instance, Value.GetValue(context), null);
		}
	}
}
