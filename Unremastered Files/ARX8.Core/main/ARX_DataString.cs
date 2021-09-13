using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARX
{
    /// <summary>
    /// Enum telling if a DataString was successful in converting
    /// a data value into a string or vice-versa.
    /// </summary>
    public enum ARX_SUCCESSCODE { SUCCESS, FAILURE };

    /// <summary>
    /// Converts multiple given primitive variables into a single string
    /// for sending event messages amongst ARX_Actors and Processes
    /// </summary>
    [System.Serializable]
    public class DataString
    {
        private object _sender;
        public object Sender
        {
            get
            {
                return _sender;
            }
        }

        public void SetSender(object obj)
        {
            _sender = obj;
        }

        public string mstr_data = "";

        const string gstrSourceTag = "source";
        
        public static char[] gstrDelimiters = new char[] {
            '|'};

        public DataString Clone
        {
            get
            {
                DataString dat = (DataString)this.MemberwiseClone();
                dat.mn_uniqueID = mn_uniqueID;
                return dat;
            }
        }

        public override string ToString()
        {
            return "ARX Datastring: " + mstr_data;
        }

        public void SetData(List<DataPair> oList)
        {
            mstr_data = CombineDataPairsIntoString(oList);
        }


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
        /// If true, this message has been processed by some process that doesn't want
        /// any other processes working on it.
        /// </summary>
        public bool IsConsumed = false;

        public void ConsumeMessage()
        {
            IsConsumed = true;
        }

        [HideInInspector]
        int mn_uniqueID = 0;
        static int gnLastID = 1;
        int GetUniqueID { get { return gnLastID++;} }
        public int UniqueID { get { return mn_uniqueID; } }

        public float mf_messageDelay = 0;

        void MessageNotProcessed()
        {
            Debug.Log("DataString not processed: | " + mstr_data + " |");
        }
        
        

        public DataString(object sender)
        {
            mn_uniqueID = GetUniqueID;
            SetSender(sender);
        }


        public List<DataPair> GetAsListOfPairs()
        {
            string[] astr = mstr_data.Split(DataString.gstrDelimiters);
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



        /// <summary>
        /// Returns the data element whose location starts at the given index on this message's data string.
        /// </summary>
        /// <param name="nStartingPosition"></param>
        /// <returns></returns>
        string GetElement(int nStartingPosition)
        {
            return GetElement(mstr_data, nStartingPosition);
        }

        /// <summary>
        /// Returns the data element whose location starts at the given index on the given string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="nStartingPosition"></param>
        /// <returns></returns>
        string GetElement(string str, int nStartingPosition)
        {
            str = str.ToLower();
            int x = 0;
            //Cycle through the given string until the next delimiter is reached,
            //then return the string that lies inbetween the starting position and the reached
            //delimiter.
            for (int i = nStartingPosition; i < str.Length + 1; i++)
            {
                if (i == str.Length || IsDelimiter(str[i]))
                    return str.Substring(nStartingPosition, x);

                x++;
            }
            return "";
        }

        /// <summary>
        /// Is the given character included in the array of delimiter characters?
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        bool IsDelimiter(char ch)
        {
            for (int i = 0; i < gstrDelimiters.Length; i++)
                if (gstrDelimiters[i] == ch)
                    return true;
            return false;
        }
        

        /// <summary>
        /// Returns a every data element that is in the given category
        /// </summary>
        /// <param name="strCategory"></param>
        /// <returns></returns>
        public string ElementCategoryValues(string strCategory)
        {
            strCategory = strCategory.ToLower();

            //Split the data into seperate strings
            string[] astrSplitElements = mstr_data.Split(gstrDelimiters);
            string strReturn = "";

            for (int i = 0; i < astrSplitElements.Length; i++)
            //Find the category string
            //If there is a string after the category string, return it
            //Else, return null
            {

                //If the current element is a source tag, as it and the next element to the string, then skip ahead by one
                if (astrSplitElements[i] == gstrSourceTag &&
                i < astrSplitElements.Length - 3
                && astrSplitElements[i + 2] == strCategory)
                {
                    strReturn += astrSplitElements[i + 1] + gstrDelimiters[0] +
                        strCategory + gstrDelimiters[0] +
                        astrSplitElements[i + 3] + gstrDelimiters[0];
                    i += 3;
                    continue;
                }

                if (astrSplitElements[i] == strCategory && i < astrSplitElements.Length + 1)
                {
                    strReturn += astrSplitElements[i + 1];
                    i++;
                }

            }

            return strReturn;
        }

        public Vector3 GetVector()
        {
            float x = GetFloat("x");
            float y = GetFloat("y");
            float z = GetFloat("z");
            return new Vector3(x, y, z);
        }

        public string GetString(ref ARX_SUCCESSCODE eCode, string strCategory)
        {
            string strReturn = GetString(strCategory);
            if (strReturn == "")
                eCode = ARX_SUCCESSCODE.FAILURE;
            else
                eCode = ARX_SUCCESSCODE.SUCCESS;
            return strReturn;
        }


        public string GetString(string strCategory)
        {
            strCategory = strCategory.ToLower();

            //Split the data into seperate strings
            string[] astrSplitElements = mstr_data.Split(gstrDelimiters);

            for (int i = 0; i < astrSplitElements.Length; i++)
            //Find the category string
            //If there is a string after the category string, return it
            //Else, return null
            {
                if (astrSplitElements[i] == strCategory && i < astrSplitElements.Length + 1)
                {
                    return astrSplitElements[i + 1];
                }
            }

            return "";
        }

        public string[] GetData(string strCategory)
        {
            strCategory = strCategory.ToLower();

            //Split the data into seperate strings
            string[] astrSplitElements = mstr_data.Split(gstrDelimiters);
            List<string> astrReturn = new List<string>();

            for (int i = 0; i < astrSplitElements.Length; i++)
            //Find the category string
            //If there is a string after the category string, return it
            //Else, return null
            {

                if (astrSplitElements[i] == strCategory && i < astrSplitElements.Length + 1)
                {
                    astrReturn.Add(astrSplitElements[i + 1]);
                }

            }

            return astrReturn.ToArray();
        }

        public int GetInt(string strCategory)
        {
            ARX_SUCCESSCODE eCode = ARX_SUCCESSCODE.FAILURE;
            return GetInt(ref eCode, strCategory);
        }

        public int GetInt(ref ARX_SUCCESSCODE eCode, string strCategory)
        {
            string[] data = GetData(strCategory.ToLower());
            int nfReturn = 0;

            if (data.Length == 0)
                return 0;

            foreach (string str in data)
            {
                int nfAdd = 0;
                try
                {
                    nfAdd = Convert.ToInt32(str);
                }
                catch (ArgumentException e)
                {

                    e.ToString();
                    eCode = ARX_SUCCESSCODE.FAILURE;
                    return 0;
                }
                nfReturn += nfAdd;
            }
            eCode = ARX_SUCCESSCODE.SUCCESS;
            return nfReturn;
        }

        public float GetFloat(string strCategory)
        {
            ARX_SUCCESSCODE eCode = ARX_SUCCESSCODE.FAILURE;
            return GetFloat(ref eCode, strCategory);
        }


        public float GetFloat(ref ARX_SUCCESSCODE eCode, string strCategory)
        {
            string[] data = GetData(strCategory.ToLower());
            float nfReturn = 0;

            if (data.Length == 0)
                return 0;

            foreach (string str in data)
            {
                float nfAdd = 0;
                try
                {
                    nfAdd = (float)Convert.ToDouble(str);
                }
                catch (ArgumentException e)
                {
                    e.ToString();
                    eCode = ARX_SUCCESSCODE.FAILURE;
                    return 0;
                }
                nfReturn += nfAdd;
            }

            eCode = ARX_SUCCESSCODE.SUCCESS;
            return nfReturn;
        }

        public double GetDouble(string strCategory)
        {
            ARX_SUCCESSCODE eCode = ARX_SUCCESSCODE.FAILURE;
            return GetDouble(ref eCode, strCategory);
        }


        public double GetDouble(ref ARX_SUCCESSCODE eCode, string strCategory)
        {
            string[] data = GetData(strCategory.ToLower());
            double nfReturn = 0;

            if (data.Length == 0)
                return 0;

            foreach (string str in data)
            {
                double nfAdd = 0;
                try
                {
                    nfAdd = (double)Convert.ToDouble(str);
                }
                catch (ArgumentException e)
                {
                    e.ToString();
                    eCode = ARX_SUCCESSCODE.FAILURE;
                    return 0;
                }
                nfReturn += nfAdd;
            }

            eCode = ARX_SUCCESSCODE.SUCCESS;
            return nfReturn;
        }

        public bool GetBool(string strCategory)
        {
            ARX_SUCCESSCODE eCode = ARX_SUCCESSCODE.FAILURE;
            return GetBool(ref eCode, strCategory);
        }


        public bool GetBool(ref ARX_SUCCESSCODE eCode, string strCategory)
        {
            string[] result = GetData(strCategory.ToLower());

            if (result.Length == 0)
                return false;

            bool bReturn = false;

            try
            {
                bReturn = Convert.ToBoolean(result[0]);

            }
            catch
            {
                eCode = ARX_SUCCESSCODE.FAILURE;
                return false;
            }


            eCode = ARX_SUCCESSCODE.SUCCESS;
            return bReturn;
        }

        public bool HasElement(string strCategory)
        {
            string result = ElementCategoryValues(strCategory.ToLower());

            if (result == "")
                return false;
            return true;
        }

        void AddElement(string strElement)
        {
            mstr_data += strElement;
        }
        
        public void SetBool(string strName, bool value)
        {
            RemoveElement(strName);
            AddElement(
                strName.ToLower() + gstrDelimiters[0] + value.ToString() + gstrDelimiters[0]);
        }
        public void SetInt(string strName, int value)
        {
            RemoveElement(strName);
            AddElement(
                strName.ToLower() + gstrDelimiters[0] + value.ToString() + gstrDelimiters[0]);
        }
        public void SetDouble(string strName, double value)
        {
            RemoveElement(strName);
            AddElement(
                strName.ToLower() + gstrDelimiters[0] + value.ToString() + gstrDelimiters[0]);
        }

        public void AddDouble(string strName, double value)
        {
            double nfCurrent = GetDouble(strName);
            value += nfCurrent;

            RemoveElement(strName);
            AddElement(
                strName.ToLower() + gstrDelimiters[0] + value.ToString() + gstrDelimiters[0]);
        }

        public void AddFloat(string strName, float value)
        {
            float nfCurrent = GetFloat(strName);
            value += nfCurrent;

            RemoveElement(strName);
            AddElement(
                strName.ToLower() + gstrDelimiters[0] + value.ToString() + gstrDelimiters[0]);
        }

        public void SetVector(Vector3 vec)
        {
            SetDouble("x", vec.x);
            SetDouble("y", vec.y);
            SetDouble("z", vec.z);
        }

        public void SetString(string strName, string value)
        {
            RemoveElement(strName);

            AddElement(
                strName.ToLower() + gstrDelimiters[0] + value + gstrDelimiters[0]);
        }

        public void RemoveElement(string strName)
        {
            //Split the data
            string strReturn = "";
            string[] asSplits = mstr_data.Split(gstrDelimiters[0]);
            mstr_data = "";

            if (asSplits.Length < 2)
            {
                return;
            }

            for (int i = 0; i < asSplits.Length && i + 1 < asSplits.Length; i+=2)
            {
                //Debug.Log("Seeking to remove |" + strName + "|. Current element is |" + asSplits[i] + "|");

                //If the current tag is the target to remove, skip
                if (asSplits[i] == strName)
                    continue;

                    ////If the previous tag is the target to remove, skip
                    //if (i > 0 && asSplits[i - 1] == strName)
                    //    continue;
                    
                    strReturn += asSplits[i] + gstrDelimiters[0] + asSplits[i+1] + gstrDelimiters[0];
                    continue;
                

            }

            mstr_data = strReturn;
        }
    }


}