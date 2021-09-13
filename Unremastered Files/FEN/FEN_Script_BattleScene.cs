using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FEN;
using ARX;

public class FEN_Script_BattleScene : MonoBehaviour
{

    void Start()
    {
        //Creates a new Battle Process and loads the current encounter into it
        CombatEncounters.BeginBattleProcess();
    }
    
}
