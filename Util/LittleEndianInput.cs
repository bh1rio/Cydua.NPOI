namespace Cydua.NPOI.Util
{
    public interface ILittleEndianInput
    {
        int Available();
        int ReadByte();
        int ReadUByte();
        short ReadShort();
        int ReadUShort();
        int ReadInt();
        long ReadLong();
        double ReadDouble();
        void ReadFully(byte[] buf);
        void ReadFully(byte[] buf, int off, int len);
    }
}