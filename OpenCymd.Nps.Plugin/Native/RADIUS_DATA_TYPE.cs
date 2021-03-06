// <auto-generated/>: silence stylecop

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

// ReSharper disable InconsistentNaming
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
// ReSharper restore InconsistentNaming
