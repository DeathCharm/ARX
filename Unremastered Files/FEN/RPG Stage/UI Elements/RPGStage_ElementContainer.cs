using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FEN;
using ARX;


/// <summary>
/// The Root Element containing references to the RPG Elements shown by this object's
/// child RPG UIElements
/// </summary>
public class RPGStage_ElementContainer : MonoBehaviour
{
    public string mstr_statID = "";

    /// <summary>
    /// Determines if this scroll view draws the unit held by its Root Element or the
    /// Global Unit
    /// </summary>
    public enum UNITTARGET { SELECTED, GLOBAL }

    public UNITTARGET me_unitTarget = UNITTARGET.SELECTED;

    #region RPG Element References
    [SerializeField]
    public FEN_Ability mo_ability = null;
    public FEN_Ability Ability
    {
        get
        {
            if (mo_ability == null)
                mo_ability = ScriptableObject.CreateInstance<FEN_Ability>();
            return mo_ability;
        }
    }

    [SerializeField]
    public FEN_AbilityList mo_abilityContainer = null;
    public FEN_AbilityList AbilityContainer
    {
        get
        {
            if (mo_abilityContainer == null)
                mo_abilityContainer = ScriptableObject.CreateInstance<FEN_AbilityList>();
            return mo_abilityContainer;
        }
    }

    [SerializeField]
    public FEN_Unit mo_unit = null;
    public FEN_Unit Unit
    {
        get
        {
            if (mo_unit == null)
            {
                switch (me_unitTarget)
                {
                    case UNITTARGET.SELECTED:
                        mo_unit = ScriptableObject.CreateInstance<FEN_Unit>();
                        break;
                    case UNITTARGET.GLOBAL:
                        mo_unit = FEN.RPGElementCollections._GlobalUnit;
                        break;
                    default:
                        mo_unit = ScriptableObject.CreateInstance<FEN_Unit>();
                        break;
                }
            }
            return mo_unit;
        }
    }

    [SerializeField]
    public FEN_UnitContainer mo_unitContainer = null;
    public FEN_UnitContainer UnitContainer
    {
        get
        {
            if (mo_unitContainer == null)
                mo_unitContainer = ScriptableObject.CreateInstance<FEN_UnitContainer>();
            return mo_unitContainer;
        }
    }

    public ARX_StatQuad mo_stat = null;
    public ARX_StatQuad Stat
    {
        get
        {
            if(mo_stat == null)
            {
                if (Ability != null)
                {
                    mo_stat = Ability.Stats.GetStat(mstr_statID);
                }
                else if (Unit != null)
                {
                    mo_stat = Unit.Stats.GetStat(mstr_statID);
                }
                else
                {
                    mo_stat = new ARX_StatQuad();
                }
            }
            return mo_stat;
        }

        set
        {
            mo_stat = value;
            mstr_statID = value.ID;
        }
    }

    #endregion

    
}
