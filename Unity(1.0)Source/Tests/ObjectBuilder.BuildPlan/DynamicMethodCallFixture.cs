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
using Microsoft.Practices.ObjectBuilder2.Tests.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.ObjectBuilder2.Tests
{
    [TestClass]
    public class DynamicMethodCallFixture
    {
        [TestMethod]
        public void CallsMethodsMarkedWithInjectionAttribute()
        {
            TestingBuilderContext context = GetContext();
            SetSingleton(context, typeof(int));
            SetSingleton(context, typeof(string));
            object intValue = 42;
            object stringValue = "Hello world";

            context.Locator.Add(typeof(int), intValue);
            context.Locator.Add(typeof(string), stringValue);

            IBuildPlanPolicy plan =
                GetPlanCreator(context).CreatePlan(context, typeof(ObjectWithInjectionMethod));

            ObjectWithInjectionMethod existing = new ObjectWithInjectionMethod();

            context.BuildKey = typeof(ObjectWithInjectionMethod);
            context.Existing = existing;
            plan.BuildUp(context);

            GC.KeepAlive(intValue);
            GC.KeepAlive(stringValue);

            Assert.IsTrue(existing.WasInjected);
            Assert.AreEqual(intValue, existing.IntValue);
            Assert.AreEqual(stringValue, existing.StringValue);
        }

        [TestMethod]
        public void ThrowsWhenBuildingPlanWithGenericInjectionMethod()
        {
            bool handled = false;
            try
            {
                TestingBuilderContext context = GetContext();
                IBuildPlanPolicy plan =
                    GetPlanCreator(context).CreatePlan(context, typeof(ObjectWithGenericInjectionMethod));
            }
            catch(BuildFailedException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(IllegalInjectionMethodException));
                handled = true;
            }
            if(!handled)
            {
                Assert.Fail("Should have gotten an exception here");
            }
        }

        [TestMethod]
        public void ThrowsWhenBuildingPlanWithMethodWithOutParam()
        {
            bool handled = false;
            try
            {
                TestingBuilderContext context = GetContext();
                IBuildPlanPolicy plan =
                    GetPlanCreator(context).CreatePlan(context, typeof(ObjectWithOutParamMethod));
            }
            catch(BuildFailedException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(IllegalInjectionMethodException));
                handled = true;
            }
            if(!handled)
            {
                Assert.Fail("Should have gotten an exception here");
            }            
        }

        [TestMethod]
        public void ThrowsWhenBuildingPlanWithMethodWithRefParam()
        {
            bool handled = false;
            try
            {
                TestingBuilderContext context = GetContext();
                IBuildPlanPolicy plan =
                    GetPlanCreator(context).CreatePlan(context, typeof(ObjectWithRefParamMethod));
            }
            catch (BuildFailedException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(IllegalInjectionMethodException));
                handled = true;
            }
            if (!handled)
            {
                Assert.Fail("Should have gotten an exception here");
            }
        }

        private TestingBuilderContext GetContext()
        {
            StagedStrategyChain<BuilderStage> chain = new StagedStrategyChain<BuilderStage>();
            chain.AddNew<DynamicMethodCallStrategy>(BuilderStage.Initialization);

            DynamicMethodBuildPlanCreatorPolicy policy =
                new DynamicMethodBuildPlanCreatorPolicy(chain);

            TestingBuilderContext context = new TestingBuilderContext();

            context.Strategies.Add(new SingletonStrategy());

            context.PersistentPolicies.SetDefault<IConstructorSelectorPolicy>(
                new ConstructorSelectorPolicy<InjectionConstructorAttribute>());
            context.PersistentPolicies.SetDefault<IPropertySelectorPolicy>(
                new PropertySelectorPolicy<DependencyAttribute>());
            context.PersistentPolicies.SetDefault<IMethodSelectorPolicy>(
                new MethodSelectorPolicy<InjectionMethodAttribute>());

            context.PersistentPolicies.SetDefault<IBuildPlanCreatorPolicy>(policy);

            return context;
        }

        private IBuildPlanCreatorPolicy GetPlanCreator(IBuilderContext context)
        {
            return context.Policies.Get<IBuildPlanCreatorPolicy>(null);
        }

        private void SetSingleton(IBuilderContext context, Type t)
        {
            context.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), t);
        }

        private class ObjectWithInjectionMethod
        {
            private int intValue;
            private string stringValue;
            private bool wasInjected = false;

            [InjectionMethod]
            public void DoSomething(int i, string s)
            {
                intValue = i;
                stringValue = s;
                wasInjected = true;
            }

            public int IntValue
            {
                get { return intValue; }
            }

            public string StringValue
            {
                get { return stringValue; }
            }

            public bool WasInjected
            {
                get { return wasInjected; }
            }
        }

        private class ObjectWithGenericInjectionMethod
        {
            [InjectionMethod]
            public void DoSomethingGeneric<T>(T input)
            {
                
            }
        }

        private class ObjectWithOutParamMethod
        {
            [InjectionMethod]
            public void DoSomething(out int result)
            {
                result = 42;
            }
        }

        private class ObjectWithRefParamMethod
        {
            [InjectionMethod]
            public void DoSomething(ref int result)
            {
                result *= 2;
            }
        }
    }
}
