using System.Collections;

using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record;

namespace Cydua.NPOI.HSSF.Record.Aggregates
{


    /// <summary>
    /// Manages the DVALRecord and DVRecords for a single sheet
    /// See OOO excelfileformat.pdf section 4.14
    /// @author Josh Micich
    /// </summary>
    public class DataValidityTable : RecordAggregate
    {

        private DVALRecord _headerRec;
        /**
         * The list of data validations for the current sheet.
         * Note - this may be empty (contrary to OOO documentation)
         */
        private IList _validationList;

        public DataValidityTable(RecordStream rs)
        {
            _headerRec = (DVALRecord)rs.GetNext();
            IList temp = new ArrayList();
            while (rs.PeekNextClass() == typeof(DVRecord))
            {
                temp.Add(rs.GetNext());
            }
            _validationList = temp;
        }

        public DataValidityTable()
        {
            _headerRec = new DVALRecord();
            _validationList = new ArrayList();
        }

        public override void VisitContainedRecords(RecordVisitor rv)
        {
            if (_validationList.Count == 0)
            {
                return;
            }
            rv.VisitRecord(_headerRec);
            for (int i = 0; i < _validationList.Count; i++)
            {
                rv.VisitRecord((Record)_validationList[i]);
            }
        }

        public void AddDataValidation(DVRecord dvRecord)
        {
            _validationList.Add(dvRecord);
            _headerRec.DVRecNo = (_validationList.Count);
        }
    }
}