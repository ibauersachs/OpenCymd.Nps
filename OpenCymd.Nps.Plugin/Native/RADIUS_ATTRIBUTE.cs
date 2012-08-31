namespace OpenCymd.Nps.Plugin.Native
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The RADIUS_ATTRIBUTE structure represents a RADIUS attribute or an extended attribute.
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/bb892009(v=vs.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class RADIUS_ATTRIBUTE
    {
        /// <summary>
        /// Stores a value from the RADIUS_ATTRIBUTE_TYPE enumeration. This value specifies the
        /// type of the attribute represented by the RADIUS_ATTRIBUTE structure.
        /// </summary>
        public uint dwAttrType;

        /// <summary>
        /// Stores a value from the RADIUS_DATA_TYPE enumeration. This value specifies the type
        /// of the value stored in the union that contains the dwValue and lpValue members.
        /// </summary>
        public RADIUS_DATA_TYPE fDataType;

        /// <summary>
        /// Stores the length, in bytes, of the data. The cbDataLength member is used only if lpValue member is used.
        /// </summary>
        public uint cbDataLength;

        /// <summary>
        /// Stores the value of the attribute. dwValue and lpValue are in a separate struct to avoid "pack"-ing problems.
        /// </summary>
        public RADIUS_ATTRIBUTE_UNION_VALUE Value;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct RADIUS_ATTRIBUTE_UNION_VALUE
    {
        /// <summary>
        /// Stores a value of type DWORD. The dwValue member is used if the fDataType member
        ///  specifies rdtAddress, rdtInteger or rdtTime.
        /// </summary>
        /// <remarks>
        /// In Windows Server 2008 the byte order format of dwValue is represented in network
        ///  byte order (big-endian) when fDataType is specified as rdtAddress. Previous
        ///  Windows versions represented network addressing using the little-endian format.
        /// </remarks>
        [FieldOffset(0)]
        public uint dwValue;

        /// <summary>
        /// Stores a multi-byte data value. The lpValue member is used if the fDataType member
        ///  specifies rdtUnknown, rdtIpv6Address, or rdtString.
        /// </summary>
        [FieldOffset(0)]
        public IntPtr lpValue; //BYTE*
    }
}