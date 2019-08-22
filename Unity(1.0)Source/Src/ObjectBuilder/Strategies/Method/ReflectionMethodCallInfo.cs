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

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// Encapsulates a reflection method call.
    /// </summary>
	public class ReflectionMethodCallInfo : IMethodCallInfo
	{
		readonly MethodInfo method;
		readonly List<IParameter> parameters;

        /// <summary>
        /// Initalize a new instance of the <see cref="ReflectionMethodCallInfo"/> class with an array of <see cref="IParameter"/> instances.
        /// </summary>
        /// <param name="method">The method to execute.</param>
        /// <param name="parameters">An array of <see cref="IParameter"/> instances.</param>
		public ReflectionMethodCallInfo(MethodInfo method,
		                                params IParameter[] parameters)
			: this(method, (IEnumerable<IParameter>)parameters) {}

        /// <summary>
        /// Initalize a new instance of the <see cref="NamedMethodCallInfo"/> class with a collection of <see cref="IParameter"/> instances.
        /// </summary>
        /// <param name="method">The method to execute.</param>
        /// <param name="parameters">A collection of <see cref="IParameter"/> instances.</param>
		public ReflectionMethodCallInfo(MethodInfo method,
		                                IEnumerable<IParameter> parameters)
		{
			this.method = method;
			this.parameters = new List<IParameter>();

			if (parameters != null)
			    this.parameters.AddRange(parameters);
		}

        /// <summary>
        /// Gets the method to execute.
        /// </summary>
        /// <value>
        /// The method to execute.
        /// </value>
		public MethodInfo Method
		{
			get { return method; }
		}

        /// <summary>
        /// Gets the parameters used in the method call.
        /// </summary>
        /// <value>
        /// The parameters used in the method call.
        /// </value>
		public ICollection<IParameter> Parameters
		{
			get { return parameters; }
		}

        /// <summary>
        /// Execute the method to be called.
        /// </summary>
        /// <param name="context">The current <see cref="IBuilderContext"/>.</param>
        /// <param name="instance">The instance to use to execute the method.</param>
        /// <param name="buildKey">The key for the object being built.</param>
		public void Execute(IBuilderContext context,
		                    object instance,
		                    object buildKey)
		{
			List<object> parameterValues = new List<object>();
			foreach (IParameter parameter in Parameters)
				parameterValues.Add(parameter.GetValue(context));

			Method.Invoke(instance, parameterValues.ToArray());
		}
	}
}
