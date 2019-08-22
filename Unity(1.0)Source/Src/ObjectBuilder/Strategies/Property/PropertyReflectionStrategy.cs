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
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// Strategy that performs injection of method policies.
    /// </summary>
    /// <typeparam name="TDefaultParameterAttribute">The <see cref="Type"/> of attribute to look for in the property.</typeparam>
	public class PropertyReflectionStrategy<TDefaultParameterAttribute> 
        : ReflectionStrategy<PropertyInfo, TDefaultParameterAttribute>
        where TDefaultParameterAttribute : ParameterAttribute, new()
	{
        /// <summary>
        /// Adds <paramref name="parameters"/> to the appropriate policy.
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <param name="member">The member that's being reflected over.</param>
        /// <param name="parameters">The parameters used to satisfy the method.</param>
		protected override void AddParametersToPolicy(IBuilderContext context,
		                                              IMemberInfo<PropertyInfo> member,
		                                              IEnumerable<IParameter> parameters)
		{
			IPropertySetterPolicy result = context.Policies.Get<IPropertySetterPolicy>(context.BuildKey);

			if (result == null)
			{
				result = new PropertySetterPolicy();
				context.Policies.Set(result, context.BuildKey);
			}

			foreach (IParameter parameter in parameters)
				result.Properties.Add(new ReflectionPropertySetterInfo(member.MemberInfo, parameter));
		}

        /// <summary>
        /// Retrieves the list of properties to iterate looking for injection attributes.
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <returns>
        /// An enumerable wrapper around the <see cref="IMemberInfo{PropertyInfo}"/> interfaces that
        /// represent the members to be inspected for reflection.
        /// </returns>
		protected override IEnumerable<IMemberInfo<PropertyInfo>> GetMembers(IBuilderContext context)
		{
			foreach (PropertyInfo propInfo in BuildKey.GetType(context.BuildKey).GetProperties())
		    if(propInfo.GetIndexParameters().Length == 0)
		    {
		        yield return new PropertyMemberInfo(propInfo);
		    }
		}

        /// <summary>
        /// Determine whether a member should be processed. 
        /// </summary>
        /// <param name="member">The member to determine processing.</param>
        /// <returns>true if the member needs processing; otherwise, false.</returns>
		protected override bool MemberRequiresProcessing(IMemberInfo<PropertyInfo> member)
		{
			return (member.GetCustomAttributes(typeof(ParameterAttribute), true).Length > 0);
		}
	}
}
