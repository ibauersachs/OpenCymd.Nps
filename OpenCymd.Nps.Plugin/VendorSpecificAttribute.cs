// -----------------------------------------------------------------------
// <copyright file="VendorSpecificAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenCymd.Nps.Plugin
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    using OpenCymd.Nps.Plugin.Native;

    public class VendorSpecificAttribute
    {
        private readonly IntPtr vsaPtr;

        private readonly RADIUS_VSA_FORMAT vsa;

        private byte[] data;

        internal VendorSpecificAttribute(IntPtr vsaPtr)
        {
            this.vsaPtr = vsaPtr;
            this.vsa = (RADIUS_VSA_FORMAT)Marshal.PtrToStructure(vsaPtr, typeof(RADIUS_VSA_FORMAT));
        }

        public uint VendorId
        {
            get
            {
                // flip bytes to convert from network byte order
                return BitConverter.ToUInt32(this.vsa.VendorId.Reverse().ToArray(), 0);
            }
        }

        public byte VendorType
        {
            get
            {
                return this.vsa.VendorType;
            }
        }

        public byte[] Data
        {
            get
            {
                if (this.data == null)
                {
                    this.data = new byte[this.vsa.VendorLength];
                    Marshal.Copy(this.vsaPtr + Marshal.SizeOf(typeof(RADIUS_VSA_FORMAT)), this.data, 0, this.vsa.VendorLength);
                }

                return this.data;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("VSA: ID=");
            sb.Append(this.VendorId);
            sb.Append(", Type=");
            sb.Append(this.VendorType);
            sb.Append(", Data=");
            foreach (var b in this.Data)
            {
                sb.AppendFormat("{0:X2}", b);
            }

            return sb.ToString();
        }
    }
}
