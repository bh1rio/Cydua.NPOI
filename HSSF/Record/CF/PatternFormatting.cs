/*
 * FontFormatting.java
 *
 * Created on January 22, 2008, 10:05 PM
 */
using System;
using System.Text;
using Cydua.NPOI.HSSF.Record;
using Cydua.NPOI.SS.UserModel;
using Cydua.NPOI.Util;

namespace Cydua.NPOI.HSSF.Record.CF
{
    /**
     * Pattern Formatting Block of the Conditional Formatting Rule Record.
     * 
     * @author Dmitriy Kumshayev
     */

    public class PatternFormatting : ICloneable
    {
        public PatternFormatting()
        {
            field_15_pattern_style = (short)0;
            field_16_pattern_color_indexes = (short)0;
        }

        /** Creates new FontFormatting */
        public PatternFormatting(RecordInputStream in1)
        {
            field_15_pattern_style = in1.ReadShort();
            field_16_pattern_color_indexes = in1.ReadShort();
        }

        // PATTERN FORMATING BLOCK
        // For Pattern Styles see constants at HSSFCellStyle (from NO_Fill to LEAST_DOTS)
        private short field_15_pattern_style;
        private static BitField FillPatternStyle = BitFieldFactory.GetInstance(0xFC00);

        private short field_16_pattern_color_indexes;
        private static BitField patternColorIndex = BitFieldFactory.GetInstance(0x007F);
        private static BitField patternBackgroundColorIndex = BitFieldFactory.GetInstance(0x3F80);

        /**
         * Get the Fill pattern 
         * @return Fill pattern
         */

        public FillPattern FillPattern
        {
            get
            {
                return (FillPattern)FillPatternStyle.GetShortValue(field_15_pattern_style);
            }
            set
            {
                field_15_pattern_style = FillPatternStyle.SetShortValue(field_15_pattern_style, (short)value);
            }
        }


        /**
         * Get the background Fill color
         * @see org.apache.poi.hssf.usermodel.HSSFPalette#GetColor(short)
         * @return Fill color
         */
        public short FillBackgroundColor
        {
            get
            {
                return patternBackgroundColorIndex.GetShortValue(field_16_pattern_color_indexes);
            }
            set
            {
                field_16_pattern_color_indexes =
                    patternBackgroundColorIndex.SetShortValue(field_16_pattern_color_indexes, value);
            }
        }

        /**
         * Get the foreground Fill color
         * @see org.apache.poi.hssf.usermodel.HSSFPalette#GetColor(short)
         * @return Fill color
         */
        public short FillForegroundColor
        {
            get
            {
                return patternColorIndex.GetShortValue(field_16_pattern_color_indexes);
            }
            set
            {
                field_16_pattern_color_indexes = patternColorIndex.SetShortValue(field_16_pattern_color_indexes, value);
            }
        }

        public override String ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("    [Pattern Formatting]\n");
            buffer.Append("          .Fillpattern= ").Append(StringUtil.ToHexString((int)FillPattern)).Append("\n");
            buffer.Append("          .fgcoloridx= ").Append(StringUtil.ToHexString(FillForegroundColor)).Append("\n");
            buffer.Append("          .bgcoloridx= ").Append(StringUtil.ToHexString(FillBackgroundColor)).Append("\n");
            buffer.Append("    [/Pattern Formatting]\n");
            return buffer.ToString();
        }

        public Object Clone()
        {
            PatternFormatting rec = new PatternFormatting();
            rec.field_15_pattern_style = field_15_pattern_style;
            rec.field_16_pattern_color_indexes = field_16_pattern_color_indexes;
            return rec;
        }

        public int Serialize(int offset, byte[] data)
        {
            LittleEndian.PutShort(data, offset, field_15_pattern_style);
            offset += 2;
            LittleEndian.PutShort(data, offset, field_16_pattern_color_indexes);
            offset += 2;
            return 4;
        }

        public void Serialize(ILittleEndianOutput out1)
        {
            out1.WriteShort(field_15_pattern_style);
            out1.WriteShort(field_16_pattern_color_indexes);
        }
    }
}
