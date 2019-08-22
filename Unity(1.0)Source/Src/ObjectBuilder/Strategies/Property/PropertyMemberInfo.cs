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
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// Represents the member inforamation for a <see cref="PropertyInfo"/>.
    /// </summary>
	public class PropertyMemberInfo : IMemberInfo<PropertyInfo>
	{
		readonly PropertyInfo prop;

        /// <summary>
        /// Initialize a new instance of the <see cref="PropertyMemberInfo"/> class with a <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="prop">The <see cref="PropertyInfo"/> to initialize the class.</param>
		public PropertyMemberInfo(PropertyInfo prop)
		{
			this.prop = prop;
		}

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/>.
        /// </summary>
        /// <value>
        /// The <see cref="PropertyInfo"/>.
        /// </value>
		public PropertyInfo MemberInfo
		{
			get { return prop; }
		}

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the poperty.
        /// </value>
		public string Name
		{
			get { return prop.Name; }
		}

        /// <summary>
        /// Gets the custom attributes for the property.
        /// </summary>
        /// <param name="attributeType">The <see cref="Type"/> of the attributes to get from the property.</param>
        /// <param name="inherit">true to get inherited attrubutes; otherwise, false.</param>
        /// <returns>An array of the custom attributes.</returns>
		public object[] GetCustomAttributes(Type attributeType,
		                                    bool inherit)
		{
			return prop.GetCustomAttributes(attributeType, inherit);
		}

        /// <summary>
        /// The parameters for the property.
        /// </summary>
        /// <returns>An array of <see cref="ParameterInfo"/> objects.</returns>
		public ParameterInfo[] GetParameters()
		{
			return new ParameterInfo[] { new CustomPropertyParameterInfo(prop) };
		}

		class CustomPropertyParameterInfo : ParameterInfo
		{
			readonly PropertyInfo prop;

			public CustomPropertyParameterInfo(PropertyInfo prop)
			{
				this.prop = prop;
			}

			public override Type ParameterType
			{
				get { return prop.PropertyType; }
			}

			public override object[] GetCustomAttributes(Type attributeType,
			                                             bool inherit)
			{
				return prop.GetCustomAttributes(attributeType, inherit);
			}
		}
	}
}
