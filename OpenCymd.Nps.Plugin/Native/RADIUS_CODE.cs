namespace OpenCymd.Nps.Plugin.Native
{
    /// <summary>
    /// The RADIUS_CODE enumeration type enumerates the possible RADIUS packet codes.
    /// </summary>
    public enum RADIUS_CODE : uint
    {
        /// <summary>
        /// The packet type is unrecognized. This is used to indicate that the disposition of a request is not being set by this extension DLL.
        /// </summary>
        rcUnknown              = 0,

        /// <summary>
        /// RADIUS Access-Request packet. See RFC 2865 for more information.
        /// </summary>
        rcAccessRequest        = 1,

        /// <summary>
        /// RADIUS Access-Accept packet. See RFC 2865 for more information.
        /// </summary>
        rcAccessAccept         = 2,

        /// <summary>
        /// RADIUS Access-Reject packet. See RFC 2865 for more information.
        /// </summary>
        rcAccessReject         = 3,

        /// <summary>
        /// RADIUS Accounting-Request packet. See RFC 2866 for more information.
        /// </summary>
        rcAccountingRequest    = 4,

        /// <summary>
        /// RADIUS Accounting-Response packet. See RFC 2866 for more information.
        /// </summary>
        rcAccountingResponse   = 5,

        /// <summary>
        /// RADIUS Access-Challenge packet. See RFC 2865 for more information.
        /// </summary>
        rcAccessChallenge      = 11,

        /// <summary>
        /// The packet was discarded.
        /// </summary>
        rcDiscard              = 256 
    }
}