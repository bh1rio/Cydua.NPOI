using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record.Chart;
using System.Diagnostics;

namespace Cydua.NPOI.HSSF.Record.Aggregates.Chart
{
    /// <summary>
    /// ATTACHEDLABEL = Text Begin Pos [FontX] [AlRuns] AI [FRAME] [ObjectLink] [DataLabExtContents] [CrtLayout12] [TEXTPROPS] [CRTMLFRT] End
    /// AI = BRAI [SeriesText]
    /// </summary>
    public class AttachedLabelAggregate : ChartRecordAggregate
    {
        private TextRecord text = null;
        private PosRecord pos = null;
        private FontXRecord fontX = null;
        private AlRunsRecord alRuns = null;
        private BRAIRecord brai = null;
        private SeriesTextRecord seriesText = null;
        private FrameAggregate frame = null;
        private ObjectLinkRecord objectLink = null;
        private DataLabExtContentsRecord dataLab = null;
        private CrtLayout12Record crtLayout = null;
        private TextPropsAggregate textProps = null;
        private CrtMlFrtAggregate crtMlFrt = null;

        private bool _isFirst;
        public bool IsFirst
        {
            get { return _isFirst; }
            set { _isFirst = value; }
        }

        public ObjectLinkRecord ObjectLink
        {
            get { return objectLink; }
        }

        public AttachedLabelAggregate(RecordStream rs, ChartRecordAggregate container)
            : base(RuleName_ATTACHEDLABEL, container)
        {
            ChartSheetAggregate cs = GetContainer<ChartSheetAggregate>(ChartRecordAggregate.RuleName_CHARTSHEET);
            _isFirst = cs.AttachLabelCount == 0;
            cs.AttachLabelCount++;
            text = (TextRecord)rs.GetNext();
            rs.GetNext();//BeginRecord
            pos = (PosRecord)rs.GetNext();
            if (rs.PeekNextChartSid() == FontXRecord.sid)
            {
                fontX = (FontXRecord)rs.GetNext();
            }
            if (rs.PeekNextChartSid() == AlRunsRecord.sid)
            {
                alRuns = (AlRunsRecord)rs.GetNext();
            }
            brai = (BRAIRecord)rs.GetNext();
            if (rs.PeekNextChartSid() == SeriesTextRecord.sid)
            {
                seriesText = (SeriesTextRecord)rs.GetNext();
            }
            if (rs.PeekNextChartSid() == FrameRecord.sid)
            {
                frame = new FrameAggregate(rs, this);
            }
            if (rs.PeekNextChartSid() == ObjectLinkRecord.sid)
            {
                objectLink = (ObjectLinkRecord)rs.GetNext();
            }
            if (rs.PeekNextChartSid() == DataLabExtContentsRecord.sid)
            {
                dataLab = (DataLabExtContentsRecord)rs.GetNext();
            }
            if (rs.PeekNextChartSid() == CrtLayout12Record.sid)
            {
                crtLayout = (CrtLayout12Record)rs.GetNext();
            }
            if (rs.PeekNextChartSid() == TextPropsStreamRecord.sid)
            {
                textProps = new TextPropsAggregate(rs, this);
            }
            if (rs.PeekNextChartSid() == CrtMlFrtRecord.sid)
            {
                crtMlFrt = new CrtMlFrtAggregate(rs, this);
            }
            Record r = rs.GetNext();//EndRecord
            Debug.Assert(r.GetType() == typeof(EndRecord));
        }
        public override void VisitContainedRecords(RecordVisitor rv)
        {
            rv.VisitRecord(text);
            rv.VisitRecord(BeginRecord.instance);
            rv.VisitRecord(pos);

            if (fontX != null)
                rv.VisitRecord(fontX);

            if (alRuns != null)
                rv.VisitRecord(alRuns);

            rv.VisitRecord(brai);

            if (seriesText != null)
                rv.VisitRecord(seriesText);

            if (frame != null)
                frame.VisitContainedRecords(rv);

            if (objectLink != null)
                rv.VisitRecord(objectLink);

            if (dataLab != null)
                rv.VisitRecord(dataLab);

            if (crtLayout != null)
                rv.VisitRecord(crtLayout);

            if (textProps != null)
                textProps.VisitContainedRecords(rv);

            if (crtMlFrt != null)
                crtMlFrt.VisitContainedRecords(rv);

            WriteEndBlock(rv);
            rv.VisitRecord(EndRecord.instance);
        }
    }
}
