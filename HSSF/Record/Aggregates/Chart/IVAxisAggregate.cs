using System.Collections.Generic;
using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record.Chart;
using System.Diagnostics;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// IVAXIS = Axis Begin [CatSerRange] AxcExt [CatLab] AXS [CRTMLFRT] End
    /// </summary>
    public class IVAxisAggregate: ChartRecordAggregate
    {
        private AxisRecord axis;
        private CatSerRangeRecord catSerRange;
        private AxcExtRecord axcExt;
        private CatLabRecord catLab;
        private AXSAggregate axs;
        //more than one CrtMlFrtRecord?
        private List<CrtMlFrtAggregate> crtmlfrtList = new List<CrtMlFrtAggregate>();


        public IVAxisAggregate(RecordStream rs, ChartRecordAggregate container, AxisRecord axis)
            : base(RuleName_IVAXIS, container)
        {
            this.axis = axis;

            if (rs.PeekNextChartSid() == CatSerRangeRecord.sid)
                catSerRange = (CatSerRangeRecord)rs.GetNext();

            Debug.Assert(rs.PeekNextChartSid() == AxcExtRecord.sid);
            axcExt = (AxcExtRecord)rs.GetNext();

            if (rs.PeekNextChartSid() == CatLabRecord.sid)
                catLab = (CatLabRecord)rs.GetNext();

            axs = new AXSAggregate(rs, this);
            while (rs.PeekNextChartSid() == CrtMlFrtRecord.sid)
                crtmlfrtList.Add(new CrtMlFrtAggregate(rs, this));

            Record r = rs.GetNext();//EndRecord
            Debug.Assert(r.GetType() == typeof(EndRecord));
        }

        public override void VisitContainedRecords(RecordVisitor rv)
        {
            rv.VisitRecord(axis);
            rv.VisitRecord(BeginRecord.instance);

            if (catSerRange != null)
                rv.VisitRecord(catSerRange);

            rv.VisitRecord(axcExt);
            if (catLab != null)
            {
                WriteStartBlock(rv);
                rv.VisitRecord(catLab);
            }
            axs.VisitContainedRecords(rv);

            foreach (CrtMlFrtAggregate crtmlfrt in crtmlfrtList)
                crtmlfrt.VisitContainedRecords(rv);

            WriteEndBlock(rv);
            rv.VisitRecord(EndRecord.instance);
        }
    }
}
