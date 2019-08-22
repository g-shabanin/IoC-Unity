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
    /// Represents the member inforamation for a <see cref="MethodBase"/>.
    /// </summary>
	public class MethodMemberInfo<TMemberInfo> : IMemberInfo<TMemberInfo>
		where TMemberInfo : MethodBase
	{
		readonly TMemberInfo memberInfo;

        /// <summary>
        /// Initialize a new instace of the <see cref="MethodMemberInfo{TMemberInfo}"/> class with the method to wrap.
        /// </summary>
        /// <param name="memberInfo">
        /// The method to wrap.
        /// </param>
		public MethodMemberInfo(TMemberInfo memberInfo)
		{
			this.memberInfo = memberInfo;
		}

        /// <summary>
        /// Gets the wrapped method.
        /// </summary>
        /// <value>
        /// The wrapped method.
        /// </value>
		public TMemberInfo MemberInfo
		{
			get { return memberInfo; }
		}

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        /// <value>
        /// The name of the method.
        /// </value>
		public string Name
		{
			get { return memberInfo.Name; }
		}

        /// <summary>
        /// Gets the custom attributes for the method.
        /// </summary>
        /// <param name="attributeType">The <see cref="Type"/> of the attributes to get from the method.</param>
        /// <param name="inherit">true to get inherited attrubutes; otherwise, false.</param>
        /// <returns>An array of the custom attributes.</returns>
		public object[] GetCustomAttributes(Type attributeType,
		                                    bool inherit)
		{
			return memberInfo.GetCustomAttributes(attributeType, inherit);
		}

        /// <summary>
        /// The parameters for the method.
        /// </summary>
        /// <returns>An array of <see cref="ParameterInfo"/> objects.</returns>
		public ParameterInfo[] GetParameters()
		{
			return memberInfo.GetParameters();
		}
	}
}
