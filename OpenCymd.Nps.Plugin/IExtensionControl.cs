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