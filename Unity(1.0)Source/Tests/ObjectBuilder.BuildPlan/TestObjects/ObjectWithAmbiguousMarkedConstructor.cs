﻿//===============================================================================
// Microsoft patterns & practices
// Unity Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace Microsoft.Practices.ObjectBuilder2.Tests.TestObjects
{
    class ObjectWithAmbiguousMarkedConstructor
    {
        public ObjectWithAmbiguousMarkedConstructor()
        {
        }

        public ObjectWithAmbiguousMarkedConstructor(int first, string second, float third)
        {
        }

        [InjectionConstructor]
        public ObjectWithAmbiguousMarkedConstructor(string first, string second, int third)
        {
            
        }    
    }
}
