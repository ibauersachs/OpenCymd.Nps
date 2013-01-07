namespace OpenCymd.Nps.Plugin.Native
{
    /// <summary>
    /// The RADIUS_DATA_TYPE type enumerates the possible data type for a RADIUS attribute or extended attribute.
    /// </summary>
    internal enum RADIUS_DATA_TYPE : uint
    {
        /// <summary>
        /// The value is a pointer to an unknown data type.
        /// </summary>
        rdtUnknown,

        /// <summary>
        /// The value of the attribute is a pointer to a character string.
        /// </summary>
        /// <remarks>Character string means just char*, but not actually a human readable string.</remarks>
        rdtString,

        /// <summary>
        /// The value of the attribute is a 32-bit DWORD value that represents an address.
        /// </summary>
        rdtAddress,

        /// <summary>
        /// The value of the attribute is a 32-bit DWORD value that represents an integer.
        /// </summary>
        rdtInteger,

        /// <summary>
        /// The value of the attribute is a 32-bit DWORD value that represents a time.
        /// </summary>
        rdtTime,

        /// <summary>
        /// The value of the attribute is a BYTE* value that represents an IPv6 address.
        /// </summary>
        rdtIpv6Address
    }
}