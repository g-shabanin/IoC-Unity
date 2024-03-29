﻿//===============================================================================
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
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.Unity
{
    /// <summary>
    /// A Unity container extension that allows you to configure
    /// which constructors, properties, and methods get injected
    /// via an API rather than through attributes.
    /// </summary>
    public class InjectedMembers : UnityContainerExtension
    {
        /// <summary>
        /// Initial the container with this extension's functionality.
        /// </summary>
        /// <remarks>
        /// When overridden in a derived class, this method will modify the given
        /// <see cref="ExtensionContext"/> by adding strategies, policies, etc. to
        /// install it's functions into the container.</remarks>
        protected override void Initialize()
        {
        }

        /// <summary>
        /// API to configure the injection settings for a particular type.
        /// </summary>
        /// <typeparam name="TTypeToInject">Type the injection is being configured for.</typeparam>
        /// <param name="injectionMembers">Objects containing the details on which members to inject and how.</param>
        /// <returns>This extension object.</returns>
        public InjectedMembers ConfigureInjectionFor<TTypeToInject>(params InjectionMember[] injectionMembers)
        {
            return ConfigureInjectionFor(typeof(TTypeToInject), null, injectionMembers);
        }

        /// <summary>
        /// API to configure the injection settings for a particular type/name pair.
        /// </summary>
        /// <typeparam name="TTypeToInject">Type the injection is being configured for.</typeparam>
        /// <param name="name">Name of registration</param>
        /// <param name="injectionMembers">Objects containing the details on which members to inject and how.</param>
        /// <returns>This extension object.</returns>
        public InjectedMembers ConfigureInjectionFor<TTypeToInject>(string name, params InjectionMember[] injectionMembers)
        {
            return ConfigureInjectionFor(typeof(TTypeToInject), name, injectionMembers);
        }

        /// <summary>
        /// API to configure the injection settings for a particular type.
        /// </summary>
        /// <param name="typeToInject">Type to configure.</param>
        /// <param name="injectionMembers">Objects containing the details on which members to inject and how.</param>
        /// <returns>This extension object.</returns>
        public InjectedMembers ConfigureInjectionFor(Type typeToInject, params InjectionMember[] injectionMembers)
        {
            return ConfigureInjectionFor(typeToInject, null, injectionMembers);
        }

        /// <summary>
        /// API to configure the injection settings for a particular type/name pair.
        /// </summary>
        /// <param name="typeToInject">Type to configure.</param>
        /// <param name="name">Name of registration.</param>
        /// <param name="injectionMembers">Objects containing the details on which members to inject and how.</param>
        /// <returns>This extension object.</returns>
        public InjectedMembers ConfigureInjectionFor(Type typeToInject, string name, params InjectionMember[] injectionMembers)
        {
            foreach(InjectionMember member in injectionMembers)
            {
                member.AddPolicies(typeToInject, name, Context.Policies);
            }

            // Flush the build plan and current resolvers so that it will get rebuilt on next resolve.
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(typeToInject, name);
            Context.Policies.Clear<IBuildPlanPolicy>(buildKey);
            DependencyResolverTrackerPolicy.RemoveResolvers(Context.Policies, buildKey);
            return this;
            
        }
    }
}
