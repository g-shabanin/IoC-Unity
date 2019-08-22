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
    public class LifetimeStrategyTest
    {
        [TestMethod]
        public void LifetimeStrategyDefaultsToTransient()
        {
            MockBuilderContext context = CreateContext();
            object result = context.ExecuteBuildUp(typeof(object), null);
            object result2 = context.ExecuteBuildUp(typeof(object), null);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreNotSame(result, result2);
        }

        [TestMethod]
        public void SingletonPolicyAffectsLifetime()
        {
            MockBuilderContext context = CreateContext();
            context.Policies.Set<ILifetimePolicy>(new SingletonLifetimePolicy(), typeof(object));

            object result = context.ExecuteBuildUp(typeof(object), null);
            object result2 = context.ExecuteBuildUp(typeof(object), null);
            Assert.IsNotNull(result);
            Assert.AreSame(result, result2);
        }

        [TestMethod]
        public void SettingDefaultPolicyToSingletonMakesEverythingSingleton()
        {
            MockBuilderContext context = CreateContext();
            context.Policies.SetDefault<ILifetimePolicy>(new SingletonLifetimePolicy());

            object result = context.ExecuteBuildUp(typeof(object), null);
            object result2 = context.ExecuteBuildUp(typeof(object), null);

            Assert.IsNotNull(result);
            Assert.AreSame(result, result2);
        }

        [TestMethod]
        public void LifetimeStrategyAddsRecoveriesToContext()
        {
            MockBuilderContext context = CreateContext();
            RecoverableLifetime recovery = new RecoverableLifetime();
            context.PersistentPolicies.Set<ILifetimePolicy>(recovery, typeof(object));

            context.ExecuteBuildUp(typeof(object), null);

            Assert.AreEqual(1, context.RecoveryStack.Count);

            context.RecoveryStack.ExecuteRecovery();
            Assert.IsTrue(recovery.WasRecovered);
        }

        private MockBuilderContext CreateContext()
        {
            MockBuilderContext context = new MockBuilderContext();
            context.Strategies.Add(new LifetimeStrategy());
            context.Strategies.Add(new CreationStrategy());

            context.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
            return context;
        }

        private class RecoverableLifetime : ILifetimePolicy, IRequiresRecovery
        {
            public bool WasRecovered = false;

            public object GetValue()
            {
                return null;
            }

            public void SetValue(object newValue)
            {
            }

            public void RemoveValue()
            {
            }

            public void Recover()
            {
                WasRecovered = true;
            }
        }
    }
}
