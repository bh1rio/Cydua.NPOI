using System;
using System.Collections.Generic;
using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record.Chart;
using System.Diagnostics;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// AXES = [IVAXIS DVAXIS [SERIESAXIS] / DVAXIS DVAXIS] *3ATTACHEDLABEL [PlotArea FRAME]
    /// </summary>
    public class AxesAggregate : ChartRecordAggregate
    {
        private IVAxisAggregate ivaxis;
        private DVAxisAggregate dvaxis;

        private DVAxisAggregate dvaxisSecond;
        private SeriesAxisAggregate seriesAxis;

        private List<AttachedLabelAggregate> attachedLabelList = new List<AttachedLabelAggregate>();
        private PlotAreaRecord plotArea;
        private FrameAggregate frame;

        public AxesAggregate(RecordStream rs, ChartRecordAggregate container)
            : base(RuleName_AXES, container)
        {
            if (rs.PeekNextChartSid() == AxisRecord.sid)
            {
                AxisRecord axis = (AxisRecord)rs.GetNext();
                rs.GetNext();
                int sid = rs.PeekNextChartSid();
                if (sid == CatSerRangeRecord.sid)
                {
                    ivaxis = new IVAxisAggregate(rs, this, axis);
                }
                else if (sid == ValueRangeRecord.sid)
                {
                    dvaxis = new DVAxisAggregate(rs, this, axis);
                }
                else
                    throw new InvalidOperationException(string.Format("Invalid record sid=0x{0:X}. Shoud be CatSerRangeRecord or ValueRangeRecord", sid));

                Debug.Assert(rs.PeekNextChartSid() == AxisRecord.sid);
                dvaxisSecond = new DVAxisAggregate(rs, this, null);
                if (rs.PeekNextChartSid() == AxisRecord.sid)
                    seriesAxis = new SeriesAxisAggregate(rs, this);

                while (rs.PeekNextChartSid() == TextRecord.sid)
                {
                    attachedLabelList.Add(new AttachedLabelAggregate(rs, this));
                }
                if (rs.PeekNextChartSid() == PlotAreaRecord.sid)
                {
                    plotArea = (PlotAreaRecord)rs.GetNext();
                    if (rs.PeekNextChartSid() == FrameRecord.sid)
                        frame = new FrameAggregate(rs, this);
                }
            }
        }

        public override void VisitContainedRecords(RecordVisitor rv)
        {
            if (ivaxis != null)
                ivaxis.VisitContainedRecords(rv);
            if (dvaxis != null)
                dvaxis.VisitContainedRecords(rv);
            dvaxisSecond.VisitContainedRecords(rv);
            if (seriesAxis != null)
                seriesAxis.VisitContainedRecords(rv);

            foreach (AttachedLabelAggregate al in attachedLabelList)
                al.VisitContainedRecords(rv);
            if (plotArea != null)
            {
                rv.VisitRecord(plotArea);
                if (frame != null)
                    frame.VisitContainedRecords(rv);
            }
        }
    }
}
