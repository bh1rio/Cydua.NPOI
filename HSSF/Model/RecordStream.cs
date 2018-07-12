using System;
using System.Collections;
using Cydua.NPOI.HSSF.Record;
using Cydua.NPOI.HSSF.Record.Chart;

namespace Cydua.NPOI.HSSF.Model
{
    /// <summary>
    /// Simplifies iteration over a sequence of Record objects.
    /// @author Josh Micich
    /// </summary>
    public class RecordStream
    {

        private IList _list;
        private int _nextIndex;
        private int _endIx;
        private int _countRead;

        public RecordStream(IList inputList, int startIndex, int endIx)
        {
            _list = inputList;
            _nextIndex = startIndex;
            _endIx = endIx;
            _countRead = 0;

        }

        public RecordStream(IList records, int startIx) : this(records, startIx, records.Count)
        {

        }

        /// <summary>
        /// Determines whether this instance has next.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance has next; otherwise, <c>false</c>.
        /// </returns>
        public bool HasNext()
        {
            return _nextIndex < _endIx;
        }

        /// <summary>
        /// Gets the next record
        /// </summary>
        /// <returns></returns>
        public Cydua.NPOI.HSSF.Record.Record GetNext()
        {
            if (_nextIndex >= _list.Count)
            {
                throw new Exception("Attempt to Read past end of record stream");
            }
            _countRead++;
            return (Cydua.NPOI.HSSF.Record.Record)_list[_nextIndex++];
        }
        /// <summary>
        /// Peeks the next sid.
        /// </summary>
        /// <returns>-1 if at end of records</returns>
        public int PeekNextSid()
        {
            if (!HasNext())
            {
                return -1;
            }
            return ((Cydua.NPOI.HSSF.Record.Record)_list[_nextIndex]).Sid;
        }
        /// <summary>
        /// Peeks the next class.
        /// </summary>
        /// <returns>the class of the next Record.return null if this stream Is exhausted.</returns>
        public Type PeekNextClass()
        {
            if (_nextIndex >= _list.Count)
            {
                return null;
            }
            return _list[_nextIndex].GetType();
        }

        public int GetCountRead()
        {
            return _countRead;
        }

        public int PeekNextChartSid()
        {
            if (!HasNext())
            {
                return -1;
            }

            while (PeekNextSid() == StartBlockRecord.sid || PeekNextSid() == EndBlockRecord.sid)
            {
                GetNext();
            }
            return PeekNextSid();
        }
        public void FindChartSubStream()
        {
            while (PeekNextSid() > -1)
            {
                Cydua.NPOI.HSSF.Record.Record r = GetNext();
                if (r.Sid == BOFRecord.sid && (r as BOFRecord).Type == BOFRecordType.Chart)
                {
                    _nextIndex--;
                    _countRead--;
                    break;
                }
            }
        }
    }
}