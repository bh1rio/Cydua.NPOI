using System;
using Cydua.NPOI.POIFS.FileSystem;

namespace Cydua.NPOI.POIFS.EventFileSystem
{


    /// <summary>
    /// EventArgs for POIFSWriter
    /// author: Tony Qu
    /// </summary>
    public class POIFSWriterEventArgs : EventArgs
    {

        private string documentName;
        private int limit;
        private POIFSDocumentPath path;
        private DocumentOutputStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="POIFSWriterEvent"/> class.
        /// </summary>
        /// <param name="stream">the POIFSDocumentWriter, freshly opened</param>
        /// <param name="path">the path of the document</param>
        /// <param name="documentName">the name of the document</param>
        /// <param name="limit">the limit, in bytes, that can be written to the stream</param>
        public POIFSWriterEventArgs(DocumentOutputStream stream, POIFSDocumentPath path, string documentName, int limit)
        {
            this.stream = stream;
            this.path = path;
            this.documentName = documentName;
            this.limit = limit;
        }
        /// <summary>
        /// Gets the limit on writing, in bytes
        /// </summary>
        /// <value>The limit.</value>
        public virtual int Limit
        {
            get
            {
                return this.limit;
            }
        }

        /// <summary>
        /// Gets the document's name
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name
        {
            get
            {
                return this.documentName;
            }
        }

        /// <summary>
        /// Gets the document's path
        /// </summary>
        /// <value>The path.</value>
        public virtual POIFSDocumentPath Path
        {
            get
            {
                return this.path;
            }
        }

        /// <summary>
        /// the POIFSDocumentWriter, freshly opened
        /// </summary>
        /// <value>The stream.</value>
        public virtual DocumentOutputStream Stream
        {
            get
            {
                return this.stream;
            }
        }
    }

    public delegate void POIFSWriterEventHandler(object sender, POIFSWriterEventArgs e);
}
