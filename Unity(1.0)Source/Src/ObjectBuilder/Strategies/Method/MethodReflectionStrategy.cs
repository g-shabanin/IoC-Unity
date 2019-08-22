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
    /// <typeparam name="TInjectionMethodAttribute">The <see cref="Type"/> of attribute to look for in the method.</typeparam>
    /// <typeparam name="TDefaultParameterAttribute">The <see cref="Type"/> of the default parameter attribute.</typeparam>
	public class MethodReflectionStrategy<
        TInjectionMethodAttribute,
        TDefaultParameterAttribute> 
        : ReflectionStrategy<MethodInfo, TDefaultParameterAttribute>
        where TInjectionMethodAttribute : Attribute
        where TDefaultParameterAttribute : ParameterAttribute, new()
	{
        /// <summary>
        /// Adds <paramref name="parameters"/> to the appropriate policy.
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <param name="member">The member that's being reflected over.</param>
        /// <param name="parameters">The parameters used to satisfy the method.</param>
		protected override void AddParametersToPolicy(IBuilderContext context,
		                                              IMemberInfo<MethodInfo> member,
		                                              IEnumerable<IParameter> parameters)
		{
			IMethodCallPolicy result = context.Policies.Get<IMethodCallPolicy>(context.BuildKey);

			if (result == null)
			{
				result = new MethodCallPolicy();
				context.Policies.Set(result, context.BuildKey);
			}

			result.Methods.Add(new ReflectionMethodCallInfo(member.MemberInfo, parameters));
		}

        /// <summary>
        /// Retrieves the list of methods to iterate looking for injection attributes.
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <returns>
        /// An enumerable wrapper around the <see cref="IMemberInfo{MethodInfo}"/> interfaces that
        /// represent the members to be inspected for reflection.
        /// </returns>
		protected override IEnumerable<IMemberInfo<MethodInfo>> GetMembers(IBuilderContext context)
		{
			foreach (MethodInfo method in BuildKey.GetType(context.BuildKey).GetMethods())
				yield return new MethodMemberInfo<MethodInfo>(method);
		}

        /// <summary>
        /// Determine whether a member should be processed. 
        /// </summary>
        /// <param name="member">The member to determine processing.</param>
        /// <returns>true if the member needs processing; otherwise, false.</returns>
		protected override bool MemberRequiresProcessing(IMemberInfo<MethodInfo> member)
		{
			return (member.GetCustomAttributes(typeof(TInjectionMethodAttribute), true).Length > 0);
		}
	}
}
