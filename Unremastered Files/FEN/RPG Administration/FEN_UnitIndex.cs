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
    /// Serializable form of a Dictionary of units arranged by Unit ID's
    /// </summary>
    [System.Serializable]
    public class FEN_UnitIndex : SerializableDictionaryClass<CardIDs.UNITID, FEN_Unit>
    {

    }
    
}
