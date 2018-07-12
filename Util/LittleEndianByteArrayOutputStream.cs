namespace Cydua.NPOI.Util
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Adapts a plain byte array to <see cref="T:NPOI.Util.ILittleEndianOutput"/>
    /// </summary>
    /// <remarks>@author Josh Micich</remarks>
    public class LittleEndianByteArrayOutputStream : ILittleEndianOutput, IDelayableLittleEndianOutput
    {
        private byte[] _buf;
        private int _endIndex;
        private int _writeIndex;

        public LittleEndianByteArrayOutputStream(byte[] buf, int startOffset, int maxWriteLen)
        {
            if (startOffset < 0 || startOffset > buf.Length)
            {
                throw new ArgumentException("Specified startOffset (" + startOffset
                        + ") is out of allowable range (0.." + buf.Length + ")");
            }
            _buf = buf;
            _writeIndex = startOffset;
            _endIndex = startOffset + maxWriteLen;
            if (_endIndex < startOffset || _endIndex > buf.Length)
            {
                throw new ArgumentException("calculated end index (" + _endIndex
                        + ") is out of allowable range (" + _writeIndex + ".." + buf.Length + ")");
            }
        }
        public LittleEndianByteArrayOutputStream(byte[] buf, int startOffset) :
            this(buf, startOffset, buf.Length - startOffset)
        {

        }

        private void CheckPosition(int i)
        {
            if (i > _endIndex - _writeIndex)
            {
                throw new RuntimeException(string.Format(CultureInfo.InvariantCulture, "Buffer overrun i={0};endIndex={1};writeIndex={2}", i, _endIndex, _writeIndex));
            }
        }

        public void WriteByte(int v)
        {
            CheckPosition(1);
            _buf[_writeIndex++] = (byte)v;
        }

        public void WriteDouble(double v)
        {
            WriteLong(BitConverter.DoubleToInt64Bits(v));
        }

        public void WriteInt(int v)
        {
            CheckPosition(4);
            int i = _writeIndex;
            _buf[i++] = (byte)((v >> 0) & 0xFF);
            _buf[i++] = (byte)((v >> 8) & 0xFF);
            _buf[i++] = (byte)((v >> 16) & 0xFF);
            _buf[i++] = (byte)((v >> 24) & 0xFF);
            _writeIndex = i;
        }

        public void WriteLong(long v)
        {
            WriteInt((int)(v >> 0));
            WriteInt((int)(v >> 32));
        }

        public void WriteShort(int v)
        {
            CheckPosition(2);
            int i = _writeIndex;
            _buf[i++] = (byte)((v >> 0) & 0xFF);
            _buf[i++] = (byte)((v >> 8) & 0xFF);
            _writeIndex = i;
        }
        public void Write(byte[] b)
        {
            int len = b.Length;
            CheckPosition(len);
            System.Array.Copy(b, 0, _buf, _writeIndex, len);
            _writeIndex += len;
        }
        public void Write(byte[] b, int offset, int len)
        {
            CheckPosition(len);
            System.Array.Copy(b, offset, _buf, _writeIndex, len);
            _writeIndex += len;
        }
        public int WriteIndex
        {
            get
            {
                return _writeIndex;
            }
        }
        public ILittleEndianOutput CreateDelayedOutput(int size)
        {
            CheckPosition(size);
            ILittleEndianOutput result = new LittleEndianByteArrayOutputStream(_buf, _writeIndex, size);
            _writeIndex += size;
            return result;
        }
    }
}