using System;
using System.Collections.Generic;
using System.IO;

using Cydua.NPOI.POIFS.FileSystem;

namespace Cydua.NPOI.HPSF
{
    /**
     * A version of {@link POIDocument} which allows access to the
     *  HPSF Properties, but no other document contents.
     * Normally used when you want to read or alter the Document Properties,
     *  without affecting the rest of the file
     */
    public class HPSFPropertiesOnlyDocument : POIDocument
    {
        public HPSFPropertiesOnlyDocument(NPOIFSFileSystem fs)
            : base(fs.Root)
        {

        }
        public HPSFPropertiesOnlyDocument(POIFSFileSystem fs)
            : base(fs)
        {

        }
        /**
         * Write out, with any properties changes, but nothing else
         */
        public override void Write(Stream out1)
        {
            POIFSFileSystem fs = new POIFSFileSystem();

            // For tracking what we've written out, so far
            List<String> excepts = new List<String>(1);

            // Write out our HPFS properties, with any changes
            WriteProperties(fs, excepts);

            // Copy over everything else unchanged
            EntryUtils.CopyNodes(directory, fs.Root, excepts);

            // Save the resultant POIFSFileSystem to the output stream
            fs.WriteFileSystem(out1);
        }
    }
}