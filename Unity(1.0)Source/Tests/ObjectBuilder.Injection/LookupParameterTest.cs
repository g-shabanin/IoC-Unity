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
	public class LookupParameterTest
	{
		[TestMethod]
		public void ConstructorPolicyCanUseLookupToFindAnObject()
		{
			MockBuilderContext ctx = new MockBuilderContext();
			ctx.Locator.Add("foo", 11);

			LookupParameter param = new LookupParameter("foo");

			Assert.AreEqual<object>(11, param.GetValue(ctx));
			Assert.AreSame(typeof(int), param.GetParameterType(ctx));
		}
	}
}
