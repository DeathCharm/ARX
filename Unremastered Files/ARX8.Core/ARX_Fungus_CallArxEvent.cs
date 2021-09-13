using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using ARX;

[CommandInfo("ARX", "Call Event", "Calls an Arx Event")]
[AddComponentMenu("")]
public class ARX_Fungus_CallArxEvent : Command
{
    public ARX_Event Event;

    public string key1, value1, key2, value2, key3, value3, key4, value4;

    public override void OnEnter()
    {
        if (Event != null)
        {
            DataString dat = new DataString(this);

            List<DataPair> oList = new List<DataPair>();
            oList.Add(new DataPair(key1, value1));
            oList.Add(new DataPair(key2, value2));
            oList.Add(new DataPair(key3, value3));
            oList.Add(new DataPair(key4, value4));

            dat.SetData(oList);
            Event.FireEvent(dat);
        }

        Continue();
    }

    public override string GetSummary()
    {
        if (Event != null)
            return "Call " + Event.name;
        return "No event chosen";
    }
}

