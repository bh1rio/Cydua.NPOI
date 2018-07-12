namespace Cydua.NPOI.DDF
{
    public class NullEscherSerializationListener : EscherSerializationListener
    {
        public void BeforeRecordSerialize(int offset, short recordId, EscherRecord record)
        {
            // do nothing
        }

        public void AfterRecordSerialize(int offset, short recordId, int size, EscherRecord record)
        {
            // do nothing
        }

    }
}
