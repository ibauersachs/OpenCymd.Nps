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
    /// <summary>
    /// The RADIUS_CODE enumeration type enumerates the possible RADIUS packet codes.
    /// </summary>
    public enum RadiusCode : uint
    {
        /// <summary>
        /// The packet type is unrecognized. This is used to indicate that the disposition of a request is not being set by this extension DLL.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// RADIUS Access-Request packet. See RFC 2865 for more information.
        /// </summary>
        AccessRequest = 1,

        /// <summary>
        /// RADIUS Access-Accept packet. See RFC 2865 for more information.
        /// </summary>
        AccessAccept = 2,

        /// <summary>
        /// RADIUS Access-Reject packet. See RFC 2865 for more information.
        /// </summary>
        AccessReject = 3,

        /// <summary>
        /// RADIUS Accounting-Request packet. See RFC 2866 for more information.
        /// </summary>
        AccountingRequest = 4,

        /// <summary>
        /// RADIUS Accounting-Response packet. See RFC 2866 for more information.
        /// </summary>
        AccountingResponse = 5,

        /// <summary>
        /// RADIUS Access-Challenge packet. See RFC 2865 for more information.
        /// </summary>
        AccessChallenge = 11,

        /// <summary>
        /// The packet was discarded.
        /// </summary>
        Discard = 256
    }
}