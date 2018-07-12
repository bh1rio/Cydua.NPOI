using System.Collections.Generic;
using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record.Chart;
using System.Diagnostics;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// GELFRAME = 1*2GelFrame *Continue [PICF]
    /// PICF = Begin PicF End
    /// </summary>
    public class GelFrameAggregate : ChartRecordAggregate
    {
        private GelFrameRecord gelFrame1;
        private GelFrameRecord gelFrame2;
        private List<ContinueRecord> continues = new List<ContinueRecord>();
        private PicFRecord picF;
        public GelFrameAggregate(RecordStream rs, ChartRecordAggregate container)
            : base(RuleName_GELFRAME, container)
        {
            gelFrame1 = (GelFrameRecord)rs.GetNext();
            int sid = rs.PeekNextChartSid();
            if (sid == GelFrameRecord.sid)
            {
                gelFrame2 = (GelFrameRecord)rs.GetNext();
                sid = rs.PeekNextChartSid();
            }
            if (sid == ContinueRecord.sid)
            {
                while (rs.PeekNextChartSid() == ContinueRecord.sid)
                {
                    continues.Add((ContinueRecord)rs.GetNext());
                }
            }
            if (rs.PeekNextChartSid() == BeginRecord.sid)
            {
                rs.GetNext();
                picF = (PicFRecord)rs.GetNext();
                Record r = rs.GetNext();//EndRecord
                Debug.Assert(r.GetType() == typeof(EndRecord));
            }
        }
        public override void VisitContainedRecords(RecordVisitor rv)
        {
            rv.VisitRecord(gelFrame1);
            if (gelFrame2 != null)
                rv.VisitRecord(gelFrame2);
            foreach (ContinueRecord cr in continues)
                rv.VisitRecord(cr);
            if (picF != null)
            {
                rv.VisitRecord(BeginRecord.instance);
                rv.VisitRecord(picF);
                rv.VisitRecord(EndRecord.instance);
            }
        }
    }
}
