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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Practices.ObjectBuilder2.Properties;

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// Dependency resolver for resolving dependencies of objects being built.
    /// </summary>
    public static class DependencyResolver
    {
        /// <summary>
        /// Resolves a dependency.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> of the build operation.</param>
        /// <param name="buildKey">The key for the object being built.</param>
        /// <param name="behavior">Describes how to behave if the dependency is not found.</param>
        /// <returns>The dependent object. If the object is not found, and notPresent
        /// is set to <see cref="NotPresentBehavior.Null"/>, will return null.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
            Justification="Validation is done by the Guard class.")]
        public static object Resolve(IBuilderContext context,
                                     object buildKey,
                                     NotPresentBehavior behavior)
        {
            Guard.ArgumentNotNull(context, "context");
            Guard.ArgumentNotNull(buildKey, "buildKey");

            if (context.Locator.Contains(buildKey))
                return context.Locator.Get(buildKey);

            switch (behavior)
            {
                case NotPresentBehavior.Build:
                    IBuilderContext recursiveContext = context.CloneForNewBuild(buildKey, null);
                    return recursiveContext.Strategies.ExecuteBuildUp(recursiveContext);

                case NotPresentBehavior.Null:
                    return null;

                case NotPresentBehavior.Throw:
                    throw new DependencyMissingException(buildKey);

                default:
                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture,
                        Resources.UnknownNotPresentBehavior,
                        behavior));
            }
        }
    }
}
