using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Xml;
using System.IO;
using UnityEngine.UI;
using ARX;

/// <summary>
/// Given a spreadsheet Text Asset, parses the text and splits it into multiple pages held in 
/// the stringDictionary object.
/// </summary>
public class ARX_SpreadsheetParser 
{
    public ARX_StringDictionary moa_stringDictionary = new ARX_StringDictionary();
    public TextAsset mo_textAsset;
    string mstr_currentLanguage = "{eng}";
    public string mstr_newPageTag = "<br>";

    public ARX_SpreadsheetParser(TextAsset oTextAsset)
    {
        mo_textAsset = oTextAsset;
        InitializeStrings();
    }
    

    public void ChangeLanguage(string strLanguage)
    {
        mstr_currentLanguage = strLanguage;
        InitializeStrings();
    }

    public bool GetPage(out string str, int nTargetPage)
    {
        if (nTargetPage < 0)
        {
            nTargetPage = 0;
        }
        int nCurrentPage = 0;
        str = "";

        string[] oaStrings = null;

        if(moa_stringDictionary.ContainsKey(mstr_currentLanguage))
            oaStrings = moa_stringDictionary[mstr_currentLanguage];
        else
            oaStrings = new string[]{mo_textAsset.text};

        string strLastString = "";

        //Scroll forward in the string dictionary until you find
        //X delimiters
        for (int i = 0; i < oaStrings.Length; i++)
        {
            if (nCurrentPage == nTargetPage)
            {
                if (oaStrings[i] == mstr_newPageTag)
                    return true;
                str += oaStrings[i];
                strLastString = oaStrings[i];
            }
            if (oaStrings[i] == mstr_newPageTag)
                nCurrentPage++;
           
        }
        if(strLastString.Length == 0)
            return false;
        return true;
    }

    public string GetLine(int nLine)
    {
        return GetLine(mstr_currentLanguage, nLine);
    }

    public string GetLine(string strCategory, int nLine)
    {
        try
        {
            string[] oa = moa_stringDictionary[strCategory];
            return oa[nLine];
        }
        catch
        {
            Debug.LogError("Could not find line " + nLine + " in category " + strCategory);
            return "LINE " + strCategory + ":" + nLine + " NOT FOUND.";
        }
    }

    void StateList(List<string> oList)
    {
        string str = "";
        foreach (string s in oList)
            str += s + " ";
        Debug.Log(str);
    }
    
    public void InitializeStrings()
    {
        if (mo_textAsset == null)
        {
            return;
        }
        moa_stringDictionary.Clear();
        string[] astr = mo_textAsset.text.Split(new char[] {'\t', '\n'});
        List<string> oList = new List<string>();

        string strCurrentLanguage = "";
        bool bFirstElement = true;

        foreach (string str in astr)
        {
            if (str.Length == 0)
                continue;
            //If the first index of the next string is
            //a delimiter for languages
            if (str[0] == '{')
            {
                //If the array count is zero
                //set as the current language and move tothe next string
                if (bFirstElement == true)
                {
                    bFirstElement = false;
                    strCurrentLanguage = str;
                    continue;
                }
                //If the count of array is greater than zero
                //Add the array to the saved arrays
                else if (oList.Count > 0)
                {
                    //StateList(oList);
                    moa_stringDictionary.Add(strCurrentLanguage, oList.ToArray());
                }
                
                oList.Clear();
                strCurrentLanguage = str;
                continue;
            }
            
            oList.Add(str);
        }

        //StateList(oList);
        moa_stringDictionary.Add(strCurrentLanguage, oList.ToArray());
    }
}
