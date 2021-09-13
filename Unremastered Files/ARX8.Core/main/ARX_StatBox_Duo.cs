using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARX;
using UnityEngine;


namespace ARX
{
    /// <summary>
    /// A Stat box for StatDuos
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "ARX/Stat Box - Duo")]
    public class ARX_StatDuoBox : ScriptableObject
    {
        StatDuo[] mo_statBoxes;
        StatDuo[] StatBoxes
        {
            get
            {
                if (mo_statBoxes == null)
                    mo_statBoxes = new StatDuo[0];
                return mo_statBoxes;
            }
            set
            {
                mo_statBoxes = value;
            }
        }

        public List<StatDuo> AsList { get { return new List<StatDuo>(mo_statBoxes); } }

        public void CreateNewBox(int new_boxsize)
        {
            StatBoxes = new StatDuo[new_boxsize];

            for (int i = 0; i < StatBoxes.Length; i++)
                StatBoxes[i] = new StatDuo();
        }
        
        void ResizeBox()
        {
            if (StatBoxes.Length == 0)
            {
                StatBoxes = new StatDuo[1];
                StatBoxes[0] = new StatDuo();
                return;
            }

            StatDuo[] buf = new StatDuo[StatBoxes.Length];

            for (int i = 0; i < StatBoxes.Length; i++)
            {
                buf[i] = new StatDuo();
                buf[i].ID = StatBoxes[i].ID;
                buf[i].Current = StatBoxes[i].Current;
            }

            StatBoxes = new StatDuo[StatBoxes.Length + 1];
            for (int i = 0; i < StatBoxes.Length - 1; i++)
            {
                StatBoxes[i] = new StatDuo();
                StatBoxes[i].ID = buf[i].ID;
                StatBoxes[i].Current = buf[i].Current;
            }

            StatBoxes[StatBoxes.Length - 1] = new StatDuo();
        }

        public bool CreateNewStat(string nID, float nValue)
        {
            ploop:
            StatDuo buf = GetOpenStat();

            if (buf == null)
            {
                ResizeBox();
                goto ploop;
            }

            buf.SetNew(nID, nValue);
            return true;
        }

        StatDuo GetOpenStat()
        {
            for (int i = 0; i < StatBoxes.Count(); i++)
            {
                if (StatBoxes[i].ID == "")
                    return StatBoxes[i];
            }
            return null;
        }

        public bool IsStatCodeInUse(string nCode)
        {
            if (GetStat(nCode) == null)
                return false;

            return true;
        }

        public StatDuo GetStat(string nCode)
        {
            for (int i = 0; i < StatBoxes.Count(); i++)
            {
                if (StatBoxes[i].ID == nCode)
                    return StatBoxes[i];
            }
            return null;
        }

        bool DeleteStat(string nCode)
        {
            StatDuo buf = GetStat(nCode);

            if (buf != null)
            {
                buf.DeleteStat();
                return true;
            }
            return false;
        }

        void DeleteAllStats()
        {
            for (int i = 0; i < StatBoxes.Count(); i++)
            {
                StatBoxes[i].DeleteStat();
            }

        }
    }

}