namespace OpenCymd.Nps.Plugin
{
    /// <summary>
    /// The RADIUS_CODE enumeration type enumerates the possible RADIUS packet codes.
    /// </summary>
    public enum RadiusCode : uint
    {
        /// <summary>
        /// The packet type is unrecognized. This is used to indicate that the disposition of a request is not being set by this extension DLL.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// RADIUS Access-Request packet. See RFC 2865 for more information.
        /// </summary>
        AccessRequest = 1,

        /// <summary>
        /// RADIUS Access-Accept packet. See RFC 2865 for more information.
        /// </summary>
        AccessAccept = 2,

        /// <summary>
        /// RADIUS Access-Reject packet. See RFC 2865 for more information.
        /// </summary>
        AccessReject = 3,

        /// <summary>
        /// RADIUS Accounting-Request packet. See RFC 2866 for more information.
        /// </summary>
        AccountingRequest = 4,

        /// <summary>
        /// RADIUS Accounting-Response packet. See RFC 2866 for more information.
        /// </summary>
        AccountingResponse = 5,

        /// <summary>
        /// RADIUS Access-Challenge packet. See RFC 2865 for more information.
        /// </summary>
        AccessChallenge = 11,

        /// <summary>
        /// The packet was discarded.
        /// </summary>
        Discard = 256
    }
}