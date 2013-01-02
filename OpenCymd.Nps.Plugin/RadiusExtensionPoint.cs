namespace OpenCymd.Nps.Plugin
{
    /// <summary>
    /// The RADIUS_EXTENSION_POINT enumeration type enumerates the possible points in the RADIUS
    /// request process when the RadiusExtensionProcess2 function can be called.
    /// </summary>
    public enum RadiusExtensionPoint : uint
    {
        /// <summary>
        /// Indicates that the RadiusExtensionProcess2 function is called at the
        /// point in the request process where authentication occurs.
        /// </summary>
        Authentication,

        /// <summary>
        /// Indicates that the RadiusExtensionProcess2 function is called at the
        /// point in the request process where authorization occurs.
        /// </summary>
        Authorization
    }
}