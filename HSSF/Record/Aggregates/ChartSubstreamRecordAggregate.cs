using System;

using Cydua.NPOI.HSSF.Model;
using System.Collections.Generic;
using System.IO;

namespace Cydua.NPOI.HSSF.Record.Aggregates
{
    /**
     * Manages the all the records associated with a chart sub-stream.<br/>
     * Includes the Initial {@link BOFRecord} and {@link EOFRecord}.
     *
     * @author Josh Micich
     */
    public class ChartSubstreamRecordAggregate : RecordAggregate
    {

        private BOFRecord _bofRec;
        /**
         * All the records between BOF and EOF
         */
        private List<RecordBase> _recs;
        private PageSettingsBlock _psBlock;

        public ChartSubstreamRecordAggregate(RecordStream rs)
        {
            _bofRec = (BOFRecord)rs.GetNext();
            List<RecordBase> temp = new List<RecordBase>();
            while (rs.PeekNextClass() != typeof(EOFRecord))
            {
                Type a = rs.PeekNextClass();
                if (PageSettingsBlock.IsComponentRecord(rs.PeekNextSid()))
                {
                    if (_psBlock != null)
                    {
                        if (rs.PeekNextSid() == HeaderFooterRecord.sid)
                        {
                            // test samples: 45538_classic_Footer.xls, 45538_classic_Header.xls
                            _psBlock.AddLateHeaderFooter((HeaderFooterRecord)rs.GetNext());
                            continue;
                        }
                        throw new InvalidDataException(
                                "Found more than one PageSettingsBlock in chart sub-stream");
                    }
                    _psBlock = new PageSettingsBlock(rs);
                    temp.Add(_psBlock);
                    continue;
                }
                temp.Add(rs.GetNext());
            }
            _recs = temp;
            Record eof = rs.GetNext(); // no need to save EOF in field
            if (!(eof is EOFRecord))
            {
                throw new InvalidOperationException("Bad chart EOF");
            }
        }

        public override void VisitContainedRecords(RecordVisitor rv)
        {
            if (_recs.Count == 0)
            {
                return;
            }
            rv.VisitRecord(_bofRec);
            for (int i = 0; i < _recs.Count; i++)
            {
                RecordBase rb = _recs[i];
                if (rb is RecordAggregate)
                {
                    ((RecordAggregate)rb).VisitContainedRecords(rv);
                }
                else
                {
                    rv.VisitRecord((Record)rb);
                }
            }
            rv.VisitRecord(EOFRecord.instance);
        }

    }
}


