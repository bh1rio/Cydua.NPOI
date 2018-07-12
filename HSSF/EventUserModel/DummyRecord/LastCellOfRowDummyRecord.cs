namespace Cydua.NPOI.HSSF.EventUserModel.DummyRecord
{
    /**
     * A dummy record to indicate that we've now had the last
     *  cell record for this row.
     */
    public class LastCellOfRowDummyRecord : DummyRecordBase
    {
        private int row;
        private int lastColumnNumber;

        public LastCellOfRowDummyRecord(int row, int lastColumnNumber)
        {
            this.row = row;
            this.lastColumnNumber = lastColumnNumber;
        }

        /**
         * Returns the (0 based) number of the row we are
         *  currently working on.
         */
        public int Row
        {
            get { return row; }
        }
        /**
         * Returns the (0 based) number of the last column
         *  seen for this row. You should have alReady been
         *  called with that record.
         * This Is -1 in the case of there being no columns
         *  for the row.
         */
        public int LastColumnNumber
        {
            get { return lastColumnNumber; }
        }
    }
}