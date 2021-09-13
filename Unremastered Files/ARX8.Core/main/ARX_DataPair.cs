using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARX;

namespace ARX
{
    /// <summary>
    /// A helper class used by DataStrings to hold a string name and value
    /// and by ScriptableObject versions of DataStrings to display its values easily
    /// </summary>
    public class DataPair
    {
        public string mstr_name = "", mstr_value = "";

        public DataPair() { }
        public DataPair(string strName, string strValue)
        {
            mstr_name = strName;
            mstr_value = strValue;
        }

        /// <summary>
        /// Combines the List of DataPairs into a single string formatted for 
        /// DataString's to use
        /// </summary>
        /// <param name="olist"></param>
        /// <returns></returns>
        public static string CombineDataPairsIntoString(List<DataPair> olist)
        {
            string str = "";

            foreach (DataPair pair in olist)
            {
                str += pair.mstr_name + DataString.gstrDelimiters[0] + pair.mstr_value + DataString.gstrDelimiters[0];
            }

            if (str.Length > 0)
                str.Remove(str.Length - 1);

            return str;
        }

        /// <summary>
        /// Splits the given DataSTring's mstr_data variable into a list of DataPair's
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public static List<DataPair> SplitDataStringIntoPairs(DataString dat)
        {
            string[] astr = dat.mstr_data.Split(DataString.gstrDelimiters);
            List<DataPair> olist = new List<DataPair>();

            for (int i = 0; i < astr.Length; i += 2)
            {
                if (i + 1 >= astr.Length)
                    break;

                string strName = astr[i];
                string strValue = astr[i + 1];
                olist.Add(new DataPair(strName, strValue));
            }
            return olist;
        }
    }
}
