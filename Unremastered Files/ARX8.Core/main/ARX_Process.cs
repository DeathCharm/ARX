using ARX;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Base class for ARX_Processes, objects that update once every frame, 
/// can contain and run subprocces and react to ARX_Events.
/// </summary>
public class ARX_Process
{
    #region Constructors and Destructors
    public ARX_Process(string strName)
    {
        name = strName;
        //Debug.Log("Created process " + name);
        AllProcesses.Add(UniqueID, this);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    ~ARX_Process()
    {
        AllProcesses.Remove(UniqueID);
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    #endregion

    #region Helper Functions

    /// <summary>
    /// Returns a string with the names of all Queued processes
    /// </summary>
    /// <returns></returns>
    public string GetNamesOfQueuedProcesses()
    {
        string strReturn = "";
        if (moa_queuedProcesses.Count > 0)
        {
            for (int i = 0; i < moa_queuedProcesses.Count; i++)
            {
                string str = i.ToString() + ". \t";

                str += moa_queuedProcesses[i].name + "\n\t";

                str += moa_queuedProcesses[i].GetNamesOfQueuedProcesses();

                strReturn += str + "\n";
            }

        }
        return strReturn;
    }

    /// <summary>
    /// Returns a string with the names of all Constant Processes
    /// </summary>
    /// <returns></returns>
    public string GetNamesOfConstantProcesses()
    {
        string buf = "";
        if (moa_constantProcesses.Count > 0)
        {
            for (int i = 0; i < moa_constantProcesses.Count; i++)
            {
                string str = i.ToString() + ". ";

                str += moa_constantProcesses[i].name;

                if (i % 2 == 1)
                    str += "\n";
                else
                    str += "\t";

                buf += str;

            }

        }
        else
            buf = "No Constant Processes";
        return buf;
    }

    public EventSubscriptionRecord EventRecord
    {
        get
        {
            if (mo_eventRecord == null)
                mo_eventRecord = new EventSubscriptionRecord();
            return mo_eventRecord;
        }
    }

    static int GetUniqueID
    {
        get
        {
            gnLastID++;
            return gnLastID;
        }
    }

    public void ChangePhase(int nPhase)
    {
        mn_phase = nPhase;
    }

    /// <summary>
    /// Virtual function determining if this process is capable
    /// of receiving messages.
    /// </summary>
    /// <returns></returns>
    public virtual bool IsAcceptingMessages()
    {
        //If this process is a constant type, it
        //will always be receiving messages
        if (me_processType == PROCESSTYPE.CONSTANT)
            return true;

        //If this process only received messages while top
        if (mb_onlyProcessMessagesWhenTop)
        {
            //If the parent is not null and this process is not the parent's Top Process
            //return false
            if (Parent != null && Parent.TopProcess != this)
            {
                //Debug.Log("This process has a parent but is not the top process. Returning false.");
                return false;
            }
            //Else, return true
            //Debug.Log("This process has a parent and is the top process. Returning true.");
            return true;
        }
        //Debug.Log("This process accepts messages at any time. Returning true.");
        return true;
    }

    public int FramesActive { get { return mn_framesSinceInitialization; } }

    public int RotationsActive { get { return mn_rotationSinceInitialization; } }

    /// <summary>
    /// This actor's unique ID. Use this variable to get this game actor from ARX.Global.GetActor(int nID)
    /// </summary>
    public int UniqueID
    {
        get
        {
            if (mn_uniqueID == 0)
                mn_uniqueID = GetUniqueID;
            return mn_uniqueID;
        }
    }

    public int ParentCount
    {
        get
        {
            int n = 0;
            ARX_Process p = Parent;

            while (p != null)
            {
                n++;
                p = p.Parent;
            }

            return n;
        }
    }

    public bool IsTerminated { get { return mb_terminated; } }

    public PROCESSTYPE ProcessType { get { return me_processType; } }

    public ARX_Process Parent { get { return mo_parent; } }

    public bool Paused
    {
        get
        {
            return mb_paused;
        }
        set
        {
            if (value == true)
                Debug.Log("Pausing " + name);
            else
                Debug.Log("Unpausing " + name);
            mb_paused = value;
        }
    }
    public List<ARX_Process> QueuedProcesses { get { return moa_queuedProcesses; } }

    #endregion

    #region Timer 
    UnityTimer mo_timer = null;
    bool mb_waiting = false;
    bool IsWaiting { get { return mb_waiting; } }

    public void BeginWait(float nSeconds)
    {
        mo_timer = new UnityTimer();
        mo_timer.Start(nSeconds);
        mb_waiting = true;
    }

    private void Wait()
    {
        if (mo_timer == null)
        {
            mb_waiting = false;
            return;
        }

        mo_timer.Tick();
        if (mo_timer.IsFinished)
        {
            EndWait();
        }
    }

    private void EndWait()
    {
        mb_waiting = false;
        mo_timer = null;
    }

    #endregion

    #region Global

    /// <summary>
    /// A list of all ARX_Processes. 
    /// </summary>
    private static Dictionary<int, ARX_Process> AllProcesses = new Dictionary<int, ARX_Process>();

    public static void AddToAllProcessList(ARX_Process process)
    {
        Main.InitializeMain();
        AllProcesses.Add(process.UniqueID, process);

    }

    public static ARX_Process GetProcessByUniqueID(int nUniqueID)
    {
        if (AllProcesses.ContainsKey(nUniqueID) == false)
            return null;
        return AllProcesses[nUniqueID];
    }

    public static ARX_Process GetProcessByUniqueID(DataString dat)
    {
        int nUniqueID = dat.GetInt(GameIDs.ValueUniqueID);
        return GetProcessByUniqueID(nUniqueID);
    }

    public static T GetProcessByUniqueID<T>(DataString dat) where T : ARX_Process
    {
        return (T)GetProcessByUniqueID(dat);
    }
    public static T GetProcessInMain<T>() where T : ARX_Process
    {
        return (T)Main.MainProcess.GetProcess<T>();
    }


    #endregion

    #region Variables

    EventSubscriptionRecord mo_eventRecord;

    public bool IsDirty = false, IsCleaning = false;

    static int gnLastID = 1;

    protected int mn_phase = 0;

    /// <summary>
    /// If true, this process will only receive messages
    /// while it is the top process
    /// </summary>
    public bool mb_onlyProcessMessagesWhenTop = false;

    /// <summary>
    /// This actor's unique ID. Use UniqueID to access it.
    /// </summary>
    int mn_uniqueID = 0;

    /// <summary>
    /// Records how long this process has been active in disk rotations.
    /// </summary>
    int mn_rotationSinceInitialization = 0;

    /// <summary>
    /// Records how long this process has been active in frames.
    /// </summary>
    int mn_framesSinceInitialization = 0;
    
    public enum PROCESSTYPE { CONSTANT, QUEUEUD };
    PROCESSTYPE me_processType = PROCESSTYPE.QUEUEUD;

    protected bool mb_terminated = false;
    
    ARX_Process mo_parent;

    public string name = "Unnamed Process";
    protected bool mb_paused = false, mb_initialized = false, mb_firstUpdate = true;
    /// <summary>
    /// List of processes where only the very top process is updated.
    /// </summary>
    List<ARX_Process> moa_queuedProcesses = new List<ARX_Process>();

    /// <summary>
    /// List of processes that are constantly updated every frame and disk rotation.
    /// </summary>
    List<ARX_Process> moa_constantProcesses = new List<ARX_Process>();

    #endregion

    #region Sub Process Functions

    public List<ARX_Process> ConstantProcesses { get { return moa_constantProcesses; } }

    public T FetchProcess<T>() where T : ARX_Process
    {
        foreach (ARX_Process process in moa_constantProcesses)
            if (process.GetType() == typeof(T))
            {
                //Debug.Log("GetType is returning process " + process.name + " of type " + process.GetType() + "  for type " + eProcessType);
                return (T)process;
            }
        foreach (ARX_Process process in moa_queuedProcesses)
            if (process.GetType() == typeof(T))
            {
                //Debug.Log("GetType is returning process " + process.name + " for type " + eProcessType);
                return (T)process;
            }

        foreach (ARX_Process process in moa_constantProcesses)
        {
            T oFechedProcess = process.FetchProcess<T>();
            if (oFechedProcess != null)
                return oFechedProcess;
        }
        foreach (ARX_Process process in moa_queuedProcesses)
        {
            T oFechedProcess = process.FetchProcess<T>();
            if (oFechedProcess != null)
                return oFechedProcess;
        }

        return null;
    }

    public T GetProcess<T>() where T : ARX_Process
    {
        foreach (ARX_Process process in moa_constantProcesses)
            if (process.GetType() == typeof(T))
            {
                //Debug.Log("GetType is returning process " + process.name + " of type " + process.GetType() + "  for type " + eProcessType);
                return (T)process;
            }
        foreach (ARX_Process process in moa_queuedProcesses)
            if (process.GetType() == typeof(T))
            {
                //Debug.Log("GetType is returning process " + process.name + " for type " + eProcessType);
                return (T)process;
            }
        return null;
    }

    public ARX_Process GetProcess(Type eProcessType)
    {
        foreach (ARX_Process process in moa_constantProcesses)
            if (process.GetType() == eProcessType)
            {
                //Debug.Log("GetType is returning process " + process.name + " of type " + process.GetType() + "  for type " + eProcessType);
                return process;
            }
        foreach (ARX_Process process in moa_queuedProcesses)
            if (process.GetType() == eProcessType)
            {
                //Debug.Log("GetType is returning process " + process.name + " for type " + eProcessType);
                return process;
            }
        return null;
    }

    public ARX_Process GetProcess(string strProcessName)
    {
        if (name == strProcessName)
            return this;

        foreach (ARX_Process process in moa_constantProcesses)
        {
            ARX_Process ret = process.GetProcess(strProcessName);
            if (ret != null)
                return ret;
        }
        foreach (ARX_Process process in moa_queuedProcesses)
        {
            ARX_Process ret = process.GetProcess(strProcessName);
            if (ret != null)
                return ret;
        }
        return null;
    }

    public ARX_Process[] GetProcesses(Type eProcessType)
    {
        List<ARX_Process> obuf = new List<ARX_Process>();
        List<Type> oeDerivedTypes = TypeEnumerator.GetDerivedTypes(eProcessType);

        foreach (ARX_Process process in moa_constantProcesses)
        {
            if (process.GetType() == eProcessType
                || oeDerivedTypes.Contains(process.GetType())
                )
            {
                //Debug.Log("GetType is returning process " + process.name + " of type " + process.GetType() + "  for type " + eProcessType);
                obuf.Add(process);
            }
            //else
            //Debug.Log("Found process of type " + process.GetType());
        }
        foreach (ARX_Process process in moa_queuedProcesses)
        {
            if (process.GetType() == eProcessType)
            {
                // Debug.Log("GetType is returning process " + process.name + " for type " + eProcessType);
                obuf.Add(process);
            }
            //else
            //Debug.Log("Found process of type " + process.GetType());
        }
        return obuf.ToArray();
    }

    /// <summary>
    /// Adds a constant process to this objects list of constant processes.
    /// If the given process is already in this object's constant processes,
    /// returns an error.
    /// </summary>
    /// <param name="oProcess"></param>
    public ARX_Process AddConstantProcess(ARX_Process oProcess)
    {
        IsDirty = true;
        if (oProcess == null)
        {
            //Debug.Log("Given process to " + name + " was null.");
            return null;
        }

        oProcess.mo_parent = this;
        oProcess.me_processType = PROCESSTYPE.CONSTANT;
        if (moa_constantProcesses.Contains(oProcess) == false)
        {
            moa_constantProcesses.Add(oProcess);
            oProcess.OnAdded();
            return oProcess;
        }
        else
        {
            Debug.LogError("Process " + name + " already contains constant process " + oProcess.name);
            return oProcess;
        }
    }
    
    /// <summary>
    /// The accessor for the top most process in the list of 
    /// queued processes. Setting to null will remove the top process
    /// from the list of queued processes.
    /// </summary>
    public ARX_Process TopProcess
    {
        get
        {
            if (moa_queuedProcesses.Count > 0)
                return moa_queuedProcesses[moa_queuedProcesses.Count - 1];
            else
                return null;
        }

        set
        {

            if (value == null)
                return;

            IsDirty = true;
            moa_queuedProcesses.Remove(value);
            moa_queuedProcesses.Add(value);

            value.mo_parent = this;
            //Debug.Log("Added " + value.name + " as Top Process of " + name);
        }
    }

    public ARX_Process BottomProcess
    {

        get
        {
            if (moa_queuedProcesses.Count > 0)
                return moa_queuedProcesses[0];
            else
                return null;
        }
        set
        {
            if (value == null)
                return;

            IsDirty = true;
            moa_queuedProcesses.Remove(value);
            moa_queuedProcesses.Insert(0, value);

            value.mo_parent = this;
        }
    }

    /// <summary>
    /// Returns true if this process has no parent 
    /// or it this process is its parent's top process
    /// </summary>
    public bool IsTopProcess
    {
        get
        {
            if (Parent == null)
                return true;
            return Parent.TopProcess == this;
        }
    }

    /// <summary>
    /// Adds the given process to the top of the process queue.
    /// If the process is already present, moves it to the top
    /// </summary>
    /// <param name="oProcess"></param>
    public ARX_Process PushToBottomProcess(ARX_Process oProcess)
    {
        IsDirty = true;
        //If given process is null or this process, return
        if (oProcess == null)
            return oProcess;

        if (oProcess == this)
        {
            Debug.LogError("Attempted to recursively itself to its own list of processes");
            return oProcess;
        }


        oProcess.mo_parent = this;

        //Set the type of the process
        oProcess.me_processType = PROCESSTYPE.QUEUEUD;

        bool bIsAlreadyTopProcess = (oProcess == TopProcess);
        bool bContains = moa_queuedProcesses.Contains(oProcess);

        BottomProcess = oProcess;

        //If the new process was previously the top process, but now
        //isn't
        if (bIsAlreadyTopProcess && TopProcess != oProcess)
            oProcess.OnLoseTopProcessFocus();

        TopProcess.OnGainTopProcessFocus();

        //If this process did not previously contain the new subprocess
        if (bContains == false)
            oProcess.OnAdded();
        return oProcess;
    }

    /// <summary>
    /// Removes the given process from the list of queued processes.
    /// Runs the Deactivate or activate Top Process as needed.
    /// </summary>
    /// <param name="oProcess"></param>
    public void RemoveQueuedProcess(ARX_Process oProcess)
    {
        IsDirty = true;
        if (oProcess == null || moa_queuedProcesses.Contains(oProcess) == false)
            return;

        bool bWasTopProcess = false;

        if (oProcess == TopProcess)
        {
            TopProcess.OnLoseTopProcessFocus();
            bWasTopProcess = true;
        }

        moa_queuedProcesses.Remove(oProcess);
        moa_constantProcesses.Remove(oProcess);

        if (bWasTopProcess && TopProcess != null)
            TopProcess.OnGainTopProcessFocus();

    }

    /// <summary>
    /// Removes the given process from the list of constant processes.
    /// </summary>
    /// <param name="oProcess"></param>
    public void RemoveConstantProcess(ARX_Process oProcess)
    {
        IsDirty = true;
        if (oProcess == null || moa_constantProcesses.Contains(oProcess) == false)
            return;


        //Debug.Log("Removing process " + oProcess.name + " from constants in " + name);

        moa_constantProcesses.Remove(oProcess);

        if (moa_queuedProcesses.Contains(oProcess))
            RemoveQueuedProcess(oProcess);
    }

    /// <summary>
    /// Adds the given process to the top of the process queue
    /// </summary>
    /// <param name="oProcess"></param>
    public ARX_Process PushToTopProcess(ARX_Process oProcess)
    {
        IsDirty = true;
        if (oProcess == null)
        {
            Debug.LogError("Top Process given to " + name + " was null.");
            return oProcess;
        }

        if (oProcess == TopProcess)
        {
            Debug.LogError("Top Process given to " + name + " was null already Top Process.");
            return oProcess;
        }

        if (oProcess == this)
        {
            Debug.LogError("Attempted to recursively itself to its own list of processes");
            return oProcess;
        }


        oProcess.mo_parent = this;

        bool bContained = moa_queuedProcesses.Contains(oProcess);

        oProcess.me_processType = PROCESSTYPE.QUEUEUD;

        if (TopProcess != null)
            TopProcess.OnLoseTopProcessFocus();

        if (bContained)
            moa_queuedProcesses.Remove(oProcess);

        TopProcess = oProcess;


        if (!bContained)
            oProcess.OnAdded();

        oProcess.OnGainTopProcessFocus();
        return oProcess;
    }

    #endregion

    #region Virtual Functions

    public virtual string GetStateReport()
    {
        return name + " State Report is empty.";
    }

    /// <summary>
    /// Virtual function ran when this process is added to a 
    /// process list
    /// </summary>
    public virtual void OnAdded()
    {

    }

    /// <summary>
    /// Virtual function ran by a non-top process
    /// upon gaining the status of the top process
    /// </summary>
    public virtual void OnGainTopProcessFocus()
    {

    }

    /// <summary>
    /// Virtual function ran by the Top process upon losing
    /// its place as the top process
    /// </summary>
    public virtual void OnLoseTopProcessFocus()
    {

    }

    /// <summary>
    /// Virtual function ran once every disk rotation while not the top process
    /// </summary>
    public virtual bool OnInactiveUpdate()
    {
        return true;
    }

    /// <summary>
    /// Virtual function ran once every frame while not the top process
    /// </summary>
    public virtual bool OnInactiveFixedUpdate()
    {
        return true;
    }

    /// <summary>
    /// Virtual function ran when this process is removed from a list 
    /// </summary>
    public virtual void OnTerminated()
    {

    }

    /// <summary>
    /// Virtual function ran once every disk rotation.
    /// If a queued process, it will only be ran while the Top process
    /// </summary>
    public virtual void OnUpdate()
    {

    }

    /// <summary>
    /// Virtual function ran once every frame
    /// If a queued process, it will only be ran while the Top process
    /// </summary>
    public virtual void OnFixedUpdate()
    {

    }

    /// <summary>
    /// Virtual function ran before this object's first update
    /// </summary>
    public virtual void OnInitialized()
    {

    }

    /// <summary>
    /// Virtual function ran on this object's first update and one rotation
    /// after OnInitialized.
    /// OnInitialized -> OnFirstUpdate -> OnUpdate
    /// </summary>
    public virtual void OnFirstUpdate()
    {

    }

    public virtual void OnSceneLoaded(Scene scene, LoadSceneMode eMode)
    {
        IsDirty = true;
        //Debug.Log(name + " process reacting to on scene loaded.");
    }

    public virtual void OnSceneUnloaded(Scene scene)
    {
        IsDirty = true;
        //Debug.Log(name + " process reacting to on scene unloaded.");
    }

    #endregion

    #region Base Functions

    /// <summary>
    /// Returns true if the value in the message's ValueUniqueID is this actor's
    /// UniqueID.
    /// </summary>
    /// <param name="dat"></param>
    /// <returns></returns>
    protected virtual bool IsTargetingThisActor(DataString dat)
    {
        return dat.GetInt(GameIDs.ValueUniqueID) == UniqueID;
    }

    /// <summary>
    /// Function that is to be ran once every disk rotation
    /// </summary>
    public void Update()
    {
        if (IsCleaning)
        {
            IsDirty = false;
            IsCleaning = false;
        }

        if (IsDirty)
        {
            IsCleaning = true;
        }

        if (Paused)
            return;

        if (mb_terminated == true)
            return;

        if (IsWaiting)
        {
            Wait();
            if (IsWaiting)
                return;
        }

        mn_rotationSinceInitialization++;

        if (mb_initialized == false)
        {
            //Debug.LogError(name + " is now being initialized.");
            OnInitialized();
            mb_initialized = true;
            //DEBUGGERY!
            //Changing to return after initialization
            //to give the OnFirstUpdate function priority over Update
            return;
        }

        if (mb_firstUpdate)
        {
            OnFirstUpdate();
            mb_firstUpdate = false;
        }

        OnUpdate();

        //Debug.Log("Updating all " +moa_queuedProcesses.Count + " queued processes of " + name);
        for (int i = 0; i < moa_queuedProcesses.Count; i++)
        {
            ARX_Process oProcess = moa_queuedProcesses[i];

            if (oProcess == TopProcess)
                oProcess.Update();
            else
                oProcess.OnInactiveUpdate();
        }

        for (int i = 0; i < moa_constantProcesses.Count; i++)
        {
            ARX_Process oProcess = moa_constantProcesses[i];

            oProcess.Update();

        }

    }

    /// <summary>
    /// Function that is to be ran once every frame
    /// </summary>
    public void FixedUpdate()
    {
        if (Paused)
            return;

        if (mb_terminated == true)
            return;

        mn_framesSinceInitialization++;

        OnFixedUpdate();

        for (int i = 0; i < moa_queuedProcesses.Count; i++)
        {
            if (moa_queuedProcesses[i] == TopProcess)
                moa_queuedProcesses[i].FixedUpdate();
            else
                moa_queuedProcesses[i].OnInactiveFixedUpdate();

        }

        for (int i = 0; i < moa_constantProcesses.Count; i++)
        {
            moa_constantProcesses[i].FixedUpdate();

        }
    }

    /// <summary>
    /// Terminates and removes all child processes, all of which
    /// will also run PurpeProcesses.
    /// </summary>
    public void PurgeProcesses()
    {
        //Debug.Log(name + " purging constant processes.");
        for (int i = 0; i < moa_constantProcesses.Count; i++)
        {
            ARX_Process oProcess = moa_constantProcesses[i];
            oProcess.Terminate();
            moa_constantProcesses.Remove(oProcess);
            i--;
        }

        //Debug.Log(name + " purging queued processes.");
        for (int i = 0; i < moa_queuedProcesses.Count; i++)
        {
            ARX_Process oProcess = moa_queuedProcesses[i];
            //Debug.LogError("Purging process " + oProcess.name);
            oProcess.Terminate();
            moa_queuedProcesses.Remove(oProcess);
            i--;
        }

        moa_queuedProcesses.Clear();
        moa_constantProcesses.Clear();
    }

    /// <summary>
    /// Destroy this process and its children and remove it from its parent process.
    /// </summary>
    public void Terminate()
    {
        OnTerminated();
        EventRecord.UnsubscribeFromAllEvents();
        PurgeProcesses();
        mb_terminated = true;
        if (Parent != null)
        {
            Parent.RemoveConstantProcess(this);
            Parent.RemoveQueuedProcess(this);
        }
    }

    #endregion
}

