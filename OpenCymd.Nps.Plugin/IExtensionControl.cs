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
    }
}