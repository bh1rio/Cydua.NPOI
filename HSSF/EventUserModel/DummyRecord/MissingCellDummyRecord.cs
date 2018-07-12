namespace Cydua.NPOI.HSSF.EventUserModel.DummyRecord
{
    /**
     * A dummy record for when we're missing a cell in a row,
     *  but still want to trigger something
     */
    public class MissingCellDummyRecord : DummyRecordBase
    {
        private int row;
        private int column;

        public MissingCellDummyRecord(int row, int column)
        {
            this.row = row;
            this.column = column;
        }


        public int Row
        {
            get { return row; }
        }
        public int Column
        {
            get { return column; }
        }
    }
}