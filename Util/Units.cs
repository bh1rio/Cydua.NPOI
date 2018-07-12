using System;

namespace Cydua.NPOI.Util
{

    /**
     * @author Yegor Kozlov
     */
    public class Units
    {
        public static int EMU_PER_PIXEL = 9525;
        public static int EMU_PER_POINT = 12700;

        public static int ToEMU(double value)
        {
            return (int)Math.Round(EMU_PER_POINT * value);
        }

        public static double ToPoints(long emu)
        {
            return (double)emu / EMU_PER_POINT;
        }

        /**
         * Converts a value of type FixedPoint to a decimal number
         *
         * @param fixedPoint
         * @return decimal number
         * 
         * @see <a href="http://msdn.microsoft.com/en-us/library/dd910765(v=office.12).aspx">[MS-OSHARED] - 2.2.1.6 FixedPoint</a>
         */
        public static double FixedPointToDecimal(int fixedPoint) {
        int i = (fixedPoint >> 16);
        int f = (fixedPoint >> 0) & 0xFFFF;
        double decimal1 = (i + f/65536.0);
        return decimal1;
    }
    }
}