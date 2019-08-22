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
    /// Interface used by the <see cref="ReflectionStrategy{TMemberInfo,TDefaultInjectionAttribute}"/> to encapsulate 
    /// the information required from members that use the strategy. This interface is required because direct access to
    /// the <see cref="MemberInfo"/> object may not give the desired results.
    /// </summary>
	public interface IMemberInfo<TMemberInfo>
	{
        /// <summary>
        /// Gets the original <see cref="MemberInfo"/>.
        /// </summary>
        /// <value>
        /// The original <see cref="MemberInfo"/>.
        /// </value>
		TMemberInfo MemberInfo { get; }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <value>
        /// The name of the member.
        /// </value>
		string Name { get; }

        /// <summary>
        /// Gets the custom attributes of the member.
        /// </summary>
        /// <returns>An array of the custom attributes.</returns>
		object[] GetCustomAttributes(Type attributeType,
		                             bool inherit);

        /// <summary>
        /// Gets the parameters to be passed to the member.
        /// </summary>
        /// <returns>The parameters to be passed to the member.</returns>
		ParameterInfo[] GetParameters();
	}
}
