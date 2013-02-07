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
namespace OpenCymd.Nps.SamplePlugin
{
    using System;
    using System.Globalization;
    using System.Text;

    using log4net;

    using OpenCymd.Nps.Plugin;

    /// <summary>
    /// Sample NPS plugin that prints all request attributes, adds 3 VSAs and prints the attributes of the accept response.
    /// </summary>
    public class SampleNpsPlugin : INpsPlugin
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SampleNpsPlugin));

        /// <inheritdoc/>
        public void Initialize()
        {
            Logger.Info("Initialize");
        }

        /// <inheritdoc/>
        public void RadiusExtensionProcess(IExtensionControl control)
        {
            Logger.Info("RadiusExtensionProcess");
            Logger.DebugFormat("RequestType: {0}", control.RequestType);
            Logger.DebugFormat("ResponseType: {0}", control.ResponseType);
            Logger.DebugFormat("ExtensionPoint: {0}", control.ExtensionPoint);
            foreach (var attrib in control.Request)
            {
                var attribName = Enum.IsDefined(typeof(RadiusAttributeType), attrib.AttributeId)
                                        ? ((RadiusAttributeType)attrib.AttributeId).ToString()
                                        : attrib.AttributeId.ToString(CultureInfo.InvariantCulture);
                var attribValue = attrib.Value is byte[] ? "byte[" + ((byte[])attrib.Value).Length + "] / " + Encoding.UTF8.GetString((byte[])attrib.Value) : attrib.Value;
                Logger.DebugFormat("{0}={1}", attribName, attribValue);
            }

            control.Response[RadiusCode.AccessAccept].Add(
                new RadiusAttribute(RadiusAttributeType.VendorSpecific, new VendorSpecificAttribute(312, 1, new byte[] { 1, 2, 3, 4 })));

            control.Response[RadiusCode.AccessChallenge].Add(
                new RadiusAttribute(RadiusAttributeType.VendorSpecific, new VendorSpecificAttribute(313, 1, new byte[] { 4, 5, 6, 7 })));

            control.Response[RadiusCode.AccessReject].Add(
                new RadiusAttribute(RadiusAttributeType.VendorSpecific, new VendorSpecificAttribute(314, 1, new byte[] { 9, 10, 11, 12 })));

            foreach (var attrib in control.Response[RadiusCode.AccessAccept])
            {
                var attribName = Enum.IsDefined(typeof(RadiusAttributeType), attrib.AttributeId)
                                     ? ((RadiusAttributeType)attrib.AttributeId).ToString()
                                     : attrib.AttributeId.ToString(CultureInfo.InvariantCulture);
                var attribValue = attrib.Value is byte[] ? "byte[" + ((byte[])attrib.Value).Length + "] / " + Encoding.UTF8.GetString((byte[])attrib.Value) : attrib.Value;
                Logger.DebugFormat("{0}={1}", attribName, attribValue);
            }
        }
    }
}
