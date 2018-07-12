using System;
using Cydua.NPOI.Util;

namespace Cydua.NPOI.HPSF
{
    public class Array
    {
        internal class ArrayDimension
        {
            public const int SIZE = 8;

            private int _indexOffset;
            internal long _size;

            public ArrayDimension(byte[] data, int offset)
            {
                _size = LittleEndian.GetUInt(data, offset);
                _indexOffset = LittleEndian.GetInt(data, offset
                        + LittleEndian.INT_SIZE);
            }
        }

        internal class ArrayHeader
        {
            private ArrayDimension[] _dimensions;
            internal int _type;

            public ArrayHeader(byte[] data, int startOffset)
            {
                int offset = startOffset;

                _type = LittleEndian.GetInt(data, offset);
                offset += LittleEndian.INT_SIZE;

                long numDimensionsUnsigned = LittleEndian.GetUInt(data, offset);
                offset += LittleEndian.INT_SIZE;

                if (!(1 <= numDimensionsUnsigned && numDimensionsUnsigned <= 31))
                    throw new IllegalPropertySetDataException(
                            "Array dimension number " + numDimensionsUnsigned
                                    + " is not in [1; 31] range");
                int numDimensions = (int)numDimensionsUnsigned;

                _dimensions = new ArrayDimension[numDimensions];
                for (int i = 0; i < numDimensions; i++)
                {
                    _dimensions[i] = new ArrayDimension(data, offset);
                    offset += ArrayDimension.SIZE;
                }
            }

            public long NumberOfScalarValues
            {
                get
                {
                    long result = 1;
                    foreach (ArrayDimension dimension in _dimensions)
                        result *= dimension._size;
                    return result;
                }
            }

            public int Size
            {
                get
                {
                    return LittleEndian.INT_SIZE * 2 + _dimensions.Length
                           * ArrayDimension.SIZE;
                }
            }

            public int Type
            {
                get { return _type; }
            }
        }

        private ArrayHeader _header;
        private TypedPropertyValue[] _values;

        public Array()
        {
        }

        public Array(byte[] data, int offset)
        {
            Read(data, offset);
        }

        public int Read(byte[] data, int startOffset)
        {
            int offset = startOffset;

            _header = new ArrayHeader(data, offset);
            offset += _header.Size;

            long numberOfScalarsLong = _header.NumberOfScalarValues;
            if (numberOfScalarsLong > int.MaxValue)
                throw new InvalidOperationException(
                        "Sorry, but POI can't store array of properties with size of "
                                + numberOfScalarsLong + " in memory");
            int numberOfScalars = (int)numberOfScalarsLong;

            _values = new TypedPropertyValue[numberOfScalars];
            int type = _header._type;
            if (type == Variant.VT_VARIANT)
            {
                for (int i = 0; i < numberOfScalars; i++)
                {
                    TypedPropertyValue typedPropertyValue = new TypedPropertyValue();
                    offset += typedPropertyValue.Read(data, offset);
                }
            }
            else
            {
                for (int i = 0; i < numberOfScalars; i++)
                {
                    TypedPropertyValue typedPropertyValue = new TypedPropertyValue(
                            type, null);
                    offset += typedPropertyValue.ReadValuePadded(data, offset);
                }
            }

            return offset - startOffset;
        }
    }
}