using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX;

namespace FEN
{
    /// <summary>
    /// Serializable for of a dictionary of Abilities arranged by int.
    /// </summary>
    [System.Serializable]
    public class FEN_AbilityIndexList : SerializableDictionaryClass<int, FEN_Ability>
    {

    }
}
