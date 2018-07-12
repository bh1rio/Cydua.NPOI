using System;
using System.IO;

namespace Cydua.NPOI.HSSF.Record
{
    /**
     * Title: Record
     * Description: All HSSF Records inherit from this class.  It
     *              populates the fields common to all records (id, size and data).
     *              Subclasses should be sure to validate the id,
     * Company:
     * @author Andrew C. Oliver
     * @author Marc Johnson (mjohnson at apache dot org)
     * @author Jason Height (jheight at chariot dot net dot au)
     * @version 2.0-pre
     */
    [Serializable]
    public abstract class Record : RecordBase
    {

        /**
         * instantiates a blank record strictly for ID matching
         */

        public Record()
        {
        }

        //public abstract int Serialize(int offset, byte[] data);
        /**
         * called by the class that is responsible for writing this sucker.
         * Subclasses should implement this so that their data is passed back in a
         * byte array.
         *
         * @return byte array containing instance data
         */

        public byte[] Serialize()
        {
            byte[] retval = new byte[RecordSize];

            Serialize(0, retval);
            return retval;
        }

        // /**
        // * gives the current Serialized size of the record. Should include the sid and recLength (4 bytes).
        // */

        //public abstract int RecordSize { get; }
        //{

        // this is kind od a stupid way to do it but for now we just Serialize
        // the record and return the size of the byte array
        //get { return Serialize().Length; }
        //}

        // /**
        // * tells whether this type of record Contains a value
        // */

        //public virtual bool IsValue
        //{
        //    get { return false; }
        //}

        // /**
        // * DBCELL, ROW, VALUES all say yes
        // */

        //public virtual bool IsInValueSection
        //{
        //    get { return false; }
        //}


        /**
         * return the non static version of the id for this record.
         */

        public abstract short Sid { get; }

        public virtual Object Clone()
        {
            throw new Exception("The class " + this.GetType().Name + " needs to define a Clone method");
        }

        public Record CloneViaReserialise()
        {
            // Do it via a re-serialization
            // It's a cheat, but it works...
            byte[] b = Serialize();
            using (MemoryStream ms = new MemoryStream(b))
            {
                RecordInputStream rinp = new RecordInputStream(ms);
                rinp.NextRecord();

                Record[] r = RecordFactory.CreateRecord(rinp);
                if (r.Length != 1)
                {
                    throw new InvalidOperationException("Re-serialised a record to clone it, but got " + r.Length + " records back!");
                }
                return r[0];
            }
        }
    }
}
