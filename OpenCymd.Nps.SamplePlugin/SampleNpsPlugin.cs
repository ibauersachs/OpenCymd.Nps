namespace OpenCymd.Nps.SamplePlugin
{
    using System;
    using System.Globalization;

    using log4net;

    using OpenCymd.Nps.Plugin;

    /// <summary>
    /// Sample NPS plugin that prints all request attributes and the attributes of the accept response.
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
                var attribValue = attrib.Value is byte[] ? "byte[" + ((byte[])attrib.Value).Length + "]" : attrib.Value;
                Logger.DebugFormat("{0}={1}", attribName, attribValue);
            }

            foreach (var attrib in control.Response[RadiusCode.AccessAccept])
            {
                var attribName = Enum.IsDefined(typeof(RadiusAttributeType), attrib.AttributeId)
                                     ? ((RadiusAttributeType)attrib.AttributeId).ToString()
                                     : attrib.AttributeId.ToString(CultureInfo.InvariantCulture);
                var attribValue = attrib.Value is byte[] ? "byte[" + ((byte[])attrib.Value).Length + "]" : attrib.Value;
                Logger.DebugFormat("{0}={1}", attribName, attribValue);
            }
        }
    }
}
