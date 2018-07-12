using System.IO;
using System.Collections;

using Cydua.NPOI.POIFS.Common;
using Cydua.NPOI.POIFS.Properties;
using Cydua.NPOI.POIFS.FileSystem;


namespace Cydua.NPOI.POIFS.Storage
{
    /// <summary>
    /// This class implements reading the small document block list from an
    /// existing file
    /// @author Marc Johnson (mjohnson at apache dot org)
    /// </summary>
    public class SmallBlockTableWriter : BlockWritable, BATManaged
    {
        private BlockAllocationTableWriter _sbat;
        private IList                       _small_blocks;
        private int                        _big_block_count;
        private RootProperty               _root;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmallBlockTableWriter"/> class.
        /// </summary>
        /// <param name="bigBlockSize">the poifs bigBlockSize</param>
        /// <param name="documents">a IList of POIFSDocument instances</param>
        /// <param name="root">the Filesystem's root property</param>
        public SmallBlockTableWriter(POIFSBigBlockSize bigBlockSize, IList documents,
                                     RootProperty root)
        {
            _sbat = new BlockAllocationTableWriter(bigBlockSize);
            _small_blocks = new ArrayList();
            _root         = root;
            IEnumerator iter = documents.GetEnumerator();

            while (iter.MoveNext())
            {
                POIFSDocument   doc    = ( POIFSDocument ) iter.Current;
                BlockWritable[] blocks = doc.SmallBlocks;

                if (blocks.Length != 0)
                {
                    doc.StartBlock=_sbat.AllocateSpace(blocks.Length);
                    for (int j = 0; j < blocks.Length; j++)
                    {
                        _small_blocks.Add(blocks[ j ]);
                    }
                } else {
            	    doc.StartBlock=POIFSConstants.END_OF_CHAIN;
                }
            }
            _sbat.SimpleCreateBlocks();
            _root.Size=_small_blocks.Count;
            _big_block_count = SmallDocumentBlock.Fill(bigBlockSize, _small_blocks);
        }

        /// <summary>
        /// Get the number of SBAT blocks
        /// </summary>
        /// <value>number of SBAT big blocks</value>
        public int SBATBlockCount
        {
            get { return (_big_block_count + 15) / 16; }
        }

        /// <summary>
        /// Gets the SBAT.
        /// </summary>
        /// <value>the Small Block Allocation Table</value>
        public BlockAllocationTableWriter SBAT
        {
            get { return _sbat; }
        }

        /// <summary>
        /// Return the number of BigBlock's this instance uses
        /// </summary>
        /// <value>count of BigBlock instances</value>
        public int CountBlocks
        {
            get{return _big_block_count;}
        }

        /// <summary>
        /// Sets the start block.
        /// </summary>
        /// <value>The start block.</value>
        public int StartBlock
        {
            set { _root.StartBlock=value; }
        }

        /// <summary>
        /// Write the storage to an OutputStream
        /// </summary>
        /// <param name="stream">the OutputStream to which the stored data should be written</param>
        public void WriteBlocks(Stream stream)
        {
            IEnumerator iter = _small_blocks.GetEnumerator();

            while (iter.MoveNext())
            {
                (( BlockWritable ) iter.Current).WriteBlocks(stream);
            }
        }
    }
}
