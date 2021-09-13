using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ARX;

/// <summary>
/// Scratch paper script used for quick testing and debugging.
/// </summary>
[ExecuteInEditMode]
public class ARX6_Scratch : MonoBehaviour
{
    public enum MODE {SWITCH, QUEST, SPLITS };
    public MODE me_mode = MODE.SWITCH;

    public bool mb_init = false;

    public string mstr_prefix = "";

    [TextArea(5, 10)]
    public string mstr_input;

    [TextArea(5, 10)]
    public string mstr_output;
    
    private void Init()
    {
        string strbuf = mstr_input.Replace("\n", "");
        strbuf = strbuf.Replace(" ", "");
        string[] strSplits = strbuf.Split(',');

        mstr_output = "";

        switch (me_mode)
        {
            case MODE.SPLITS:
                foreach (string str in strSplits)
                {
                    mstr_output += str + "\n";
                }
                break;
            case MODE.SWITCH:
                mstr_output = "switch(me_id)\n{\n";

                foreach (string str in strSplits)
                {
                    string buf = str;
                    buf = buf.Replace("\n", "");
                    mstr_output += "case QUESTID." + buf + ":\n";
                    mstr_output += "return new " + mstr_prefix + buf + "();\n";
                }
                mstr_output += "}";
                break;
            case MODE.QUEST:
                foreach (string str in strSplits)
                {
                    string buf = str;
                    buf = buf.Replace("\n", "");
                    string strClassname = mstr_prefix + buf;

                    mstr_output += "public class " + strClassname+ " : ARX_Quest {\n";
                    mstr_output += "public " + strClassname + "():base(\"\"){}}\n";

                }
                break;
        }
    }
    
    private void Update()
    {

        if (mb_init)
        {
            Init();
            mb_init = false;
        }
    }

}
