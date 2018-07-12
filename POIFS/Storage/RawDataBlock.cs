using System;
using System.IO;

using Cydua.NPOI.POIFS.Common;
using Cydua.NPOI.Util;


namespace Cydua.NPOI.POIFS.Storage
{
    /// <summary>
    /// A big block created from an InputStream, holding the raw data
    /// @author Marc Johnson (mjohnson at apache dot org
    /// </summary>
    public class RawDataBlock : ListManagedBlock
    {
        private byte[] _data;
        private bool _eof;
        private bool _hasData;
        private static POILogger log = POILogFactory.GetLogger(typeof(RawDataBlock));

        /// <summary>
        /// Constructor RawDataBlock
        /// </summary>
        /// <param name="stream">the Stream from which the data will be read</param>
        public RawDataBlock(Stream stream)
            : this(stream, POIFSConstants.SMALLER_BIG_BLOCK_SIZE)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RawDataBlock"/> class.
        /// </summary>
        /// <param name="stream">the Stream from which the data will be read</param>
        /// <param name="blockSize">the size of the POIFS blocks, normally 512 bytes {@link POIFSConstants#BIG_BLOCK_SIZE}</param>
        public RawDataBlock(Stream stream, int blockSize)
        {
            _data = new byte[blockSize];
            int count = IOUtils.ReadFully(stream, _data);
            _hasData = (count > 0);

            if (count == -1)
            {
                _eof = true;
            }
            else if (count != blockSize)
            {
                // IOUtils.readFully will always read the
                //  requested number of bytes, unless it hits
                //  an EOF
                _eof = true;
                String type = " byte" + ((count == 1) ? ("")
                                                      : ("s"));

                log.Log(POILogger.ERROR,
                        "Unable to read entire block; " + count
                         + type + " read before EOF; expected "
                         + blockSize + " bytes. Your document "
                         + "was either written by software that "
                         + "ignores the spec, or has been truncated!"
                );
            }
            else
            {
                _eof = false;
            }
        }

        /// <summary>
        /// When we read the data, did we hit end of file?
        /// </summary>
        /// <value><c>true</c> if the EoF was hit during this block, or; otherwise, <c>false</c>if not. If you have a dodgy short last block, then
        /// it's possible to both have data, and also hit EoF...</value>
        public bool EOF
        {
            get { return _eof; }
        }
        /// <summary>
        /// Did we actually find any data to read? It's possible,
        /// in the event of a short last block, to both have hit
        /// the EoF, but also to have data
        /// </summary>
        /// <value><c>true</c> if this instance has data; otherwise, <c>false</c>.</value>
        public bool HasData
        {
            get { return _hasData; }
        }

        /// <summary>
        /// Get the data from the block
        /// </summary>
        /// <value>the block's data as a byte array</value>
        public byte[] Data
        {
            get
            {
                if (!HasData)
                {
                    // TODO return null instead of raising an unexpected exception (CA1065)
                    //return null;
                    throw new IOException("Cannot return empty data");
                }
                return _data;
            }
        }

        public override string ToString()
        {
            return "RawDataBlock of size " + _data.Length;
        }

        public int BigBlockSize
        {
            get { return _data.Length; }
        }
    }
}