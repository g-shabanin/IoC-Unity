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
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.ObjectBuilder2.Tests
{
	[TestClass]
	public class ReflectionStrategyTest
	{
		[TestMethod]
		public void CallsAddParametersToPolicy()
		{
			TestableReflectionStrategy strategy = new TestableReflectionStrategy();
			MockBuilderContext context = new MockBuilderContext();
		    context.Strategies.Add(strategy);
			MethodInfo method = typeof(Dummy).GetMethod("Method1");
			strategy.GetMembers__Result.Add(new MethodMemberInfo<MethodInfo>(method));

		    context.ExecuteBuildUp("foo", null);

			Assert.AreSame(context, strategy.AddParametersToPolicy_Context);
			Assert.AreSame("foo", strategy.AddParametersToPolicy_BuildKey);
			Assert.AreSame(method, strategy.AddParametersToPolicy_Member);
		}

		[TestMethod]
		public void CallsGetMembers()
		{
			TestableReflectionStrategy strategy = new TestableReflectionStrategy();
			MockBuilderContext context = new MockBuilderContext();
		    context.Strategies.Add(strategy);
			object existing = new object();

		    context.ExecuteBuildUp("foo", existing);

			Assert.AreSame(context, strategy.GetMembers_Context);
			Assert.AreSame("foo", strategy.GetMembers_BuildKey);
			Assert.AreSame(existing, strategy.GetMembers_Existing);
		}

		[TestMethod]
		public void CallsMemberRequiresProcessing()
		{
			TestableReflectionStrategy strategy = new TestableReflectionStrategy();
			MockBuilderContext context = new MockBuilderContext();
		    context.Strategies.Add(strategy);
			MethodInfo method1 = typeof(Dummy).GetMethod("Method1");
			MethodInfo method2 = typeof(Dummy).GetMethod("Method2");
			strategy.GetMembers__Result.Add(new MethodMemberInfo<MethodInfo>(method1));
			strategy.GetMembers__Result.Add(new MethodMemberInfo<MethodInfo>(method2));

		    context.ExecuteBuildUp("foo", null);

			AssertHelper.Contains(method1, strategy.MemberRequiresProcessing_Members);
			AssertHelper.Contains(method2, strategy.MemberRequiresProcessing_Members);
		}

		[TestMethod]
		public void DefaultParameterBehaviorIsBuildDependencyByType()
		{
			TestableReflectionStrategy strategy = new TestableReflectionStrategy();
			MockBuilderContext context = new MockBuilderContext();
            context.Strategies.Add(strategy);
			MethodInfo method = typeof(Dummy).GetMethod("Method1");
			strategy.GetMembers__Result.Add(new MethodMemberInfo<MethodInfo>(method));

		    context.ExecuteBuildUp("foo", null);

			IParameter parameter = strategy.AddParametersToPolicy_Parameters[0];
			DependencyParameter dependency = AssertHelper.IsType<DependencyParameter>(parameter);
			Assert.AreEqual<object>(typeof(int), dependency.BuildKey);
			Assert.AreEqual(NotPresentBehavior.Build, dependency.NotPresentBehavior);
		}

		[TestMethod]
		[ExpectedException(typeof(BuildFailedException))]
		public void MultipleAttributesNotAllowed()
		{
			TestableReflectionStrategy strategy = new TestableReflectionStrategy();
			MockBuilderContext context = new MockBuilderContext();
            context.Strategies.Add(strategy);
			MethodInfo method = typeof(Dummy).GetMethod("Method3");
			strategy.GetMembers__Result.Add(new MethodMemberInfo<MethodInfo>(method));

		    context.ExecuteBuildUp("foo", null);
		}

		[TestMethod]
		public void UsesAttributeInforationWhenPresent()
		{
			TestableReflectionStrategy strategy = new TestableReflectionStrategy();
			MockBuilderContext context = new MockBuilderContext();
		    context.Strategies.Add(strategy);
			MethodInfo method = typeof(Dummy).GetMethod("Method2");
			strategy.GetMembers__Result.Add(new MethodMemberInfo<MethodInfo>(method));

		    context.ExecuteBuildUp("foo", null);

			IParameter parameter = strategy.AddParametersToPolicy_Parameters[0];
			DependencyParameter dependency = AssertHelper.IsType<DependencyParameter>(parameter);
			Assert.AreEqual<object>("bar", dependency.BuildKey);
			Assert.AreEqual(NotPresentBehavior.Throw, dependency.NotPresentBehavior);
		}

		internal class Dummy
		{
			public void Method1(int x) {}

			public void Method2([Dependency("bar", NotPresentBehavior = NotPresentBehavior.Throw)] int x) {}

			public void Method3([Dependency] [DummyParameter] int x) {}
		}

		internal class DummyParameterAttribute : ParameterAttribute
		{
			public override IParameter CreateParameter(Type annotatedMemberType)
			{
				return new ValueParameter<string>("baz");
			}
		}

		internal class TestableReflectionStrategy : ReflectionStrategy<MethodInfo, DependencyAttribute>
		{
			public object AddParametersToPolicy_BuildKey;
			public IBuilderContext AddParametersToPolicy_Context;
			public MethodInfo AddParametersToPolicy_Member;
			public List<IParameter> AddParametersToPolicy_Parameters = new List<IParameter>();
			public List<IMemberInfo<MethodInfo>> GetMembers__Result = new List<IMemberInfo<MethodInfo>>();
			public object GetMembers_BuildKey;
			public IBuilderContext GetMembers_Context;
			public object GetMembers_Existing;
			public List<MethodInfo> MemberRequiresProcessing_Members = new List<MethodInfo>();

			protected override void AddParametersToPolicy(IBuilderContext context,
			                                              IMemberInfo<MethodInfo> member,
			                                              IEnumerable<IParameter> parameters)
			{
				AddParametersToPolicy_Context = context;
				AddParametersToPolicy_BuildKey = context.BuildKey;
				AddParametersToPolicy_Member = member.MemberInfo;
				AddParametersToPolicy_Parameters.AddRange(parameters);
			}

			protected override IEnumerable<IMemberInfo<MethodInfo>> GetMembers(IBuilderContext context)
			{
				GetMembers_Context = context;
				GetMembers_BuildKey = context.BuildKey;
				GetMembers_Existing = context.Existing;

				return GetMembers__Result;
			}

			protected override bool MemberRequiresProcessing(IMemberInfo<MethodInfo> member)
			{
				MemberRequiresProcessing_Members.Add(member.MemberInfo);

				return true;
			}
		}
	}
}
