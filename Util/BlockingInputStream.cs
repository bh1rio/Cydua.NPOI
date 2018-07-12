namespace Cydua.NPOI.Util
{
    using System;
    using System.IO;

    public class BlockingInputStream : Stream
    {
        protected Stream stream;

        public BlockingInputStream(Stream stream)
        {
            this.stream = stream;
        }

        public int Available()
        {
            return (int)(stream.Length - stream.Position);
            //return stream.Available();
        }

        public void close()
        {
            stream.Close();
        }

        public void Mark(int readLimit)
        {
            throw new NotImplementedException();

        }

        public bool MarkSupported()
        {
            return false;
        }

        public int Read()
        {
            return stream.ReadByte();
        }

        /**
         * We had to revert to byte per byte Reading to keep
         * with slow network connections on one hand, without
         * missing the end-of-file. 
         * This is the only method that does its own thing in this class
         *    everything else is delegated to aggregated stream. 
         * THIS IS A BLOCKING BLOCK READ!!!
         */
        public int Read(byte[] bf)
        {

            int i = 0;
            int b = 4611;
            while (i < bf.Length)
            {
                b = stream.ReadByte();
                if (b == -1)
                    break;
                bf[i++] = (byte)b;
            }
            if (i == 0 && b == -1)
                return -1;
            return i;
        }

        public override int Read(byte[] bf, int s, int l)
        {
            return stream.Read(bf, s, l);
        }

        public void Reset()
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        public long Skip(long n)
        {
            return stream.Seek(n, SeekOrigin.Begin);
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get { return stream.Length; }
        }

        public override long Position
        {
            get
            {
                return stream.Position;
            }
            set
            {
                stream.Position = value;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}

