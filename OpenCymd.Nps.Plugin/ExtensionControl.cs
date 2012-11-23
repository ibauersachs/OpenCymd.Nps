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

    using log4net;

    using OpenCymd.Nps.Plugin.Native;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class ExtensionControl : IExtensionControl
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ExtensionControl));

        private readonly IntPtr ecbPtr;

        private readonly RADIUS_EXTENSION_CONTROL_BLOCK ecb;

        private RadiusAttributeList requestList;

        internal ExtensionControl(IntPtr ecbPtr)
        {
            this.ecbPtr = ecbPtr;
            this.ecb = (RADIUS_EXTENSION_CONTROL_BLOCK)Marshal.PtrToStructure(this.ecbPtr, typeof(RADIUS_EXTENSION_CONTROL_BLOCK));
            if (Logger.IsInfoEnabled)
            {
                Logger.InfoFormat(
                    "Processing extension at {0} with request-type {1} and response-type {2}.",
                    this.ecb.repPoint,
                    this.ecb.rcRequestType,
                    this.ecb.rcResponseType);
            }
        }

        public virtual RADIUS_EXTENSION_POINT ExtensionPoint
        {
            get
            {
                return this.ecb.repPoint;
            }
        }

        public virtual RADIUS_CODE RequestType
        {
            get
            {
                return this.ecb.rcRequestType;
            }
        }

        public virtual RADIUS_CODE ResponseType
        {
            get
            {
                return this.ecb.rcResponseType;
            }
        }

        public virtual IList<RadiusAttribute> Request
        {
            get
            {
                return this.requestList ?? (this.requestList = new RadiusAttributeList(this.ecb.GetRequest(this.ecbPtr)));
            }
        }
    }
}
