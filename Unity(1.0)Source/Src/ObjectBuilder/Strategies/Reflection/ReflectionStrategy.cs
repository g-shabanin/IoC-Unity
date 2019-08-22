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
    /// Represents a generic strategy for all injection attribute processors.
    /// </summary>
    /// <typeparam name="TMemberInfo">The <see cref="Type"/> of member to reflect.</typeparam>
    /// <typeparam name="TDefaultInjectionAttribute">The <see cref="Type"/> of attribute for the member.</typeparam>
	public abstract class ReflectionStrategy<TMemberInfo, TDefaultInjectionAttribute> : BuilderStrategy 
        where TDefaultInjectionAttribute : ParameterAttribute, new()
	{
        /// <summary>
        /// Adds <paramref name="parameters"/> to the appropriate policy.
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <param name="member">The member that's being reflected over.</param>
        /// <param name="parameters">The parameters used to satisfy the member call.</param>
		protected abstract void AddParametersToPolicy(IBuilderContext context,
		                                              IMemberInfo<TMemberInfo> member,
		                                              IEnumerable<IParameter> parameters);


        /// <summary>
        /// Called during the chain of responsibility for a build operation. The
        /// PreBuildUp method is called when the chain is being executed in the
        /// forward direction.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        // FxCop suppression: Validation is done by Guard class
        public override void PreBuildUp(IBuilderContext context)
        {
            Type type;
            BuildKey.TryGetType(context.BuildKey, out type);

            foreach (IMemberInfo<TMemberInfo> member in GetMembers(context))
            {
                if (MemberRequiresProcessing(member))
                {
                    IEnumerable<IParameter> parameters =
                        GenerateIParametersFromParameterInfos(member.GetParameters(), type, member.Name);

                    AddParametersToPolicy(context, member, parameters);
                }
            }

            
        }

		static IEnumerable<IParameter> GenerateIParametersFromParameterInfos(IEnumerable<ParameterInfo> parameterInfos,
		                                                                     Type type,
		                                                                     string memberName)
		{
			List<IParameter> result = new List<IParameter>();

			foreach (ParameterInfo parameterInfo in parameterInfos)
			{
				ParameterAttribute attribute = GetInjectionAttribute(parameterInfo, type, memberName);
				result.Add(attribute.CreateParameter(parameterInfo.ParameterType));
			}

			return result;
		}

		static ParameterAttribute GetInjectionAttribute(ICustomAttributeProvider parameterInfo,
		                                                Type type,
		                                                string memberName)
		{
			ParameterAttribute[] attributes = (ParameterAttribute[])parameterInfo.GetCustomAttributes(typeof(ParameterAttribute), true);

			switch (attributes.Length)
			{
			case 0:
				return new TDefaultInjectionAttribute();

			case 1:
				return attributes[0];

			default:
				throw new InvalidAttributeException(type, memberName);
			}
		}

        /// <summary>
        /// Retrieves the list of members to iterate looking for 
        /// injection attributes, such as properties and constructor 
        /// parameters.
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <returns>
        /// An enumerable wrapper around the <see cref="IMemberInfo{TMemberInfo}"/> interfaces that
        /// represent the members to be inspected for reflection.
        /// </returns>
		protected abstract IEnumerable<IMemberInfo<TMemberInfo>> GetMembers(IBuilderContext context);

        /// <summary>
        /// Determine whether a member should be processed. 
        /// </summary>
        /// <param name="member">The member to determine processing.</param>
        /// <returns>true if the member needs processing; otherwise, false.</returns>
		protected abstract bool MemberRequiresProcessing(IMemberInfo<TMemberInfo> member);
	}
}
