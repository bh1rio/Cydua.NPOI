using System;
using System.IO;

namespace Cydua.NPOI.HPSF
{
    /**
     * The Character Encoding is not supported.
     *
     * @author  Asmus Freytag
     * @since   JDK1.1
     */
    public class UnsupportedEncodingException : IOException
    {
        //private static long serialVersionUID = -4274276298326136670L;

        /**
         * Constructs an UnsupportedEncodingException without a detail message.
         */
        public UnsupportedEncodingException()
            : base()
        {

        }

        /**
         * Constructs an UnsupportedEncodingException with a detail message.
         * @param s Describes the reason for the exception.
         */
        public UnsupportedEncodingException(String s)
            : base(s)
        {

        }
    }
}