using Cydua.NPOI.HSSF.Record.Chart;
using Cydua.NPOI.HSSF.Model;
using System.Diagnostics;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// FRAME = Frame Begin LineFormat AreaFormat [GELFRAME] [SHAPEPROPS] End
    /// </summary>
    public class FrameAggregate : ChartRecordAggregate
    {
        private FrameRecord frame = null;
        private LineFormatRecord lineFormat = null;
        private AreaFormatRecord areaFormat = null;
        private GelFrameAggregate gelFrame = null;
        private ShapePropsAggregate shapeProps = null;
        public FrameAggregate(RecordStream rs, ChartRecordAggregate container)
            : base(RuleName_FRAME, container)
        {
            frame = (FrameRecord)rs.GetNext();
            rs.GetNext();//BeginRecord
            lineFormat = (LineFormatRecord)rs.GetNext();
            areaFormat = (AreaFormatRecord)rs.GetNext();
            if (rs.PeekNextChartSid() == GelFrameRecord.sid)
            {
                gelFrame = new GelFrameAggregate(rs, this);
            }
            
            if (rs.PeekNextChartSid() == ShapePropsStreamRecord.sid)
            {
                shapeProps = new ShapePropsAggregate(rs, this);
            }
            
            Record r = rs.GetNext();//EndRecord
            Debug.Assert(r.GetType() == typeof(EndRecord));
        }

        public override void VisitContainedRecords(RecordVisitor rv)
        {
            rv.VisitRecord(frame);
            rv.VisitRecord(BeginRecord.instance);
            rv.VisitRecord(lineFormat);
            rv.VisitRecord(areaFormat);
            if (gelFrame != null)
                gelFrame.VisitContainedRecords(rv);

            //TODO: write StartBlockRecord

            if (shapeProps != null)
            {
                //WriteStartBlock(rv);
                shapeProps.VisitContainedRecords(rv);
            }
            WriteEndBlock(rv);
            rv.VisitRecord(EndRecord.instance);
        }

        protected override bool ShoudWriteStartBlock()
        {
            if (IsInStartObject)
                return false;
            return false;
        }
    }
}
