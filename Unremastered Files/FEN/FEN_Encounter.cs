using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FEN;
using ARX;
using UnityEngine.Serialization;
using Fungus;

[CreateAssetMenu(menuName ="FEN/Encounter")]
public class FEN_Encounter : ScriptableObject
{

    #region Variables
    /// <summary>
    /// Developer notes regarding the encounter.
    /// </summary>
    [TextArea(10, 10)]
    public string mstr_notes;

    public GameObject mo_preBattleFlowchart, mo_postBattleFlowchart;

    public Flowchart PreBattleFlowchart
    {
        get
        {
            if (mo_preBattleFlowchart == null)
                return null;
            return mo_preBattleFlowchart.GetComponent<Flowchart>();
        }
    }

    public Flowchart PostBattleFlowchart
    {
        get
        {
            if (mo_postBattleFlowchart == null)
                return null;
            return mo_postBattleFlowchart.GetComponent<Flowchart>();
        }
    }


    [SerializeField]
    [FormerlySerializedAs("moa_layoutOne")]
    public FEN_Unit[] moa_enemyLayout;
    public FEN_Unit[] LayoutEnemies
    {
        get
        {
            if (moa_enemyLayout == null)
                moa_enemyLayout = new FEN_Unit[0];
            return moa_enemyLayout;
        }
    }
    
    
    EventSubscriptionRecord mo_eventRecord;
    protected EventSubscriptionRecord EventRecord
    {
        get
        {
            if (mo_eventRecord == null)
                mo_eventRecord = new EventSubscriptionRecord();
            return mo_eventRecord;
        }
    }

    /// <summary>
    /// Auto generated file for loading encounters
    /// </summary>
    [TextArea(5,10)]
    public string mstr_fileOuput = "";


    #endregion
    
    

    public override string ToString()
    {
        string str = "Encounter " + name + ": ";
        foreach (FEN_Unit enemy in LayoutEnemies)
        {
            str += enemy.name;
        }
        return str;
    }
    


   

    private void OnDestroy()
    {
        Destroy();

    }
    
    
    
    public void Destroy()
    {
        EventRecord.UnsubscribeFromAllEvents();
    }
    
    
  
}


