// -----------------------------------------------------------------------
// <copyright file="INpsPlugin.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenCymd.Nps.Plugin
{
    using System.ComponentModel.Composition;

    [InheritedExport]
    public interface INpsPlugin
    {
        void Initialize();

        void RadiusExtensionProcess(ExtensionControl control);
    }
}
