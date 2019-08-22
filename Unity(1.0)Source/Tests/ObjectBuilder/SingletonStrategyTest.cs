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
	public class SingletonStrategyTest
	{
		static MockBuilderContext BuildContext()
		{
			MockBuilderContext ctx = new MockBuilderContext();

			ctx.Strategies.Add(new SingletonStrategy());
			ctx.Strategies.Add(new SimpleCreationStrategy());

			return ctx;
		}

		static MockBuilderContext BuildContext(IReadWriteLocator locator)
		{
			MockBuilderContext ctx = new MockBuilderContext(locator);

			ctx.Strategies.Add(new SingletonStrategy());
			ctx.Strategies.Add(new SimpleCreationStrategy());

			return ctx;
		}

		[TestMethod]
		public void BuildingASingletonTwiceReturnsSameInstance()
		{
			MockBuilderContext ctx = BuildContext();
		    ctx.BuildKey = typeof (object);
			ctx.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), typeof(object));

			object i1 = ctx.Strategies.ExecuteBuildUp(ctx);
		    ctx.Existing = null;
		    ctx.BuildComplete = false;
			object i2 = ctx.Strategies.ExecuteBuildUp(ctx);

			Assert.AreSame(i1, i2);
		}

		[TestMethod]
		public void ChildLocatorBeforeParent()
		{
			Locator parentLocator = new Locator();
			Locator childLocator = new Locator(parentLocator);
			MockBuilderContext ctx = BuildContext(childLocator);
		    ctx.BuildKey = typeof (string);
			parentLocator.Add(typeof(string), "Hello world");
			childLocator.Add(typeof(string), "Goodbye world");

		    string result = (string) ctx.Strategies.ExecuteBuildUp(ctx);

			Assert.AreEqual("Goodbye world", result);
		}

		[TestMethod]
		public void SearchesParentLocator()
		{
			Locator parentLocator = new Locator();
			Locator childLocator = new Locator(parentLocator);
			MockBuilderContext ctx = BuildContext(childLocator);
		    ctx.BuildKey = typeof (string);
			parentLocator.Add(typeof(string), "Hello world");

			string result = (string)ctx.Strategies.ExecuteBuildUp(ctx);

			Assert.AreEqual("Hello world", result);
		}

		class SimpleCreationStrategy : BuilderStrategy
		{
			public override void PreBuildUp(IBuilderContext context)
			{
				Type type;

				if (context.Existing == null && BuildKey.TryGetType(context.BuildKey, out type))
					context.Existing = Activator.CreateInstance(type);
			}
		}
	}
}
