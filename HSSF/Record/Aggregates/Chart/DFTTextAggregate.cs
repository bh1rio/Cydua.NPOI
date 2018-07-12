using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record.Chart;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// DFTTEXT = [DataLabExt StartObject] DefaultText ATTACHEDLABEL [EndObject]
    /// </summary>
    public class DFTTextAggregate : ChartRecordAggregate
    {
        private DataLabExtRecord dataLabExt = null;
        private ChartStartObjectRecord startObject = null;
        private DefaultTextRecord defaultText = null;
        private AttachedLabelAggregate attachedLabel = null;
        private ChartEndObjectRecord endObject = null;

        public DefaultTextRecord DefaultText
        {
            get { return defaultText; }
        }

        public DFTTextAggregate(RecordStream rs, ChartRecordAggregate container)
            : base(RuleName_DFTTEXT, container)
        {
            if (rs.PeekNextChartSid() == DataLabExtRecord.sid)
            {
                dataLabExt = (DataLabExtRecord)rs.GetNext();
                startObject = (ChartStartObjectRecord)rs.GetNext();
            }
            defaultText = (DefaultTextRecord)rs.GetNext();
            attachedLabel = new AttachedLabelAggregate(rs, this);

            if (startObject != null)
                endObject = (ChartEndObjectRecord)rs.GetNext();
        }
        public override void VisitContainedRecords(RecordVisitor rv)
        {
            if (dataLabExt != null)
            {
                rv.VisitRecord(dataLabExt);
                rv.VisitRecord(startObject);
                IsInStartObject = true;
            }

            rv.VisitRecord(defaultText);
            attachedLabel.VisitContainedRecords(rv);

            if (endObject != null)
            {
                rv.VisitRecord(endObject);
                IsInStartObject = false;
            }
        }
    }
}
