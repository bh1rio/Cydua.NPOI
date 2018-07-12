using System;
using System.IO;

namespace Cydua.NPOI.POIFS.FileSystem
{
    internal class CloseIgnoringInputStream : Stream
    {

        private Stream _is;

        public CloseIgnoringInputStream(Stream stream)
        {
            _is = stream;
        }
        public int Read()
        {
            return (int)_is.ReadByte();
        }
        public override int Read(byte[] b, int off, int len)
        {
            return _is.Read(b, off, len);
        }
        public override void Close()
        {
            // do nothing
        }
        public override void Flush()
        {
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0L;
        }

        public override void SetLength(long value)
        {
        }
        // Properties
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override long Length
        {
            get
            {
                return (long)this._is.Length;
            }
        }

        public override long Position
        {
            get
            {
                return (long)this._is.Position;
            }
            set
            {
                this._is.Position = Convert.ToInt32(value);
            }
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
        }
    }
}
