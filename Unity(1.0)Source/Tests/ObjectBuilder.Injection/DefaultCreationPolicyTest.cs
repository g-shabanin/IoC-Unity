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
	public class DefaultCreationPolicyTest
	{
		[TestMethod]
		public void ConstructorWithValueType()
		{
			MockBuilderContext ctx = CreateContext();

			CtorValueTypeObject result = (CtorValueTypeObject)ctx.ExecuteBuildUp(typeof(CtorValueTypeObject), null);

			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.IntValue);
		}

		static MockBuilderContext CreateContext()
		{
			MockBuilderContext result = new MockBuilderContext();
			result.Strategies.Add(new CreationStrategy());
			result.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			return result;
		}

		[TestMethod]
		public void DependencyChainIsFollowed()
		{
			MockBuilderContext context = CreateContext();

			CascadingCtor result = (CascadingCtor)context.ExecuteBuildUp(typeof(CascadingCtor), null);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.CtorObject);
			Assert.IsNotNull(result.CtorObject.Foo);
		}

		[TestMethod]
		public void MultiParameterConstructor()
		{
			MockBuilderContext context = CreateContext();

			MultiParamCtor result = (MultiParamCtor)context.ExecuteBuildUp(typeof(MultiParamCtor), null);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.O1);
			Assert.IsNotNull(result.O2);
		}

		[TestMethod]
		public void ParameterizedConstructor()
		{
			MockBuilderContext context = CreateContext();

			ParameterizedCtor result = (ParameterizedCtor)context.ExecuteBuildUp(typeof(ParameterizedCtor), null);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Foo);
		}

		[TestMethod]
		public void ParameterlessConstructor()
		{
			MockBuilderContext context = CreateContext();

			object result = context.ExecuteBuildUp(typeof(object), null);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void ValueTypeCanBeConstructed()
		{
			MockBuilderContext context = CreateContext();

			int result = (int)context.ExecuteBuildUp(typeof(int), null);

			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void ValueTypeWithConstructor()
		{
			MockBuilderContext context = CreateContext();

			ValueTypeWithCtor result = (ValueTypeWithCtor)context.ExecuteBuildUp(typeof(ValueTypeWithCtor), null);

			Assert.IsNotNull(result.ObjectValue);
		}

	    [TestMethod]
	    public void WorksWithNonTypeBuildKey()
	    {
	        MockBuilderContext context = CreateContext();
	        object result =
	            context.ExecuteBuildUp(new NamedTypeBuildKey(typeof (object), "happy happy"), null);
            Assert.IsNotNull(result);
	    }


		internal struct ValueTypeWithCtor
		{
			public readonly object ObjectValue;

			public ValueTypeWithCtor(object o)
			{
				ObjectValue = o;
			}
		}

		class CascadingCtor
		{
			public readonly ParameterizedCtor CtorObject;

			public CascadingCtor(ParameterizedCtor ctorObject)
			{
				CtorObject = ctorObject;
			}
		}

		class CtorValueTypeObject
		{
			public readonly int IntValue;

			public CtorValueTypeObject(int i)
			{
				IntValue = i;
			}
		}

		class MultiParamCtor
		{
			public readonly object O1;
			public readonly object O2;

			public MultiParamCtor(object o1,
			                      object o2)
			{
				O1 = o1;
				O2 = o2;
			}
		}

		class ParameterizedCtor
		{
			public readonly object Foo;

			public ParameterizedCtor(object foo)
			{
				Foo = foo;
			}
		}
	}
}
