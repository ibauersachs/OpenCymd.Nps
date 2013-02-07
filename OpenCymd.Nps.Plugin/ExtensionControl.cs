// --------------------------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (C) 2012-2013 University of Applied Sciences Northwestern Switzerland
//   
//   This library is free software; you can redistribute it and/or
//   modify it under the terms of the GNU Lesser General Public
//   License as published by the Free Software Foundation; either
//   version 2.1 of the License, or (at your option) any later version.
//   
//   This library is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//   Lesser General Public License for more details.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OpenCymd.Nps.Plugin
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    using OpenCymd.Nps.Plugin.Native;

    /// <summary>
    /// Managed implementation of <see cref="RADIUS_EXTENSION_CONTROL_BLOCK"/>.
    /// </summary>
    internal class ExtensionControl : IExtensionControl
    {
        private readonly IntPtr ecbPtr;

        private readonly RADIUS_EXTENSION_CONTROL_BLOCK ecb;

        private RadiusAttributeList requestList;

        private IResponseDictionary responseDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionControl"/> class.
        /// </summary>
        /// <param name="ecbPtr">Pointer to the RADIUS_EXTENSION_CONTROL_BLOCK.</param>
        internal ExtensionControl(IntPtr ecbPtr)
        {
            this.ecbPtr = ecbPtr;
            this.ecb = (RADIUS_EXTENSION_CONTROL_BLOCK)Marshal.PtrToStructure(this.ecbPtr, typeof(RADIUS_EXTENSION_CONTROL_BLOCK));
        }

        /// <inheritdoc/>
        public virtual RadiusExtensionPoint ExtensionPoint
        {
            get
            {
                return this.ecb.repPoint;
            }
        }

        /// <inheritdoc/>
        public virtual RadiusCode RequestType
        {
            get
            {
                return this.ecb.rcRequestType;
            }
        }

        /// <inheritdoc/>
        public virtual RadiusCode ResponseType
        {
            get
            {
                return this.ecb.rcResponseType;
            }

            set
            {
                if (this.ecb.SetResponseType(this.ecbPtr, value) != 0)
                {
                    throw new Exception("{0} is not valid for ResponseType");
                }
            }
        }

        /// <inheritdoc/>
        public virtual IList<RadiusAttribute> Request
        {
            get
            {
                return this.requestList ?? (this.requestList = new RadiusAttributeList(this.ecb.GetRequest(this.ecbPtr)));
            }
        }

        /// <inheritdoc/>
        public virtual IResponseDictionary Response
        {
            get
            {
                return this.responseDictionary ?? (this.responseDictionary = new ResponseDictionary(this));
            }
        }

        private class ResponseDictionary : IResponseDictionary
        {
            private readonly ExtensionControl outer;

            private readonly Dictionary<RadiusCode, IList<RadiusAttribute>> responseLists =
                new Dictionary<RadiusCode, IList<RadiusAttribute>>(Enum.GetNames(typeof(RadiusCode)).Length);

            public ResponseDictionary(ExtensionControl outer)
            {
                this.outer = outer;
            }

            public IList<RadiusAttribute> this[RadiusCode responseType]
            {
                get
                {
                    if (!this.responseLists.ContainsKey(responseType))
                    {
                        this.responseLists[responseType] = new RadiusAttributeList(this.outer.ecb.GetResponse(this.outer.ecbPtr, responseType));
                    }

                    return this.responseLists[responseType];
                }
            }
        }
    }
}
