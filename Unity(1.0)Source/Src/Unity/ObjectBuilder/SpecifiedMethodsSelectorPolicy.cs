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

using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.Unity.ObjectBuilder
{
    class SpecifiedMethodsSelectorPolicy : SpecifiedMemberSelectorPolicy, IMethodSelectorPolicy
    {
        private List<Pair<MethodInfo, IEnumerable<InjectionParameterValue>>> methods =
            new List<Pair<MethodInfo, IEnumerable<InjectionParameterValue>>>();

        public void AddMethodAndParameters(MethodInfo method, IEnumerable<InjectionParameterValue> parameters)
        {
            methods.Add(Pair.Make(method, parameters));
        }

        /// <summary>
        /// Return the sequence of methods to call while building the target object.
        /// </summary>
        /// <param name="context">Current build context.</param>
        /// <returns>Sequence of methods to call.</returns>
        public IEnumerable<SelectedMethod> SelectMethods(IBuilderContext context)
        {
            foreach(Pair<MethodInfo, IEnumerable<InjectionParameterValue>> method in methods)
            {
                SelectedMethod selectedMethod = new SelectedMethod(method.First);
                AddParameterResolvers(context.PersistentPolicies, method.Second, selectedMethod);
                yield return selectedMethod;
            }
        }
    }
}
