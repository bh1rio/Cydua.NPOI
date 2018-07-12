using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record.Chart;
using System.Diagnostics;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// DVAXIS = Axis Begin [ValueRange] [AXM] AXS [CRTMLFRT] End
    /// </summary>
    public class DVAxisAggregate: ChartRecordAggregate
    {
        private AxisRecord axis;
        private ValueRangeRecord valueRange;
        private AXMAggregate axm;
        private AXSAggregate axs;
        private CrtMlFrtAggregate crtmlfrt;

        public AxisRecord Axis
        {
            get { return axis; }
        }
        public DVAxisAggregate(RecordStream rs, ChartRecordAggregate container, AxisRecord axis)
            : base(RuleName_DVAXIS, container)
        {
            if (axis == null)
            {
                this.axis = (AxisRecord)rs.GetNext();
                rs.GetNext();
            }
            else
            {
                this.axis = axis;
            }

            if (rs.PeekNextChartSid() == ValueRangeRecord.sid)
                valueRange = (ValueRangeRecord)rs.GetNext();

            if (rs.PeekNextChartSid() == YMultRecord.sid)
                axm = new AXMAggregate(rs, this);

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
            if (valueRange != null)
                rv.VisitRecord(valueRange);
            if (axm != null)
                axm.VisitContainedRecords(rv);

            axs.VisitContainedRecords(rv);

            if (crtmlfrt != null)
                crtmlfrt.VisitContainedRecords(rv);

            WriteEndBlock(rv);
            rv.VisitRecord(EndRecord.instance);
        }
    }
}
