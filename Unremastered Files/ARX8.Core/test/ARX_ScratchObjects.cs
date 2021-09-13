using FEN;
using ARX;
using UnityEngine;

//Auto-generated Ability Process Definition by FenAssetForge
namespace FEN
{

    public class FEN_AbilityProcess_Anything : FEN_AbilityProcess
    {

        public FEN_AbilityProcess_Anything(FEN_Ability ability, FEN_Unit owner, CardIDs.CARDID eID) : base(ability, owner, CardIDs.CARDID.ANYTHING)
        {

        }

        public override void OnInitialized()
        {

        }


        #region Accessors
        public int GetBaseHpInt
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("hp").BaseInt;
            }

        }
        public int GetCurrentHpInt
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("hp").CurrentInt;
            }

        }
        public int GetMaxHpInt
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("hp").MaxInt;
            }

        }
        public float GetBaseHp
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("hp").Base;
            }

        }
        public float GetCurrentHp
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("hp").Current;
            }

        }
        public float GetMaxHp
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("hp").Max;
            }

        }
        public int GetBaseDefInt
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("def").BaseInt;
            }

        }
        public int GetCurrentDefInt
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("def").CurrentInt;
            }

        }
        public int GetMaxDefInt
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("def").MaxInt;
            }

        }
        public float GetBaseDef
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("def").Base;
            }

        }
        public float GetCurrentDef
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("def").Current;
            }

        }
        public float GetMaxDef
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("def").Max;
            }

        }
        public int GetBaseStrInt
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("str").BaseInt;
            }

        }
        public int GetCurrentStrInt
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("str").CurrentInt;
            }

        }
        public int GetMaxStrInt
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("str").MaxInt;
            }

        }
        public float GetBaseStr
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("str").Base;
            }

        }
        public float GetCurrentStr
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("str").Current;
            }

        }
        public float GetMaxStr
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("str").Max;
            }

        }
        public int GetBaseButtsInt
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("butts").BaseInt;
            }

        }
        public int GetCurrentButtsInt
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("butts").CurrentInt;
            }

        }
        public int GetMaxButtsInt
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("butts").MaxInt;
            }

        }
        public float GetBaseButts
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("butts").Base;
            }

        }
        public float GetCurrentButts
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("butts").Current;
            }

        }
        public float GetMaxButts
        {
            get
            {
                return mo_targetAbility.Stats.GetStat("butts").Max;
            }

        }
        #endregion

    }
}
