using System;
using Cydua.NPOI.DDF;
using Cydua.NPOI.HSSF.UserModel;

namespace Cydua.NPOI.HSSF.Model
{


    public class ConvertAnchor
    {
        /// <summary>
        /// Creates the anchor.
        /// </summary>
        /// <param name="userAnchor">The user anchor.</param>
        /// <returns></returns>
        public static EscherRecord CreateAnchor(HSSFAnchor userAnchor)
        {
            if (userAnchor is HSSFClientAnchor)
            {
                HSSFClientAnchor a = (HSSFClientAnchor)userAnchor;

                EscherClientAnchorRecord anchor = new EscherClientAnchorRecord();
                anchor.RecordId = EscherClientAnchorRecord.RECORD_ID;
                anchor.Options = (short)0x0000;
                anchor.Flag = (short)a.AnchorType;
                anchor.Col1 = (short)Math.Min(a.Col1, a.Col2);
                anchor.Dx1 = (short)a.Dx1;
                anchor.Row1 = (short)Math.Min(a.Row1, a.Row2);
                anchor.Dy1 = (short)a.Dy1;

                anchor.Col2 = (short)Math.Max(a.Col1, a.Col2);
                anchor.Dx2 = (short)a.Dx2;
                anchor.Row2 = (short)Math.Max(a.Row1, a.Row2);
                anchor.Dy2 = (short)a.Dy2;
                return anchor;
            }
            else
            {
                HSSFChildAnchor a = (HSSFChildAnchor)userAnchor;
                EscherChildAnchorRecord anchor = new EscherChildAnchorRecord();
                anchor.RecordId = EscherChildAnchorRecord.RECORD_ID;
                anchor.Options = (short)0x0000;
                anchor.Dx1 = (short)Math.Min(a.Dx1, a.Dx2);
                anchor.Dy1 = (short)Math.Min(a.Dy1, a.Dy2);
                anchor.Dx2 = (short)Math.Max(a.Dx2, a.Dx1);
                anchor.Dy2 = (short)Math.Max(a.Dy2, a.Dy1);
                return anchor;
            }
        }

    }
}