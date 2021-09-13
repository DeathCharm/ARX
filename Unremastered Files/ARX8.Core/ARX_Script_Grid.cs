using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;

public class ARX_Script_Grid : ARX_Script_Actor
{
    public static ARX_Script_Grid Instance;
    public bool mb_createOnStart = false;
    public ARX_Grid mo_grid;

    public override void V_OnInitialize()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (mo_grid == null)
            mo_grid = GetComponent<ARX_Grid>();
    }

    public override bool V_OnUpdate()
    {
        if(mb_createOnStart /*|| Input.GetKeyDown(KeyCode.Space)*/)
        {
            mb_createOnStart = false;
            mo_grid.Destroy();

            //ASMO_Level oFirstFloor = new ASMO_Level_EntranceFloor(mo_grid);
            //ASMO_Level oSecondFloor = new ASMO_Level_SecondFloor(mo_grid);
            //ASMO_Level oParking = new ASMO_Level_Parking(mo_grid);

            //oFirstFloor.RunLevelCreation();
            //oParking.RunLevelCreation();
            //oSecondFloor.RunLevelCreation();
        }

        return true;
    }

    
    void CreateThirdFloor()
    {
        //Get the cells from the first floor and bloom them to serve at the second floor

        //From the two Stairwells from the first floor, draw a Hallway path

        //Get a random hallway tile and draw a rectangle from it. If the rectangle is not fully draw, reverse and try again.
        //Do this until a revolution max is reached or there are 6 rectangles drawn. Set each rectangle to a different path.

        //For each corner in the hallway except the last, set a soft challenge

        //For the final corner in the hallway, set a hard challenge

        //Run the populate function

    }
    
}
