using Cydua.NPOI.HPSF;
using Cydua.NPOI.HPSF.Extractor;

namespace Cydua.NPOI
{
    /// <summary>
    /// Common Parent for OLE2 based Text Extractors
    /// of POI Documents, such as .doc, .xls
    /// You will typically find the implementation of
    /// a given format's text extractor under NPOI.Format.Extractor
    /// </summary>
    /// <remarks>
    /// @see org.apache.poi.hssf.extractor.ExcelExtractor
    /// @see org.apache.poi.hslf.extractor.PowerPointExtractor
    /// @see org.apache.poi.hdgf.extractor.VisioTextExtractor
    /// @see org.apache.poi.hwpf.extractor.WordExtractor
    /// </remarks>
    public abstract class POIOLE2TextExtractor : POITextExtractor
    {
        /// <summary>
        /// Creates a new text extractor for the given document
        /// </summary>
        /// <param name="document"></param>
        public POIOLE2TextExtractor(POIDocument document)
            : base(document)
        {

        }

        /// <summary>
        /// Returns the document information metadata for the document
        /// </summary>
        /// <value>The doc summary information.</value>
        public virtual DocumentSummaryInformation DocSummaryInformation
        {
            get
            {
                return document.DocumentSummaryInformation;
            }
        }
        /// <summary>
        /// Returns the summary information metadata for the document
        /// </summary>
        /// <value>The summary information.</value>
        public virtual SummaryInformation SummaryInformation
        {
            get
            {
                return document.SummaryInformation;
            }
        }

        /// <summary>
        /// Returns an HPSF powered text extractor for the
        /// document properties metadata, such as title and author.
        /// </summary>
        /// <value></value>
        public override POITextExtractor MetadataTextExtractor
        {
            get
            {
                return new HPSFPropertiesExtractor(this);
            }
        }
    }
}