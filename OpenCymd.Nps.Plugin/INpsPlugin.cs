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
    using System;

    /// <summary>
    /// Extension point for NPS plugins.
    /// </summary>
    /// <remarks>
    /// Plugins that need to free resources can implement <see cref="IDisposable"/>.
    /// </remarks>
    public interface INpsPlugin
    {
        /// <summary>
        /// Called after the startup of the NPS or when the plugin chain changes.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Called for authentication and authorization requests.
        /// </summary>
        /// <param name="control">Control that provides information about the current RADIUS request.</param>
        void RadiusExtensionProcess(IExtensionControl control);
    }
}
