using System;
using Cydua.NPOI.HSSF.Record;

namespace Cydua.NPOI.HSSF.Record.Aggregates
{
    public interface RecordVisitor
    {
        /**
         * Implementors may call non-mutating methods on Record r.
         * @param r must not be <c>null</c>
         */
        void VisitRecord(Record r);
    }

    /**
     * <c>RecordAggregate</c>s are groups of of BIFF <c>Record</c>s that are typically stored 
     * together and/or updated together.  Workbook / Sheet records are typically stored in a sequential
     * list, which does not provide much structure to coordinate updates.
     * 
     * @author Josh Micich
     */
    [Serializable]
    public abstract class RecordAggregate : RecordBase
    {
        public virtual short Sid
        {
            get
            {
                throw new NotImplementedException("Should not be called");
            }
        }
        // there seams to be nothing to free: public abstract void Dispose();
        /**
         * Visit each of the atomic BIFF records contained in this {@link RecordAggregate} in the order
         * that they should be written to file.  Implementors may or may not return the actual 
         * {@link Record}s being used to manage POI's internal implementation.  Callers should not
         * assume either way, and therefore only attempt to modify those {@link Record}s after cloning
         */
        public abstract void VisitContainedRecords(RecordVisitor rv);

        public override int Serialize(int offset, byte[] data)
        {
            SerializingRecordVisitor srv = new SerializingRecordVisitor(data, offset);
            VisitContainedRecords(srv);
            return srv.CountBytesWritten();
        }
        public override int RecordSize
        {
            get
            {
                RecordSizingVisitor rsv = new RecordSizingVisitor();
                VisitContainedRecords(rsv);
                return rsv.TotalSize;
            }
        }


        private class SerializingRecordVisitor : RecordVisitor
        {

            private byte[] _data;
            private int _startOffset;
            private int _countBytesWritten;

            public SerializingRecordVisitor(byte[] data, int startOffset)
            {
                _data = data;
                _startOffset = startOffset;
                _countBytesWritten = 0;
            }
            public int CountBytesWritten()
            {
                return _countBytesWritten;
            }
            public void VisitRecord(Record r)
            {
                int currentOffset = _startOffset + _countBytesWritten;
                _countBytesWritten += r.Serialize(currentOffset, _data);
            }
        }
        private class RecordSizingVisitor : RecordVisitor
        {

            private int _totalSize;

            public RecordSizingVisitor()
            {
                _totalSize = 0;
            }
            public int TotalSize
            {
                get
                {
                    return _totalSize;
                }
            }
            public void VisitRecord(Record r)
            {
                _totalSize += r.RecordSize;
            }
        }
        public virtual Record CloneViaReserialise()
        {
            throw new NotImplementedException("Please implement it in subclass");
        }
    }
    /**
     * A wrapper for {@link RecordVisitor} which accumulates the sizes of all
     * records visited.
     */
    public class PositionTrackingVisitor : RecordVisitor
    {
        private RecordVisitor _rv;
        private int _position;

        public PositionTrackingVisitor(RecordVisitor rv, int initialPosition)
        {
            _rv = rv;
            _position = initialPosition;
        }
        public void VisitRecord(Record r)
        {
            _position += r.RecordSize;
            _rv.VisitRecord(r);
        }
        public int Position
        {
            get
            {
                return _position;

            }
            set { _position = value; }
        }
    }
}