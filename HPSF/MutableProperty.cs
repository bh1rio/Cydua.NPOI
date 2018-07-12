using System.IO;
using Cydua.NPOI.Util;

namespace Cydua.NPOI.HPSF
{
    /// <summary>
    /// Adds writing capability To the {@link Property} class.
    /// Please be aware that this class' functionality will be merged into the
    /// {@link Property} class at a later time, so the API will Change.
    /// @author Rainer Klute 
    /// <a href="mailto:klute@rainer-klute.de">&lt;klute@rainer-klute.de&gt;</a>
    /// @since 2003-08-03
    /// </summary>
    public class MutableProperty : Property
    {

        /// <summary>
        /// Creates an empty property. It must be Filled using the Set method To
        /// be usable.
        /// </summary>
        public MutableProperty()
        { }



        /// <summary>
        /// Initializes a new instance of the <see cref="MutableProperty"/> class.
        /// </summary>
        /// <param name="p">The property To copy.</param>
        public MutableProperty(Property p)
        {
            this.ID = p.ID;
            this.Type = p.Type;
            this.Value = p.Value;
        }


        /// <summary>
        /// Writes the property To an output stream.
        /// </summary>
        /// <param name="out1">The output stream To Write To.</param>
        /// <param name="codepage">The codepage To use for writing non-wide strings</param>
        /// <returns>the number of bytes written To the stream</returns>
        public int Write(Stream out1, int codepage)
        {
            int length = 0;
            long variantType = this.Type;

            /* Ensure that wide strings are written if the codepage is Unicode. */
            if (codepage == CodePageUtil.CP_UNICODE && variantType == Variant.VT_LPSTR)
                variantType = Variant.VT_LPWSTR;

            length += TypeWriter.WriteUIntToStream(out1, (uint)variantType);
            length += VariantSupport.Write(out1, variantType, this.Value, codepage);
            return length;
        }

    }
}