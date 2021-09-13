using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace ARX
{

    /// <summary>
    /// A Stat box for StatQuads
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "ARX/ Stat Box - Quad")]
    public class ARX_StatBox_Quad : ScriptableObject
    {
        [FormerlySerializedAs("moa_statBoxes")]
        [SerializeField]
        protected List<ARX_StatQuad> moa_statBoxes = new List<ARX_StatQuad>();
        
        public List<ARX_StatQuad> AsList { get { return moa_statBoxes; } }

        public void Clear()
        {
            moa_statBoxes.Clear();
        }

        public void SetStatBoxes(List<ARX_StatQuad> oaStats)
        {
            moa_statBoxes = new List<ARX_StatQuad>(oaStats.ToArray());
        }

        public ARX_StatBox_Quad Clone
        {
            get
            {


                ARX_StatBox_Quad other = ScriptableObject.CreateInstance<ARX_StatBox_Quad>();

                List<ARX_StatQuad> obuf = new List<ARX_StatQuad>();

                for (int i = 0; i < moa_statBoxes.Count; i++)
                {
                    ARX_StatQuad quad = (ARX_StatQuad)moa_statBoxes[i].Clone;
                    other.CreateNewStat(quad.ID, quad.Base, quad.Bonus, quad.Current);
                }

                return other;
            }
        }

        public int Count
        {
            get
            {
                return moa_statBoxes.Count;
            }
        }

        public ARX_StatQuad CreateNewStat(string nID, float nBase, float nBonus, float nCurrent)
        {
            ARX_StatQuad buf = new ARX_StatQuad();

            buf.SetNew(nID, nBase, nBonus, nCurrent);
            moa_statBoxes.Add(buf);
            return buf;
        }

        public ARX_StatQuad CreateNewStat(ARX_StatQuad other)
        {
            return CreateNewStat(other.ID, other.Base, other.Bonus, other.Current);
        }

        //public ARX_StatQuad CreateNewStat(string nID, float nBase, bool bDeletable = false)
        //{
        //    ARX_StatQuad quad = CreateNewStat(nID, nBase, 0, nBase);
        //    quad.IsDeleteable = bDeletable;

        //    return quad;
        //}

        //public ARX_StatQuad CreateNewStat(ARX_StatQuad oQuad)
        //{
        //    ARX_StatQuad quad = CreateNewStat(oQuad.ID, oQuad.Base, oQuad.Bonus, oQuad.Current);
        //    quad.IsDeleteable = oQuad.IsDeleteable;

        //    return quad;
        //}


        /// <summary>
        /// Adds any stats the other box has that this is missing.
        /// Does not combine values of stats.
        /// </summary>
        /// <param name="other"></param>
        public void Merge(ARX_StatBox_Quad other)
        {
            foreach (ARX_StatQuad quad in other.moa_statBoxes)
            {
                if (HasStat(quad.ID) == false)
                {
                    CreateNewStat(quad);
                }
            }
        }
        
        public bool HasStat(string nCode)
        {
            foreach (ARX_StatQuad quad in moa_statBoxes)
                if (quad.ID == nCode)
                    return true;
            return false;
        }

        public ARX_StatQuad GetStat(string nCode)
        {
            for (int i = 0; i < moa_statBoxes.Count(); i++)
            {
                if (moa_statBoxes[i].ID == nCode)
                {
                    moa_statBoxes[i].Reevaluate();
                    return moa_statBoxes[i];
                }
            }

            return CreateNewStat(nCode, 0, 0, 0);
        }

        public bool DeleteStat(ARX_StatQuad oStat)
        {
            moa_statBoxes.Remove(oStat);
            return true;
        }

        public override string ToString()
        {
            string strRet = "";
            foreach (ARX_StatQuad quad in moa_statBoxes)
                strRet += quad.ToString() + "     ";
            return strRet;
        }

        public bool DeleteStat(string nCode)
        {
            ARX_StatQuad stat = GetStat(nCode);

            if (stat != null)
            {
                return DeleteStat(stat);
            }
            return false;
        }

        public string[] GetStringStats
        {
            get
            {
                List<string> olist = new List<string>();
                foreach (ARX_StatQuad stat in moa_statBoxes)
                    olist.Add(stat.AsStatString);
                return olist.ToArray();
            }
        }

        public void LoadStat(string strStat)
        {
            try
            {
                string[] astrSplits = strStat.Split(ARX_StatQuad.delimiter);
                string strID = astrSplits[0];
                ARX_StatQuad stat = GetStat(strID);
                stat.Base = Convert.ToInt32(astrSplits[1]);
                stat.Bonus = Convert.ToInt32(astrSplits[2]);
                stat.Current = Convert.ToInt32(astrSplits[3]);
            }
            catch
            {
                Debug.LogError("Stat string " + strStat + " was invalid.");
            }
        }

        public string GetSerializedString()
        {
            return ARX_File.SerializeObject(this);
        }

    }
}
