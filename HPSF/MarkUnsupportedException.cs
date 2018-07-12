using System;

namespace Cydua.NPOI.HPSF
{
    /// <summary>
    /// This exception is thrown if an {@link java.io.InputStream} does
    /// not support the {@link java.io.InputStream#mark} operation.
    /// @author Rainer Klute 
    /// <a href="mailto:klute@rainer-klute.de">&lt;klute@rainer-klute.de&gt;</a>
    /// @since 2002-02-09
    /// </summary>
    [Serializable]
    public class MarkUnsupportedException : HPSFException
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkUnsupportedException"/> class.
        /// </summary>
        public MarkUnsupportedException():base()
        {
            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MarkUnsupportedException"/> class.
        /// </summary>
        /// <param name="msg">The exception's message string.</param>
        public MarkUnsupportedException(String msg):base(msg)
        {
            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MarkUnsupportedException"/> class.
        /// </summary>
        /// <param name="reason">This exception's underlying reason.</param>
        public MarkUnsupportedException(Exception reason):base(reason)
        {
            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MarkUnsupportedException"/> class.
        /// </summary>
        /// <param name="msg">The exception's message string</param>
        /// <param name="reason">This exception's underlying reason</param>
        public MarkUnsupportedException(String msg, Exception reason):base(msg, reason)
        {
            
        }

    }
}