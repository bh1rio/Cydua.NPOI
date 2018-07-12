using System;
using System.IO;
using System.Text;

namespace Cydua.NPOI.Util
{


    /// <summary>
    /// CRC Verification
    /// </summary>
    public class CRC32
    {

        protected ulong[] crc32Table;


        /// <summary>
        /// Initializes a new instance of the <see cref="CRC32"/> class.
        /// </summary>
        public CRC32()
        {
            const ulong ulPolynomial = 0xEDB88320;
            ulong dwCrc;
            crc32Table = new ulong[256];
            int i, j;
            for (i = 0; i < 256; i++)
            {
                dwCrc = (ulong)i;
                for (j = 8; j > 0; j--)
                {
                    if ((dwCrc & 1) == 1)
                        dwCrc = (dwCrc >> 1) ^ ulPolynomial;
                    else
                        dwCrc >>= 1;
                }
                crc32Table[i] = dwCrc;
            }
        }

        /// <summary>
        ///  CRC Bytes.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public ulong ByteCRC(ref byte[] buffer)
        {
            ulong ulCRC = 0xffffffff;
            ulong len;
            len = (ulong)buffer.Length;
            for (ulong buffptr = 0; buffptr < len; buffptr++)
            {
                ulong tabPtr = ulCRC & 0xFF;
                tabPtr = tabPtr ^ buffer[buffptr];
                ulCRC = ulCRC >> 8;
                ulCRC = ulCRC ^ crc32Table[tabPtr];
            }
            return ulCRC ^ 0xffffffff;
        }


        /// <summary>
        /// String CRC
        /// </summary>
        /// <param name="sInputString">the string</param>
        /// <returns></returns>
        public ulong StringCRC(string sInputString)
        {
            byte[] buffer = Encoding.Default.GetBytes(sInputString);
            return ByteCRC(ref buffer);
        }

        /// <summary>
        /// File CRC
        /// </summary>
        /// <param name="sInputFilename">the input file</param>
        /// <returns></returns>
        public long FileCRC(string sInputFilename)
        {
            using (FileStream inFile = new System.IO.FileStream(sInputFilename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                byte[] bInput = new byte[inFile.Length];
                inFile.Read(bInput, 0, bInput.Length);

                return (long)ByteCRC(ref bInput);
            }
        }

        /// <summary>
        /// Stream CRC
        /// </summary>
        /// <param name="inFile">the input stream</param>
        /// <returns></returns>
        public long StreamCRC(Stream inFile)
        {
            try
            {
                byte[] bInput = new byte[inFile.Length];
                inFile.Read(bInput, 0, bInput.Length);
                inFile.Close();

                return (long)ByteCRC(ref bInput);
            }
            catch (IOException)
            {
                throw;
            }
        }
    }

}