namespace Cydua.NPOI.POIFS.Properties
{
    /// <summary>
    /// Constants used by Properties namespace
    /// </summary>
    public class PropertyConstants
    {
        public const int  PROPERTY_TYPE_OFFSET = 0x42;

        // the property types
        public const byte INVALID_TYPE = 0;
        public const byte DIRECTORY_TYPE       = 1; //STGTY_STORAGE
        public const byte DOCUMENT_TYPE        = 2; //STGTY_STREAM
        public const byte LOCKBYTES_TYPE = 3;       //STGTY_LOCKBYTES
        public const byte PROPERT_TYPE = 4;         //STGTY_PROPERTY
        public const byte ROOT_TYPE = 5;            //STGTY_ROOT
    }
}
