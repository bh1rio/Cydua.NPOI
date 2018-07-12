using Cydua.NPOI.POIFS.Common;
using Cydua.NPOI.POIFS.Storage;

namespace Cydua.NPOI.POIFS.Properties
{
    public class RootProperty : DirectoryProperty
    {
        private const string NAME = "Root Entry";

        public RootProperty() : base(NAME)
        {
            this.NodeColor = _NODE_BLACK;
            this.PropertyType = PropertyConstants.ROOT_TYPE;
            this.StartBlock = POIFSConstants.END_OF_CHAIN;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootProperty"/> class.
        /// </summary>
        /// <param name="index">index number</param>
        /// <param name="array">byte data</param>
        /// <param name="offset">offset into byte data</param>
        public RootProperty(int index, byte[] array,
                               int offset) : base(index, array, offset)
        {

        }

        /// <summary>
        /// Gets or sets the size of the document associated with this Property
        /// </summary>
        /// <value>the size of the document, in bytes</value>
        public override int Size
        {
            set
            {
                base.Size = SmallDocumentBlock.CalcSize(value);
            }
        }
    }
}
