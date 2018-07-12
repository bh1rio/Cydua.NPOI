using System.Collections.Generic;
using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record.Chart;
using System.Diagnostics;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// AXISPARENT = AxisParent Begin Pos [AXES] 1*4CRT End
    /// </summary>
    public class AxisParentAggregate: ChartRecordAggregate
    {
        private AxisParentRecord axisPraent = null;
        private PosRecord pos = null;
        private AxesAggregate axes = null;
        private List<CRTAggregate> crtList = new List<CRTAggregate>();

        public AxisParentRecord AxisParent
        {
            get { return axisPraent; }
        }

        public AxisParentAggregate(RecordStream rs, ChartRecordAggregate container)
            : base(RuleName_AXISPARENT, container)
        {
            axisPraent = (AxisParentRecord)rs.GetNext();
            rs.GetNext();
            pos = (PosRecord)rs.GetNext();
            if (ChartFormatRecord.sid != rs.PeekNextChartSid())
            {
                try
                {
                    axes = new AxesAggregate(rs, this);
                }
                catch
                {
                    Debug.Print("not find axes rule records");
                    axes = null;
                }
            }
            Debug.Assert(ChartFormatRecord.sid == rs.PeekNextChartSid());
            while (ChartFormatRecord.sid == rs.PeekNextChartSid())
            {
                crtList.Add(new CRTAggregate(rs, this));
            }
            Record r = rs.GetNext();//EndRecord
            Debug.Assert(r.GetType() == typeof(EndRecord));
        }

        public override void VisitContainedRecords(RecordVisitor rv)
        {
            rv.VisitRecord(axisPraent);
            rv.VisitRecord(BeginRecord.instance);
            rv.VisitRecord(pos);
            if (axes != null)
                axes.VisitContainedRecords(rv);
            foreach (CRTAggregate crt in crtList)
                crt.VisitContainedRecords(rv);

            WriteEndBlock(rv);
            rv.VisitRecord(EndRecord.instance);
        }
    }
}
