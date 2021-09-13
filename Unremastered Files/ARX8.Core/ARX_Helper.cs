using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace ARX
{
    public static class Helper
    {
        public static string GetCurrentMethodName([CallerMemberName]string strName = "")
        {
            return strName;
        }
    }
}
