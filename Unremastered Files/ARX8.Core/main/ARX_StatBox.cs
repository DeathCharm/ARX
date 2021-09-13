using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ARX
{
    /// <summary>
    /// A Stat box for StatSingles
    /// </summary>
    [Serializable]
    public class ARX_StatSingleBox
    {

        public List<ARX_StatSingle> moa_statBoxes = new List<ARX_StatSingle>();
        public STATTYPE me_statType = STATTYPE.FLOAT;
        public string mstr_defaultString = "";
        public string mstr_defaultID = "";

        public int Count
        {
            get
            {
                return moa_statBoxes.Count;
            }
        }


        public ARX_StatSingle CreateNewStat(bool bDeletable = false)
        {

            ARX_StatSingle buf = new ARX_StatSingle();
            buf.IsDeleteable = bDeletable;
            buf.me_statType = me_statType;
            buf.ID = mstr_defaultID;
            buf.mstr_attributeValue = mstr_defaultString;
            moa_statBoxes.Add(buf);
            return buf;
        }

        public ARX_StatSingle CreateNewStat(string nID, string strValue, bool bDeletable = false)
        {
            ARX_StatSingle buf = new ARX_StatSingle();
            buf.IsDeleteable = bDeletable;
            buf.ID = nID;
            buf.me_statType = STATTYPE.STRING;
            buf.StringValue = strValue;
            moa_statBoxes.Add(buf);
            return buf;
        }

        public ARX_StatSingle CreateNewStat(string nID, float nBase, float nBonus, float nCurrent, bool bDeletable = false)
        {
            ARX_StatSingle buf = new ARX_StatSingle();
            buf.IsDeleteable = bDeletable;
            buf.SetNew(nID, nBase, nBonus, nCurrent);
            moa_statBoxes.Add(buf);
            return buf;
        }

        public ARX_StatSingle CreateNewStat(string nID, float nBase, bool bDeletable = false)
        {
            ARX_StatSingle quad = CreateNewStat(nID, nBase, 0, nBase);
            quad.IsDeleteable = bDeletable;
            return quad;
        }


        public bool IsStatCodeInUse(string nCode)
        {
            if (GetStat(nCode) == null)
                return false;

            return true;
        }

        public ARX_StatSingle GetStat(string nCode, STATTYPE eType = STATTYPE.FLOAT)
        {
            for (int i = 0; i < moa_statBoxes.Count(); i++)
            {
                if (moa_statBoxes[i].ID == nCode)
                {
                    moa_statBoxes[i].Reevaluate();
                    return moa_statBoxes[i];
                }
            }

            return CreateNewStat(nCode, 0);
        }

        public bool DeleteStat(ARX_StatSingle oStat)
        {
            moa_statBoxes.Remove(oStat);
            return true;
        }

        public bool DeleteStat(string nCode)
        {
            ARX_StatSingle stat = GetStat(nCode);

            if (stat != null)
            {
                return DeleteStat(stat);
            }
            return false;
        }

    }

}