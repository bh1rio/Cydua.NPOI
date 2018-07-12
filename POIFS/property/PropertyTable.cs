using System.IO;
using System.Collections.Generic;

using Cydua.NPOI.POIFS.Storage;
using Cydua.NPOI.POIFS.Common;

namespace Cydua.NPOI.POIFS.Properties
{
    public class PropertyTable : PropertyTableBase, BlockWritable
    {

        private POIFSBigBlockSize _bigBigBlockSize;
        private BlockWritable[] _blocks;
        /**
         * Default constructor
         */
        public PropertyTable(HeaderBlock headerBlock) : base(headerBlock)
        {
            _bigBigBlockSize = headerBlock.BigBlockSize;
            _blocks = null;
        }

        /**
         * reading constructor (used when we've read in a file and we want
         * to extract the property table from it). Populates the
         * properties thoroughly
         *
         * @param startBlock the first block of the property table
         * @param blockList the list of blocks
         *
         * @exception IOException if anything goes wrong (which should be
         *            a result of the input being NFG)
         */
        public PropertyTable(HeaderBlock headerBlock, 
                             RawDataBlockList blockList)
            : base(headerBlock, 
                    PropertyFactory.ConvertToProperties( blockList.FetchBlocks(headerBlock.PropertyStart, -1) ) )
        {
            _bigBigBlockSize = headerBlock.BigBlockSize;
            _blocks      = null;

        }

        /**
         * Prepare to be written Leon
         */

        public void PreWrite()
        {

            List<Property> properties = new List<Property>(_properties.Count);

            for (int i = 0; i < _properties.Count; i++)
                properties.Add(_properties[i]);


            // give each property its index
            for (int k = 0; k < properties.Count; k++)
            {
                properties[ k ].Index = k;
            }

            // allocate the blocks for the property table
            _blocks = PropertyBlock.CreatePropertyBlockArray(_bigBigBlockSize, properties);

            // prepare each property for writing
            for (int k = 0; k < properties.Count; k++)
            {
                properties[ k ].PreWrite();
            }
        }



        /* ********** START implementation of BATManaged ********** */

        /**
         * Return the number of BigBlock's this instance uses
         *
         * @return count of BigBlock instances
         */

        public override int CountBlocks
        {
            get { return (_blocks == null) ? 0 : _blocks.Length; }
        }

        /**
         * Write the storage to an Stream
         *
         * @param stream the Stream to which the stored data should
         *               be written
         *
         * @exception IOException on problems writing to the specified
         *            stream
         */

        public void WriteBlocks(Stream stream)
        {
            if (_blocks != null)
            {
                for (int j = 0; j < _blocks.Length; j++)
                {
                    _blocks[ j ].WriteBlocks(stream);
                }
            }
        }
    }
}
