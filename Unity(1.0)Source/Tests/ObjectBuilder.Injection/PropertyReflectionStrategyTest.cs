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
	public class PropertyReflectionStrategyTest
	{
		[TestMethod]
		public void DecoratedProperty()
		{
			MockBuilderContext context = new MockBuilderContext();
			PropertyReflectionStrategy<DependencyAttribute> strategy = 
                new PropertyReflectionStrategy<DependencyAttribute>();
		    context.Strategies.Add(strategy);

		    context.ExecuteBuildUp(typeof (Decorated), null);

			IPropertySetterPolicy policy = context.Policies.Get<IPropertySetterPolicy>(typeof(Decorated));
			Assert.IsNotNull(policy);
			AssertHelper.NotEmpty(policy.Properties);
			foreach (ReflectionPropertySetterInfo property in policy.Properties)
				Assert.AreEqual(typeof(Decorated).GetProperty("Property"), property.Property);
		}

		[TestMethod]
		public void NoDecoratedProperties()
		{
			MockBuilderContext context = new MockBuilderContext();
			PropertyReflectionStrategy<DependencyAttribute> strategy = 
                new PropertyReflectionStrategy<DependencyAttribute>();
		    context.Strategies.Add(strategy);

		    context.ExecuteBuildUp(typeof (object), null);

			IPropertySetterPolicy policy = context.Policies.Get<IPropertySetterPolicy>(typeof(object));
			Assert.IsNull(policy);
		}

	    [TestMethod]
	    public void IndexersAreIgnored()
	    {
	        MockBuilderContext context = new MockBuilderContext();
            PropertyReflectionStrategy<DependencyAttribute> strategy =
                new PropertyReflectionStrategy<DependencyAttribute>();
	        context.Strategies.Add(strategy);

	        context.ExecuteBuildUp(typeof (DecoratedIndexer), null);

	        IPropertySetterPolicy policy =
	            context.Policies.Get<IPropertySetterPolicy>(typeof(DecoratedIndexer));
	        Assert.IsNull(policy);
	    }


		internal class Decorated
		{
			[Dependency]
			public object Property
			{
				set { Console.WriteLine(value); }
			}

			public object UndecoratedProperty
			{
				set { Console.WriteLine(value); }
			}
		}

        internal class DecoratedIndexer
        {
            [Dependency]
            public object this[int index]
            {
                get { return null; }
                set { }
            }
        }
	}
}
