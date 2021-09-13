using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ARX;



/// <summary>
/// Currently not implemented class containing commented out methods for
/// using the Fungus API to save and load profiles.
/// Awaiting rework into a more generalized, inclusive form.
/// </summary>
public class FEN_SavedProfileLoading : MonoBehaviour
{
    //[Serializable]
    //public class FEN_SaveProfile
    //{
    //    public FEN_SaveProfile()
    //    {
    //        playerName = "Rebecca";
    //        date = System.DateTime.Now.ToShortDateString();
    //        missionsComplete = "0";
    //        percentageDone = "0";
    //        rank = "Asleep";
    //        exp = "0";
    //        mstr_lastCompletedMission = "nomission";
    //    }


    //    public string playerName, date, missionsComplete, percentageDone, rank, exp;
    //    public string mstr_lastCompletedMission;
    //    public bool mb_savedByPlayer;
    //    public string mo_missionProgress;
    //    public string mstr_fungusVariables;

    //    public static void RecordProfile(FEN_SaveProfile profile, bool bPlayerSave)
    //    {
    //        int nMissionsCompleted = GetMissionsCompleted();
    //        profile.missionsComplete = nMissionsCompleted.ToString();
    //        profile.date = System.DateTime.Now.ToShortDateString();
    //        profile.mb_savedByPlayer = bPlayerSave;
    //        profile.mo_missionProgress = GameStats.IntegerVariables.SaveToString();
    //        profile.mstr_fungusVariables = Fungus.FungusManager.Instance.SaveManager.GetSaveHistory();
    //        Debug.Log("Fungus save = " + profile.mstr_fungusVariables);
    //    }

    //    public ARX_VariableDictionary GetMissionProgress()
    //    {
    //        return ARX_VariableDictionary.Deserialize(mo_missionProgress);
    //    }

    //    static int GetMissionsCompleted()
    //    {
    //        int n = 0;
    //        foreach (FEN_Mission mission in FEN_Mission.MissionIndex.Values)
    //            if (mission.isCompleted)
    //                n++;
    //        return n;
    //    }

    //}

    //protected virtual void LoadSavedGame(FEN.FEN_SaveProfile profile)
    //{
    //    Debug.Log("Loading Fungus Variables from FEN Profile " + profile.mstr_fungusVariables);
    //    var historyData = profile.mstr_fungusVariables;

    //    if (!string.IsNullOrEmpty(historyData))
    //    {
    //        var tempSaveHistory = JsonUtility.FromJson<SaveHistory>(historyData);
    //        if (tempSaveHistory != null)
    //        {
    //            saveHistory = tempSaveHistory;
    //            saveHistory.ClearRewoundSavePoints();
    //            saveHistory.LoadLatestSavePoint();
    //        }
    //    }
    //}

    //public void LoadFromFENProfile(FEN.FEN_SaveProfile profile)
    //{
    //    // Set a load action to be executed on next update
    //    loadAction = () => LoadSavedGame(profile);
    //}
}
