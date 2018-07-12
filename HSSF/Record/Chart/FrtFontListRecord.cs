namespace Cydua.NPOI.HSSF.Record.Chart
{
    /// <summary>
    /// The FrtFontList record specifies font information used on the chart and specifies the 
    /// beginning of a collection of Font records as defined by the Chart Sheet Substream ABNF.
    /// </summary>
    /// <remarks>
    /// author: Antony liu (antony.apollo at gmail.com)
    /// </remarks>
    public class FrtFontListRecord : RowDataRecord
    {
        public const short sid = 0x85a;
        public FrtFontListRecord(RecordInputStream ris)
            : base(ris)
        {
        }

        protected override int DataSize
        {
            get
            {
                return base.DataSize;
            }
        }
        public override void Serialize(NPOI.Util.ILittleEndianOutput out1)
        {
            base.Serialize(out1);
        }

        public override short Sid
        {
            get { return sid; }
        }
    }
}
