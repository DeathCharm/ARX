using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ARX;
using FEN;

[ExecuteInEditMode]
public partial class ARX_Script_RPGStage : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Child object serving as the root of anchor posiiton view spheres.
    /// </summary>
    public GameObject mo_viewSphereRoot = null;

    /// <summary>
    /// This stage anchor will be deleted from the list of anchors at the end of the Update cycle
    /// </summary>
    private StageAnchor mo_deleteMeThisFrame = null;

    /// <summary>
    /// The name of the family whose gizmos will always be drawn.
    /// </summary>
    const string DEFAULTDRAWNFAMILY = "default";

    /// <summary>
    /// The name of the group of anchor gizmos currently being drawn.
    /// </summary>
    public string mstr_viewedFamily = "";

    /// <summary>
    /// The model for the stage.
    /// </summary>
    [HideInInspector]
    public GameObject mo_stageCube;

    /// <summary>
    /// The model for the stage's backborder.
    /// </summary>
    [HideInInspector]
    public GameObject mo_borderCube;

    /// <summary>
    /// Root Image reference. If this is not null, the stage will be sized according to the
    /// image's dimensions.
    /// </summary>
    public Image mo_rootImage = null;

    /// <summary>
    /// The width of the anchor by which the stage is sized.
    /// This value will be used to size the stage is the mo_canvas
    /// variable is null.
    /// </summary>
    public float mnf_anchorWidth = 1024;

    /// <summary>
    /// The width of the anchor by which the stage is sized.
    /// This value will be used to size the stage is the mo_canvas
    /// variable is null.
    /// </summary>
    public float mnf_anchorHeight = 768;

    /// <summary>
    /// The scale of the stage.
    /// The stage's size will be Anchor / 1000 * Stage Scale
    /// </summary>
    public float mnf_stageScale = 10.0F;

    public float mnf_stageWidth, mnf_stageHeight;

    /// <summary>
    /// Used by the OnValueChanged function to calculate the new scale of the stage
    /// when its scale values are tweaked.
    /// </summary>
    [SerializeField]
    [HideInInspector]
    private float mnf_lastStageScale = 10.0F;

    /// <summary>
    /// The size of the drawn gizmo
    /// </summary>
    public float mnf_gizmoSize = 5F;

    /// <summary>
    /// The output of the generated Get functions
    /// </summary>
    [TextArea(1, 10)]
    public string mstr_output = "No Output";

    /// <summary>
    /// The saved positions of this 
    /// </summary>
    public List<StageAnchor> moa_stagePositions = new List<StageAnchor>();


    /// <summary>
    /// Private reference for the RPGSTage global instance.
    /// </summary>
    private static ARX_Script_RPGStage mo_instance;
    #endregion

    #region Accessors

    float StageWidth
    {
        get
        {
            const float canvasToUnityScale = 1000;
            return ViewWidth / canvasToUnityScale * mnf_stageScale;
        }
    }

    float StageHeight
    {
        get
        {
            const float canvasToUnityScale = 1000;
            return ViewHeight / canvasToUnityScale * mnf_stageScale;
        }
    }


    /// <summary>
    /// Returns the stage's height after calculating the scale and anchor.
    /// </summary>
    float ViewHeight
    {
        get
        {
            if (mo_rootImage != null)
            {
                return mo_rootImage.rectTransform.rect.height;
            }

            return mnf_anchorHeight;
        }
    }

    /// <summary>
    /// Returns the stage's width after calculating the scale and anchor.
    /// </summary>
    float ViewWidth
    {
        get
        {
            if (mo_rootImage != null)
            {
                return mo_rootImage.rectTransform.rect.width;
            }

            return mnf_anchorWidth;
        }
    }
    #endregion

    #region Helper

    /// <summary>
    /// Set's the Gismo Color bases on the Stage Anchor's settings
    /// </summary>
    void SetGizmoColor(StageAnchor oPos)
    {
        if (oPos.mstr_family != DEFAULTDRAWNFAMILY)
            Gizmos.color = oPos.color;
        else
            Gizmos.color = Color.blue;
    }

    /// <summary>
    /// Converts the given UI element world position to 
    /// </summary>
    /// <param name="vecPosition"></param>
    /// <returns></returns>
    Vector2 ConvertUIToStage(RectTransform oRectTransform)
    {
        Vector2 vecWorldPositionOfUIElement = oRectTransform.transform.position;

        float nfXScale = StageWidth / ViewWidth;
        float nfYScale = StageHeight / ViewHeight;
        
        float nfX = vecWorldPositionOfUIElement.x * nfXScale * mnf_stageScale;
        float nfY = vecWorldPositionOfUIElement.y * nfYScale * mnf_stageScale;
        

        Vector3 vecReturn = new Vector2(nfX, nfY);

        Debug.Log("Vector " + vecWorldPositionOfUIElement + " was converted to stage position " + vecReturn);

        return vecReturn;
    }

    /// <summary>
    /// If the given anchor has a stage element, merge its values
    /// </summary>
    /// <param name="oPos"></param>
    void ScanForElement(StageAnchor oPos)
    {
        if (oPos.mo_mergeElement == null)
            return;

        Vector2 vec = ConvertUIToStage(oPos.mo_mergeElement);

        oPos.SetStagePositionByWorldPosition(vec, this);
    }

    /// <summary>
    /// Returns an instance of the RPGStage if it exists.
    /// </summary>
    public static ARX_Script_RPGStage Instance
    {
        get
        {
            if (mo_instance == null)
            {
                mo_instance = FindObjectOfType<ARX_Script_RPGStage>();
                if (mo_instance == null)
                {
                    Debug.LogError("No RPGStage object exists.");
                    return null;
                }
            }
            return mo_instance;

        }
    }

    /// <summary>
    /// Creates and saves a new stage cube.
    /// </summary>
    void CreateStageCube()
    {
        if (mo_stageCube == null)
        {
            mo_stageCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mo_stageCube.transform.localScale = new Vector3(1, 1, 0.1F);
            //mo_stageCube.hideFlags = HideFlags.HideInHierarchy;
            mo_stageCube.AddComponent<ARX_Script_IndividualColor>().mo_color = new Color(1, 0, 1, 1);
            mo_stageCube.name = "Stage Cube";
            name = "RPG Stage";
        }

        if (mo_borderCube == null)
        {
            mo_borderCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mo_borderCube.transform.localScale = new Vector3(1, 1, 0.1F);
            //mo_boarderCube.hideFlags = HideFlags.HideInHierarchy;
            mo_borderCube.AddComponent<ARX_Script_IndividualColor>().mo_color = Color.cyan;
            mo_borderCube.name = "Border Background";
        }

    }

    /// <summary>
    /// Sets the size of the 
    /// </summary>
    public void SetStageCubeSize()
    {
        mnf_anchorWidth = ViewWidth;
        mnf_anchorHeight = ViewHeight;

        //One UI canvas pixel = One Unity Unit 
        //So to shrink a UI canvas down to a normal unity model
        //divide the UI canvas dimensions by 1000
        const float canvasToUnityScale = 1000;

        //Divide the Stage (aka UI canvas) by 1000 to the sizes of the stage cube and multiply it by the stage scale
        float nfX = ViewWidth / canvasToUnityScale * mnf_stageScale;
        float nfY = ViewHeight / canvasToUnityScale * mnf_stageScale;

        mo_stageCube.transform.localScale = new Vector3(nfX, nfY, 0.01F);
        mo_stageCube.transform.SetParent(transform);
        mo_stageCube.transform.localPosition = Vector3.zero;

        mo_borderCube.transform.localScale = new Vector3(nfX * 2, nfY * 2, 0.01F);
        mo_borderCube.transform.SetParent(transform);
        mo_borderCube.transform.localPosition = Vector3.zero + new Vector3(0, 0, 0.01F);

    }

    /// <summary>
    /// Returns the world position of the given anchor on the RPG Stage
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector3 GetPositionOnStage(StageAnchor pos)
    {
        return transform.position + (pos.mvec_localPosition);
    }

    /// <summary>
    /// Returns the world position of the given anchor on the UI Canvas
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector3 GetPositionOnCanvas(StageAnchor pos)
    {
        Vector3 vecWorldPosition = pos.mvec_localPosition * 1000 / mnf_stageScale;
        vecWorldPosition.x += ViewWidth / 2;
        vecWorldPosition.y += ViewHeight / 2;
        return vecWorldPosition;
    }

    /// <summary>
    /// Returns a list of stage anchors whose gizmos should be drawn.
    /// </summary>
    public List<StageAnchor> GetGizmosToDraw
    {
        get
        {
            //If the viewed family is blank, return all stage positions
            if (mstr_viewedFamily == "")
                return moa_stagePositions;
            List<StageAnchor> oa = new List<StageAnchor>();
            foreach (StageAnchor stageAnchor in moa_stagePositions)
            {
                //If the faimly to view is default or the currently viewed family, 
                //add it to the list to be drawn
                if (stageAnchor.mstr_family == DEFAULTDRAWNFAMILY ||
                    stageAnchor.mstr_family == mstr_viewedFamily)
                    oa.Add(stageAnchor);
            }

            return oa;
        }
    }

    public StageAnchor GetStageAnchorByName(string str)
    {
        foreach (StageAnchor anchor in moa_stagePositions)
            if (anchor.mstr_name == str)
                return anchor;
        return null;
    }

    #endregion

    #region Global
    public static StageAnchor GetStageAnchor(RPGStage.RelativePositions ePosition)
    {
        //Not implemented yet
        return null;
    }

    /// <summary>
    /// Returns the stage anchor
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static StageAnchor GetStageAnchor(string str)
    {

        return Instance.GetStageAnchorByName(str);
    }

    /// <summary>
    /// Sets the given transform's parent to the root image of the RPG stage.
    /// </summary>
    /// <param name="obj"></param>
    public static void AddToUICanvas(Transform obj)
    {
        if (Instance.mo_rootImage == null)
            return;
        obj.transform.SetParent(Instance.mo_rootImage.transform);
    }
    #endregion

    #region Nested Classes
    [System.Serializable]
    public class StageAnchor
    {
        public Color color = Color.green;
        public string mstr_family = DEFAULTDRAWNFAMILY;
        public string mstr_name = "New Position";
        public bool mb_viewFamily = false;
        public Vector3 mvec_localPosition = new Vector3(3, 3, 0);
        public bool mb_lockZAxis = true;
        public bool mb_deleteMe = false;
        public RectTransform mo_mergeElement = null;

        /// <summary>
        /// Sets the given transform's parent to the root image of the RPG stage.
        /// </summary>
        /// <param name="obj"></param>
        public void AddToUICanvas(Transform obj)
        {
            if (Instance.mo_rootImage == null)
                return;
            obj.transform.SetParent(Instance.mo_rootImage.transform);
            obj.transform.position = GetPositionOnCanvas();
        }

        public void AddToStage(Transform obj)
        {
            obj.transform.position = GetPositionOnStage();
        }

        /// <summary>
        /// Returns a string containing a definition of static Get Function for this anchor
        /// </summary>
        public string GetFunctionOutput
        {
            get
            {
                string strPrefix = @"public static " + ToolBox.GetQualifiedType(this.GetType()) + " Get" + mstr_name.FirstCharToUpper().TrimWhitespace() + "{\nget\n{\n";
                string strFunction = "return " + ToolBox.GetQualifiedType(typeof(ARX_Script_RPGStage)) + ".GetStageAnchor(\"" + mstr_name + "\");\n}\n}\n\n";
                return strPrefix + strFunction;
            }
        }

        /// <summary>
        /// Given the world position of a UI element, changes this Anchor's local position
        /// </summary>
        /// <param name="vecNewWorldPosition"></param>
        /// <param name="stage"></param>
        public void SetStagePositionByWorldPosition(Vector3 vecNewWorldPosition, ARX_Script_RPGStage stage)
        {
            Vector3 vecNewLocal = vecNewWorldPosition - stage.transform.position;
            mvec_localPosition = vecNewLocal;
        }

        /// <summary>
        /// Returns the world stage position of this anchor
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPositionOnStage()
        {
            return ARX_Script_RPGStage.Instance.GetPositionOnStage(this);
        }

        /// <summary>
        /// Returns the world UI Canvas position of this anchor
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPositionOnCanvas()
        {

            return ARX_Script_RPGStage.Instance.GetPositionOnCanvas(this);
        }

    }
    #endregion

    #region Unity Overrides
    private void Start()
    {
        mnf_lastStageScale = mnf_stageScale;
    }

    // Update is called once per frame
    void Update()
    {
        mnf_stageWidth = StageWidth;
        mnf_stageHeight = StageHeight;

        //If there is no stage object, create a new stage object
        CreateStageCube();

        //Set the size of the stage
        SetStageCubeSize();

        //Set the code output
        mstr_output = GetOutput;

    }

    private void OnDrawGizmos()
    {
        if (mnf_gizmoSize < 0)
            mnf_gizmoSize = 0.1F;

        //The gizmo can be pretty hard to size
        float nfPracticalGismoSize = mnf_gizmoSize / 100 * mnf_stageScale;

        foreach (ARX_Script_RPGStage.StageAnchor oPos in GetGizmosToDraw)
        {
            //Scan the position to see if it's values need to be set to a
            //UI element's
            ScanForElement(oPos);

            if (oPos.mb_deleteMe)
            {
                mo_deleteMeThisFrame = oPos;
            }

            if (oPos.mb_lockZAxis)
                oPos.mvec_localPosition.z = 0;

            Vector3 vecStagePosition = GetPositionOnStage(oPos);

            //If the anchor's family name is "default", draw it in blue
            //Else, draw it in green.

            SetGizmoColor(oPos);
            
            //Draw the gizmo on the Stage
            Gizmos.DrawSphere(vecStagePosition, nfPracticalGismoSize);
            
            SetGizmoColor(oPos);

            //Draw the gizmo on the Canvas
            Gizmos.DrawSphere(GetPositionOnCanvas(oPos), nfPracticalGismoSize * 30);
        }


        if (mo_deleteMeThisFrame != null)
        {
            moa_stagePositions.Remove(mo_deleteMeThisFrame);
            mo_deleteMeThisFrame = null;
            
        }
    }

    private void OnValidate()
    {
        #region Set Stage Scale
        //If the stage scale has been changed
        if (mnf_lastStageScale != mnf_stageScale)
        {
            //Change the values of each stage position
            float nfChangeScale = ToolBox.Derp.MultChange(mnf_stageScale, mnf_lastStageScale);
            foreach (StageAnchor pos in moa_stagePositions)
            {
                pos.mvec_localPosition = pos.mvec_localPosition * nfChangeScale;
            }
        }

        mnf_lastStageScale = mnf_stageScale;
        #endregion

        #region Check for set family to view
        foreach (StageAnchor stageAnchor in moa_stagePositions)
        {
            if (stageAnchor.mb_viewFamily)
            {
                stageAnchor.mb_viewFamily = false;
                mstr_viewedFamily = stageAnchor.mstr_family;
            }
        }

        #endregion
    }

    #endregion

    #region Get Code Generation

    private const string mstr_using =
 @"using FEN;
using ARX;" + "\n\n";
    private const string mstr_autoSource = "//Auto-Generated by RPGStage. A Static class containing Get functions for every RPG Stage anchor\n";
    private const string mstr_namespace = "namespace FEN{\n";
    private const string mstr_staticClass = "public static class StagePositions{\n";

    /// <summary>
    /// Returns a string containing a definition of a static class containing Get functions for all RPGStage anchor positions.
    /// </summary>
    private string GetOutput
    {
        get
        {
            string strReturn = "";
            strReturn += mstr_using + mstr_autoSource + mstr_namespace + mstr_staticClass;

            foreach (StageAnchor anchor in moa_stagePositions)
                strReturn += anchor.GetFunctionOutput + "\n";

            strReturn += "}\n}";
            return strReturn;
        }
    }
    #endregion
}
