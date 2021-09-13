using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;

namespace FEN
{
    /// <summary>
    /// Calculates the strings of auto-generated files.
    /// </summary>
    public static class AutoCodeCalculations
    {

        public static string GetAttributeListFIleOutput(List<string> Attributes)
        {
            if (Attributes.Count == 0)
                return "No Attributes Defined";

            string strReturn = @"//Auto-Generated by ARX_AttributeList
#region Attributes
public string ";


            for (int i = 0; i < Attributes.Count; i++)
            {
                string str = Attributes[i];
                string strAttributeName = str.TrimWhitespace();
                string strValue = str.ToLower();
                
                strReturn += strAttributeName + "=\"" + strValue + "\"";

                if (i + 1 == Attributes.Count)
                {
                    strReturn += ";";
                }
                else
                {
                    strReturn += ", ";
                }
            }

            strReturn += "\n#endregion\n";
            return strReturn;

        }

        public static string GetAbilityCases()
        {
            List<FEN_Ability> oaAllAbilitiesInEditor = EditorHelper.FindAssetsByType<FEN_Ability>();

            string strAbilityIDType = ToolBox.GetQualifiedType(typeof(CardIDs.CARDID));
            string str = "";

            foreach (FEN_Ability ab in oaAllAbilitiesInEditor)
            {
                str += "case " + strAbilityIDType + "." + ab.me_abilityID.ToString() + ":\n" +
                    "ability.mo_abilityProcess = new FEN_AbilityProcess_" +
                    ab.me_abilityID.ToString().ToLower().FirstCharToUpper() + "(ability, owner, " + strAbilityIDType + "." + ab.me_abilityID.ToString() + ");\nbreak;\n";
            }

            str += "default:\nreturn null;\n";
            return str;
        }

        public static string GetLoadFenAbilityProcessOutput()
        {
            string strUsing =
                @"using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;
using FEN;";

            string strAbilityIDType = ToolBox.GetQualifiedType(typeof(CardIDs.CARDID));

            string strFunc =
                @"
namespace FEN{

public class LoadAbilityAI
{
    public static FEN_AbilityProcess LoadAI(FEN_Ability ability, FEN_Unit owner, " + strAbilityIDType + @" eID)
    {
        switch(eID)
        {" +
            GetAbilityCases() +
            "}\n}";

            return strUsing + "\n\nAuto-Generated by FEN Ability Asset Forge\n\n" + strFunc + "\n}\n}\n";
        }
    }
}
