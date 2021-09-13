using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ARX_Script_Cell : ARX_Script_Actor
{
    public enum FLANKSTATUS {DEFAULT, FLANKED, WALLEDOFF }

    Color HighlightedColor { get { return Color.yellow; } }
    Color StartColor;
    
    ARX_Cell mo_cell;

    public Vector3 vecSpacing = Vector3.one;
    public GameObject mo_north, mo_south, mo_east, mo_west, mo_up, mo_down, mo_none;

    [HideInInspector]
    public List<Text> moa_textLabels = new List<Text>();
    
    const string strDefault = "default", strFlanked = "flanked", walledoff = "walledoff";

    private void Start()
    {
        moa_textLabels.Clear();
        Renderer ren = GetComponent<Renderer>();
        if (ren != null)
        {
            ren.receiveShadows = false;
            ren.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        if (transform.childCount == 0)
            CreateChildren();
    }

    void CreateChildren()
    {
        //GameObject oTextCanvases = new GameObject();
        //oTextCanvases.name = "Text canvases";
        //oTextCanvases.transform.SetParent(transform);
        //oTextCanvases.transform.localPosition = Vector3.zero;

        for (int i = 0; i < System.Enum.GetValues(typeof(DIRECTION)).Length; i++)
        {
            DIRECTION dir = (DIRECTION)i;

            GameObject obj = CreateChild(dir);
            switch (dir)
            {
                case DIRECTION.DOWN:
                    mo_down = obj;
                    break;
                case DIRECTION.EAST:
                    mo_east = obj;
                    break;
                case DIRECTION.NONE:
                    mo_none = obj;
                    break;
                case DIRECTION.NORTH:
                    mo_north = obj;
                    break;
                case DIRECTION.SOUTH:
                    mo_south = obj;
                    break;
                case DIRECTION.UP:
                    mo_up = obj;
                    break;
                case DIRECTION.WEST:
                    mo_west = obj;
                    break;
            }
            

            ////Create Canvas
            //if (dir != DIRECTION.NONE)
            //{
            //    GameObject oWorldCanvas = FEN.Loading.Load("worldcanvas");
            //    oWorldCanvas.transform.SetParent(oTextCanvases.transform);
            //    oWorldCanvas.name = dir.ToString().ToLower() + " canvas";
            //    Text oTextLabel = oWorldCanvas.GetComponentInChildren<Text>(true);

            //    Vector3 vecDirection = ToolBox.GetTranslation(dir);
            //    Vector3 vecTranslation = ToolBox.MultiplyVector3(vecDirection, vecSpacing);
            //    vecTranslation = ToolBox.MultiplyVector3(vecTranslation, transform.localScale);
            //    oWorldCanvas.transform.localPosition = vecTranslation;

            //    string str = dir.ToString();
            //    while (str.Length > 1)
            //    {
            //        str = str.Remove(1);
            //    }
            //    oTextLabel.text = str;

            //}
        }
    }
    
    GameObject CreateChild(DIRECTION dir)
    {
        GameObject obj = new GameObject();
        obj.name = dir.ToString().ToLower();

        //Create Default
        GameObject oDefault = new GameObject();
        oDefault.name = strDefault;

        //Create Flanked 
        GameObject oFlanked = new GameObject();
        oFlanked.name = strFlanked;

        //Create Empty 
        GameObject oEmpty = new GameObject();
        oEmpty.name = walledoff;

        obj.transform.SetParent(transform);
        oDefault.transform.SetParent(obj.transform);
        oFlanked.transform.SetParent(obj.transform);
        oEmpty.transform.SetParent(obj.transform);

        return obj;

    }

    public GameObject GetChild(DIRECTION dir)
    {
        switch (dir)
        {
            case DIRECTION.DOWN:
                return mo_down;
            case DIRECTION.EAST:
                return mo_east;
            case DIRECTION.NONE:
                return mo_none;
            case DIRECTION.NORTH:
                return mo_north;
            case DIRECTION.SOUTH:
                return mo_south;
            case DIRECTION.UP:
                return mo_up;
            case DIRECTION.WEST:
                return mo_west;
            default:
                return null;
        }
    }

    public GameObject GetChild(DIRECTION dir, FLANKSTATUS eStatus)
    {
        Transform obj = null;
        string strDir = dir.ToString().ToLower();
        string strStatus = eStatus.ToString().ToLower();
        
        //Get the child object with the same name as the given direction
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == strDir)
            {
                obj = transform.GetChild(i);
            }
        }
        
        if (obj == null)
            return null;

        //Get the child object with the same name as the given flank status
        for (int i = 0; i < obj.childCount; i++)
        {
            if (obj.GetChild(i).name == strStatus)
            {
                return obj.GetChild(i).gameObject;
            }
        }

        return null;
    }

    public void SetCell(ARX_Cell cell)
    {
        mo_cell = cell;
    }

    public override void V_OnMouseEnter()
    {
        string str = "Mouse on this thingie.";
        if (mo_cell != null)
            str = mo_cell.ToString();

        Vector2 vecoffset = new Vector2(200, 100);

        //Tooltip.SetText(str);
        //Tooltip.SetPositionOnMouse(vecoffset);
        //Tooltip.ShowTooltip();

        ARX_Script_IndividualColor mo_colorScript = GetComponent<ARX_Script_IndividualColor>();
        StartColor = mo_colorScript.mo_color;
        mo_colorScript.mo_color = HighlightedColor;
    }

    public override void V_OnMouseExit()
    {
        //Tooltip.HideTooltip();
        if (mo_cell != null)
            mo_cell.Recolor();

        ARX_Script_IndividualColor mo_colorScript = GetComponent<ARX_Script_IndividualColor>();
        mo_colorScript.mo_color = StartColor;
    }

    private void OnDrawGizmos()
    {
        //Vector3 vecDirection = ToolBox.GetTranslation(DIRECTION.NORTH);
        //Vector3 vecTranslation = ToolBox.MultiplyVector3(vecDirection, vecSpacing);
        //vecTranslation = ToolBox.MultiplyVector3(vecTranslation, transform.localScale);
        //vecTranslation += transform.position;


        //Gizmos.DrawSphere(vecTranslation, 1);
    }

    public void SetFlankingGameObject(DIRECTION dir, FLANKSTATUS eFlankStatus)
    {
        GameObject oFlanked = GetChild(dir, FLANKSTATUS.FLANKED);
        GameObject oWalled = GetChild(dir, FLANKSTATUS.WALLEDOFF);

        switch (eFlankStatus)
        {
            case FLANKSTATUS.WALLEDOFF:
                if(oWalled != null)
                    oWalled.SetActive(true);
                if (oFlanked != null)
                    oFlanked.SetActive(false);
                break;
            case FLANKSTATUS.FLANKED:
                if (oWalled != null)
                    oWalled.SetActive(false);
                if (oFlanked != null)
                    oFlanked.SetActive(true);
                break;
        }

    }

    public void SetAllToWalled()
    {
        System.Array array = System.Enum.GetValues(typeof(DIRECTION));
        for (int i = 0; i < array.Length; i++)
        {
            DIRECTION dir = (DIRECTION)i;
            if (dir == DIRECTION.NONE)
                continue;
            SetFlankingGameObject(dir, FLANKSTATUS.WALLEDOFF);
        }
    }

    public void SetAllToOpenWalls()
    {
        System.Array array = System.Enum.GetValues(typeof(DIRECTION));
        for (int i = 0; i < array.Length; i++)
        {
            DIRECTION dir = (DIRECTION)i;
            if (dir == DIRECTION.NONE)
                continue;
            SetFlankingGameObject(dir, FLANKSTATUS.FLANKED);
        }
    }
    
}
