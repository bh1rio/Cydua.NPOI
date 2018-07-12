using System;
namespace Cydua.NPOI.POIFS.Storage
{
    public class DataInputBlock
    {

        /**
         * Possibly any size (usually 512K or 64K).  Assumed to be at least 8 bytes for all blocks
         * before the end of the stream.  The last block in the stream can be any size except zero. 
         */
        private byte[] _buf;
        private int _readIndex;
        private int _maxIndex;

        internal DataInputBlock(byte[] data, int startOffset)
        {
            _buf = data;
            _readIndex = startOffset;
            _maxIndex = _buf.Length;
        }
        public int Available()
        {
            return _maxIndex - _readIndex;
        }

        public int ReadUByte()
        {
            return _buf[_readIndex++] & 0xFF;
        }

        /**
         * Reads a <c>short</c> which was encoded in <em>little endian</em> format.
         */
        public int ReadUshortLE()
        {
            int i = _readIndex;

            int b0 = _buf[i++] & 0xFF;
            int b1 = _buf[i++] & 0xFF;
            _readIndex = i;
            return (b1 << 8) + (b0 << 0);
        }

        /**
         * Reads a <c>short</c> which spans the end of <c>prevBlock</c> and the start of this block.
         */
        public int ReadUshortLE(DataInputBlock prevBlock)
        {
            // simple case - will always be one byte in each block
            int i = prevBlock._buf.Length - 1;

            int b0 = prevBlock._buf[i++] & 0xFF;
            int b1 = _buf[_readIndex++] & 0xFF;
            return (b1 << 8) + (b0 << 0);
        }

        /**
         * Reads an <c>int</c> which was encoded in <em>little endian</em> format.
         */
        public int ReadIntLE()
        {
            int i = _readIndex;

            int b0 = _buf[i++] & 0xFF;
            int b1 = _buf[i++] & 0xFF;
            int b2 = _buf[i++] & 0xFF;
            int b3 = _buf[i++] & 0xFF;
            _readIndex = i;
            return (b3 << 24) + (b2 << 16) + (b1 << 8) + (b0 << 0);
        }

        /**
         * Reads an <c>int</c> which spans the end of <c>prevBlock</c> and the start of this block.
         */
        public int ReadIntLE(DataInputBlock prevBlock, int prevBlockAvailable)
        {
            byte[] buf = new byte[4];

            ReadSpanning(prevBlock, prevBlockAvailable, buf);
            int b0 = buf[0] & 0xFF;
            int b1 = buf[1] & 0xFF;
            int b2 = buf[2] & 0xFF;
            int b3 = buf[3] & 0xFF;
            return (b3 << 24) + (b2 << 16) + (b1 << 8) + (b0 << 0);
        }

        /**
         * Reads a <c>long</c> which was encoded in <em>little endian</em> format.
         */
        public long ReadLongLE()
        {
            int i = _readIndex;

            int b0 = _buf[i++] & 0xFF;
            int b1 = _buf[i++] & 0xFF;
            int b2 = _buf[i++] & 0xFF;
            int b3 = _buf[i++] & 0xFF;
            int b4 = _buf[i++] & 0xFF;
            int b5 = _buf[i++] & 0xFF;
            int b6 = _buf[i++] & 0xFF;
            int b7 = _buf[i++] & 0xFF;
            _readIndex = i;
            return (((long)b7 << 56) +
                    ((long)b6 << 48) +
                    ((long)b5 << 40) +
                    ((long)b4 << 32) +
                    ((long)b3 << 24) +
                    (b2 << 16) +
                    (b1 << 8) +
                    (b0 << 0));
        }

        /**
         * Reads a <c>long</c> which spans the end of <c>prevBlock</c> and the start of this block.
         */
        public long ReadLongLE(DataInputBlock prevBlock, int prevBlockAvailable)
        {
            byte[] buf = new byte[8];

            ReadSpanning(prevBlock, prevBlockAvailable, buf);

            int b0 = buf[0] & 0xFF;
            int b1 = buf[1] & 0xFF;
            int b2 = buf[2] & 0xFF;
            int b3 = buf[3] & 0xFF;
            int b4 = buf[4] & 0xFF;
            int b5 = buf[5] & 0xFF;
            int b6 = buf[6] & 0xFF;
            int b7 = buf[7] & 0xFF;
            return (((long)b7 << 56) +
                    ((long)b6 << 48) +
                    ((long)b5 << 40) +
                    ((long)b4 << 32) +
                    ((long)b3 << 24) +
                    (b2 << 16) +
                    (b1 << 8) +
                    (b0 << 0));
        }

        /**
         * Reads a small amount of data from across the boundary between two blocks.  
         * The {@link #_readIndex} of this (the second) block is updated accordingly.
         * Note- this method (and other code) assumes that the second {@link DataInputBlock}
         * always is big enough to complete the read without being exhausted.
         */
        private void ReadSpanning(DataInputBlock prevBlock, int prevBlockAvailable, byte[] buf)
        {
            Array.Copy(prevBlock._buf, prevBlock._readIndex, buf, 0, prevBlockAvailable);
            int secondReadLen = buf.Length - prevBlockAvailable;
            Array.Copy(_buf, 0, buf, prevBlockAvailable, secondReadLen);
            _readIndex = secondReadLen;
        }

        /**
         * Reads <c>len</c> bytes from this block into the supplied buffer.
         */
        public void ReadFully(byte[] buf, int off, int len)
        {
            Array.Copy(_buf, _readIndex, buf, off, len);
            _readIndex += len;
        }
    }
}

