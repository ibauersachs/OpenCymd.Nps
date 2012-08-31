// -----------------------------------------------------------------------
// <copyright file="ExtensionControl.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenCymd.Nps.Plugin
{
    using System;
    using System.Runtime.InteropServices;

    using OpenCymd.Nps.Plugin.Native;

    using log4net;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExtensionControl
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

        public RADIUS_EXTENSION_POINT ExtensionPoint
        {
            get
            {
                return this.ecb.repPoint;
            }
        }

        public RADIUS_CODE RequestType
        {
            get
            {
                return this.ecb.rcRequestType;
            }
        }

        public RADIUS_CODE ResponseType
        {
            get
            {
                return this.ecb.rcResponseType;
            }
        }

        public RadiusAttributeList Request
        {
            get
            {
                return this.requestList ?? (this.requestList = new RadiusAttributeList(this.ecb.GetRequest(this.ecbPtr)));
            }
        }
    }
}
