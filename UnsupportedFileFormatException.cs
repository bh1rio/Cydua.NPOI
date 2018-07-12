namespace Cydua.NPOI
{
    using System;

    /**
     * Base class of all the exceptions that POI throws in the event
     * that it's given a file that isn't supported
     */
    public abstract class UnsupportedFileFormatException : ArgumentException
    {
        public UnsupportedFileFormatException(String s)
            : base(s)
        {
        }
    }
}