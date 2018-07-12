using System.IO;
using System.Collections.Generic;

using Cydua.NPOI.POIFS.Properties;
using Cydua.NPOI.POIFS.Common;

namespace Cydua.NPOI.POIFS.Storage
{
    /// <summary>
    /// A block of Property instances
    /// @author Marc Johnson (mjohnson at apache dot org)
    /// </summary>
    public class PropertyBlock : BigBlock
    {
        private class AnonymousProperty : Property
        {
            public override void PreWrite()
            {
            }

            public override bool IsDirectory
            {
                get
                {
                    return false;
                }
            }
        }



        private Property[]       _properties;

        /// <summary>
        /// Create a single instance initialized with default values
        /// </summary>
        /// <param name="bigBlockSize"></param>
        /// <param name="properties">the properties to be inserted</param>
        /// <param name="offset">the offset into the properties array</param>
        protected PropertyBlock(POIFSBigBlockSize bigBlockSize, Property[] properties, int offset) : base(bigBlockSize)
        {
            _properties = new Property[ bigBlockSize.GetPropertiesPerBlock() ];
            for (int j = 0; j < _properties.Length; j++)
            {
                _properties[ j ] = properties[ j + offset ];
            }
        }

        /// <summary>
        /// Create an array of PropertyBlocks from an array of Property
        /// instances, creating empty Property instances to make up any
        /// shortfall
        /// </summary>
        /// <param name="bigBlockSize"></param>
        /// <param name="properties">the Property instances to be converted into PropertyBlocks, in a java List</param>
        /// <returns>the array of newly created PropertyBlock instances</returns>
        public static BlockWritable [] CreatePropertyBlockArray( POIFSBigBlockSize bigBlockSize,
                                        List<Property> properties)
            {
            int _properties_per_block = bigBlockSize.GetPropertiesPerBlock();

            int blockCount = (properties.Count + _properties_per_block - 1) / _properties_per_block;

            Property[] toBeWritten = new Property[blockCount * _properties_per_block];

            System.Array.Copy(properties.ToArray(), 0, toBeWritten, 0, properties.Count);

            for (int i = properties.Count; i < toBeWritten.Length; i++)
            {
                toBeWritten[i] = new AnonymousProperty();
            }

            BlockWritable[] rvalue = new BlockWritable[blockCount];

            for (int i = 0; i < blockCount; i++)
                rvalue[i] = new PropertyBlock(bigBlockSize, toBeWritten, i * _properties_per_block);

            return rvalue;
        }

        /// <summary>
        /// Write the block's data to an OutputStream
        /// </summary>
        /// <param name="stream">the OutputStream to which the stored data should be written</param>
        public override void WriteData(Stream stream)
        {
            int _properties_per_block = bigBlockSize.GetPropertiesPerBlock();

            for (int i = 0; i < _properties_per_block; i++)
                _properties[i].WriteData(stream);
        }
    }
}
