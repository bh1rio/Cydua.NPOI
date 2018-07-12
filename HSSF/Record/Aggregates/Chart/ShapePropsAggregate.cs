using System.Collections.Generic;
using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record.Chart;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// SHAPEPROPS = ShapePropsStream *ContinueFrt12
    /// </summary>
    public class ShapePropsAggregate : ChartRecordAggregate
    {
        private ShapePropsStreamRecord shapProps = null;
        private List<ContinueFrt12Record> continues = new List<ContinueFrt12Record>();
        public ShapePropsAggregate(RecordStream rs, ChartRecordAggregate container)
            : base(RuleName_SHAPEPROPS, container)
        {
            shapProps = (ShapePropsStreamRecord)rs.GetNext();
            if (rs.PeekNextChartSid() == CrtMlFrtContinueRecord.sid)
            {
                while (rs.PeekNextChartSid() == CrtMlFrtContinueRecord.sid)
                {
                    continues.Add((ContinueFrt12Record)rs.GetNext());
                }
            }
        }
        public override void VisitContainedRecords(RecordVisitor rv)
        {
            WriteStartBlock(rv);
            rv.VisitRecord(shapProps);
            foreach (ContinueFrt12Record cr in continues)
                rv.VisitRecord(cr);
        }
    }
}
