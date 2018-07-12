using System.IO;
using System.Collections;
using Cydua.NPOI.HSSF.Record;

namespace Cydua.NPOI.HSSF.EventModel
{
    /**
     * Event-based record factory.  As opposed to RecordFactory
     * this refactored version throws record events as it comes
     * accross the records.  I throws the "lazily" one record behind
     * to ensure that ContinueRecords are Processed first.
     * 
     * @author Andrew C. Oliver (acoliver@apache.org) - probably to blame for the bugs (so yank his chain on the list)
     * @author Marc Johnson (mjohnson at apache dot org) - methods taken from RecordFactory
     * @author Glen Stampoultzis (glens at apache.org) - methods taken from RecordFactory
     * @author Csaba Nagy (ncsaba at yahoo dot com)
     */
    public class EventRecordFactory
    {

        private IERFListener _listener;
        private ArrayList _sids;

        /**
         * Create an EventRecordFactory
         * @param abortable specifies whether the return from the listener 
         * handler functions are obeyed.  False means they are ignored. True
         * means the event loop exits on error.
         */
        public EventRecordFactory(IERFListener listener, ArrayList sids)
        {
            _listener = listener;
            _sids = sids;

            if (_sids == null)
            {
                _sids = null;
            }
            else
            {
                if (_sids == null)
                    _sids = new ArrayList();
                _sids.Sort(); // for faster binary search
            }
        }
        private bool IsSidIncluded(int sid)
        {
            if (_sids == null)
            {
                return true;
            }
            return _sids.BinarySearch((short)sid) >= 0;
        }
        /**
     * sends the record event to all registered listeners.
     * @param record the record to be thrown.
     * @return <c>false</c> to abort.  This aborts
     * out of the event loop should the listener return false
     */
        private bool ProcessRecord(Cydua.NPOI.HSSF.Record.Record record)
        {
            if (!IsSidIncluded(record.Sid))
            {
                return true;
            }
            return _listener.ProcessRecord(record);
        }
        /**
         * Create an array of records from an input stream
         *
         * @param in the InputStream from which the records will be
         *           obtained
         *
         * @exception RecordFormatException on error Processing the
         *            InputStream
         */
        public void ProcessRecords(Stream in1)
        {
            Cydua.NPOI.HSSF.Record.Record last_record = null;

            RecordInputStream recStream = new RecordInputStream(in1);

            while (recStream.HasNextRecord)
            {
                recStream.NextRecord();
                Cydua.NPOI.HSSF.Record.Record[] recs = RecordFactory.CreateRecord(recStream);   // handle MulRK records
                if (recs.Length > 1)
                {
                    for (int k = 0; k < recs.Length; k++)
                    {
                        if (last_record != null)
                        {
                            if (!ProcessRecord(last_record))
                            {
                                return;
                            }
                        }
                        last_record = recs[k]; // do to keep the algorithm homogeneous...you can't
                    }                            // actually continue a number record anyhow.
                }
                else
                {
                    Cydua.NPOI.HSSF.Record.Record record = recs[0];

                    if (record != null)
                    {
                        if (last_record != null)
                        {
                            if (!ProcessRecord(last_record))
                            {
                                return;
                            }
                        }
                        last_record = record;
                    }
                }
            }

            if (last_record != null)
            {
                ProcessRecord(last_record);
            }
        }
    }
}