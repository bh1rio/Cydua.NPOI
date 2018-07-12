using System.Collections.Generic;
using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record.Chart;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// AXS = [IFmtRecord] [Tick] [FontX] *4(AxisLine LineFormat) [AreaFormat] 
    /// [GELFRAME] *4SHAPEPROPS [TextPropsStream *ContinueFrt12]
    /// </summary>
    public class AXSAggregate : ChartRecordAggregate
    {
        private IFmtRecordRecord ifmt = null;
        private TickRecord tick = null;
        private FontXRecord fontx = null;
        private List<AxisLineRecord> axisLines = new List<AxisLineRecord>();
        private List<LineFormatRecord> lineFormats = new List<LineFormatRecord>();
        private AreaFormatRecord areaFormat = null;
        private GelFrameAggregate gelFrame = null;
        private List<ShapePropsAggregate> shapes = new List<ShapePropsAggregate>();
        private TextPropsStreamRecord textProps = null;
        private List<ContinueFrt12Record> continues = new List<ContinueFrt12Record>();

        public AXSAggregate(RecordStream rs, ChartRecordAggregate container)
            : base(RuleName_AXS, container) 
        {
            if (rs.PeekNextChartSid() == IFmtRecordRecord.sid)
                ifmt = (IFmtRecordRecord)rs.GetNext();
            if (rs.PeekNextChartSid() == TickRecord.sid)
                tick = (TickRecord)rs.GetNext();
            if (rs.PeekNextChartSid() == FontXRecord.sid)
                fontx = (FontXRecord)rs.GetNext();
            if (rs.PeekNextChartSid() == AxisLineRecord.sid)
            {
                while (rs.PeekNextChartSid() == AxisLineRecord.sid)
                {
                    axisLines.Add((AxisLineRecord)rs.GetNext());
                    lineFormats.Add((LineFormatRecord)rs.GetNext());
                }
            }

            if (rs.PeekNextChartSid() == AreaFormatRecord.sid)
                areaFormat = (AreaFormatRecord)rs.GetNext();

            if (rs.PeekNextChartSid() == GelFrameRecord.sid)
                gelFrame = new GelFrameAggregate(rs, this);
            if (rs.PeekNextChartSid() == ShapePropsStreamRecord.sid)
            {
                while (rs.PeekNextChartSid() == ShapePropsStreamRecord.sid)
                {
                    shapes.Add(new ShapePropsAggregate(rs,this));
                }
            }
            if (rs.PeekNextChartSid() == TextPropsStreamRecord.sid)
            {
                textProps = (TextPropsStreamRecord)rs.GetNext();
                while (rs.PeekNextChartSid() == ContinueFrt12Record.sid)
                {
                    continues.Add((ContinueFrt12Record)rs.GetNext());
                }
            }
        }

        public override void VisitContainedRecords(RecordVisitor rv)
        {
            if (ifmt != null) rv.VisitRecord(ifmt);
            if (tick != null) rv.VisitRecord(tick);
            if (fontx != null) rv.VisitRecord(fontx);
            for (int i = 0; i < axisLines.Count; i++)
            {
                rv.VisitRecord(axisLines[i]);
                rv.VisitRecord(lineFormats[i]);
            }

            if (areaFormat != null) rv.VisitRecord(areaFormat);
            if (gelFrame != null) gelFrame.VisitContainedRecords(rv);
            foreach (ShapePropsAggregate shape in shapes)
                shape.VisitContainedRecords(rv);
            if (textProps != null)
            {
                rv.VisitRecord(textProps);
                foreach (ContinueFrt12Record c in continues)
                    rv.VisitRecord(c);
            }
        }
    }
}
