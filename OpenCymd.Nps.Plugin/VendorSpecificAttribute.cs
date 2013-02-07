// --------------------------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (C) 2012-2013 University of Applied Sciences Northwestern Switzerland
//   
//   This library is free software; you can redistribute it and/or
//   modify it under the terms of the GNU Lesser General Public
//   License as published by the Free Software Foundation; either
//   version 2.1 of the License, or (at your option) any later version.
//   
//   This library is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//   Lesser General Public License for more details.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OpenCymd.Nps.Plugin
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    using OpenCymd.Nps.Plugin.Native;

    /// <summary>
    /// Data representation of a <see cref="RadiusAttribute"/> that is not standard type.
    /// </summary>
    public class VendorSpecificAttribute
    {
        private readonly IntPtr vsaPtr;

        private readonly RADIUS_VSA_FORMAT vsa;

        private byte[] data;

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorSpecificAttribute"/> class.
        /// </summary>
        /// <param name="vendorId">Vendor id for the attribute data</param>
        /// <param name="vendorType">Vendor type for the attribute data.</param>
        /// <param name="data">Data for the attribute data.</param>
        public VendorSpecificAttribute(uint vendorId, byte vendorType, byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentNullException("data");
            }

            if (data.Length > byte.MaxValue - 2)
            {
                throw new ArgumentException("data cannot be longer than (Byte.MaxValue - 2)", "data");
            }

            this.vsa = new RADIUS_VSA_FORMAT
                           {
                               VendorId = BitConverter.GetBytes(vendorId).Reverse().ToArray(),
                               VendorType = vendorType,
                               VendorLength = (byte)(data.Length + 2)
                           };
            this.data = new byte[data.Length];
            Array.Copy(data, this.data, data.Length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorSpecificAttribute"/> class.
        /// </summary>
        /// <param name="vsaPtr">Pointer to the native attribute data.</param>
        internal VendorSpecificAttribute(IntPtr vsaPtr)
        {
            this.vsaPtr = vsaPtr;
            this.vsa = (RADIUS_VSA_FORMAT)Marshal.PtrToStructure(vsaPtr, typeof(RADIUS_VSA_FORMAT));
        }

        /// <summary>
        /// Gets the vendor id of the attribute data.
        /// </summary>
        public uint VendorId
        {
            get
            {
                // flip bytes to convert from network byte order
                return BitConverter.ToUInt32(this.vsa.VendorId.Reverse().ToArray(), 0);
            }
        }

        /// <summary>
        /// Gets the vendor type of the attribute data.
        /// </summary>
        public byte VendorType
        {
            get
            {
                return this.vsa.VendorType;
            }
        }

        /// <summary>
        /// Gets the data of the attribute data.
        /// </summary>
        public byte[] Data
        {
            get
            {
                if (this.data == null)
                {
                    this.data = new byte[this.vsa.VendorLength - 2];
                    Marshal.Copy(this.vsaPtr + Marshal.SizeOf(typeof(RADIUS_VSA_FORMAT)), this.data, 0, this.vsa.VendorLength - 2);
                }

                return this.data;
            }
        }

        /// <summary>
        /// Gets the native representation of this VSA.
        /// </summary>
        internal RADIUS_VSA_FORMAT NativeFormat
        {
            get
            {
                return this.vsa;
            }
        }

        /// <summary>
        /// Converts the VSA into the raw representation of the attribute.
        /// </summary>
        /// <param name="vsa">The VSA to convert.</param>
        /// <returns>Byte array of the attribute, including the mandatory Vendor-Id.</returns>
        public static implicit operator byte[](VendorSpecificAttribute vsa)
        {
            var raw = new byte[4 + vsa.vsa.VendorLength];
            vsa.vsa.VendorId.CopyTo(raw, 0);
            raw[4] = vsa.VendorType;
            raw[5] = vsa.vsa.VendorLength;
            vsa.Data.CopyTo(raw, 6);
            return raw;
        }

        /// <summary>
        /// Gets the values of this instance as a <see cref="string"/>.
        /// </summary>
        /// <returns>String in the format VSA: ID=VendorId, Type=VendorType, Data=0x1234</returns>
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
