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
	public class PropertySetterStrategyTest
	{
		[TestMethod]
		public void NoInstance()
		{
			Spy.PropertyValue = null;
			object obj = new object();
			MockBuilderContext context = new MockBuilderContext();
			context.Strategies.Add(new PropertySetterStrategy());
			PropertySetterPolicy policy = new PropertySetterPolicy();
			policy.Properties.Add(new ReflectionPropertySetterInfo(typeof(Spy).GetProperty("Property"), new ValueParameter<object>(obj)));
			context.Policies.Set<IPropertySetterPolicy>(policy, typeof(Spy));

			context.ExecuteBuildUp(typeof(Spy), null);

			Assert.IsNull(Spy.PropertyValue);
		}

		[TestMethod]
		public void SetsPropertyInPolicy()
		{
			Spy.PropertyValue = null;
			object obj = new object();
			MockBuilderContext context = new MockBuilderContext();
			context.Strategies.Add(new PropertySetterStrategy());
			PropertySetterPolicy policy = new PropertySetterPolicy();
			policy.Properties.Add(new ReflectionPropertySetterInfo(typeof(Spy).GetProperty("Property"), new ValueParameter<object>(obj)));
			context.Policies.Set<IPropertySetterPolicy>(policy, typeof(Spy));

			context.ExecuteBuildUp(typeof(Spy), new Spy());

			Assert.AreSame(obj, Spy.PropertyValue);
		}

		internal class Spy
		{
			public static object PropertyValue;

			public object Property
			{
				set { PropertyValue = value; }
			}
		}
	}
}
