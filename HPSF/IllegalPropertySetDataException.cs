using System;

namespace Cydua.NPOI.HPSF
{
    /// <summary>
    /// This exception is thrown when there is an illegal value Set in a
    /// {@link PropertySet}. For example, a {@link Variant#VT_BOOL} must
    /// have a value of <c>-1 (true)</c> or <c>0 (false)</c>.
    /// Any other value would trigger this exception. It supports a nested
    /// "reason" throwable, i.e. an exception that caused this one To be
    /// thrown.
    /// @author Drew Varner(Drew.Varner atDomain sc.edu)
    /// @since 2002-05-26
    /// </summary>
    [Serializable]
    public class IllegalPropertySetDataException : HPSFRuntimeException
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalPropertySetDataException"/> class.
        /// </summary>
        public IllegalPropertySetDataException()
            : base()
        {

        }



        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalPropertySetDataException"/> class.
        /// </summary>
        /// <param name="msg">The exception's message string</param>
        public IllegalPropertySetDataException(String msg)
            : base(msg)
        {

        }



        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalPropertySetDataException"/> class.
        /// </summary>
        /// <param name="reason">This exception's underlying reason</param>
        public IllegalPropertySetDataException(Exception reason)
            : base(reason)
        {

        }



        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalPropertySetDataException"/> class.
        /// </summary>
        /// <param name="msg"> The exception's message string</param>
        /// <param name="reason">This exception's underlying reason</param>
        public IllegalPropertySetDataException(String msg,
                                               Exception reason)
            : base(msg, reason)
        {

        }

    }
}
