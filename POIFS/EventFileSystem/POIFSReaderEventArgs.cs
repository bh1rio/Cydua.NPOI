using System;

using Cydua.NPOI.POIFS.FileSystem;

namespace Cydua.NPOI.POIFS.EventFileSystem
{
    /// <summary>
    /// EventArgs for POIFSReader
    /// author: Tony Qu
    /// </summary>
    public class POIFSReaderEventArgs:EventArgs
    {
        public POIFSReaderEventArgs(string name, POIFSDocumentPath path, POIFSDocument document)
        {
            this.name = name;
            this.path = path;
            this.document = document;
        }

        private POIFSDocumentPath path;
        private POIFSDocument document;
        private string name;

        public virtual POIFSDocumentPath Path
        {
            get { return path; }
        }
        public virtual POIFSDocument Document
        {
            get { return document; }
        }
        public virtual DocumentInputStream Stream
        {
            get { 
                return new DocumentInputStream(this.document); 
            }
        }
        public virtual string Name
        {
            get { return name; }
        }
    }

    public delegate void POIFSReaderEventHandler(object sender, POIFSReaderEventArgs e);
}
