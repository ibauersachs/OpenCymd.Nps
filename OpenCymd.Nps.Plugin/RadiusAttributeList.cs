namespace OpenCymd.Nps.Plugin
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    using OpenCymd.Nps.Plugin.Native;

    /// <summary>
    /// Managed wrapper to access RADIUS attributes.
    /// </summary>
    public class RadiusAttributeList : IList<RadiusAttribute>
    {
        private readonly IntPtr radiusAttributeArrayPtr;

        private readonly RADIUS_ATTRIBUTE_ARRAY radiusAttributeArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadiusAttributeList"/> class.
        /// </summary>
        /// <param name="radiusAttributeArrayPtr">Pointer to the native <see cref="RADIUS_ATTRIBUTE_ARRAY"/>.</param>
        internal RadiusAttributeList(IntPtr radiusAttributeArrayPtr)
        {
            this.radiusAttributeArrayPtr = radiusAttributeArrayPtr;
            this.radiusAttributeArray = (RADIUS_ATTRIBUTE_ARRAY)Marshal.PtrToStructure(radiusAttributeArrayPtr, typeof(RADIUS_ATTRIBUTE_ARRAY));
        }

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                return (int)this.radiusAttributeArray.GetSize(this.radiusAttributeArrayPtr);
            }
        }

        /// <inheritdoc/>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public RadiusAttribute this[int index]
        {
            get
            {
                return new RadiusAttribute(this.radiusAttributeArray.AttributeAt(this.radiusAttributeArrayPtr, (uint)index));
            }

            set
            {
                if (index < 0 || index > this.Count - 1)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                if (value == null)
                {
                    this.RemoveAt(index);
                    return;
                }

                var result = this.radiusAttributeArray.SetAt(this.radiusAttributeArrayPtr, (uint)index, value.GetNativeAttribute());
                switch (result)
                {
                    case 0:
                        return; // success
                    case 5:
                        throw new UnauthorizedAccessException("The specified attribute is read-only.");
                    case 87:
                        throw new ArgumentException("The index is out of range.");
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerator<RadiusAttribute> GetEnumerator()
        {
            for (var i = 0; i < this.Count; i++)
            {
                yield return this[i];
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc/>
        public void Add(RadiusAttribute item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var result = this.radiusAttributeArray.Add(this.radiusAttributeArrayPtr, item.GetNativeAttribute());
            switch (result)
            {
                case 0:
                    return;
                case 0x80070005:
                case 0x80000009:
                    throw new UnauthorizedAccessException(
                        "The NPS Extension DLL attempted to add an attribute that Extension DLLs are not allowed to add, or the DLL attempted to add an additional instance of an attribute of which only a single instance is allowed.");
                case 87:
                    throw new ArgumentException("The index is out of range.");
                default:
                    throw new Exception("Unable to add attribute, error code=" + result);
            }
        }

        /// <summary>
        /// Not implemented, throws a <see cref="NotImplementedException"/>.
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Contains(RadiusAttribute item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            for (uint i = 0; i < this.Count; i++)
            {
                // warning: this is a hack and assumes that the dwAttrType is the first item in the RADIUS_ATTRIBUTE struct.
                if ((uint)Marshal.ReadInt32(this.radiusAttributeArray.AttributeAt(this.radiusAttributeArrayPtr, i)) == item.AttributeId)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public void CopyTo(RadiusAttribute[] array, int arrayIndex)
        {
            for (var i = 0; i < this.Count; i++)
            {
                array[i + arrayIndex] = this[i];
            }
        }

        /// <inheritdoc/>
        public bool Remove(RadiusAttribute item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var pos = this.IndexOf(item);
            if (pos > -1)
            {
                return this.radiusAttributeArray.RemoveAt(this.radiusAttributeArrayPtr, (uint)pos) == 0;
            }

            return false;
        }

        /// <inheritdoc/>
        public int IndexOf(RadiusAttribute item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            for (uint i = 0; i < this.Count; i++)
            {
                // warning: this is a hack and assumes that the dwAttrType is the first item in the RADIUS_ATTRIBUTE struct.
                if ((uint)Marshal.ReadInt32(this.radiusAttributeArray.AttributeAt(this.radiusAttributeArrayPtr, i)) == item.AttributeId)
                {
                    return (int)i;
                }
            }

            return -1;
        }

        /// <inheritdoc/>
        public void Insert(int index, RadiusAttribute item)
        {
            if (index < 0 || index > this.Count - 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var result = this.radiusAttributeArray.InsertAt(this.radiusAttributeArrayPtr, (uint)index, item.GetNativeAttribute());
            switch (result)
            {
                case 0:
                    return; // success
                case 5:
                case 0x80070005:
                case 0x80000009:
                    throw new UnauthorizedAccessException(
                        "The NPS Extension DLL attempted to add an attribute that Extension DLLs are not allowed to add, or the DLL attempted to add an additional instance of an attribute of which only a single instance is allowed.");
                case 87:
                    throw new ArgumentException("The index is out of range.");
                default:
                    throw new Exception("Unable to add attribute, error code=" + result);
            }
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            if (index < 0 || index > this.Count - 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            var result = this.radiusAttributeArray.RemoveAt(this.radiusAttributeArrayPtr, (uint)index);
            switch (result)
            {
                case 0:
                    return; // success
                case 5:
                    throw new UnauthorizedAccessException("The specified attribute is read-only.");
                case 87:
                    throw new ArgumentException("The index is out of range.");
            }
        }
    }
}
