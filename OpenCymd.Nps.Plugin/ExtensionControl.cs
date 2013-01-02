// -----------------------------------------------------------------------
// <copyright file="ExtensionControl.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenCymd.Nps.Plugin
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    using OpenCymd.Nps.Plugin.Native;

    internal class ExtensionControl : IExtensionControl
    {
        private readonly IntPtr ecbPtr;

        private readonly RADIUS_EXTENSION_CONTROL_BLOCK ecb;

        private RadiusAttributeList requestList;

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
    }
}
