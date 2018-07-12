namespace Cydua.NPOI.Util
{
    using System;

    /**
     * A common exception thrown by our binary format Parsers
     *  (especially HSSF and DDF), when they hit invalid
     *  format or data when Processing a record.
     */
    [Serializable]
    public class RecordFormatException
        : RuntimeException
    {
        public RecordFormatException(String exception):
            base(exception)
        {
        }

        public RecordFormatException(String exception, Exception ex)
            : base(exception, ex)
        {

        }

        public RecordFormatException(Exception ex):
            base(ex)
        {
        }
    }

}