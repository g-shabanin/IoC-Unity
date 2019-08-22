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
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// A creation policy where the constructor to choose is derived from the parameters
    /// provided by the user.
    /// </summary>
	public class ConstructorCreationPolicy : ICreationPolicy
	{
		readonly ConstructorInfo constructor;
		readonly List<IParameter> parameters;

        /// <summary>
        /// Initalize a new instance of the <see cref="ActivatorCreationPolicy"/> class with a <see cref="ConstructorInfo"/> and an array of <see cref="IParameter"/> instances.
        /// </summary>
        /// <param name="constructor">The <see cref="ConstructorInfo"/> to use to create the object.</param>
        /// <param name="parameters">An array of <see cref="IParameter"/> instances.</param>
		public ConstructorCreationPolicy(ConstructorInfo constructor,
		                                 params IParameter[] parameters)
			: this(constructor, (IEnumerable<IParameter>)parameters) {}

        /// <summary>
        /// Initalize a new instance of the <see cref="ActivatorCreationPolicy"/> class with a <see cref="ConstructorInfo"/> and a collection of <see cref="IParameter"/> instances.
        /// </summary>
        /// <param name="constructor">The <see cref="ConstructorInfo"/> to use to create the object.</param>
        /// <param name="parameters">A collection of <see cref="IParameter"/> instances.</param>
		public ConstructorCreationPolicy(ConstructorInfo constructor,
		                                 IEnumerable<IParameter> parameters)
		{
			Guard.ArgumentNotNull(constructor, "constructor");

			this.constructor = constructor;
			this.parameters = new List<IParameter>();

			if (parameters != null)
				this.parameters.AddRange(parameters);
		}

        /// <summary>
        /// Determines if the policy supports reflection.
        /// </summary>
        /// <value>
        /// Returns true.
        /// </value>
		public bool SupportsReflection
		{
			get { return true; }
		}

        /// <summary>
        /// Create the object for the given <paramref name="context"/> and <paramref name="buildKey"/>.
        /// </summary>
        /// <param name="context">The builder context.</param>
        /// <param name="buildKey">The key for the object being built.</param>
        /// <returns>The created object.</returns>
		public object Create(IBuilderContext context,
		                     object buildKey)
		{
			Guard.ArgumentNotNull(context, "context");

			return constructor.Invoke(GetParameters(context, constructor));
		}



        /// <summary>
        /// Gets the constructor to be used to create the object.
        /// </summary>
        /// <param name="context">The builder context.</param>
        /// <param name="buildKey">The key for the object being built.</param>
        /// <returns>The constructor to use; returns null if no suitable constructor can be found.</returns>
		public ConstructorInfo GetConstructor(IBuilderContext context,
		                                      object buildKey)
		{
			return constructor;
		}

        /// <summary>
        /// Gets the parameter values to be passed to the constructor.
        /// </summary>
        /// <param name="context">The builder context.</param>
        /// <param name="constructor">The constructor that will be used.</param>
        /// <returns>An array of parameters to pass to the constructor.</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames", MessageId = "constructor",
            Justification="constructor argument is from interface, isn't actually used here")]
        public object[] GetParameters(IBuilderContext context,
                                      ConstructorInfo constructor)
		{
			object[] result = new object[parameters.Count];

			for (int idx = 0; idx < parameters.Count; idx++)
				result[idx] = parameters[idx].GetValue(context);

			return result;
		}
	}
}
