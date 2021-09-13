using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves the position of an object on Start and 
/// Tweens to it when activated.
/// </summary>
public class ARX_Script_SavedPositions : MonoBehaviour
{
    /// <summary>
    /// Tweens an object to a given position.
    /// </summary>
    [System.Serializable]
    public class SavedPosition
    {
        public Vector3 targetPosition, eulerAngles, targetScale;
        public string name;

        public SavedPosition(Vector3 vecPosition, string strName)
        {
            targetPosition = vecPosition;
            name = strName;
        }

        public SavedPosition()
        {

        }

        public void SetPosition(Vector3 position) {
            targetPosition = position;
        }
        public void SetRotation(Vector3 euler) {
            eulerAngles = euler;
        }
        public void SetScale(Vector3 scale)
        {
            targetScale = scale;
        }

        public void TweenTo(GameObject obj, float time = 1.0F, float delay = 0.0F)
        {
            iTween.MoveTo(obj, targetPosition, time, delay);
        }
    }

    [HideInInspector]
    public List<SavedPosition> moa_savedPositions = new List<SavedPosition>();

    public bool HasPosition(string gstr)
    {
        foreach (SavedPosition pos in moa_savedPositions)
            if (pos.name == gstr)
                return true;
        return false;
    }

    private void Start()
    {
        SaveNew("_origin");
    }

    public void RevertToOrigin()
    {
        gameObject.transform.position = GetVectorPosition("_origin");

    }

    public SavedPosition GetSavedPosition(string strName, bool bThrowWarning = true)
    {
        if (strName == "")
        {
            return null;
        }

        foreach (SavedPosition pos in moa_savedPositions)
            if (pos.name == strName)
                return pos;
        if(bThrowWarning)
        Debug.LogError("No position by the name of " + strName + " was found.");
        return null;
    }

    public void ScaleTo(SavedPosition pos)
    {
        ScaleTo(pos.targetScale);
    }

    public void ScaleTo(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void RotateTo(SavedPosition pos)
    {
        RotateTo(pos.eulerAngles);
    }

    public void RotateTo(Vector3 euler)
    {
        transform.eulerAngles = euler;
    }

    public void MoveToPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void MoveToPosition(SavedPosition pos)
    {
        transform.position = pos.targetPosition;
    }

    public void MoveToPosition(string str)
    {
        SavedPosition pos = GetSavedPosition(str);
        if (pos != null)
            transform.position = pos.targetPosition;
    }

    public void TweenToPosition(string strPos)
    {
        SavedPosition pos = GetSavedPosition(strPos);
        if (pos != null)
            pos.TweenTo(gameObject);
    }

    public Vector3 GetVectorPosition(string strName, bool bThrowWarning = true)
    {
        SavedPosition pos = GetSavedPosition(strName);
        if (pos != null)
            return pos.targetPosition;

        if(bThrowWarning)
        Debug.LogError("No position by the name of " + strName + " was found.");
        return Vector3.zero;
    }

    public void SaveCurrentPosition(string strName = "")
    {
        SavedPosition pos = GetSavedPosition(strName, false);

        if (pos != null)
        {
            pos.targetPosition = transform.position;
        }
        else
            SaveNew(strName);

    }

    void SaveNew(string strName)
    {
        ARX_Script_SavedPositions.SavedPosition newPosition = new ARX_Script_SavedPositions.SavedPosition();
        newPosition.targetPosition = transform.position;
        newPosition.name = strName;
        moa_savedPositions.Add(newPosition);
    }
}
