using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX;

namespace FEN
{
    /// <summary>
    /// Serializable form of a Dictionary of Abilities arranged by Ability IDs.
    /// </summary>
    [System.Serializable]
    public class FEN_AbilityIndex : SerializableDictionaryClass<CardIDs.CARDID, FEN_Ability>
    {

    }
    
}
