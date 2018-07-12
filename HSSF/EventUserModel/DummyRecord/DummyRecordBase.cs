using Cydua.NPOI.HSSF.Record;
using Cydua.NPOI.Util;

namespace Cydua.NPOI.HSSF.EventUserModel.DummyRecord
{
    /**
     */
    public abstract class DummyRecordBase : Cydua.NPOI.HSSF.Record.Record
    {

        protected DummyRecordBase()
        {
            //
        }

        public override short Sid
        {
            get
            {
                return -1;
            }
        }
        public override int Serialize(int offset, byte[] data)
        {
            throw new RecordFormatException("Cannot serialize a dummy record");
        }
        public override int RecordSize
        {
            get
            {
                throw new RecordFormatException("Cannot serialize a dummy record");
            }
        }
    }

}