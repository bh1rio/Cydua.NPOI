﻿using System;


namespace Cydua.NPOI.POIFS.FileSystem
{
    /// <summary>
    /// This exception is thrown when we try to open a file that's actually
    /// an Office 2007+ XML file, rather than an OLE2 file (which is what
    /// POIFS works with)
    /// </summary>
    [Serializable]
    public class OfficeXmlFileException:ArgumentException
    {
        public OfficeXmlFileException(String s):base(s)
        { 
        
        }
    }
}
