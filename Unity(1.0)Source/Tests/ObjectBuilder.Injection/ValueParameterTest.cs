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
	public class ValueParameterTest
	{
		[TestMethod]
		public void ValueParameterReturnsStoredTypeAndValue()
		{
			ValueParameter<int> x = new ValueParameter<int>(12);

			Assert.AreEqual(typeof(int), x.GetParameterType(null));
			Assert.AreEqual<object>(12, x.GetValue(null));
		}
	}
}
