using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record.Chart;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// AXM = YMult StartObject ATTACHEDLABEL EndObject
    /// </summary>
    public class AXMAggregate : ChartRecordAggregate
    {
        private YMultRecord yMult = null;
        private ChartStartObjectRecord startObject = null;
        private AttachedLabelAggregate attachedLabel = null;
        private ChartEndObjectRecord endObject = null;

        public AXMAggregate(RecordStream rs, ChartRecordAggregate container)
            : base(RuleName_AXM, container)
        {
            yMult = (YMultRecord)rs.GetNext();
            startObject = (ChartStartObjectRecord)rs.GetNext();
            attachedLabel = new AttachedLabelAggregate(rs, this);
            
            endObject = (ChartEndObjectRecord)rs.GetNext();
        }
        public override void VisitContainedRecords(RecordVisitor rv)
        {
            WriteStartBlock(rv);//??
            rv.VisitRecord(yMult);
            rv.VisitRecord(startObject);
            IsInStartObject = true;
            attachedLabel.VisitContainedRecords(rv);
            IsInStartObject = false;
            rv.VisitRecord(endObject);
        }
    }
}
