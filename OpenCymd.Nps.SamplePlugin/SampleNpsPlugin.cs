namespace OpenCymd.Nps.SamplePlugin
{
    using System;
    using System.Globalization;

    using log4net;

    using OpenCymd.Nps.Plugin;
    using OpenCymd.Nps.Plugin.Native;

    public class SampleNpsPlugin : INpsPlugin
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SampleNpsPlugin));

        public void Initialize()
        {
            Logger.Info("Initialize");
        }

        public void RadiusExtensionProcess(IExtensionControl control)
        {
            Logger.Info("RadiusExtensionProcess");
            if (Logger.IsDebugEnabled)
            {
                Logger.DebugFormat("RequestType: {0}", control.RequestType);
                Logger.DebugFormat("ResponseType: {0}", control.ResponseType);
                Logger.DebugFormat("ExtensionPoint: {0}", control.ExtensionPoint);
                if (control.ResponseType == RadiusCode.AccessAccept && control.ExtensionPoint == RadiusExtensionPoint.Authorization)
                {
                    foreach (var attrib in control.Request)
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
    }
}
