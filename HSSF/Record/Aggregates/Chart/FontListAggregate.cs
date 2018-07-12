using System.Collections.Generic;
using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record.Chart;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// FONTLIST = FrtFontList StartObject *(Font [Fbi]) EndObject
    /// </summary>
    public class FontListAggregate : ChartRecordAggregate
    {
        private FrtFontListRecord frtFontList = null;
        private ChartStartObjectRecord startObject = null;
        private Dictionary<FontRecord, FbiRecord> dicFonts = new Dictionary<FontRecord, FbiRecord>();
        private ChartEndObjectRecord endObject = null;
        public FontListAggregate(RecordStream rs, ChartRecordAggregate container)
            : base(RuleName_FONTLIST, container)
        {
            frtFontList = (FrtFontListRecord)rs.GetNext();
            startObject = (ChartStartObjectRecord)rs.GetNext();
            FontRecord f = null;
            FbiRecord fbi = null;
            while (rs.PeekNextChartSid() == FontRecord.sid)
            {
                f = (FontRecord)rs.GetNext();
                if (rs.PeekNextChartSid() == FbiRecord.sid)
                {
                    fbi = (FbiRecord)rs.GetNext();
                }
                else
                {
                    fbi = null;
                }
                dicFonts.Add(f, fbi);
            }

            endObject = (ChartEndObjectRecord)rs.GetNext();
        }

        public override void VisitContainedRecords(RecordVisitor rv)
        {
            WriteStartBlock(rv);//??
            rv.VisitRecord(frtFontList);
            rv.VisitRecord(startObject);
            IsInStartObject = true;
            foreach (KeyValuePair<FontRecord, FbiRecord> kv in dicFonts)
            {
                rv.VisitRecord(kv.Key);
                if (kv.Value != null)
                    rv.VisitRecord(kv.Value);
            }
            IsInStartObject = false;
            rv.VisitRecord(endObject);
        }
    }
}
