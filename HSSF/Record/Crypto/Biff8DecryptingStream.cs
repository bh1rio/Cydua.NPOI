using System;
using System.IO;

using Cydua.NPOI.Util;
using Cydua.NPOI.HSSF.Record;

namespace Cydua.NPOI.HSSF.Record.Crypto
{
    /**
     *
     * @author Josh Micich
     */
    public class Biff8DecryptingStream : BiffHeaderInput, ILittleEndianInput
    {

        private ILittleEndianInput _le;
        private Biff8RC4 _rc4;

        public Biff8DecryptingStream(Stream in1, int InitialOffSet, Biff8EncryptionKey key)
        {
            _rc4 = new Biff8RC4(InitialOffSet, key);

            if (in1 is ILittleEndianInput)
            {
                // accessing directly is an optimisation
                _le = (ILittleEndianInput)in1;
            }
            else
            {
                // less optimal, but should work OK just the same. Often occurs in junit tests.
                _le = new LittleEndianInputStream(in1);
            }
        }

        public int Available()
        {
            return _le.Available();
        }

        /**
         * Reads an unsigned short value without decrypting
         */
        public int ReadRecordSID()
        {
            int sid = _le.ReadUShort();
            _rc4.SkipTwoBytes();
            _rc4.StartRecord(sid);
            return sid;
        }

        /**
         * Reads an unsigned short value without decrypting
         */
        public int ReadDataSize()
        {
            int dataSize = _le.ReadUShort();
            _rc4.SkipTwoBytes();
            return dataSize;
        }

        public double ReadDouble()
        {
            long valueLongBits = ReadLong();
            double result = BitConverter.Int64BitsToDouble(valueLongBits);
            if (Double.IsNaN(result))
            {
                throw new Exception("Did not expect to read NaN"); // (Because Excel typically doesn't write NaN
            }
            return result;
        }

        public void ReadFully(byte[] buf)
        {
            ReadFully(buf, 0, buf.Length);
        }

        public void ReadFully(byte[] buf, int off, int len)
        {
            _le.ReadFully(buf, off, len);
            _rc4.Xor(buf, off, len);
        }


        public int ReadUByte()
        {
            return _rc4.XorByte(_le.ReadUByte());
        }
        public int ReadByte()
        {
            return _rc4.XorByte(_le.ReadUByte());
        }


        public int ReadUShort()
        {
            return _rc4.Xorshort(_le.ReadUShort());
        }
        public short ReadShort()
        {
            return (short)_rc4.Xorshort(_le.ReadUShort());
        }

        public int ReadInt()
        {
            return _rc4.XorInt(_le.ReadInt());
        }

        public long ReadLong()
        {
            return _rc4.XorLong(_le.ReadLong());
        }
    }
}

