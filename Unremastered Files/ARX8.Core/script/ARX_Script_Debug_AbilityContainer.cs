using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;

/// <summary>
/// Script to create an ARX_AbilityContainer in the Hierarchy and to
/// alter its values in realtime.
/// Used for debugging purposes.
/// </summary>
public class ARX_Script_Debug_AbilityContainer : MonoBehaviour{
    [SerializeField]
    List<ARX_Script_Debug_Ability> moa_abilities = new List<ARX_Script_Debug_Ability>();
    public string mstr_name = "Unnamed Container";

    public void AddAbility(ARX_Script_Debug_Ability oAbility) { moa_abilities.Add(oAbility); }
    public void AddAbility()
    {
        AddAbility(gameObject.AddComponent<ARX_Script_Debug_Ability>());
    }

    public ARX_Script_Debug_Ability GetAbilityBySlot(int nSlot)
    {
        if (nSlot < 0 || nSlot >= moa_abilities.Count)
            return null;
        return moa_abilities[nSlot];
    }

    public List<ARX_Script_Debug_Ability> AbilityList { get {


            moa_abilities = new List<ARX_Script_Debug_Ability>( GetComponents<ARX_Script_Debug_Ability>());
            return moa_abilities; } }

    ARX_Script_Actor mo_owner;
    
    public string OwnerName
    {
        get
        {
            if (mo_owner == null)
                mo_owner = GetComponent<ARX_Script_Actor>();
            if (mo_owner != null)
                return mo_owner.name;
            else
                return "No owner";
        }
    }
    
}
