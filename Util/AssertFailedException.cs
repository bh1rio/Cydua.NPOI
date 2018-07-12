using System;
using System.Collections.Generic;
using System.Text;

namespace Cydua.NPOI.Util
{
    internal class AssertFailedException : ApplicationException
    {
        public AssertFailedException(string message)
            : base(message)
        {

        }
    }
}
