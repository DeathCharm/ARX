using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ARX;
using UnityEngine.SceneManagement;

namespace ARX
{
    public static class Main
    {
        static ARX_Script_Main mo_mainScript = null;

        static ARX_Process mo_mainProcess = null;

        /// <summary>
        /// If the 
        /// </summary>
        public static void InitializeMain()
        {
            if (Main.MainScript == null)
            {

                SceneManager.sceneLoaded += Main.OnSceneLoaded;
                SceneManager.sceneUnloaded += Main.OnSceneUnloaded;

                mo_mainProcess = new ARX_Process("main");

                ARX.GameEvents.Initialize();

                GameObject obj = new GameObject();
                ARX_Script_Main script = obj.AddComponent<ARX_Script_Main>();
                mo_mainScript = script;
                script.Awake();
            }
        }

        public static void SetMainScript(ARX_Script_Main oScript)
        {
            mo_mainScript = oScript;
        }

        public static ARX_Script_Main MainScript
        {
            get
            {
                return mo_mainScript;
            }
        }

        public static ARX_Process MainProcess
        {
            get
            {
                if (mo_mainProcess == null)
                {
                    InitializeMain();
                }

                return mo_mainProcess;
            }

            set
            {
                mo_mainProcess = value;
            }
        }

        public static void Update() { MainProcess.Update(); }
        public static void FixedUpdate() { MainProcess.FixedUpdate(); }

        public static void RandomizeSeed()
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        }

        public static ARX_Process AddConstantProcess(ARX_Process oProcess)
        {
            MainProcess.AddConstantProcess(oProcess);
            return oProcess;
        }

        public static ARX_Process PushToTopQueuedProcess(ARX_Process oProcess)
        {
            MainProcess.PushToTopProcess(oProcess);
            return oProcess;
        }

        public static ARX_Process PushBottomProcess(ARX_Process oProcess)
        {
            MainProcess.PushToBottomProcess(oProcess);
            return oProcess;
        }

        public static void OnSceneLoaded(Scene scene, LoadSceneMode eMode)
        {
            Debug.Log("Main process reacting to on scene loaded.");
        }

        public static void OnSceneUnloaded(Scene scene)
        {
            Debug.Log("Main process reacting to on scene unloaded.");
        }

    }

}


[ExecuteInEditMode]
public class ARX_Script_Main : MonoBehaviour
{
    public bool mb_setRandomSeed = true;
    
    public void Awake()
    {
        if(Application.isPlaying == false)
        {
            Debug.LogError("ARX_Script_Main is not to be added manually to any object.");
            DestroyImmediate(this);
            return;
        }

        name = "ARX_Main";
        //Quits application if it senses more than one ARX_Main script
        if (Main.MainScript != null && Main.MainScript != this)
        {
            Debug.LogError("Only one instance of ARX_Script_Main can exist.");
            Destroy(this);
            return;
        }

        gameObject.hideFlags = HideFlags.HideInHierarchy;
        gameObject.AddComponent<ARX_Script_DontDestroyOnLoad>();
    }

    private void Start()
    {
        if (Main.MainScript != this)
        {
            Destroy(gameObject);
        }

    }

    private void OnApplicationQuit()
    {

    }

    private void Update()
    {

        if (Application.isPlaying)
        {
            Main.Update();
        }
        else if(Application.isEditor)
        {
            Debug.LogError("ARX_Script_Main is not to be added manually to any object.");
            DestroyImmediate(this);
        }
    }

    private void FixedUpdate()
    {
        if (Application.isPlaying)
            Main.FixedUpdate();
    }


}

