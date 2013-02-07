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
    /// The RADIUS_EXTENSION_POINT enumeration type enumerates the possible points in the RADIUS
    /// request process when the RadiusExtensionProcess2 function can be called.
    /// </summary>
    public enum RadiusExtensionPoint : uint
    {
        /// <summary>
        /// Indicates that the RadiusExtensionProcess2 function is called at the
        /// point in the request process where authentication occurs.
        /// </summary>
        Authentication,

        /// <summary>
        /// Indicates that the RadiusExtensionProcess2 function is called at the
        /// point in the request process where authorization occurs.
        /// </summary>
        Authorization
    }
}