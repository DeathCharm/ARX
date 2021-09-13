using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A scriptable object that stores information about Licensed items
/// included in the project.
/// </summary>
[CreateAssetMenu(menuName = "ARX/License List")]
public class ARX_LicenseListing : ScriptableObject
{
    [System.Serializable]
    public class License
    {
        public string mstr_name, mstr_type, mstr_notes;
    }

    [SerializeField]
    public List<License> moa_licenses;

    public List<License> Licenses
    {
        get
        {
            if (moa_licenses == null)
                moa_licenses = new List<License>();

            return moa_licenses;
        }
    }


}
