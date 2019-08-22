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

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// Encapsulates a call to a named property.
    /// </summary>
	public class NamedPropertySetterInfo : IPropertySetterInfo
	{
		readonly string propertyName;
		readonly IParameter value;

        /// <summary>
        /// Initialize a new instance of the <see cref="NamedPropertySetterInfo"/> class with the property name and the parameter value.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The paramer value.</param>
		public NamedPropertySetterInfo(string propertyName,
		                               IParameter value)
		{
			this.propertyName = propertyName;
			this.value = value;
		}

        /// <summary>
        /// Sets the value on the property.
        /// </summary>
        /// <param name="context">The current <see cref="IBuilderContext"/>.</param>
        /// <param name="instance">The instance to use to execute the method.</param>
        /// <param name="buildKey">The key for the object being built.</param>
        // FxCop suppression: validation is done by Guard class.
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public void Set(IBuilderContext context,
		                object instance,
		                object buildKey)
		{
            Guard.ArgumentNotNull(instance, "instance");
			PropertyInfo property = instance.GetType().GetProperty(propertyName);
			property.SetValue(instance, value.GetValue(context), null);
		}
	}
}
