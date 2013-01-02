namespace OpenCymd.Nps.Plugin.Native
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The GetRequest function returns the attributes received in the RADIUS request process and any internal attributes describing the request state.
    /// </summary>
    /// <param name="This">
    /// Pointer to a RADIUS_EXTENSION_CONTROL_BLOCK structure. NPS passes the Extension DLL 
    /// a pointer to this structure when it calls the RadiusExtensionProcess2 structure.
    /// </param>
    /// <returns>
    /// If the function succeeds, the return value is a pointer to a RADIUS_ATTRIBUTE_ARRAY 
    /// structure that represents the attributes in the RADIUS response.
    /// If the function fails, the return value is a NULL pointer.
    /// </returns>
    /// <remarks>
    /// The Extension DLL can modify the attributes in the RADIUS request. For example, if NPS
    ///  is acting as a RADIUS proxy, an Extension DLL could filter which attributes are forwarded
    ///  to the remote RADIUS server.
    /// To modify the attributes, the Extension DLL uses the functions provided as members
    ///  of the RADIUS_ATTRIBUTE_ARRAY structure.
    /// This function is provided by NPS. NPS returns a pointer to this function as a member of
    ///  the RADIUS_EXTENSION_CONTROL_BLOCK structure.
    /// </remarks>
    internal delegate IntPtr GetRequest(IntPtr This);

    /// <summary>
    /// The GetResponse function returns the attributes that are sent in the response if the
    /// final outcome of request processing matches the specified response type.
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/bb892016(v=vs.85).aspx
    /// </summary>
    /// <param name="This">
    /// Pointer to a RADIUS_EXTENSION_CONTROL_BLOCK structure. NPS passes the Extension DLL
    /// a pointer to this structure when it calls the RadiusExtensionProcess2 function.
    /// </param>
    /// <param name="rcResponseType">
    /// Specifies the response type. This parameter must be one of the values enumerated by
    /// the RADIUS_CODE enumeration type. Otherwise, the function fails, returning NULL.
    /// </param>
    /// <returns>
    /// If the function succeeds, the return value is a pointer to a RADIUS_ATTRIBUTE_ARRAY structure
    /// that represents the attributes in the RADIUS response.
    /// If the function fails, the return value is a NULL pointer.
    /// </returns>
    /// <remarks>
    /// An Extension DLL can use GetResponse to retrieve and modify the attributes for any valid
    ///  response type regardless of the request's current disposition. For example, an Extension
    ///  DLL could set the response type to rcAccessAccept, but still add attributes to those
    ///  returned in the case of an rcAccessReject. The response specified by the Extension DLL
    ///  (rcAccessAccept in this example) could be overridden during further processing.
    /// To modify the attributes, the Extension DLL uses the functions provided as members of the
    ///  RADIUS_ATTRIBUTE_ARRAY structure.
    /// This function is provided by NPS. NPS returns a pointer to this function as a member
    ///  of the RADIUS_EXTENSION_CONTROL_BLOCK structure.
    /// </remarks>
    internal delegate IntPtr GetResponse(IntPtr This, RadiusCode rcResponseType);

    /// <summary>
    /// The SetResponseType function sets the final disposition of the request.
    /// </summary>
    /// <param name="This">
    /// Pointer to a RADIUS_EXTENSION_CONTROL_BLOCK structure. NPS passes the Extension DLL
    /// a pointer to this structure when it calls the RadiusExtensionProcess2 function.
    /// </param>
    /// <param name="rcResponseType">
    /// Specifies the response type. This parameter must be one of the values contained within
    ///  the RADIUS_CODE enumerated type and is related to the rcRequestType member of the
    ///  RADIUS_EXTENSION_CONTROL_BLOCK structure. If rcRequestType equals rcAccessRequest,
    ///  this value may be rcAccessAccept, rcAccessReject, rcAccessChallenge, or rcDiscard.
    ///  If rcRequestType equals rcAccountingRequest, this value can be rcAccountingResponse
    ///  or rcDiscard. Otherwise, the function fails, returning ERROR_INVALID_PARAMETER.
    /// </param>
    /// <returns>
    /// If the function succeeds, the return value is NO_ERROR.
    /// If the function fails, the return value is one of the following error codes.
    /// <list type="table">
    /// <item><term>ERROR_INVALID_PARAMETER</term><description>The specified response type is invalid for the request type.</description></item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// Note that the disposition set by the Extension DLL can be overridden during further processing.
    ///  For example, the Extension DLL may set the response type to rcAccessAccept, but during further
    ///  processing, the response can be changed to rcAccessReject.
    /// This function is provided by NPS. NPS returns a pointer to this function as a member of
    ///  the RADIUS_EXTENSION_CONTROL_BLOCK structure.
    /// </remarks>
    internal delegate uint SetResponseType(IntPtr This, RadiusCode rcResponseType);


    /// <summary>
    /// The RADIUS_EXTENSION_CONTROL_BLOCK structure provides information about the current RADIUS request.
    /// It also provides functions for obtaining the attributes associated with the request, and for
    /// setting the disposition of the request.
    /// </summary>
    /// <remarks>
    /// The Extension DLL must not modify this structure. Changes to the array of attributes should
    ///  be made by calling the functions provided as members of this structure.
    /// NPS passes this structure to the Extension DLL when it calls the Extension DLL's
    ///  implementation of RadiusExtensionProcess2.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal class RADIUS_EXTENSION_CONTROL_BLOCK
    {
        /// <summary>
        /// Specifies the size of the structure.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// Specifies the version of the structure.
        /// </summary>
        public uint dwVersion;

        /// <summary>
        /// Specifies a value of type RADIUS_EXTENSION_POINT that indicates at what
        /// point in the request process RadiusExtensionProcess2 was called.
        /// </summary>
        public RadiusExtensionPoint repPoint;

        /// <summary>
        /// Specifies a value of type RADIUS_CODE that specifies the type of RADIUS 
        /// request received by the Internet Authentication Service server.
        /// </summary>
        public RadiusCode rcRequestType;

        /// <summary>
        /// Specifies a value of type RADIUS_CODE that indicates the disposition of the RADIUS request.
        /// </summary>
        public RadiusCode rcResponseType;

        /// <summary>
        /// Pointer to the GetRequest function provided by NPS. NPS sets the value of this member.
        /// </summary>
        public GetRequest GetRequest;

        /// <summary>
        /// Pointer to the GetResponse function provided by NPS. NPS sets the value of this member.
        /// </summary>
        public GetResponse GetResponse;

        /// <summary>
        /// Pointer to the SetResponseType function provided by NPS. NPS sets the value of this member.
        /// </summary>
        public SetResponseType SetResponseType;
    }
}
