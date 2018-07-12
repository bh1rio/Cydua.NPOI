using System;
using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record;
using System.Collections.Generic;

namespace Cydua.NPOI.HSSF.Record.Aggregates
{
    /**
     * Manages the all the records associated with a 'Custom View Settings' sub-stream.<br/>
     * Includes the Initial USERSVIEWBEGIN(0x01AA) and USERSVIEWEND(0x01AB).
     * 
     * @author Josh Micich
     */
    public class CustomViewSettingsRecordAggregate : RecordAggregate
    {

        private Record _begin;
        private Record _end;
        /**
         * All the records between BOF and EOF
         */
        private List<RecordBase> _recs;
        private PageSettingsBlock _psBlock;

        public CustomViewSettingsRecordAggregate(RecordStream rs)
        {
            _begin = rs.GetNext();
            if (_begin.Sid != UserSViewBegin.sid)
            {
                throw new InvalidOperationException("Bad begin record");
            }
            List<RecordBase> temp = new List<RecordBase>();
            while (rs.PeekNextSid() != UserSViewEnd.sid)
            {
                if (PageSettingsBlock.IsComponentRecord(rs.PeekNextSid()))
                {
                    if (_psBlock != null)
                    {
                        throw new InvalidOperationException(
                                "Found more than one PageSettingsBlock in custom view Settings sub-stream");
                    }
                    _psBlock = new PageSettingsBlock(rs);
                    temp.Add(_psBlock);
                    continue;
                }
                temp.Add(rs.GetNext());
            }
            _recs = temp;
            _end = rs.GetNext(); // no need to save EOF in field
            if (_end.Sid != UserSViewEnd.sid)
            {
                throw new InvalidOperationException("Bad custom view Settings end record");
            }
        }

        public override void VisitContainedRecords(RecordVisitor rv)
        {
            if (_recs.Count == 0)
            {
                return;
            }
            rv.VisitRecord(_begin);
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
            rv.VisitRecord(_end);
        }

        public static bool IsBeginRecord(int sid)
        {
            return sid == UserSViewBegin.sid;
        }

        public void Append(RecordBase r)
        {
            _recs.Add(r);
        }
    }
}

