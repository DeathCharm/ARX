using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FEN
{
    public class FEN_UnitContainer: ScriptableObject
    {
        public List<FEN_Unit> mo_units = null;
        public List<FEN_Unit> Units
        {
            get
            { if (mo_units == null)
                    mo_units = new List<FEN_Unit>();
                return mo_units;
            }
        }

        public int Count { get { return Units.Count; } }

        /// <summary>
        /// Adds an already instantiated unit to thie container's list.
        /// To load a unit first, use LoadUnit
        /// </summary>
        /// <param name="unit"></param>
        public void AddUnit(FEN_Unit unit)
        {
            Units.Add(unit);
        }

        public void Clear()
        {
            foreach(FEN_Unit unit in Units)
            {
                unit.Destroy();
            }
            Units.Clear();
        }

        void RemoveUnit(int nIndex)
        {
            if (nIndex < 0 || nIndex >= Units.Count)
                return;

            if (mo_units == null)
                return;
            mo_units.Remove(mo_units[nIndex]);
        }

        public void RemoveUnit(FEN_Unit enemy)
        {
            for (int i = 0; i < Units.Count; i++)
            {
                if (Units[i] == enemy)
                {
                    RemoveUnit(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Loads copies of all of the LayoutEnemies FEN_Units, then saves them to this
        /// container.
        /// </summary>
        /// <param name="encounter"></param>
        public void LoadEncounterUnits(FEN_Encounter encounter)
        {
            foreach(FEN_Unit unit in encounter.LayoutEnemies)
            {
                LoadUnit(unit);
            }
        }

        /// <summary>
        /// Loads a copy of the given unit from file, then adds it to this container.
        /// </summary>
        /// <param name="unit"></param>
        public void LoadUnit(FEN_Unit unit)
        {
            FEN_Unit clone = FEN.Loading.LoadUnit(unit.name);
            AddUnit(clone);
        }

    }
}
