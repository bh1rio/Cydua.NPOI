using System;

namespace Cydua.NPOI
{
    [Serializable]
    public class OldFileFormatException : UnsupportedFileFormatException
    {
        public OldFileFormatException(String s)
            : base(s)
        { }

    }
}
