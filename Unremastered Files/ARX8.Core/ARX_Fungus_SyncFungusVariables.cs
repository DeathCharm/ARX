using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;
using Fungus;

[CommandInfo("ARX", "Sync Fungus Variables", "Calls the Arx Event that requests syncing Fungus variables")]
[AddComponentMenu("")]
public class ARX_Fungus_SyncFungusVariables : Command
{

    public override void OnEnter()
    {
        //if (ARX.GameEvents.onSyncFungusVariables != null)
        //    ARX.GameEvents.onSyncFungusVariables.FireEvent();

        Continue();
    }
}