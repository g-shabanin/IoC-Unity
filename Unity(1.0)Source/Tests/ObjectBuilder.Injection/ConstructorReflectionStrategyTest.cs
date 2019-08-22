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
	public class ConstructorReflectionStrategyTest
	{
		[TestMethod]
		[ExpectedException(typeof(BuildFailedException))]
		public void MultipleDecoratedConstructors()
		{
			MockBuilderContext context = new MockBuilderContext();
			ConstructorReflectionStrategy<InjectionConstructorAttribute, DependencyAttribute> strategy = 
                new ConstructorReflectionStrategy<InjectionConstructorAttribute, DependencyAttribute>();
		    context.Strategies.Add(strategy);

			context.ExecuteBuildUp(typeof(MultiDecorated), null);
		}

		[TestMethod]
		public void NoDecoratedConstructors()
		{
			MockBuilderContext context = new MockBuilderContext();
            ConstructorReflectionStrategy<InjectionConstructorAttribute, DependencyAttribute> strategy =
                new ConstructorReflectionStrategy<InjectionConstructorAttribute, DependencyAttribute>();
            context.Strategies.Add(strategy);

			context.ExecuteBuildUp(typeof(Undecorated), null);

			ICreationPolicy policy = context.Policies.Get<ICreationPolicy>(typeof(Undecorated));
			Undecorated undecorated = AssertHelper.IsType<Undecorated>(policy.Create(context, typeof(Undecorated)));
			Assert.IsTrue(undecorated.Constructor__Called);
		}

		[TestMethod]
		public void OneDecoratedConstructor()
		{
			MockBuilderContext context = new MockBuilderContext();
            ConstructorReflectionStrategy<InjectionConstructorAttribute, DependencyAttribute> strategy =
                new ConstructorReflectionStrategy<InjectionConstructorAttribute, DependencyAttribute>();
		    context.Strategies.Add(strategy);

			context.ExecuteBuildUp(typeof(Decorated), null);

			ICreationPolicy policy = context.Policies.Get<ICreationPolicy>(typeof(Decorated));
			Decorated decorated = AssertHelper.IsType<Decorated>(policy.Create(context, typeof(Decorated)));
			Assert.IsTrue(decorated.Constructor__Called);
		}

		[TestMethod]
		public void ZeroConstructorsOnReferenceType()
		{
			MockBuilderContext context = new MockBuilderContext();
            ConstructorReflectionStrategy<InjectionConstructorAttribute, DependencyAttribute> strategy =
                new ConstructorReflectionStrategy<InjectionConstructorAttribute, DependencyAttribute>();
            context.Strategies.Add(strategy);

			context.ExecuteBuildUp(typeof(ZeroClass), null);

			ICreationPolicy policy = context.Policies.Get<ICreationPolicy>(typeof(ZeroClass));
			Assert.IsNotNull(policy);
			AssertHelper.IsType<ZeroClass>(policy.Create(context, typeof(ZeroClass)));
		}

		[TestMethod]
		public void ZeroConstructorsOnValueType()
		{
			MockBuilderContext context = new MockBuilderContext();
            ConstructorReflectionStrategy<InjectionConstructorAttribute, DependencyAttribute> strategy =
                new ConstructorReflectionStrategy<InjectionConstructorAttribute, DependencyAttribute>();
            context.Strategies.Add(strategy);

			context.ExecuteBuildUp(typeof(ZeroStruct), null);

			Assert.IsNull(context.Policies.Get<ICreationPolicy>(typeof(ZeroStruct)));
		}

		internal class Decorated
		{
			public bool Constructor__Called;

			[InjectionConstructor]
			public Decorated()
			{
				Constructor__Called = true;
			}

#pragma warning disable 168
			public Decorated(int dummy)
			{
				Assert.IsTrue(false, "Incorrect constructor was called");
			}
#pragma warning restore 168
		}

		internal class MultiDecorated
		{
			[InjectionConstructor]
			public MultiDecorated() {}

#pragma warning disable 168
			[InjectionConstructor]
			public MultiDecorated(int dummy) {}
#pragma warning restore 168
		}

		internal class Undecorated
		{
			public bool Constructor__Called;

			public Undecorated()
			{
				Constructor__Called = true;
			}
		}

		internal class ZeroClass {}

		internal struct ZeroStruct {}
	}
}
