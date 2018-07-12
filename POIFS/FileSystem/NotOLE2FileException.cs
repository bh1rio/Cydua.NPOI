namespace Cydua.NPOI.POIFS.FileSystem
{
    using System;
    using System.IO;



    /**
     * This exception is thrown when we try to open a file that doesn't
     *  seem to actually be an OLE2 file After all
     */
    public class NotOLE2FileException : IOException
    {
        public NotOLE2FileException(String s)
            : base(s)
        {

        }
    }

}