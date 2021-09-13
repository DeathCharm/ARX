using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ARX;
using FEN;

public class RPGStage_Test : MonoBehaviour
{
    public string mstr_testedAnchorName = "topleft";
    public Text mo_text;

    private void Start()
    {
        if (mo_text != null)
            mo_text.text = "Press SpaceBar to begin test";
    }

    // Update is called once per frame
    void Update()
    {
        mstr_testedAnchorName = "topleft";
        
        if(mo_text == null)
        { 
            Debug.LogError("Text script for RPGStage is null. Aborting Test.");
            return;
        }
        

        if (Input.GetKeyDown(KeyCode.Space))
            RunRPGStageTest();
    }


    void RunRPGStageTest()
    {
        mo_text.text = "Beginning RPG Stage Placement Test\n";

        //Load Canvas Object
        GameObject oCanvasObject = null;

        try
        {
            oCanvasObject = Instantiate(Resources.Load("gun")) as GameObject;
            mo_text.text += "\nGun Icon loaded";
        }
        //If the CanvasObject could not be found or loaded
        catch
        {
            mo_text.text += "\nERROR - Canvas Object could not be found or loaded. Make sure a prefab gameobject named gun exists in the resources folder";
            mo_text.text += "\nTest Aborted";
            return;
        }

        //If the Canvas object does not contain an Image
        if(oCanvasObject.GetComponentInChildren<Image>() == null && oCanvasObject.GetComponent<Image>() == null)
        {
            mo_text.text += "\nERROR - Canvas Object does not contain an Image component in itself or any of its children.";
            mo_text.text += "\nCanvas object will not be visible in the UI Canvas.";
        }

        //If the stage anchor named "topleft" does not exist
        ARX_Script_RPGStage.StageAnchor anchor = ARX_Script_RPGStage.GetStageAnchor(mstr_testedAnchorName);
        if(anchor == null)
        {
            mo_text.text += "\nERROR - RPG Stage does not contain an anchor point named " + mstr_testedAnchorName + ". Add an anchor named " + mstr_testedAnchorName + " to the stage.";
            mo_text.text += "\nTest Aborted";
            return;
        }
        mo_text.text += "\nAnchor position " + mstr_testedAnchorName + " retrieved from RPGStage";



        //Add a primitive cube to the stage and the loaded Canvas object to the Canvas
        GameObject oNewCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        oNewCube.transform.localScale = Vector3.one;
        oNewCube.transform.position = new Vector3(1000, 1000, 1000);


        anchor.AddToUICanvas(oCanvasObject.transform);
        mo_text.text += "\nCanvas Icon placed onto canvas";
        anchor.AddToStage(oNewCube.transform);
        mo_text.text += "\nCube placed onto stage";
        
        mo_text.text += "\nTest Successful. There should be an image of a Gun icon and a primitive cube on the screen in the same position as the " + mstr_testedAnchorName + " anchor";
    }
   
}
