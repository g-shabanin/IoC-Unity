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

using Microsoft.Practices.ObjectBuilder2.Tests.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.ObjectBuilder2.Tests
{
    [TestClass]
    public class DynamicMethodPropertySetterFixture
    {
        [TestMethod]
        public void CanInjectProperties()
        {
            TestingBuilderContext context = GetContext();
            object existingObject = new object();
            context.Locator.Add(typeof(object), existingObject);
            context.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), typeof(object));

            IBuildPlanPolicy plan =
                GetPlanCreator(context).CreatePlan(context, typeof(OnePropertyClass));

            OnePropertyClass existing = new OnePropertyClass();
            context.Existing = existing;
            context.BuildKey = typeof(OnePropertyClass);
            plan.BuildUp(context);

            Assert.IsNotNull(existing.Key);
            Assert.AreSame(existingObject, existing.Key);
        }

        private TestingBuilderContext GetContext()
        {
            StagedStrategyChain<BuilderStage> chain = new StagedStrategyChain<BuilderStage>();
            chain.AddNew<DynamicMethodPropertySetterStrategy>(BuilderStage.Initialization);

            DynamicMethodBuildPlanCreatorPolicy policy =
                new DynamicMethodBuildPlanCreatorPolicy(chain);

            TestingBuilderContext context = new TestingBuilderContext();

            context.Strategies.Add(new SingletonStrategy());

            context.Policies.SetDefault<IConstructorSelectorPolicy>(
                new ConstructorSelectorPolicy<InjectionConstructorAttribute>());
            context.Policies.SetDefault<IPropertySelectorPolicy>(
                new PropertySelectorPolicy<DependencyAttribute>());
            context.Policies.SetDefault<IBuildPlanCreatorPolicy>(policy);

            return context;
        }

        private IBuildPlanCreatorPolicy GetPlanCreator(IBuilderContext context)
        {
            return context.Policies.Get<IBuildPlanCreatorPolicy>(null);
        }


        private class OnePropertyClass
        {
            private object key;

            [Dependency]
            public object Key
            {
                get { return key; }
                set { key = value; }
            }
        }
    }
}
