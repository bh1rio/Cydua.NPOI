namespace Cydua.NPOI.HPSF
{
    public class VersionedStream
    {
        private GUID _versionGuid;
        private IndirectPropertyName _streamName;

        public VersionedStream(byte[] data, int offset)
        {
            _versionGuid = new GUID(data, offset);
            _streamName = new IndirectPropertyName(data, offset + GUID.SIZE);
        }

        public int Size
        {
            get { return GUID.SIZE + _streamName.Size; }
        }
    }
}