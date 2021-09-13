using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;

/// <summary>
/// Simple process that exists only to pause the main RPG Battle Process for a time, 
/// usually until an attack animation ends.
/// </summary>
public class FEN_PauseRPGStage : ARX_Process
{
    public FEN_PauseRPGStage(string strName):base(strName)
    {

    }
}
