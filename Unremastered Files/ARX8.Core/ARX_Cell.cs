using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX;
using UnityEngine;

public struct ARX_CellSettings
{
    private int mn_path;
    public List<string> mo_attributes;
    
    public List<string> Attributes
    {
        get
        {
            if (mo_attributes == null)
                mo_attributes = new List<string>();
            return mo_attributes;
        }

    }

    public int Path { get { return mn_path; } }

    public string Name;

    public bool mb_enclosePathWalls;

    public ARX_CellSettings(string strName, int nPath = 1)
    {
        Name = strName;
        mn_path = -1;
        mo_attributes = new List<string>();
        mb_enclosePathWalls = true;
    }

    public ARX_CellSettings(string strName, int nPath, string[] astrAttributes)
    {
        Name = strName;
        mn_path = -1;
        mo_attributes = new List<string>(astrAttributes);
        mb_enclosePathWalls = true;
    }

    public void SetPath(int nPath)
    {
        mn_path = nPath;
    }

    public void Add(string strAttribute)
    {
        if (Attributes.Contains(strAttribute) == false)
            Attributes.Add(strAttribute);
    }
}

public class ARX_Cell
{
    private ARX_Grid mo_parentGrid;
    private GameObject mo_gameObject;

    private ARX_Script_Cell mo_cellScript;
    public ARX_Script_Cell CellScript
    {
        get
        {
            if(mo_cellScript == null)
                mo_cellScript = mo_gameObject.GetComponent<ARX_Script_Cell>();
            return mo_cellScript;
        }
    }

    private ARX_Script_IndividualColor mo_colorScript;
    public ARX_Script_IndividualColor ColorScript { get
        {
            if (mo_colorScript == null)
                mo_colorScript = gameObject.AddComponent<ARX_Script_IndividualColor>();
            return mo_colorScript;
        }
    }

    public GameObject gameObject { get { return mo_gameObject; } }

    public int Path { get { return mo_cellSettings.Path; } }

    public ARX_CellSettings mo_cellSettings;

    private Vector3Int mvec_position;
    
    public Vector3Int Position
    {
        get
        {
            return mvec_position;
        }
    }

    public ARX_Cell(ARX_Grid oParent, Vector3Int position, string strCellPrefab, int nPath = -1)
    {
        mo_parentGrid = oParent;
        mvec_position = position;
        mo_cellSettings.SetPath(nPath);
        InitializeGameObject(strCellPrefab);
    }

    public ARX_Cell(ARX_Grid oParent, ARX_CellSettings oSettings, string strCellPrefab)
    {
        mo_parentGrid = oParent;
        mo_cellSettings = oSettings;
        InitializeGameObject(strCellPrefab);
    }

    public virtual void InitializeGameObject(string strCellPrefab)
    {
        //mo_gameObject = FEN.Loading.Load(strCellPrefab);
        //ARX_Script_Cell oScript = mo_gameObject.GetComponent<ARX_Script_Cell>();
        //oScript.SetCell(this);
        //mo_gameObject.transform.SetParent(mo_parentGrid.transform);
        //mo_gameObject.name = "Cell " + Position;

        //Recolor();
    }

    public void Recolor()
    {
        Color color = mo_parentGrid.GetPathColor(Path);
        ColorScript.mo_color = color;
    }

    public void Destroy()
    {
        GameObject.Destroy(mo_gameObject);
    }

    public void Set(ARX_CellSettings settings)
    {
        mo_cellSettings = settings;
        Recolor();
        if (HasAttribute(ARX_Grid.empty) == false && HasAttribute(ARX_Grid.nullpanel) == false)
        {
            mo_parentGrid.AddToBounds(this);
        }
    }

    public void SetPath(int nPath)
    {
        mo_cellSettings.SetPath(nPath);
        Recolor();
    }

    public bool HasAttribute(string strAttribute)
    {
        return mo_cellSettings.Attributes.Contains(strAttribute);
    }

    public bool HasAttributes(string[] strAttribute)
    {
        foreach (string str in strAttribute)
            if (HasAttribute(str) == false)
                return false;
        return true;
    }

    public void Add(string strAttribute)
    {
        if (mo_cellSettings.Attributes.Contains(strAttribute) == false)
            mo_cellSettings.Attributes.Add(strAttribute);
    }

    public override string ToString()
    {
        string str = mo_cellSettings.Name + "\n " + Position.x + ", " + Position.y + ", " + Position.z + "\tPath " + mo_cellSettings.Path + "\n";
        str += "\nAttributes ";

        bool bFirst = true;
        foreach (string att in mo_cellSettings.Attributes)
        {
            if (!bFirst)
                str += ", "; 

            str += att;

            bFirst = false;
        }
        
        return str;
    }
}

public delegate bool CellValidate(ARX_Cell cell);