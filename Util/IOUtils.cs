namespace Cydua.NPOI.Util
{
    using System.IO;

    public class IOUtils
    {
        /// <summary>
        /// Reads all the data from the input stream, and returns
        /// the bytes Read.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        /// <remarks>Tony Qu changed the code</remarks>
        public static byte[] ToByteArray(Stream stream)
        {
            byte[] outputBytes=new byte[stream.Length];
            stream.Read(outputBytes,0, (int)stream.Length);
            return outputBytes;
        }
           public static byte[] ToByteArray(ByteBuffer buffer, int length)
        {
            if (buffer.HasBuffer && buffer.Offset == 0)
            {
                // The backing array should work out fine for us
                return buffer.Buffer;
            }

            byte[] data = new byte[length];
            buffer.Read(data);
            return data;
        }


        /// <summary>
        /// Reads the fully.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static int ReadFully(Stream stream, byte[] b)
        {
            return ReadFully(stream, b, 0, b.Length);
        }

        /// <summary>
        /// Same as the normal 
        /// <c>in.Read(b, off, len)</c>
        /// , but tries to ensure that the entire len number of bytes is Read.
        /// If the end of file is reached before any bytes are Read, returns -1.
        /// If the end of the file is reached after some bytes are
        /// Read, returns the number of bytes Read.
        /// If the end of the file isn't reached before len
        /// bytes have been Read, will return len bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="b">The b.</param>
        /// <param name="off">The off.</param>
        /// <param name="len">The len.</param>
        /// <returns></returns>

        public static int ReadFully(Stream stream, byte[] b, int off, int len)
        {
            int total = 0;
            while (true)
            {
                int got = stream.Read(b, off + total, len - total - off);
                total += got;
                if (stream.Position == stream.Length)
                {
                    return total;
                }
                if (total == len)
                {
                    return total;
                }
            }

        }

        /// <summary>
        /// Copies all the data from the given InputStream to the OutputStream. It
        /// leaves both streams open, so you will still need to close them once done.
        /// </summary>
        /// <param name="inp"></param>
        /// <param name="out1"></param>
        public static void Copy(Stream inp, Stream out1)
        {
            byte[] buff = new byte[4096];
            inp.Position = 0;
            int count;
            while ((count = inp.Read(buff, 0, buff.Length)) >0)
            {
                out1.Write(buff, 0, count);
            }
        }

        public static long CalculateChecksum(byte[] data)
        {
            CRC32 sum = new CRC32();
            return (long)sum.ByteCRC(ref data);
        }
    }
}
