using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record.Chart;
using System.Diagnostics;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// DROPBAR = DropBar Begin LineFormat AreaFormat [GELFRAME] [SHAPEPROPS] End
    /// </summary>
    public class DropBarAggregate : ChartRecordAggregate
    {
        private DropBarRecord dropBar = null;
        private LineFormatRecord lineFormat = null;
        private AreaFormatRecord areaFormat = null;
        private GelFrameAggregate gelFrame = null;
        private ShapePropsAggregate shapProps = null;
        public DropBarAggregate(RecordStream rs, ChartRecordAggregate container)
            : base(RuleName_DROPBAR, container)
        {
            dropBar = (DropBarRecord)rs.GetNext();
            rs.GetNext();
            lineFormat = (LineFormatRecord)rs.GetNext();
            areaFormat = (AreaFormatRecord)rs.GetNext();

            if (rs.PeekNextChartSid() == GelFrameRecord.sid)
            {
                gelFrame = new GelFrameAggregate(rs, this);
            }
            if (rs.PeekNextChartSid() == ShapePropsStreamRecord.sid)
            {
                shapProps = new ShapePropsAggregate(rs, this);
            }

            Record r = rs.GetNext();//EndRecord
            Debug.Assert(r.GetType() == typeof(EndRecord));
        }

        public override void VisitContainedRecords(RecordVisitor rv)
        {
            rv.VisitRecord(dropBar);
            rv.VisitRecord(BeginRecord.instance);
            rv.VisitRecord(lineFormat);
            rv.VisitRecord(areaFormat);

            if (gelFrame != null)
                gelFrame.VisitContainedRecords(rv);
            if (shapProps != null)
                shapProps.VisitContainedRecords(rv);

            WriteEndBlock(rv);
            rv.VisitRecord(EndRecord.instance);
        }
    }
}
