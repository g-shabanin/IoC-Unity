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
    /// Represents a policy for <see cref="SingletonStrategy"/>.
    /// </summary>
	public class SingletonPolicy : ISingletonPolicy
	{
		readonly bool isSingleton;

        /// <summary>
        /// Initialize a new instance of the <see cref="SingletonPolicy"/> class with a 
        /// value determiniting if the the object should be singleton or not.
        /// </summary>
        /// <param name="isSingleton">true if the object should be a singleton; otherwise, false.</param>
		public SingletonPolicy(bool isSingleton)
		{
			this.isSingleton = isSingleton;
		}

        /// <summary>
        /// Determines if the object should be a singleton.
        /// </summary>
        /// <value>
        /// true if the object should be a singleton; otherwise, false.
        /// </value>
		public bool IsSingleton
		{
			get { return isSingleton; }
		}
	}
}
