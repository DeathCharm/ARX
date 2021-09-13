using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ARX;

namespace ARX
{
    [CreateAssetMenu(menuName = "ARX/Variable Box")]
    public class ARX_QuestVariableBox : ScriptableObject
    {


        [SerializeField]
        ARX_VariableDictionary moa_variables;
        public ARX_VariableDictionary Variables
        {
            get
            {
                if (moa_variables == null)
                    moa_variables = new ARX_VariableDictionary();
                return moa_variables;
            }
        }
    }
}
