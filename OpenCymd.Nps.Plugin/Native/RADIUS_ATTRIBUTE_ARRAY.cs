namespace OpenCymd.Nps.Plugin.Native
{
    using System;
    using System.Runtime.InteropServices;

    internal delegate uint Add(IntPtr This, RADIUS_ATTRIBUTE pAttr);

    internal delegate IntPtr AttributeAt(IntPtr This, uint dwIndex);

    internal delegate uint GetSize(IntPtr This);

    internal delegate uint InsertAt(IntPtr This, uint dwIndex, RADIUS_ATTRIBUTE pAttr);

    internal delegate uint RemoveAt(IntPtr This, uint dwIndex);

    internal delegate uint SetAt(IntPtr This, uint dwIndex, RADIUS_ATTRIBUTE pAttr);

    /// <summary>
    /// The RADIUS_ATTRIBUTE_ARRAY structure represents an array of attributes.
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/bb892010(v=vs.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class RADIUS_ATTRIBUTE_ARRAY
    {
        /// <summary>
        /// Specifies the size of the structure.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// Pointer to the Add function provided by NPS. NPS sets the value of the member.
        /// </summary>
        public Add Add;

        /// <summary>
        /// Pointer to the AttributeAt function provided by NPS. NPS sets the value of the member.
        /// </summary>
        public AttributeAt AttributeAt;

        /// <summary>
        /// Pointer to the GetSize function provided by NPS. NPS sets the value of the member.
        /// </summary>
        public GetSize GetSize;

        /// <summary>
        /// Pointer to the InsertAt function provided by NPS. NPS sets the value of the member.
        /// </summary>
        public InsertAt InsertAt;

        /// <summary>
        /// Pointer to the RemoveAt function provided by NPS. NPS sets the value of the member.
        /// </summary>
        public RemoveAt RemoveAt;

        /// <summary>
        /// Pointer to the SetAt function provided by NPS. NPS sets the value of the member.
        /// </summary>
        public SetAt SetAt;
    }
}