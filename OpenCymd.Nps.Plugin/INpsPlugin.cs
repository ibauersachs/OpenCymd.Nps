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
