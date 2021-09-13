using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ARX;
using ARX.VarGen;

public class ARX_ImageToEditorWindow : ARX_Tree<Image>
{
    #region Constructor
    public ARX_ImageToEditorWindow(Image img, ARX_Script_CreateEditorWindow tester) : base(img)
    {
        mo_tester = tester;
        mo_mainImage = img;
    }
    #endregion

    #region Variables

    ARX_Script_CreateEditorWindow mo_tester;
    Image mo_mainImage;

    #endregion

    #region Abstract Overrides

    public override void I_BeforeFirstElement(Image img)
    {
        mo_tester.mstr_constants += "#region Constants\n\n";

        mo_tester.mstr_onDrawFunction += "void Draw(){\n";
        mo_tester.mstr_drawfunctionoutput += "#region Draw Functions\n";
    }

    public override void I_AfterLastElement(Image img)
    {
        mo_tester.mstr_constants += "#endregion //Constants\n";

        AddOnGUIMethod();
        mo_tester.mstr_onDrawFunction += "}\n";

        mo_tester.mstr_drawfunctionoutput += "\n\n" + mo_tester.mstr_onDrawFunction;
        mo_tester.mstr_drawfunctionoutput += "\n\n#endregion";
    }

    public override void I_OpenElement(Image element)
    {
        SetConstOutput(element);
        SetDrawOutput(element.name, element);
        SetDrawFunctionOutput(element.name, element);
    }


    public override Image I_CloseElement(Image element)
    {
        mo_tester.mstr_constants += "#endregion " + "// " + element.name + " End\n\n";
        return element;
    }

    public override Image I_GetFirstChild(Image element)
    {
        List<Image> children = new List<Image>(element.GetComponentsInChildren<Image>(true));
        children.Remove(element);
        return children[0];
    }

    public override Image I_GetParent(Image element)
    {
        Transform parent = element.transform.parent;
        if (parent == null)
            return null;

        return parent.GetComponent<Image>();

        //return true;

        //List<Image> elementsInParent = new List<Image>(element.GetComponentsInParent<Image>(true));
        //elementsInParent.Remove(element);
        //if (elementsInParent.Count == 0)
        //    return null;

        //Image parent = elementsInParent[0];
        //return parent;
    }

    public override Image I_GetNextSibling(Image element)
    {
        int nSiblingIndex = element.transform.GetSiblingIndex();

        Transform sibling = element.transform.parent.GetChild(nSiblingIndex + 1);
        return sibling.GetComponent<Image>();
    }

    public override bool I_HasChildren(Image element)
    {
        List<Image> children = new List<Image>(element.GetComponentsInChildren<Image>(true));
        children.Remove(element);
        return children.Count > 0;
    }

    public override bool I_HasNextSiblingInList(Image element)
    {
        if (element == null)
            return false;

        int nSiblingIndex = element.transform.GetSiblingIndex();
        nSiblingIndex++;
        return nSiblingIndex < element.transform.parent.childCount;
    }

    public override bool I_HasParent(Image element)
    {
        Transform parent = element.transform.parent;
        if (parent == null)
            return false;

        if (parent.GetComponent<Image>() == null)
            return false;

        return true;
    }


    #endregion

    #region Get Strings
    /// <summary>
    /// Returns a string calling the Draw method of the given region.
    /// Example: Given a region "MAIN_WINDOW", 
    /// returns "DrawMAIN_WINDOW();"
    /// </summary>
    /// <param name="strRegion"></param>
    /// <returns></returns>
    string GetCallDrawMethodString(string strRegion)
    {
        return GetDrawMethodName(strRegion) + "();\n";
    }

    /// <summary>
    /// Returns a string containing "Draw" + the given region's name.
    /// </summary>
    /// <param name="strRegion"></param>
    /// <returns></returns>
    string GetDrawMethodName(string strRegion)
    {
        return "Draw" + strRegion;
    }

    /// <summary>
    /// Returns a string defining the "void OnGUI" method, 
    /// which will call the "Draw()" method
    /// </summary>
    /// <returns></returns>
    string AddOnGUIMethod()
    {
        string str = @"private void OnGUI()
        {
            Draw();
        }";

        mo_tester.mstr_drawfunctionoutput += str;
        return str;
    }

    /// <summary>
    /// Returns a string defining a Draw method for the given region.
    /// The returned method will also draw a simple rect of the region.
    /// </summary>
    /// <param name="strRegion"></param>
    /// <param name="img"></param>
    /// <returns></returns>
    string CreateDrawMethod(string strRegion)
    {
        string strOutput = "void " + GetDrawMethodName(strRegion) + "(){\n";
        strOutput += CreateDrawRectString(strRegion);
        strOutput += "}\n";
        return strOutput;
    }

    /// <summary>
    /// Returns const variables extracted from the given rect and named
    /// after the given region. The variables returned are the rect's
    /// x, y, width and height.
    /// </summary>
    /// <param name="strRegion"></param>
    /// <param name="rect"></param>
    /// <returns></returns>
    string GetConstVariablesFromRect(string strRegion, Rect rect)
    {
        string strVariableType = "float";
        strRegion = strRegion.ToUpper();
        string strPrefix = "const " + strVariableType + " " + strRegion;
        string strOutput = "";

        //Width/Height
        string width = strPrefix + "_WIDTH = " + rect.width + "F;\n";
        string height = strPrefix + "_HEIGHT = " + rect.height + "F;\n";

        //Position
        string x = strPrefix + "_X = " + rect.x + "F;\n";
        string y = strPrefix + "_Y = " + rect.y + "F;\n";

        strOutput = width + height + x + y;
        return strOutput;
    }

    /// <summary>
    /// Returns a string defining the name of a Rect Accessor named after the given region.
    /// </summary>
    /// <param name="strRegion"></param>
    /// <returns></returns>
    string GetRectAccessorName(string strRegion)
    {
        return "Get" + strRegion + "Rect";
    }

    /// <summary>
    /// Returns a string defining a Rect Accessor named after the given region.
    /// </summary>
    /// <param name="strRegion"></param>
    /// <returns></returns>
    string CreateGetRectAccessor(string strRegion, Rect rect)
    {
        string strUpper = strRegion.ToUpper();
        string strOutput = $"private Rect " + GetRectAccessorName(strRegion) + "\n{\n";
        strOutput += "\tget{\n\t";
        strOutput += "return new Rect(" + strUpper + "_X, " + strUpper + "_Y, " + strUpper + "_WIDTH, " + strUpper + "_HEIGHT);";

        strOutput += "\n\t}\n}\n";
        return strOutput;
    }

    /// <summary>
    /// Returns a string defining a Rect Accessor named after the given region.
    /// </summary>
    /// <param name="strRegion"></param>
    /// <returns></returns>
    string CreateGetRectAccessor(string strRegion, Image img)
    {
        bool bIsBaseImage = img == mo_mainImage;
        return CreateGetRectAccessor(strRegion, ARX_Rect.ExtractEditorRectFromUIRect(img, bIsBaseImage));
    }
    /// <summary>
    /// Returns a string defining a Color named after the given region.
    /// The given color is extracted from the given image.
    /// </summary>
    /// <param name="strRegion"></param>
    /// <param name="img"></param>
    /// <returns></returns>
    string GetColorMemberVariableName(string strRegion, Image img)
    {
        string strOutput = "readonly Color " + GetColorName(strRegion) + " = new Color";
        strOutput += VariableGen.ExtractColorOverOne(img);
        return strOutput;
    }

    /// <summary>
    /// Returns the name of a color named after the given regino.
    /// Example: "MYREGION_COLOR"
    /// </summary>
    /// <param name="strRegion"></param>
    /// <returns></returns>
    string GetColorName(string strRegion)
    {
        return strRegion.ToUpper() + "_COLOR";
    }

    /// <summary>
    /// Returns a string calling "EditorGUI.DrawRect" to draw the 
    /// given region to the editor. Assumes the region's color and rect
    /// to have been defined prior.
    /// Requires
    /// </summary>
    /// <param name="strRegion"></param>
    /// <param name="img"></param>
    /// <returns></returns>
    string CreateDrawRectString(string strRegion)
    {
        string strGetRectName = GetRectAccessorName(strRegion);
        string strColor = GetColorName(strRegion);

        string strDraw = "EditorGUI.DrawRect(" + strGetRectName + ", " + strColor + ");\n";
        return strDraw;
    }

    #endregion

    #region Output

    /// <summary>
    /// Extracts and adds the const variables from the given element to the constOutput string.
    /// Includes the element's x, y, width, height and color.
    /// </summary>
    /// <param name="element"></param>
    void SetConstOutput(Image element)
    {
        string strRegion = element.name;
        string strOutput = "";
        string strRegionName = element.gameObject.name;

        //Validate region name to follow C# naming rules

        strOutput = "#region " + strRegionName + "\n\n";

        bool bIsBaseImage = element == mo_mainImage;
        //Extract Rect
        Rect rect;
        rect = ARX_Rect.ExtractEditorRectFromUIRect(element, bIsBaseImage);
        //Set Rect const variables

        strOutput += GetConstVariablesFromRect(strRegionName, rect);
        
        //Add to Nester output string

        mo_tester.mstr_constants += strOutput + "";
        mo_tester.mstr_constants += GetColorMemberVariableName(strRegion, element);
        mo_tester.mstr_constants += "\n";
        mo_tester.mstr_constants += CreateGetRectAccessor(strRegion, element);

        mo_tester.mstr_constants += "\n\n";
    }

    /// <summary>
    /// Adds a Draw Method string to the drawFunctionOutput
    /// </summary>
    /// <param name="strRegion"></param>
    /// <param name="img"></param>
    void SetDrawFunctionOutput(string strRegion, Image img)
    {
        mo_tester.mstr_drawfunctionoutput += CreateDrawMethod(strRegion) + "\n";

    }


    /// <summary>
    /// Adds a call to a Draw Method string to the OnDraw() string which calls
    /// the Draw Methods of all regions.
    /// </summary>
    /// <param name="strRegion"></param>
    /// <param name="img"></param>
    void SetDrawOutput(string strRegion, Image img)
    {
        mo_tester.mstr_onDrawFunction += GetCallDrawMethodString(strRegion);
    }

    #endregion
}
