using Cydua.NPOI.HSSF.Model;
using System.Diagnostics;
using Cydua.NPOI.HSSF.Record.Chart;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// SERIESAXIS = Axis Begin [CatSerRange] AXS [CRTMLFRT] End
    /// </summary>
    public class SeriesAxisAggregate: ChartRecordAggregate
    {
        private AxisRecord axis;
        private CatSerRangeRecord catSerRange;
        private AXSAggregate axs;
        private CrtMlFrtAggregate crtmlfrt;
        public SeriesAxisAggregate(RecordStream rs, ChartRecordAggregate container)
            : base("SERIESAXIS", container)
        {
            axis = (AxisRecord)rs.GetNext();
            rs.GetNext();

            if (rs.PeekNextChartSid() == CatSerRangeRecord.sid)
                catSerRange = (CatSerRangeRecord)rs.GetNext();

            axs = new AXSAggregate(rs, this);
            if (rs.PeekNextChartSid() == CrtMlFrtRecord.sid)
                crtmlfrt = new CrtMlFrtAggregate(rs, this);

            Record r = rs.GetNext();//EndRecord
            Debug.Assert(r.GetType() == typeof(EndRecord));
        }

        public override void VisitContainedRecords(RecordVisitor rv)
        {
            rv.VisitRecord(axis);
            rv.VisitRecord(BeginRecord.instance);

            if (catSerRange != null)
                rv.VisitRecord(catSerRange);

            axs.VisitContainedRecords(rv);
            if (crtmlfrt != null)
                crtmlfrt.VisitContainedRecords(rv);

            WriteEndBlock(rv);
            rv.VisitRecord(EndRecord.instance);
        }
    }
}
