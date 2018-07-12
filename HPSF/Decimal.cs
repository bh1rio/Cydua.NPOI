using Cydua.NPOI.Util;

namespace Cydua.NPOI.HPSF
{
    public class Decimal
    {
        public const int SIZE = 16;

        private short field_1_wReserved;
        private byte field_2_scale;
        private byte field_3_sign;
        private int field_4_hi32;
        private long field_5_lo64;

        public Decimal(byte[] data, int startOffset)
        {
            int offset = startOffset;

            field_1_wReserved = LittleEndian.GetShort(data, offset);
            offset += LittleEndian.SHORT_SIZE;

            field_2_scale = data[offset];
            offset += LittleEndian.BYTE_SIZE;

            field_3_sign = data[offset];
            offset += LittleEndian.BYTE_SIZE;

            field_4_hi32 = LittleEndian.GetInt(data, offset);
            offset += LittleEndian.INT_SIZE;

            field_5_lo64 = LittleEndian.GetLong(data, offset);
            offset += LittleEndian.LONG_SIZE;
        }
    }
}