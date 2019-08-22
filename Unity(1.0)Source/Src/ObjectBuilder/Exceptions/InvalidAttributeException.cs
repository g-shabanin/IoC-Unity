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
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Practices.ObjectBuilder2
{
    /// <summary>
    /// Represents an invalid combination of dependency injection attributes were used.
    /// </summary>
    [Serializable]
    public class InvalidAttributeException : Exception
    {
        private Type type;
        private string memberName;

        /// <summary>
        /// Initialize a new instance of the <see cref="InvalidAttributeException"/> class with the <see cref="Type"/> and member name.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of object with the invalid attribute.</param>
        /// <param name="memberName">The member name of <paramref name="type"/> with the invalid attribute.</param>
        public InvalidAttributeException(Type type,
                                         string memberName)
        {
            this.type = type;
            this.memberName = memberName;
        }

        /// <summary>
        /// Create a new instance of the <see cref="InvalidAttributeException"/> class with uninitialized values.
        /// </summary>
        public InvalidAttributeException()
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="InvalidAttributeException"/> class with a string message.
        /// </summary>
        /// <remarks>The string passed here is ignored, use the 
        /// <see cref="InvalidAttributeException(System.Type, System.String)"/> constructor instead.
        /// </remarks>
        /// <param name="message">Some random string.</param>
        public InvalidAttributeException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="InvalidAttributeException"/> class with a string message
        /// and the given inner exception.
        /// </summary>
        /// <remarks>The string passed here is ignored, use the 
        /// <see cref="InvalidAttributeException(System.Type, System.String)"/> constructor instead.
        /// </remarks>
        /// <param name="message">Some random string.</param>
        /// <param name="innerException">Some other exception.</param>
        public InvalidAttributeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAttributeException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination. </param>
        // FxCop suppression: Validation is done by Guard class
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected InvalidAttributeException(SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
            Guard.ArgumentNotNull(info, "info");
            info.AddValue("type", type);
            info.AddValue("memberName", memberName);
        }


        ///<summary>
        ///When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> with information about the exception.
        ///</summary>
        ///
        ///<param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination. </param>
        ///<param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown. </param>
        ///<exception cref="T:System.ArgumentNullException">The info parameter is a null reference (Nothing in Visual Basic). </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" /></PermissionSet>
        // FxCop suppression: Validation is done by Guard class
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.ArgumentNotNull(info, "info");
            base.GetObjectData(info, context);
            type = (Type)( info.GetValue("type", typeof(Type)) );
            memberName = info.GetString("memberName");
        }

        /// <summary>
        /// Type containing the member with the invalid attributes.
        /// </summary>
        public Type Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Name of the member with the invalid attributes.
        /// </summary>
        public string MemberName
        {
            get { return memberName; }
            set { memberName = value; }
        }

        ///<summary>
        ///Gets a message that describes the current exception.
        ///</summary>
        ///
        ///<returns>
        ///The error message that explains the reason for the exception, or an empty string("").
        ///</returns>
        ///<filterpriority>1</filterpriority>
        public override string Message
        {
            get 
            {
                if (type != null && !string.IsNullOrEmpty(memberName))
                {
                    return string.Format(CultureInfo.CurrentCulture,
                        "Too many dependency injection attributes defined on {0}.{1}.",
                        type,
                        memberName);
                }
                else
                {
                    return string.Format(CultureInfo.CurrentCulture,
                        "Too many dependency injection attributes, type and member name not supplied");
                }
            }
        }
    }
}
