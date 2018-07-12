using System.Collections.Generic;
using Cydua.NPOI.HSSF.Record.Chart;
using Cydua.NPOI.HSSF.Model;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// CRTMLFRT = CrtMlFrt *CrtMlFrtContinue
    /// </summary>
    public class CrtMlFrtAggregate : ChartRecordAggregate
    {
        private CrtMlFrtRecord crtmlFrt = null;
        private List<CrtMlFrtContinueRecord> continues = new List<CrtMlFrtContinueRecord>();
        public CrtMlFrtAggregate(RecordStream rs, ChartRecordAggregate container)
            : base(RuleName_CRTMLFRT, container)
        {
            crtmlFrt = (CrtMlFrtRecord)rs.GetNext();
            if (rs.PeekNextChartSid() == CrtMlFrtContinueRecord.sid)
            {
                while (rs.PeekNextChartSid() == CrtMlFrtContinueRecord.sid)
                {
                    continues.Add((CrtMlFrtContinueRecord)rs.GetNext());
                }
            }
        }
        public override void VisitContainedRecords(RecordVisitor rv)
        {
            WriteStartBlock(rv);
            rv.VisitRecord(crtmlFrt);
            foreach (CrtMlFrtContinueRecord cr in continues)
                rv.VisitRecord(cr);
        }
    }
}
