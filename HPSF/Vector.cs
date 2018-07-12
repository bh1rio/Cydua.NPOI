using System;
using Cydua.NPOI.Util;

namespace Cydua.NPOI.HPSF
{
    public class Vector
    {
        private short _type;

        private TypedPropertyValue[] _values;

        public Vector(byte[] data, int startOffset, short type)
        {
            this._type = type;
            Read(data, startOffset);
        }

        public Vector(short type)
        {
            this._type = type;
        }

        public int Read(byte[] data, int startOffset)
        {
            int offset = startOffset;

            long longLength = LittleEndian.GetUInt(data, offset);
            offset += LittleEndian.INT_SIZE;

            if (longLength > int.MaxValue)
                throw new InvalidOperationException("Vector is too long -- "
                        + longLength);
            int length = (int)longLength;

            _values = new TypedPropertyValue[length];

            if (_type == Variant.VT_VARIANT)
            {
                for (int i = 0; i < length; i++)
                {
                    TypedPropertyValue value = new TypedPropertyValue();
                    offset += value.Read(data, offset);
                    _values[i] = value;
                }
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    TypedPropertyValue value = new TypedPropertyValue(_type, null);
                    // be aware: not padded here
                    offset += value.ReadValue(data, offset);
                    _values[i] = value;
                }
            }
            return offset - startOffset;
        }

        public TypedPropertyValue[] Values
        {
            get { return _values; }
        }
    }
}