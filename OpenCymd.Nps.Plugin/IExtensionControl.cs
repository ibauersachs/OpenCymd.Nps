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
    using System.Collections.Generic;

    /// <summary>
    /// Provides information about the current RADIUS request, the attributes and the setting of the request disposal (<see cref="ResponseType"/>).
    /// </summary>
    public interface IExtensionControl
    {
        /// <summary>
        /// Gets at what point in the request process plugin was called.
        /// </summary>
        RadiusExtensionPoint ExtensionPoint { get; }

        /// <summary>
        /// Gets the type of RADIUS request received by the NPS server.
        /// </summary>
        RadiusCode RequestType { get; }

        /// <summary>
        /// Gets or sets the final disposition of the request.
        /// </summary>
        RadiusCode ResponseType { get; set; }

        /// <summary>
        /// Gets the attributes received in the RADIUS request process and any internal attributes describing the request state.
        /// </summary>
        IList<RadiusAttribute> Request { get; }

        /// <summary>
        /// Gets the attributes that are sent in the RADIUS response if the final outcome of request processing matches the specified response type.
        /// </summary>
        IResponseDictionary Response { get; }
    }

    /// <summary>
    /// Do not implement. Helper interface to provide an index property.
    /// </summary>
    public interface IResponseDictionary
    {
        /// <summary>
        /// Gets the attributes that are sent in the RADIUS response if the final outcome of request processing matches the specified response type.
        /// </summary>
        /// <param name="responseType">The response for which the attribute list is to be retrieved.</param>
        /// <returns>The attributes list for the specified response type.</returns>
        IList<RadiusAttribute> this[RadiusCode responseType] { get; }
    }
}