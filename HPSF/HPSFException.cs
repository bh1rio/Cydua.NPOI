using System;

namespace Cydua.NPOI.HPSF
{
    /// <summary>
    /// This exception is the superclass of all other checked exceptions thrown
    /// in this package. It supports a nested "reason" throwable, i.e. an exception
    /// that caused this one To be thrown.
    /// @author Rainer Klute 
    /// <a href="mailto:klute@rainer-klute.de">&lt;klute@rainer-klute.de&gt;</a>
    /// @since 2002-02-09
    /// </summary>
    [Serializable]
    public class HPSFException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="HPSFException"/> class.
        /// </summary>
        public HPSFException()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HPSFException"/> class.
        /// </summary>
        /// <param name="msg">The message string.</param>
        public HPSFException(String msg)
            : base(msg)
        {

        }



        /// <summary>
        /// Initializes a new instance of the <see cref="HPSFException"/> class.
        /// </summary>
        /// <param name="reason">The reason, i.e. a throwable that indirectly
        /// caused this exception.</param>
        public HPSFException(Exception reason)
            : base("", reason)
        {

        }



        /// <summary>
        /// Initializes a new instance of the <see cref="HPSFException"/> class.
        /// </summary>
        /// <param name="msg">The message string.</param>
        /// <param name="reason">The reason, i.e. a throwable that indirectly
        /// caused this exception.</param>
        public HPSFException(String msg, Exception reason)
            : base(msg, reason)
        {
        }



        /// <summary>
        /// Returns the {@link Exception} that caused this exception To
        /// be thrown or null if there was no such {@link
        /// Exception}.
        /// </summary>
        /// <value>The reason.</value>
        public Exception Reason
        {
            get { return this.InnerException; }
        }

    }
}