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
using System.Configuration;
using System.Globalization;
using System.Xml;
using Microsoft.Practices.Unity.Configuration.Properties;

namespace Microsoft.Practices.Unity.Configuration
{
    /// <summary>
    /// Configuration element for storing the values of method / constructor parameters.
    /// </summary>
    public class MethodParameterElement : TypeResolvingConfigurationElement
    {
        private InjectionParameterValueElement valueElement;

        /// <summary>
        /// Name of this parameter. Doesn't have to match the actual
        /// parameter name in the method or constructor, it's just
        /// required for uniqueness.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Type of this parameter.
        /// </summary>
        [ConfigurationProperty("parameterType", IsRequired = true)]
        public string ParameterTypeName
        {
            get { return (string)this["parameterType"]; }
            set { this["parameterType"] = value; }
        }

        ///<summary>
        ///Gets a value indicating whether an unknown element is encountered during deserialization.
        ///</summary>
        ///
        ///<returns>
        ///true when an unknown element is encountered while deserializing; otherwise, false.
        ///</returns>
        ///
        ///<param name="reader">The <see cref="T:System.Xml.XmlReader"></see> being used for deserialization.</param>
        ///<param name="elementName">The name of the unknown subelement.</param>
        ///<exception cref="T:System.Configuration.ConfigurationErrorsException">The element identified by elementName is locked.- or -One or more of the element's attributes is locked.- or -elementName is unrecognized, or the element has an unrecognized attribute.- or -The element has a Boolean attribute with an invalid value.- or -An attempt was made to deserialize a property more than once.- or -An attempt was made to deserialize a property that is not a valid member of the element.- or -The element cannot contain a CDATA or text element.</exception>
        protected override bool OnDeserializeUnrecognizedElement(
            string elementName, XmlReader reader)
        {
            GuardOnlyOneValue();

            switch(elementName)
            {
            case "value":
                return DeserializeValueElement(reader);

            case "dependency":
                return DeserializeDependencyValueElement(reader);

            default:
                return DeserializePolymorphicElement(elementName, reader);
            }
        }

        private void GuardOnlyOneValue()
        {
            if(valueElement != null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture,
                        Resources.OnlyOneValueElementAllowed,
                        Name));
            }
        }

        private bool DeserializeValueElement(XmlReader reader)
        {
            InstanceValueElement element = new InstanceValueElement();
            element.DeserializeElement(reader);
            valueElement = element;
            return true;
        }

        private bool DeserializeDependencyValueElement(XmlReader reader)
        {
            DependencyValueElement element = new DependencyValueElement();
            element.DeserializeElement(reader);
            valueElement = element;
            return true;
        }

        private bool DeserializePolymorphicElement(string elementName, XmlReader reader)
        {
            string elementTypeName = reader.GetAttribute("elementType");
            if(!string.IsNullOrEmpty(elementTypeName))
            {
                Type elementType = Type.GetType(elementTypeName);
                InjectionParameterValueElement element = 
                    (InjectionParameterValueElement)Activator.CreateInstance(elementType);
                element.DeserializeElement(reader);
                valueElement = element;
                return true;
            }

            return base.OnDeserializeUnrecognizedElement(elementName, reader);
        }

        /// <summary>
        /// Get the <see cref="InjectionParameterValue"/> object for this parameter
        /// to feed to the container configuration API.
        /// </summary>
        /// <returns>The created <see cref="InjectionParameterValue"/> object.</returns>
        public InjectionParameterValue CreateInjectionParameterValue()
        {
            valueElement.TypeResolver = TypeResolver;
            return valueElement.CreateParameterValue(TypeResolver.ResolveType(ParameterTypeName));
        }

    }

}
