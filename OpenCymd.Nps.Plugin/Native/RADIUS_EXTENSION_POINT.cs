namespace OpenCymd.Nps.Plugin.Native
{
    /// <summary>
    /// The RADIUS_EXTENSION_POINT enumeration type enumerates the possible points in the RADIUS
    /// request process when the RadiusExtensionProcess2 function can be called.
    /// </summary>
    public enum RADIUS_EXTENSION_POINT : uint
    {
        /// <summary>
        /// Indicates that the RadiusExtensionProcess2 function is called at the
        /// point in the request process where authentication occurs.
        /// </summary>
        repAuthentication,

        /// <summary>
        /// Indicates that the RadiusExtensionProcess2 function is called at the
        /// point in the request process where authorization occurs.
        /// </summary>
        repAuthorization
    }
}