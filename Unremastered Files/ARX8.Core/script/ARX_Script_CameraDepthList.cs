using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shows a list of all Cameras in the current scene and their allows quick altering of their depths.
/// </summary>
public class ARX_Script_CameraDepthList : MonoBehaviour
{
    /// <summary>
    /// Camera sorter class. Sorts cameras by their depths.
    /// </summary>
    class CameraBubbleSort : ARX.ARX_Algorithm_BubbleSort<Camera>
    {
        public override bool I_IsHigherPriority(Camera movingItem, Camera listedItem)
        {
            return movingItem.depth > listedItem.depth;
        }
    }

    /// <summary>
    /// Returns a sorted list of all cameras in the current scene.
    /// </summary>
    /// <returns></returns>
    public List<Camera> GetSortedCameraList()
    {
        List<Camera> oCameras = new List<Camera>(Camera.FindObjectsOfType<Camera>());

        CameraBubbleSort oSorter = new CameraBubbleSort();
        oSorter.Sort(oCameras);
        oCameras = oSorter.Sort(oCameras);

        return oCameras;
    }
    
}
