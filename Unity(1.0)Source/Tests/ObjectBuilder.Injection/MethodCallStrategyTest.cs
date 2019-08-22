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
	public class MethodCallStrategyTest
	{
		[TestMethod]
		public void ExecutesMethodsInPolicy()
		{
			Spy.Executed = false;
			MockBuilderContext context = new MockBuilderContext();
			MethodCallStrategy strategy = new MethodCallStrategy();
			MethodCallPolicy policy = new MethodCallPolicy();
			policy.Methods.Add(new ReflectionMethodCallInfo(typeof(Spy).GetMethod("InjectionMethod")));
			context.Policies.Set<IMethodCallPolicy>(policy, typeof(Spy));
		    context.Strategies.Add(strategy);

		    context.ExecuteBuildUp(typeof (Spy), new Spy());

			Assert.IsTrue(Spy.Executed);
		}

		[TestMethod]
		public void NoInstance()
		{
			Spy.Executed = false;
			MockBuilderContext context = new MockBuilderContext();
			MethodCallStrategy strategy = new MethodCallStrategy();
			MethodCallPolicy policy = new MethodCallPolicy();
			policy.Methods.Add(new ReflectionMethodCallInfo(typeof(Spy).GetMethod("InjectionMethod")));
			context.Policies.Set<IMethodCallPolicy>(policy, typeof(Spy));
		    context.Strategies.Add(strategy);

		    context.ExecuteBuildUp(typeof (Spy), null);

			Assert.IsFalse(Spy.Executed);
		}

		internal class Spy
		{
			public static bool Executed;

			public void InjectionMethod()
			{
				Executed = true;
			}
		}
	}
}
