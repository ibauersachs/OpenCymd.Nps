namespace OpenCymd.Nps.Plugin
{
    using System.Collections.Generic;

    using OpenCymd.Nps.Plugin.Native;

    public interface IExtensionControl
    {
        RADIUS_EXTENSION_POINT ExtensionPoint { get; }

        RADIUS_CODE RequestType { get; }

        RADIUS_CODE ResponseType { get; }

        IList<RadiusAttribute> Request { get; }
    }
}