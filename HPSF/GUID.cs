using Cydua.NPOI.Util;

namespace Cydua.NPOI.HPSF
{
    public class GUID
    {
        public const int SIZE = 16;

        private int _data1;
        private short _data2;
        private short _data3;
        private long _data4;
        public GUID(byte[] data, int offset)
        {
            _data1 = LittleEndian.GetInt(data, offset + 0);
            _data2 = LittleEndian.GetShort(data, offset + 4);
            _data3 = LittleEndian.GetShort(data, offset + 6);
            _data4 = LittleEndian.GetLong(data, offset + 8);
        }
    }
}