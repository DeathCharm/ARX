using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;


[CreateAssetMenu(menuName = "ARX/Message")]
public class ARX_DataStringAsset : ScriptableObject {

    public DataString mo_dataString = new DataString(null);

    public List<DataPair> DataPairs { get { return DataPair.SplitDataStringIntoPairs(mo_dataString); } }
}
