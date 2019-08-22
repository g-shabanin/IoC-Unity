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
	public class MethodReflectionStrategyTest
	{
		[TestMethod]
		public void DecoratedMethod()
		{
			MockBuilderContext context = new MockBuilderContext();
			context.Strategies.Add( 
                new MethodReflectionStrategy<InjectionMethodAttribute, DependencyAttribute>());

			context.ExecuteBuildUp(typeof(Decorated), null);

			IMethodCallPolicy policy = context.Policies.Get<IMethodCallPolicy>(typeof(Decorated));
			Assert.IsNotNull(policy);
			AssertHelper.NotEmpty(policy.Methods);
			foreach (ReflectionMethodCallInfo method in policy.Methods)
				Assert.AreEqual(typeof(Decorated).GetMethod("Method"), method.Method);
		}

		[TestMethod]
		public void NoDecoratedMethods()
		{
			MockBuilderContext context = new MockBuilderContext();
            context.Strategies.Add(
                new MethodReflectionStrategy<InjectionMethodAttribute, DependencyAttribute>());

			context.ExecuteBuildUp(typeof(object), null);

			IMethodCallPolicy policy = context.Policies.Get<IMethodCallPolicy>(typeof(object));
			Assert.IsNull(policy);
		}

		internal class Decorated
		{
			[InjectionMethod]
			public void Method() {}

			public void UndecoratedMethod() {}
		}
	}
}
