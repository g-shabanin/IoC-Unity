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
	public class DependencyAttributeTest
	{
		[TestMethod]
		public void DefaultBuildKeyIsAnnotatedMemberType()
		{
			DependencyAttribute attribute = new DependencyAttribute();

			IParameter result = attribute.CreateParameter(typeof(object));

			DependencyParameter parameter = AssertHelper.IsType<DependencyParameter>(result);
			Assert.AreEqual<object>(typeof(object), parameter.BuildKey);
		}

		[TestMethod]
		public void DefaultNotPresentBehaviorIsBuild()
		{
			DependencyAttribute attribute = new DependencyAttribute("Foo");

			IParameter result = attribute.CreateParameter(typeof(object));

			DependencyParameter parameter = AssertHelper.IsType<DependencyParameter>(result);
			Assert.AreEqual(NotPresentBehavior.Build, parameter.NotPresentBehavior);
		}

		[TestMethod]
		public void ReturnsDependencyParameter()
		{
			DependencyAttribute attribute = new DependencyAttribute("Foo");
			attribute.NotPresentBehavior = NotPresentBehavior.Throw;

			IParameter result = attribute.CreateParameter(typeof(object));

			DependencyParameter parameter = AssertHelper.IsType<DependencyParameter>(result);
			Assert.AreEqual<object>("Foo", parameter.BuildKey);
			Assert.AreEqual(NotPresentBehavior.Throw, parameter.NotPresentBehavior);
		}
	}
}
