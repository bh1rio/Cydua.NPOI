using Cydua.NPOI.HSSF.Record.Chart;
using Cydua.NPOI.HSSF.Model;
using System.Diagnostics;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// DAT = Dat Begin LD End
    /// </summary>
    public class DatAggregate : ChartRecordAggregate
    {
        private DatRecord dat = null;
        private LDAggregate ld = null;

        public DatAggregate(RecordStream rs, ChartRecordAggregate container)
            : base(RuleName_DAT, container)
        {
            dat = (DatRecord)rs.GetNext();
            rs.GetNext();
            ld = new LDAggregate(rs, this);

            Record r = rs.GetNext();//EndRecord
            Debug.Assert(r.GetType() == typeof(EndRecord));
        }

        public override void VisitContainedRecords(RecordVisitor rv)
        {
            rv.VisitRecord(dat);
            rv.VisitRecord(BeginRecord.instance);
            ld.VisitContainedRecords(rv);
            WriteEndBlock(rv);
            rv.VisitRecord(EndRecord.instance);
        }
    }
}
