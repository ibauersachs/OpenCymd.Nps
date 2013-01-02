namespace OpenCymd.Nps.Plugin.Native
{
    /// <summary>
    /// Enumerates the different actions an extension DLL can generate in response to an Access-Request.
    /// </summary>
    internal enum RADIUS_ACTION
    {
       raContinue,
       raReject,
       raAccept
    }
}
