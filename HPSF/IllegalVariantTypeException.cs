using System;
using Cydua.NPOI.Util;

namespace Cydua.NPOI.HPSF
{
    /// <summary>
    /// This exception is thrown if HPSF encounters a variant type that is illegal
    /// in the current context.
    /// @author Rainer Klute 
    /// <a href="mailto:klute@rainer-klute.de">&lt;klute@rainer-klute.de&gt;</a>
    /// @since 2004-06-21
    /// </summary>
    [Serializable]
    public class IllegalVariantTypeException : VariantTypeException
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalVariantTypeException"/> class.
        /// </summary>
        /// <param name="variantType">The unsupported variant type</param>
        /// <param name="value">The value</param>
        /// <param name="msg">A message string</param>
        public IllegalVariantTypeException(long variantType,
                                           Object value, String msg) : base(variantType, value, msg)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalVariantTypeException"/> class.
        /// </summary>
        /// <param name="variantType">The unsupported variant type</param>
        /// <param name="value">The value.</param>
        public IllegalVariantTypeException(long variantType,
                                           Object value) : this(variantType, value, "The variant type " + variantType + " (" +
                 Variant.GetVariantName(variantType) + ", " +
                 HexDump.ToHex(variantType) + ") is illegal in this context.")
        {

        }

    }
}