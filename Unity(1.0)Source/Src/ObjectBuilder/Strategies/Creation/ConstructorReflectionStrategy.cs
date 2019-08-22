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
    /// Strategy that performs injection of constructor policies.
    /// </summary>
    /// <typeparam name="TInjectionConstructorAttribute">The <see cref="Type"/> of attribute to look for in the constructor.</typeparam>
    /// <typeparam name="TDefaultInjectionAttribute">The <see cref="Type"/> of the default constructor attribute.</typeparam>
	public class ConstructorReflectionStrategy<
        TInjectionConstructorAttribute,
        TDefaultInjectionAttribute> 
        : ReflectionStrategy<ConstructorInfo, TDefaultInjectionAttribute>
        where TDefaultInjectionAttribute : ParameterAttribute, new()
        where TInjectionConstructorAttribute : Attribute
	{
        /// <summary>
        /// Adds <paramref name="parameters"/> to the appropriate policy.
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <param name="member">The member that's being reflected over.</param>
        /// <param name="parameters">The parameters used to satisfy the constructor.</param>
		protected override void AddParametersToPolicy(IBuilderContext context,
		                                              IMemberInfo<ConstructorInfo> member,
		                                              IEnumerable<IParameter> parameters)
		{
			ConstructorCreationPolicy policy = new ConstructorCreationPolicy(member.MemberInfo, parameters);
			context.Policies.Set<ICreationPolicy>(policy, context.BuildKey);
		}

        /// <summary>
        /// Retrieves the list of constructors to iterate looking for injection attributes.
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <returns>
        /// An enumerable wrapper around the <see cref="IMemberInfo{ConstructorInfo}"/> interfaces that
        /// represent the members to be inspected for reflection.
        /// </returns>
		protected override IEnumerable<IMemberInfo<ConstructorInfo>> GetMembers(IBuilderContext context)
		{
			ICreationPolicy existingPolicy = context.Policies.GetNoDefault<ICreationPolicy>(context.BuildKey, false);

			if (context.Existing == null && existingPolicy == null)
			{
				Type typeToBuild = BuildKey.GetType(context.BuildKey);
				ConstructorInfo injectionCtor = null;
				ConstructorInfo[] ctors = typeToBuild.GetConstructors();

				if (ctors.Length == 1)
					injectionCtor = ctors[0];
				else
					foreach (ConstructorInfo ctor in ctors)
						if (Attribute.IsDefined(ctor, typeof(TInjectionConstructorAttribute)))
						{
							if (injectionCtor != null)
								throw new InvalidAttributeException(typeToBuild, ".ctor");

							injectionCtor = ctor;
						}

				if (injectionCtor != null)
					yield return new MethodMemberInfo<ConstructorInfo>(injectionCtor);
			}
		}

        /// <summary>
        /// Determine whether a constructor should be processed. 
        /// </summary>
        /// <param name="member">The member to determine processing.</param>
        /// <returns>Always returns true.</returns>
		protected override bool MemberRequiresProcessing(IMemberInfo<ConstructorInfo> member)
		{
			return true;
		}
	}
}
