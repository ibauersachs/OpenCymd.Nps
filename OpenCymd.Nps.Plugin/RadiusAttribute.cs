namespace OpenCymd.Nps.Plugin
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Runtime.InteropServices;

    using OpenCymd.Nps.Plugin.Native;

    public class RadiusAttribute
    {
        private readonly uint attributeId;

        private readonly object value;

        private readonly IntPtr radiusAttributePtr;

        private RADIUS_ATTRIBUTE radiusAttribute;

        internal RadiusAttribute(IntPtr radiusAttributePtr)
        {
            this.radiusAttributePtr = radiusAttributePtr;
            this.radiusAttribute = (RADIUS_ATTRIBUTE)Marshal.PtrToStructure(this.radiusAttributePtr, typeof(RADIUS_ATTRIBUTE));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadiusAttribute"/> class.
        /// </summary>
        /// <param name="attributeId">
        /// </param>
        /// <param name="value">
        /// </param>
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
        /// <param name="attributeId">
        /// </param>
        /// <param name="value">
        /// </param>
        public RadiusAttribute(RadiusAttributeType attributeId, object value) : this ((int)attributeId, value)
        {
        }

        public virtual int AttributeId
        {
            get
            {
                return (int)(this.radiusAttribute == null ? this.attributeId : this.radiusAttribute.dwAttrType);
            }
        }

        public virtual object Value
        {
            get
            {
                return this.value ?? this.GetAttributeValue();
            }
        }

        internal RADIUS_ATTRIBUTE GetNativeAttribute()
        {
            if (this.radiusAttribute == null)
            {
                this.radiusAttribute = new RADIUS_ATTRIBUTE { dwAttrType = this.attributeId };
                this.SetAttributeValue();
                Marshal.StructureToPtr(this, this.radiusAttributePtr, false);
            }

            return this.radiusAttribute;
        }

        private void SetAttributeValue()
        {
            var ip = this.value as IPAddress;
            if (ip != null)
            {
                switch (ip.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        this.radiusAttribute.fDataType = RADIUS_DATA_TYPE.rdtAddress;
                        this.radiusAttribute.dwAttrType = BitConverter.ToUInt32(ip.GetAddressBytes().Reverse().ToArray(), 0);
                        break;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
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
                this.radiusAttribute.Value.lpValue = Marshal.StringToHGlobalAnsi(s);
                this.radiusAttribute.cbDataLength = (uint)s.Length;
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
                this.radiusAttribute.fDataType = RADIUS_DATA_TYPE.rdtUnknown;
                this.radiusAttribute.Value.lpValue = Marshal.AllocHGlobal(bytes.Length);
                this.radiusAttribute.cbDataLength = (uint)bytes.Length;
                Marshal.Copy(bytes, 0, this.radiusAttribute.Value.lpValue, bytes.Length);
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
                    result = Marshal.PtrToStringAnsi(this.radiusAttribute.Value.lpValue, (int)this.radiusAttribute.cbDataLength);
                    break;
                case RADIUS_DATA_TYPE.rdtUnknown:
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