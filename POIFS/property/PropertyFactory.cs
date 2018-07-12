using System.Collections.Generic;

using Cydua.NPOI.POIFS.Storage;
using Cydua.NPOI.POIFS.Common;

namespace Cydua.NPOI.POIFS.Properties
{
    public class PropertyFactory
    {
        private PropertyFactory()
        {
        }

        /// <summary>
        /// Convert raw data blocks to an array of Property's
        /// </summary>
        /// <param name="blocks">The blocks to be converted</param>
        /// <returns>the converted List of Property objects. May contain
        /// nulls, but will not be null</returns>
        public static List<Property> ConvertToProperties(ListManagedBlock [] blocks)
        {
            List<Property> properties = new List<Property>();

            for (int j = 0; j < blocks.Length; j++)
            {
                byte[] data = blocks[j].Data;
                ConvertToProperties(data, properties);
            }

            return properties;
        }

        public static void ConvertToProperties(byte[] data, List<Property> properties)
        {
            int property_count = data.Length / POIFSConstants.PROPERTY_SIZE;
                int    offset         = 0;

                for (int k = 0; k < property_count; k++)
                {
                switch (data[offset + PropertyConstants.PROPERTY_TYPE_OFFSET])
                    {
                    case PropertyConstants.DIRECTORY_TYPE:
                        properties.Add(new DirectoryProperty(properties.Count, data, offset));
                            break;
                    case PropertyConstants.DOCUMENT_TYPE:
                        properties.Add(new DocumentProperty(properties.Count, data, offset));
                            break;
                    case PropertyConstants.ROOT_TYPE:
                        properties.Add(new RootProperty(properties.Count, data, offset));
                            break;
                    default:
                            properties.Add(null);
                            break;
                    }
                    offset += POIFSConstants.PROPERTY_SIZE;
                }
        }
    }
}
