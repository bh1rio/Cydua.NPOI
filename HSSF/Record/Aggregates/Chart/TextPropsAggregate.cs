using System.Collections.Generic;
using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record.Chart;
namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// TEXTPROPS = (RichTextStream / TextPropsStream) *ContinueFrt12
    /// </summary>
    public class TextPropsAggregate : ChartRecordAggregate
    {
        private TextPropsStreamRecord textPropsStream = null;
        private RichTextStreamRecord richTextStream = null;
        private List<ContinueFrt12Record> continues = new List<ContinueFrt12Record>();
        public TextPropsAggregate(RecordStream rs, ChartRecordAggregate container)
            : base(RuleName_TEXTPROPS, container)
        {
            if (rs.PeekNextChartSid() == TextPropsStreamRecord.sid)
                textPropsStream = (TextPropsStreamRecord)rs.GetNext();
            else
                richTextStream = (RichTextStreamRecord)rs.GetNext();

            if (rs.PeekNextChartSid() == ContinueFrt12Record.sid)
            {
                while (rs.PeekNextChartSid() == ContinueFrt12Record.sid)
                {
                    continues.Add((ContinueFrt12Record)rs.GetNext());
                }
            }
        }
        public override void VisitContainedRecords(RecordVisitor rv)
        {
            WriteStartBlock(rv);
            if(textPropsStream!=null)
                rv.VisitRecord(textPropsStream);
            if (richTextStream != null)
                rv.VisitRecord(richTextStream);
            foreach (ContinueFrt12Record cr in continues)
                rv.VisitRecord(cr);
        }
    }
}
