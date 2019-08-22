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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.ObjectBuilder2.Tests
{
	[TestClass]
	public class DependencyResolverTest
	{
		[TestMethod]
		public void CanBuildObjectWhenNotPresent()
		{
			MockBuilderContext context = CreateContext();

			object result = DependencyResolver.Resolve(context, typeof(MockObject), NotPresentBehavior.Build);

			Assert.IsNotNull(result);
			AssertHelper.IsType<MockObject>(result);
		}

		[TestMethod]
		public void CanReturnNullWhenNotPresent()
		{
			MockBuilderContext context = CreateContext();

			object result = DependencyResolver.Resolve(context, typeof(object), NotPresentBehavior.Null);

			Assert.IsNull(result);
		}

		[TestMethod]
		[ExpectedException(typeof(DependencyMissingException))]
		public void CanThrowWhenNotPresent()
		{
			MockBuilderContext context = CreateContext();

			DependencyResolver.Resolve(context, typeof(object), NotPresentBehavior.Throw);
		}

		static MockBuilderContext CreateContext()
		{
			MockBuilderContext result = new MockBuilderContext();
			result.Strategies.Add(new SingletonStrategy());
			result.Strategies.Add(new CreationStrategy());
			result.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			result.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			return result;
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullBuildKey()
		{
			MockBuilderContext context = new MockBuilderContext();

			DependencyResolver.Resolve(context, null, NotPresentBehavior.Null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullContext()
		{
			DependencyResolver.Resolve(null, "foo", NotPresentBehavior.Null);
		}

		[TestMethod]
		public void ReturnsSingletonInstanceWhenPresent()
		{
			MockBuilderContext context = new MockBuilderContext();
			object obj = new object();
			context.Locator.Add("foo", obj);

			Assert.AreSame(obj, DependencyResolver.Resolve(context, "foo", NotPresentBehavior.Null));
		}

		interface IMockObject {}

		class MockObject : IMockObject {}
	}
}
