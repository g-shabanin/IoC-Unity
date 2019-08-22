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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.ObjectBuilder2.Tests
{
	[TestClass]
	public class ActivatorCreationPolicyTest
	{
		[TestMethod]
		public void CreatesObjectAndPassesValue()
		{
			MockBuilderContext context = new MockBuilderContext();
			ActivatorCreationPolicy policy = new ActivatorCreationPolicy(Params(42));

			Dummy result = (Dummy)policy.Create(context, typeof(Dummy));

			Assert.IsNotNull(result);
			Assert.AreEqual(42, result.val);
		}

		[TestMethod]
		[ExpectedException(typeof(MissingMethodException))]
		public void NoMatchingConstructor()
		{
			MockBuilderContext context = new MockBuilderContext();
			ActivatorCreationPolicy policy = new ActivatorCreationPolicy(Params("foo"));

			policy.Create(context, typeof(Dummy));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullContext()
		{
			ActivatorCreationPolicy policy = new ActivatorCreationPolicy();

			policy.Create(null, typeof(Dummy));
		}

		static IEnumerable<IParameter> Params(params object[] parameters)
		{
			foreach (object parameter in parameters)
				yield return new ValueParameter(parameter.GetType(), parameter);
		}

		internal class Dummy
		{
			public readonly int val;

			public Dummy(int val)
			{
				this.val = val;
			}
		}
	}
}
