using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FEN;
using ARX;


namespace FEN
{
    public static class Party
    {
        public enum ALIGNMENT {ALLIED, ENEMY }

        private static FEN_UnitContainer _alliedParty;
        public static FEN_UnitContainer AlliedParty
        {
            get
            {
                if (_alliedParty == null)
                    _alliedParty = ScriptableObject.CreateInstance<FEN_UnitContainer>();
                return _alliedParty;
            }
        }

        private static FEN_UnitContainer _enemyParty;
        public static FEN_UnitContainer EnemyParty
        {
            get
            {
                if (_enemyParty == null)
                    _enemyParty = ScriptableObject.CreateInstance<FEN_UnitContainer>();
                return _enemyParty;
            }
        }

        public static FEN_UnitContainer GetParty(ALIGNMENT eAlignment)
        {
            switch(eAlignment)
            {
                case ALIGNMENT.ALLIED:
                    return AlliedParty;
                case ALIGNMENT.ENEMY:
                    return EnemyParty;
                default:
                    return null;
            }
        }

        public static void InitializeParty(FEN_Encounter encounter, ALIGNMENT eAlignment)
        {
            if(encounter == null)
            {
                Debug.LogError("The given encounter was null.");
                return;
            }
            FEN_UnitContainer container = GetParty(eAlignment);
            container.LoadEncounterUnits(encounter);
        }
        
        public static void ClearParty(ALIGNMENT eAlignment)
        {
            FEN_UnitContainer container = GetParty(eAlignment);
            for (int i = 0; i < EnemyParty.Count; i++)
            {
                EnemyParty.mo_units[i].Destroy();
            }
            EnemyParty.Clear();
        }
        
        public static void RemoveEnemy(FEN_Unit enemy, ALIGNMENT eAlignment)
        {
            FEN_UnitContainer container = GetParty(eAlignment);
            container.RemoveUnit(enemy);
        }

        public static FEN_Unit AddUnit(string strEnemyFileName, ALIGNMENT eAlignment)
        {
            FEN_Unit enemy;
            enemy = FEN.Loading.LoadUnit(strEnemyFileName);
            return AddUnit(enemy, eAlignment);
        }

        public static FEN_Unit AddUnit(FEN_Unit unit, ALIGNMENT eAlignment)
        {
            FEN_UnitContainer container = GetParty(eAlignment);

            container.AddUnit(unit);
            Debug.Log("Added " + eAlignment.ToString() + " unit " + unit.name);
            return unit;
        }
        
        public static List<FEN_Unit> GetLivingUnits(ALIGNMENT eAlignment)
        {
            FEN_UnitContainer container = GetParty(eAlignment);

            List<FEN_Unit> obuf = new List<FEN_Unit>();
                foreach (FEN_Unit u in container.Units)
                    if (u.IsDead == false)
                        obuf.Add(u);
                return obuf;
            
        }

        public static List<FEN_Unit> GetUnitsByAttribute(string strAttribute, ALIGNMENT eAlignment)
        {
            FEN_UnitContainer container = GetParty(eAlignment);

            List<FEN_Unit> buf = new List<FEN_Unit>();
            foreach (FEN_Unit unit in container.mo_units)
                if (unit.HasAttribute(strAttribute))
                    buf.Add(unit);
            return buf;
        }

    }
}