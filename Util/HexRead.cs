using System;
using System.Text;
using System.IO;
using System.Collections;

namespace Cydua.NPOI.Util
{
    public class HexRead
    {
        /// <summary>
        /// This method reads hex data from a filename and returns a byte array.
        /// The file may contain line comments that are preceeded with a # symbol.
        /// </summary>
        /// <param name="filename">The filename to read</param>
        /// <returns>The bytes read from the file.</returns>
        /// <exception cref="IOException">If there was a problem while reading the file.</exception>
        public static byte[] ReadData( String filename )
        {
            FileStream stream = new FileStream(filename,FileMode.Open,FileAccess.Read);
            try
            {
                return ReadData( stream, -1 );
            }
            finally
            {
                stream.Close();
            }
        }

        /// <summary>
        /// Same as ReadData(String) except that this method allows you to specify sections within
        /// a file.  Sections are referenced using section headers in the form:
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="section">The section.</param>
        /// <returns></returns>
        public static byte[] ReadData(Stream stream, String section )
        {
        	
            try
            {
                StringBuilder sectionText = new StringBuilder();
                bool inSection = false;
                int c = stream.ReadByte();
                while ( c != -1 )
                {
                    switch ( c )
                    {
                        case '[':
                            inSection = true;
                            break;
                        case '\n':
                        case '\r':
                            inSection = false;
                            sectionText = new StringBuilder();
                            break;
                        case ']':
                            inSection = false;
                            if (sectionText.ToString().Equals(section))
                            {
                                return ReadData(stream, '[');
                            }
                            sectionText = new StringBuilder();
                            break;
                        default:
                            if ( inSection ) sectionText.Append( (char) c );
                            break;
                    }
                    c = stream.ReadByte();
                }
            }
            finally
            {
                stream.Close();
            }
            throw new IOException( "Section '" + section + "' not found" );
        }
        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="section">The section.</param>
        /// <returns></returns>
        public static byte[] ReadData( String filename, String section )
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                return ReadData(stream, section);
            }
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="eofChar">The EOF char.</param>
        /// <returns></returns>
        public static byte[] ReadData( Stream stream, int eofChar )
        {
            int characterCount = 0;
            byte b = (byte) 0;
            ArrayList bytes = new ArrayList();
            bool done = false;
            while ( !done )
            {
                int count = stream.ReadByte();
                char baseChar = 'a';
                if ( count == eofChar ) break;
                switch ( count )
                {
                    case '#':
                        ReadToEOL( stream );
                        break;
                    case '0': case '1': case '2': case '3': case '4': case '5':
                    case '6': case '7': case '8': case '9':
                        b <<= 4;
                        b += (byte) ( count - '0' );
                        characterCount++;
                        if ( characterCount == 2 )
                        {
                            bytes.Add( (byte)b );
                            characterCount = 0;
                            b = (byte) 0;
                        }
                        break;
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                        baseChar = 'A';
                        b <<= 4;
                        b += (byte)(count + 10 - baseChar);
                        characterCount++;
                        if (characterCount == 2)
                        {
                            bytes.Add((byte)b);
                            characterCount = 0;
                            b = (byte)0;
                        }
                        break;
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                        b <<= 4;
                        b += (byte) ( count + 10 - baseChar );
                        characterCount++;
                        if ( characterCount == 2 )
                        {
                            bytes.Add( (byte) b );
                            characterCount = 0;
                            b = (byte) 0;
                        }
                        break;
                    case -1:
                        done = true;
                        break;
                    default :
                        break;
                }
            }
            byte[] polished = (byte[]) bytes.ToArray(typeof(byte) );
            //byte[] rval = new byte[polished.Length];
            //for ( int j = 0; j < polished.Length; j++ )
            //{
            //    rval[j] = polished[j];
            //}
            return polished;
        }

        /// <summary>
        /// Reads from string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] ReadFromString(String data)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                return ReadData(ms, -1);
            }
        }

        /// <summary>
        /// Reads to EOL.
        /// </summary>
        /// <param name="stream">The stream.</param>
        static private void ReadToEOL( Stream stream )
        {
            int c = stream.ReadByte();
            while ( c != -1 && c != '\n' && c != '\r' )
            {
                c = stream.ReadByte();
            }
        }
    }
}
