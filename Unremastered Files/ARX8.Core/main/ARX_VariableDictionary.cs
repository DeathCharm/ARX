using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX;

namespace ARX
{
    [Serializable]
    public class ARX_VariableDictionary : SerializableDictionaryClass<string, int>
    {
        public ARX_VariableDictionary Clone
        {
            get
            {
                return (ARX_VariableDictionary)this.MemberwiseClone();
            }
        }

            const char gstrLimiter = '|';
            const char gstrPairLimiter = '~';

        public string SaveToString()
        {
            string str = "";

            foreach (KeyValuePair<string, int> pair in this)
            {
                str += pair.Key + gstrLimiter + pair.Value + gstrPairLimiter;
            }

            return str;
        }

        public static ARX_VariableDictionary Deserialize(string str)
        {
            ARX_VariableDictionary oRet = new ARX_VariableDictionary();

            string[] pairs = str.Split(gstrPairLimiter);

            foreach (string pair in pairs)
            {
                string[] strValues = pair.Split(gstrLimiter);

                if (strValues.Length < 2)
                    continue;

                string strKey = strValues[0];
                int nValue = Convert.ToInt32( strValues[1]);
                oRet[strKey] = nValue;
            }

            return oRet;

        }

    }

    
}
