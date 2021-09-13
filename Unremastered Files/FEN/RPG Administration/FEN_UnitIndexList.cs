using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX;
using FEN;

namespace FEN
{
    /// <summary>
    /// Serializable form of a dictionary of Abilities arranged by int.
    /// </summary>
    [System.Serializable]
    public class FEN_UnitIndexList : SerializableDictionaryClass<int, FEN_Unit>
    {

    }
}