// -----------------------------------------------------------------------
// <copyright file="RADIUS_VSA_FORMAT.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenCymd.Nps.Plugin.Native
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Struct representing the layout of a RADIUS Vendor-Specific attribute. This
    /// is useful when interpreting the RADIUS_ATTRIBUTE lpValue field when
    /// dwAttrType is ratVendorSpecific.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class RADIUS_VSA_FORMAT
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] VendorId;

        public byte VendorType;

        public byte VendorLength;

        // attribute data begins here
    }
}
