using Cydua.NPOI.Util;

namespace Cydua.NPOI.HPSF
{
    internal class Currency
    {
        public const int SIZE = 8;

        private byte[] _value;

        public Currency(byte[] data, int offset)
        {
            _value = LittleEndian.GetByteArray(data, offset, SIZE);
        }
    }
}