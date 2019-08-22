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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.ObjectBuilder2.Tests
{
	[TestClass]
	public class CreationStrategyTest
	{
		static MockBuilderContext CreateContext()
		{
			MockBuilderContext result = new MockBuilderContext();
			result.Strategies.Add(new SingletonStrategy());
			result.Strategies.Add(new CreationStrategy());
			return result;
		}

		[TestMethod]
		public void DoesNotUsePolicyWhenPassedExistingObject()
		{
			object existing = new object();
			MockBuilderContext context = CreateContext();
			StubCreationPolicy policy = new StubCreationPolicy();
			context.Policies.SetDefault<ICreationPolicy>(policy);

			object result = context.ExecuteBuildUp(typeof(object), existing);

			Assert.IsFalse(policy.Create__Called);
			Assert.AreSame(existing, result);
		}

		[TestMethod]
		[ExpectedException(typeof(BuildFailedException))]
		public void NoCreationPolicy()
		{
			MockBuilderContext context = CreateContext();

			context.ExecuteBuildUp(typeof(object), null);
		}

		[TestMethod]
		public void UsesPolicyToCreateObject()
		{
			object obj = new object();
			MockBuilderContext context = CreateContext();
			StubCreationPolicy policy = new StubCreationPolicy();
			policy.Create__Result = obj;
			context.Policies.SetDefault<ICreationPolicy>(policy);

			object result = context.ExecuteBuildUp(typeof(object), null);

			Assert.IsTrue(policy.Create__Called);
			Assert.AreSame(context, policy.Create_Context);
			Assert.AreSame(typeof(object), policy.Create_BuildKey);
			Assert.AreSame(obj, result);
		}

		internal abstract class AbstractClass {}

		internal class Dependent {}

		internal class Depending
		{
			public readonly object ConstructorObject;

			public Depending(Dependent obj)
			{
				ConstructorObject = obj;
			}
		}
	}
}
