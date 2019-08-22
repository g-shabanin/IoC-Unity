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
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.Unity.ObjectBuilder
{
    class SpecifiedPropertiesSelectorPolicy : IPropertySelectorPolicy
    {
        private List<Pair<PropertyInfo, InjectionParameterValue>> propertiesAndValues = 
            new List<Pair<PropertyInfo, InjectionParameterValue>>();

        public void AddPropertyAndValue(PropertyInfo property, InjectionParameterValue value)
        {
            propertiesAndValues.Add(Pair.Make(property, value));
        }

        /// <summary>
        /// Returns sequence of properties on the given type that
        /// should be set as part of building that object.
        /// </summary>
        /// <param name="context">Current build context.</param>
        /// <returns>Sequence of <see cref="PropertyInfo"/> objects
        /// that contain the properties to set.</returns>
        public IEnumerable<SelectedProperty> SelectProperties(IBuilderContext context)
        {
            foreach(Pair<PropertyInfo, InjectionParameterValue> pair in propertiesAndValues)
            {
                string key = Guid.NewGuid().ToString();
                context.PersistentPolicies.Set<IDependencyResolverPolicy>(
                    pair.Second.GetResolverPolicy(), key);
                yield return new SelectedProperty(pair.First, key);
            }
        }
    }
}
