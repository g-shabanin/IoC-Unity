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
    /// Encapsulates a call to a named method.
    /// </summary>
	public class NamedMethodCallInfo : IMethodCallInfo
	{
		readonly string methodName;
		readonly List<IParameter> parameters;

        /// <summary>
        /// Initalize a new instance of the <see cref="NamedMethodCallInfo"/> class with an array of <see cref="IParameter"/> instances.
        /// </summary>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="parameters">An array of <see cref="IParameter"/> instances.</param>
		public NamedMethodCallInfo(string methodName,
		                           params IParameter[] parameters)
			: this(methodName, (IEnumerable<IParameter>)parameters) {}

        /// <summary>
        /// Initalize a new instance of the <see cref="NamedMethodCallInfo"/> class with a collection of <see cref="IParameter"/> instances.
        /// </summary>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="parameters">A collection of <see cref="IParameter"/> instances.</param>
		public NamedMethodCallInfo(string methodName,
		                           IEnumerable<IParameter> parameters)
		{
			this.methodName = methodName;
			this.parameters = new List<IParameter>();

			if (parameters != null)
				this.parameters.AddRange(parameters);
		}

        /// <summary>
        /// ExecuteBuildUp the method to be called.
        /// </summary>
        /// <param name="context">The current <see cref="IBuilderContext"/>.</param>
        /// <param name="instance">The instance to use to execute the method.</param>
        /// <param name="buildKey">The key for the object being built.</param>
		public void Execute(IBuilderContext context,
		                    object instance,
		                    object buildKey)
		{
			List<Type> parameterTypes = new List<Type>();
			foreach (IParameter parameter in parameters)
				parameterTypes.Add(parameter.GetParameterType(context));

			List<object> parameterValues = new List<object>();
			foreach (IParameter parameter in parameters)
				parameterValues.Add(parameter.GetValue(context));

			MethodInfo method = instance.GetType().GetMethod(methodName, parameterTypes.ToArray());
			method.Invoke(instance, parameterValues.ToArray());
		}
	}
}
