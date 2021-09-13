using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FEN;
using ARX;

namespace FEN
{
    public class FEN_Project : ScriptableObject
    {
        [SerializeField]
        public List<string> mastr_abilityEnums = new List<string>();

        [SerializeField]
        public List<string> mastr_unitEnums = new List<string>();
        
        [SerializeField]
        public List<string> mastr_attributes = new List<string>();

        public void Validate(List<string> astrList, List<string> aMyList)
        {
            //If the given object's list contains a string that this object does not, fire a warning message
            foreach (string str in astrList)
            {
                if (aMyList.Contains(str) == false)
                {
                    Debug.LogError("Attribute list " + name + " does not contain the string " + str);
                }
            }
        }

        public List<string> Alphebetize(List<string> oaInput)
        {
            Sort_Bubble_Alphebetize sorter = new Sort_Bubble_Alphebetize();
            List<string> oaBuf = sorter.Sort(mastr_attributes);
            return oaBuf;
        }


        public const string AbilityFolderPath = "Assets/FEN/Resources/ability";
        public const string UnitFolderPath = "Assets/FEN/Resources/unit";

    }
}
