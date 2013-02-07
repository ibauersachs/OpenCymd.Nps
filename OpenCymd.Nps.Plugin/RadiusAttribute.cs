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
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Text;

    using OpenCymd.Nps.Plugin.Native;

    /// <summary>
    /// Attribute in a RADIUS message.
    /// </summary>
    public class RadiusAttribute
    {
        private readonly uint attributeId;

        private readonly object value;

        private readonly IntPtr radiusAttributePtr;

        private RADIUS_ATTRIBUTE radiusAttribute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadiusAttribute"/> class.
        /// </summary>
        /// <param name="attribute">ID of the attribute according to RFC2865.</param>
        /// <param name="value">The value of the attribute.</param>
        public RadiusAttribute(RadiusAttributeType attribute, object value)
            : this((int)attribute, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadiusAttribute"/> class.
        /// </summary>
        /// <param name="attributeId">ID of the attribute according to RFC2865.</param>
        /// <param name="value">The value of the attribute.</param>
        public RadiusAttribute(int attributeId, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (attributeId < 0)
            {
                throw new ArgumentOutOfRangeException("attributeId", "must be positive");
            }

            this.attributeId = (uint)attributeId;
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadiusAttribute"/> class.
        /// </summary>
        /// <param name="radiusAttributePtr">Pointer to the native attribute.</param>
        internal RadiusAttribute(IntPtr radiusAttributePtr)
        {
            this.radiusAttributePtr = radiusAttributePtr;
            this.radiusAttribute = (RADIUS_ATTRIBUTE)Marshal.PtrToStructure(this.radiusAttributePtr, typeof(RADIUS_ATTRIBUTE));
        }

        /// <summary>
        /// Gets the ID of the attribute according to RFC2865.
        /// </summary>
        public virtual int AttributeId
        {
            get
            {
                return (int)(this.radiusAttribute == null ? this.attributeId : this.radiusAttribute.dwAttrType);
            }
        }

        /// <summary>
        /// Gets the value of the attribute.
        /// </summary>
        public virtual object Value
        {
            get
            {
                return this.value ?? this.GetAttributeValue();
            }
        }

        /// <summary>
        /// Gets the native representation of this attribute. The caller is responsible to free the memory allocated in this method with a call to <see cref="FreeNativeAttribute"/>.
        /// </summary>
        /// <returns>The native representation of this attribute.</returns>
        internal RADIUS_ATTRIBUTE GetNativeAttribute()
        {
            if (this.radiusAttribute == null)
            {
                this.radiusAttribute = new RADIUS_ATTRIBUTE { dwAttrType = this.attributeId };
                this.SetAttributeValue();
            }

            return this.radiusAttribute;
        }

        /// <summary>
        /// Releases the memory allocated during a call to <see cref="GetNativeAttribute"/>
        /// </summary>
        internal void FreeNativeAttribute()
        {
            if (this.radiusAttribute == null)
            {
                return;
            }

            var ip = this.value as IPAddress;
            if (ip != null && ip.AddressFamily == AddressFamily.InterNetworkV6)
            {
                Marshal.FreeHGlobal(this.radiusAttribute.Value.lpValue);
                this.radiusAttribute = null;
                return;
            }

            if (this.value is string ||
                this.value is byte[] ||
                this.value is VendorSpecificAttribute)
            {
                Marshal.FreeHGlobal(this.radiusAttribute.Value.lpValue);
                this.radiusAttribute = null;
            }
        }

        private void SetAttributeValue()
        {
            var ip = this.value as IPAddress;
            if (ip != null)
            {
                switch (ip.AddressFamily)
                {
                    case AddressFamily.InterNetwork:
                        this.radiusAttribute.fDataType = RADIUS_DATA_TYPE.rdtAddress;
                        this.radiusAttribute.dwAttrType = BitConverter.ToUInt32(ip.GetAddressBytes().Reverse().ToArray(), 0);
                        break;
                    case AddressFamily.InterNetworkV6:
                        this.radiusAttribute.fDataType = RADIUS_DATA_TYPE.rdtIpv6Address;
                        this.radiusAttribute.Value.lpValue = Marshal.AllocHGlobal(ip.GetAddressBytes().Length);
                        this.radiusAttribute.cbDataLength = (uint)ip.GetAddressBytes().Length;
                        Marshal.Copy(
                            ip.GetAddressBytes(), 0, this.radiusAttribute.Value.lpValue, ip.GetAddressBytes().Length);
                        break;
                    default:
                        throw new ArgumentException("An address must be of type IPv4 or IPv6, other types are not supported.");
                }

                return;
            }

            var s = this.value as string;
            if (s != null)
            {
                this.radiusAttribute.fDataType = RADIUS_DATA_TYPE.rdtString;

                // Marshal.StringToHGlobal* are inappropriate as they are not UTF8 and include a terminating null char
                var utf8 = Encoding.UTF8.GetBytes(s);
                this.radiusAttribute.Value.lpValue = Marshal.AllocHGlobal(utf8.Length);
                Marshal.Copy(utf8, 0, this.radiusAttribute.Value.lpValue, utf8.Length);
                this.radiusAttribute.cbDataLength = (uint)utf8.Length;
                return;
            }

            if (this.value is int)
            {
                this.radiusAttribute.fDataType = RADIUS_DATA_TYPE.rdtInteger;
                this.radiusAttribute.Value.dwValue = (uint)(int)this.value;
                return;
            }

            if (this.value is uint)
            {
                this.radiusAttribute.fDataType = RADIUS_DATA_TYPE.rdtInteger;
                this.radiusAttribute.Value.dwValue = (uint)this.value;
                return;
            }

            if (this.value is DateTime)
            {
                this.radiusAttribute.fDataType = RADIUS_DATA_TYPE.rdtTime;
                var dt = (DateTime)this.value;
                this.radiusAttribute.Value.dwValue = (uint)dt.Ticks;
                return;
            }

            var bytes = this.value as byte[];
            if (bytes != null)
            {
                this.radiusAttribute.fDataType = RADIUS_DATA_TYPE.rdtString;
                this.radiusAttribute.Value.lpValue = Marshal.AllocHGlobal(bytes.Length);
                this.radiusAttribute.cbDataLength = (uint)bytes.Length;
                Marshal.Copy(bytes, 0, this.radiusAttribute.Value.lpValue, bytes.Length);
                return;
            }

            var vsa = this.value as VendorSpecificAttribute;
            if (vsa != null)
            {
                this.radiusAttribute.fDataType = RADIUS_DATA_TYPE.rdtString;
                byte[] vsaData = vsa;
                this.radiusAttribute.Value.lpValue = Marshal.AllocHGlobal(vsaData.Length);
                this.radiusAttribute.cbDataLength = (uint)vsaData.Length;
                Marshal.Copy(vsaData, 0, this.radiusAttribute.Value.lpValue, vsaData.Length);
                return;
            }

            throw new ArgumentException(string.Format("Type {0} is not supported.", this.value.GetType().FullName));
        }

        private object GetAttributeValue()
        {
            object result = null;
            if (this.radiusAttribute.dwAttrType == (uint)RadiusAttributeType.VendorSpecific)
            {
                return new VendorSpecificAttribute(this.radiusAttribute.Value.lpValue);
            }

            switch (this.radiusAttribute.fDataType)
            {
                case RADIUS_DATA_TYPE.rdtAddress:
                    // as of Win 2008, dwValue arrives in Network Byte Order, which is exactly what the constructor expects
                    result = new IPAddress(this.radiusAttribute.Value.dwValue);
                    break;
                case RADIUS_DATA_TYPE.rdtIpv6Address:
                    {
                        var data = new byte[this.radiusAttribute.cbDataLength];
                        Marshal.Copy(this.radiusAttribute.Value.lpValue, data, 0, (int)this.radiusAttribute.cbDataLength);
                        result = new IPAddress(data);
                    }

                    break;
                case RADIUS_DATA_TYPE.rdtInteger:
                    result = this.radiusAttribute.Value.dwValue;
                    break;
                case RADIUS_DATA_TYPE.rdtTime:
                    result = new DateTime(this.radiusAttribute.Value.dwValue);
                    break;
                case RADIUS_DATA_TYPE.rdtString:
                case RADIUS_DATA_TYPE.rdtUnknown:
                    // string means actually byte[], any real string would be 'text', but that is attribute-type specific
                    {
                        var data = new byte[this.radiusAttribute.cbDataLength];
                        Marshal.Copy(this.radiusAttribute.Value.lpValue, data, 0, (int)this.radiusAttribute.cbDataLength);
                        result = data;
                    }

                    break;
            }

            return result;
        }
    }
}