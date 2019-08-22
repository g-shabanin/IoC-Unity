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

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// Enumeration describing how to handle when a dependency is not present.
    /// </summary>
	public enum NotPresentBehavior
	{
        /// <summary>
        /// Create the object.
        /// </summary>
		Build,

        /// <summary>
        /// Return null. Do not build the object.
        /// </summary>
		Null,

        /// <summary>
        /// Throw a dependency missing exception.
        /// </summary>
		Throw,
	}
}
