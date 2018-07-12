using System.IO;
using Cydua.NPOI.POIFS.Common;
using System.Collections.Generic;

namespace Cydua.NPOI.POIFS.Storage
{
    /// <summary>
    /// A list of RawDataBlocks instances, and methods to manage the list
    /// @author Marc Johnson (mjohnson at apache dot org
    /// </summary>
    public class RawDataBlockList:BlockListImpl
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="RawDataBlockList"/> class.
        /// </summary>
        /// <param name="stream">the InputStream from which the data will be read</param>
        /// <param name="bigBlockSize">The big block size, either 512 bytes or 4096 bytes</param>
        public RawDataBlockList(Stream stream, POIFSBigBlockSize bigBlockSize)
        {
            List<RawDataBlock> blocks = new List<RawDataBlock>();

            while (true)
            {
                RawDataBlock block = new RawDataBlock(stream, bigBlockSize.GetBigBlockSize());
                
                // If there was data, add the block to the list
                if(block.HasData) {
            	    blocks.Add(block);
                }

                // If the stream is now at the End Of File, we're done
                if (block.EOF) {
                    break;
                }
            }
             SetBlocks((ListManagedBlock[])blocks.ToArray());
        }
    }
}
