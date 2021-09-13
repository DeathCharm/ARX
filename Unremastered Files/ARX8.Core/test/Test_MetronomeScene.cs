using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;

public class Test_MetronomeScene : MonoBehaviour
{
    ARX_Process_Metronome metronome;

    // Start is called before the first frame update
    void Start()
    {
        metronome = (ARX_Process_Metronome)Main.MainProcess.PushToTopProcess(new ARX_Process_Metronome());
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Main.MainProcess.PushToTopProcess(new ARX_WaitProcess(1.5F, true));
    }
}
