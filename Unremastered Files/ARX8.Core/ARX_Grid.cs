using ARX;
using System.Collections.Generic;
using UnityEngine;


public class ARX_Pathway
{
    ARX_Grid mo_grid;
    ARX_Cell mo_start;
    ARX_Cell mo_end;
    ARX_CellSettings mo_cs;

    bool mb_includeGiven;
    int mn_minSectionLength;
    int mn_maxSectionLength;

    public List<ARX_Cell> moa_pathCells = new List<ARX_Cell>();
    public List<ARX_Cell> moa_hallwayCells = new List<ARX_Cell>();
    public List<ARX_Cell> moa_cornerCells = new List<ARX_Cell>();

    DIRECTION me_vertical;
    DIRECTION me_horizontal;
    DIRECTION me_currentDirection;
    List<DIRECTION> moe_remainingDirections = new List<DIRECTION>();

    ARX_Cell mo_currentCell;

    public int mn_turnChance = 30;

    public List<ARX_Cell> Run(ARX_Cell start, ARX_Cell end, bool includeGiven, int minSectionLength, int maxSectionLength, ARX_Grid oGrid, ARX_CellSettings cs)
    {
        mo_grid = oGrid;
        mo_start = start;
        mo_end = end;
        mo_cs = cs;
        mb_includeGiven = includeGiven;
        mn_minSectionLength = minSectionLength;
        mn_maxSectionLength = maxSectionLength;

        SetDirections();
        RunRevolutions();
        return moa_pathCells;
    }

    /// <summary>
    /// Get the two directions the path will take to reach its conclusion
    /// </summary>
    void SetDirections()
    {
        Vector3Int vecDif = mo_end.Position - mo_start.Position;
        Vector2 vecHorizontal = new Vector2(vecDif.x, 0);
        Vector2 vecVertical = new Vector2(0, vecDif.z);

        me_vertical = ToolBox.GetDirection(vecVertical);
        me_horizontal = ToolBox.GetDirection(vecHorizontal);
        moe_remainingDirections.Add(me_vertical);
        moe_remainingDirections.Add(me_horizontal);
        mo_currentCell = mo_start;
    }

    /// <summary>
    /// Remove Direction if at destination's position. Returns false if the path is completed.
    /// </summary>
    /// <returns></returns>
    void RemoveDirections()
    {
        if (mo_currentCell.Position.x == mo_end.Position.x && moe_remainingDirections.Count > 1)
        {
            moe_remainingDirections.Remove(me_horizontal);
            ChangeDirection(me_currentDirection);
        }
        if (mo_currentCell.Position.z == mo_end.Position.z && moe_remainingDirections.Count > 1)
        {
            moe_remainingDirections.Remove(me_vertical);
            ChangeDirection(me_currentDirection);
        }

    }

    void RunRevolutions()
    {
        int nMovementsInCurrentSection = 0;
        int nRevolutions = 0;

        RemoveDirections();
        me_currentDirection = ToolBox.GetRandom<DIRECTION>(moe_remainingDirections);

        //Include the start cell in the list of returned cells if required
        if (mb_includeGiven)
        {
            moa_pathCells.Add(mo_currentCell);
            moa_hallwayCells.Add(mo_currentCell);
        }

        while (nRevolutions < 500)
        {
            RemoveDirections();
            //If the end has been reached, exit
            if (mo_currentCell == mo_end)
                break;

            #region Debug Revolutions just in case function fails

            nRevolutions++;
            if (nRevolutions == 500)
            {
                Debug.LogError("Failed Make Path function.");
                return;
            }
            #endregion

            //If the max section is reached, change direction if possible. Else, continue in same direction
            if (nMovementsInCurrentSection >= mn_maxSectionLength && moe_remainingDirections.Count > 1)
            {
                //Change Direction
                me_currentDirection = ChangeDirection(me_currentDirection);
                nMovementsInCurrentSection = 0;
            }

            //If the min section is not yet reached, continue in the given direction
            if (nMovementsInCurrentSection < mn_minSectionLength)
            {
                //Continue in same direction
                if (!MoveInCurrentnDirection())
                    break;

                //Increment movements
                nMovementsInCurrentSection++;
                //Continue
                continue;
            }

            //If the min section is reached, have a chance to change direction
            if (nMovementsInCurrentSection >= mn_minSectionLength)
            {
                int nRandom = UnityEngine.Random.Range(0, 100);
                if (nRandom < mn_turnChance)
                {
                    me_currentDirection = ChangeDirection(me_currentDirection);
                    nMovementsInCurrentSection = 0;
                }

                //Continue in same direction
                if (!MoveInCurrentnDirection())
                    break;

                //Increment movements
                nMovementsInCurrentSection++;
                //Continue
                continue;
            }
        }

        //Include the end cell in the list of returned cells if required
        if (mb_includeGiven)
        {
            moa_pathCells.Add(mo_currentCell);
            moa_hallwayCells.Add(mo_currentCell);
        }
    }

    bool MoveInCurrentnDirection()
    {
        mo_currentCell = GetNextCell(mo_currentCell, me_currentDirection);

        if (mo_currentCell == null)
            return false;

        if (mo_currentCell == mo_end && mb_includeGiven)
        {
            mo_currentCell.Set(mo_cs);

        }
        else if (mo_currentCell != mo_end)
        {
            mo_currentCell.Set(mo_cs);
        }

        moa_pathCells.Add(mo_currentCell);
        moa_hallwayCells.Add(mo_currentCell);
        return true;
    }

    DIRECTION ChangeDirection(DIRECTION eCurrent)
    {
        if (moe_remainingDirections.Count <= 1)
        {
            me_currentDirection = moe_remainingDirections[0];
            return me_currentDirection;
        }

        foreach (DIRECTION dir in moe_remainingDirections)
            if (dir != eCurrent)
            {
                me_currentDirection = dir;
                moa_cornerCells.Add(mo_currentCell);
                return dir;
            }
        return eCurrent;
    }

    ARX_Cell GetNextCell(ARX_Cell oCurrentCell, DIRECTION dir)
    {
        return mo_grid.GetAdjacentCell(oCurrentCell, dir);
    }

    public void SetCornerCells(ARX_CellSettings cs)
    {
        foreach (ARX_Cell c in moa_cornerCells)
            c.Set(cs);
    }

    public void SetCornerCells(ARX_CellSettings cs, CellValidate[] fCellValidates)
    {
        foreach (ARX_Cell cell in moa_cornerCells)
        {
            bool bPassesCellValidation = true;
            foreach (CellValidate validator in fCellValidates)
            {
                if (validator(cell) == false)
                    bPassesCellValidation = false;
            }

            if (bPassesCellValidation)
                cell.Set(cs);
        }
    }

    public void SetHallwayCells(ARX_CellSettings cs)
    {
        foreach (ARX_Cell c in moa_hallwayCells)
            c.Set(cs);
    }

    public void SetHallwayCells(ARX_CellSettings cs, CellValidate[] fCellValidates)
    {
        foreach (ARX_Cell cell in moa_hallwayCells)
        {
            bool bPassesCellValidation = true;
            foreach (CellValidate validator in fCellValidates)
            {
                if (validator(cell) == false)
                    bPassesCellValidation = false;
            }

            if (bPassesCellValidation)
                cell.Set(cs);
        }
    }
}

[ExecuteInEditMode]
[RequireComponent(typeof(Grid))]
public class ARX_Grid : MonoBehaviour
{
    #region Veriables

    bool bDirty = false;

    Grid mo_placementGrid;
    public bool mb_autoSpacing = false;
    public Vector3Int vecRotation;

    public List<ARX_Cell> Cells = new List<ARX_Cell>();
    public Dictionary<int, Color> PathColors = new Dictionary<int, Color>();
    private Dictionary<int, Rect> LevelBounds = new Dictionary<int, Rect>();
    #endregion

    #region Native Unity Functions

    private void Start()
    {
        PathColors[-1] = Color.cyan;
        PathColors[9999] = Color.white;

        mo_placementGrid = GetComponent<Grid>();
        if (mo_placementGrid == null)
        {
            mo_placementGrid = gameObject.AddComponent<Grid>();
        }
    }

    // Update is called once per frame
    void Update()
    {
            SpaceGridItems();
    }
    
    private void FixedUpdate()
    {
        if (bDirty)
        {
            foreach (ARX_Cell c in Cells)
                c.Recolor();
            bDirty = false;
        }
    }
    #endregion

    #region Set

    /// <summary>
    /// Sets the cell with the given cellSettings.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    public ARX_Cell SetCell(ARX_Cell cell, ARX_CellSettings cellSetting)
    {
        cell.Set(cellSetting);
        return cell;
    }

    /// <summary>
    /// Sets the cell at the given position. 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    public ARX_Cell SetCell(Vector3Int position, ARX_CellSettings cellSetting)
    {
        ARX_Cell cell = GetCell(position);
        cell.Set(cellSetting);
        return cell;
    }

    /// <summary>
    /// Sets the cell at the given position translated by the given offset. 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="offset"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    public ARX_Cell SetCell(Vector3Int position, Vector3Int offset, ARX_CellSettings cellSetting)
    {
        return SetCell(position + offset, cellSetting);
    }

    /// <summary>
    /// Sets the cell adjacent to the given position on the side of the given direction 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="offset"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    public ARX_Cell SetCell(Vector3Int position, DIRECTION direction, ARX_CellSettings cellSetting)
    {
        return SetCell(position + ToolBox.GetTranslation(direction), cellSetting);
    }

    /// <summary>
    /// Sets the cell adjacent to the given position on the side of the given direction 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="offset"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    public ARX_Cell SetCell(ARX_Cell position, DIRECTION direction, ARX_CellSettings cellSetting)
    {
        return SetCell(position.Position + ToolBox.GetTranslation(direction), cellSetting);
    }

    /// <summary>
    /// Sets the cell at the given positions.
    /// </summary>
    /// <param name="positions"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    public ARX_Cell[] SetCells(Vector3Int[] positions, ARX_CellSettings cellSetting)
    {
        List<ARX_Cell> oList = new List<ARX_Cell>();
        foreach (Vector3Int vec in positions)
        {
            oList.Add(SetCell(vec, cellSetting));
        }
        return oList.ToArray();
    }

    /// <summary>
    /// Sets all cells to the given cell setting.
    /// </summary>
    /// <param name="positions"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    public void SetCells(ARX_CellSettings cellSetting)
    {
        foreach (ARX_Cell c in Cells)
        {
            c.Set(cellSetting);
        }
    }

    /// <summary>
    /// Sets the cell at the given positions.
    /// </summary>
    /// <param name="positions"></param>
    /// <param name="cells"></param>
    /// <returns></returns>
    public ARX_Cell[] SetCells(Vector3Int[] positions, ARX_CellSettings[] cells)
    {
        List<ARX_Cell> oList = new List<ARX_Cell>();
        for (int i = 0; i < positions.Length; i++)
        {
            if (i < cells.Length)
            {
                oList.Add(SetCell(positions[i], cells[i]));
            }
        }

        return oList.ToArray();
    }

    /// <summary>
    /// Sets the cells of the given path. Returns false if path is not found.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    public ARX_Cell[] SetCells(int path, ARX_CellSettings cellSetting)
    {
        ARX_Cell[] cells = GetAllCellsByPath(path);

        foreach (ARX_Cell cell in cells)
            cell.Set(cellSetting);

        return cells;
    }

    /// <summary>
    /// Sets the cells of the given paths. 
    /// </summary>
    /// <param name="paths"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    public ARX_Cell[] SetCells(int[] paths, ARX_CellSettings cellSetting)
    {
        List<ARX_Cell> oList = new List<ARX_Cell>();
        foreach (int nPath in paths)
        {
            ARX_Cell[] cells = GetAllCellsByPath(nPath);
            foreach (ARX_Cell cell in cells)
            {
                cell.Set(cellSetting);
                oList.Add(cell);
            }
        }

        return oList.ToArray();
    }

    /// <summary>
    /// Sets the cells of the given paths to the given cell settings. 
    /// Path[0] will be set to cellsetting[0], Path[1] to cellsetting[1], etc.
    /// </summary>
    /// <param name="paths"></param>
    /// <param name="cells"></param>
    /// <returns></returns>
    public ARX_Cell[] SetCells(int[] paths, ARX_CellSettings[] cellSettings)
    {
        List<ARX_Cell> oList = new List<ARX_Cell>();
        for (int i = 0; i < paths.Length; i++)
        {
            ARX_Cell[] cells = GetAllCellsByPath(paths[i]);
            if (i < cellSettings.Length)
            {
                foreach (ARX_Cell c in cells)
                {
                    c.Set(cellSettings[i]);
                    oList.Add(c);
                }
            }
        }
        return oList.ToArray();
    }

    /// <summary>
    /// Sets the cell at the given position translated by the given offset. 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="offset"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    public ARX_Cell SetCell(ARX_Cell position, Vector3Int offset, ARX_CellSettings cellSetting)
    {
        return SetCell(position.Position + offset, cellSetting);
    }

    /// <summary>
    /// Sets the cell at the given positions.
    /// </summary>
    /// <param name="positions"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    public ARX_Cell[] SetCells(ARX_Cell[] positions, ARX_CellSettings cellSetting)
    {
        List<ARX_Cell> oList = new List<ARX_Cell>();
        foreach (ARX_Cell c in positions)
        {
            c.Set(cellSetting);
            oList.Add(c);
        }
        return oList.ToArray();
    }

    /// <summary>
    /// Sets the given cells to the given path.
    /// </summary>
    /// <param name="cells"></param>
    /// <param name="path"></param>
    public void SetAsPath(ARX_Cell[] cells, int path)
    {
        foreach (ARX_Cell c in cells)
            c.SetPath(path);
    }

    /// <summary>
    /// Sets the given cell to the given path.
    /// </summary>
    /// <param name="cells"></param>
    /// <param name="path"></param>
    public ARX_Cell SetAsPath(Vector3Int vec, int path)
    {
        ARX_Cell cell = GetCell(vec);
        cell.SetPath(path);
        return cell;
    }

    /// <summary>
    /// Sets the given cells to the given path
    /// </summary>
    /// <param name="positions"></param>
    /// <param name="path"></param>
    public ARX_Cell[] SetAsPath(Vector3Int[] positions, int path)
    {
        List<ARX_Cell> oList = new List<ARX_Cell>();
        foreach (Vector3Int vec in positions)
        {
            oList.Add(SetAsPath(vec, path));
        }
        return oList.ToArray();
    }

    #endregion

    #region Get

    /// <summary>
    /// Returns the Rect bounds of the given floor, which is calculated when the floor is created.
    /// </summary>
    /// <param name="nFloor"></param>
    /// <returns></returns>
    public Rect GetBounds(int nFloor)
    {
        return LevelBounds[nFloor];
    }

    /// <summary>
    /// Returns a random direction.
    /// </summary>
    /// <param name="includeVertical"></param>
    /// <returns></returns>
    public DIRECTION GetRandomDirection(bool includeVertical = false)
    {
        return ToolBox.GetRandomDirection(includeVertical);
    }

    /// <summary>
    /// Returns the cell at the given position. 
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public ARX_Cell GetCell(Vector3Int position)
    {
        foreach (ARX_Cell c in Cells)
            if (c.Position == position)
                return c;
        return null;
    }

    /// <summary>
    /// Returns the cell at the given position plus offset.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public ARX_Cell GetCell(Vector3Int position, Vector3Int offset)
    {
        return GetCell(position + offset);
    }


    /// <summary>
    /// Returns all cells adjacent to the given position if the given CellValidate delegates all resolve true.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="afuncValidateCell"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentCells(Vector3Int position, CellValidate[] afuncValidateCell, bool includeVertical = false)
    {
        ARX_Cell[] aCellArray = GetAdjacentCells(position, includeVertical);
        List<ARX_Cell> oCellList = new List<ARX_Cell>(aCellArray);

        for (int i = 0; i < oCellList.Count; i++)
        {
            ARX_Cell cell = oCellList[i];
            foreach (CellValidate v in afuncValidateCell)
            {
                if (cell == null || v(cell) == false)
                {
                    oCellList.Remove(cell);
                    i--;
                    break;
                }
            }
        }

        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns all cells adjacent to the given positions if the given CellValidate delegates all resolve true.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="afuncValidateCell"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentCells(Vector3Int[] position, CellValidate[] afuncValidateCell, bool includeVertical = false)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (Vector3Int vec in position)
        {
            oCellList.AddRange(GetAdjacentCells(vec, afuncValidateCell, includeVertical));
        }

        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns all cells adjacent to the given position.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="includeVertical"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentCells(Vector3Int position, bool includeVertical = false)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();

        foreach (DIRECTION dir in System.Enum.GetValues(typeof(DIRECTION)))
        {
            if (dir == DIRECTION.NONE)
                continue;
            if (includeVertical == false)
            {
                if (dir == DIRECTION.UP || dir == DIRECTION.DOWN)
                    continue;
            }

            oCellList.Add(GetAdjacentCell(position, dir));
        }

        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns all cells adjacent to the given positions.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="includeVertical"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentCells(Vector3Int[] position, bool includeVertical = false)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (Vector3Int vec in position)
            oCellList.AddRange(GetAdjacentCells(vec, includeVertical));
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns all cells adjacent to the given position if the given CellValidate delegates all resolve true.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="afuncValidateCell"></param>
    /// <param name="includeVertical"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentCells(ARX_Cell position, CellValidate[] afuncValidateCell, bool includeVertical = false)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>(GetAdjacentCells(position, includeVertical));

        for (int i = 0; i < oCellList.Count; i++)
        {
            ARX_Cell cell = oCellList[i];

            foreach (CellValidate v in afuncValidateCell)
            {
                if (v(cell) == false)
                {
                    oCellList.Remove(cell);
                    i--;
                    break;
                }
            }
        }

        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns all cells adjacent to the given positions if the given CellValidate delegates all resolve true.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="afuncValidateCell"></param>
    /// <param name="includeVertical"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentCells(ARX_Cell[] position, CellValidate[] afuncValidateCell, bool includeVertical = false)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        
        foreach (ARX_Cell cell in position)
        {
            oCellList.AddRange(GetAdjacentCells(cell.Position, afuncValidateCell, includeVertical));
        }

        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns all cells adjacent to the given positions if the given CellValidate delegates all resolve true.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="afuncValidateCell"></param>
    /// <param name="includeVertical"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentCells(ARX_Cell[] position, DIRECTION dir)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();

        foreach (ARX_Cell cell in position)
        {
            oCellList.Add(GetAdjacentCell(cell.Position, dir));
        }

        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns all cells adjacent to the given position.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="afuncValidateCell"></param>
    /// <param name="includeVertical"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentCells(ARX_Cell position, bool includeVertical = false)
    {

        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (DIRECTION dir in System.Enum.GetValues(typeof(DIRECTION)))
        {
            if (dir == DIRECTION.NONE)
                continue;
            if (includeVertical == false)
            {
                if (dir == DIRECTION.UP || dir == DIRECTION.DOWN)
                    continue;
            }

            oCellList.Add(GetAdjacentCell(position, dir));

        }
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns all cells adjacent to the given positions.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="afuncValidateCell"></param>
    /// <param name="includeVertical"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentCells(ARX_Cell[] position, bool includeVertical = false)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (ARX_Cell cell in position)
        {
            oCellList.AddRange(GetAdjacentCells(cell, includeVertical));
        }
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns the cells adjacent to the given cells in the given direction if the CellValidates resolve true.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="eDirection"></param>
    /// <param name="afuncValidateCell"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentCell(Vector3Int[] position, DIRECTION eDirection, CellValidate[] afuncValidateCell)
    {

        List<ARX_Cell> oCellList = new List<ARX_Cell>();

        foreach (Vector3Int vec in position)
        {
            ARX_Cell cell = GetAdjacentCell(vec, eDirection);
            bool bAdd = true;
            foreach (CellValidate v in afuncValidateCell)
                if (v(cell) == false)
                    bAdd = false;

            if (bAdd)
                oCellList.Add(cell);
        }

        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns the cells adjacent to the given cells in the given direction if the CellValidates resolve true. 
    /// Given position(0,0) and eDirection{NORTH, EAST, WEST}
    /// this function will return the cells (0, 1)(NORTH), (1, 0)(EAST) and (-1, 0)(WEST) 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="eDirection"></param>
    /// <param name="afuncValidateCell"></param>
    /// <returns></returns>
    public ARX_Cell GetAdjacentCell(Vector3Int[] position, DIRECTION[] eDirection, CellValidate[] afuncValidateCell)
    {

        List<ARX_Cell> oCellList = new List<ARX_Cell>();

        foreach (Vector3Int vec in position)
        {
            foreach (DIRECTION dir in eDirection)
            {
                ARX_Cell cell = GetAdjacentCell(vec, dir);

                bool bAdd = true;
                foreach (CellValidate v in afuncValidateCell)
                    if (v(cell) == false)
                        bAdd = false;

                if (bAdd)
                    oCellList.Add(cell);
            }
        }

        return null;
    }

    /// <summary>
    /// Returns the cell adjacent to the given position in the given direction.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="eDirection"></param>
    /// <returns></returns>
    public ARX_Cell GetAdjacentCell(Vector3Int position, DIRECTION eDirection)
    {
        return GetCell(position + ToolBox.GetTranslation(eDirection));
    }

    /// <summary>
    /// Returns the cells adjacent to the given positions in the given direction.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="eDirection"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentCell(Vector3Int[] positions, DIRECTION eDirection)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (Vector3Int vec in positions)
        {
            oCellList.Add(GetAdjacentCell(vec, eDirection));
        }
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns the cells adjacent to the given positions in the given directions. 
    /// Given position(0,0) and eDirection{NORTH, EAST, WEST}
    /// this function will return the cells (0, 1)(NORTH), (1, 0)(EAST) and (-1, 0)(WEST) 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="eDirection"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentCell(Vector3Int[] positions, DIRECTION[] eDirection)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();

        foreach (DIRECTION dir in eDirection)
        {
            oCellList.AddRange(GetAdjacentCell(positions, dir));
        }

        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns the cells adjacent to the given cells in the given direction if the CellValidates resolve true.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="eDirection"></param>
    /// <param name="afuncValidateCell"></param>
    /// <returns></returns>
    public ARX_Cell GetAdjacentValidCell(ARX_Cell position, DIRECTION eDirection, CellValidate[] afuncValidateCell)
    {
        ARX_Cell cell = GetAdjacentCell(position, eDirection);
        foreach (CellValidate v in afuncValidateCell)
            if (v(cell) == false)
                return null;

        return cell;
    }

    /// <summary>
    /// Returns the cells adjacent to the given cells in the given direction if the CellValidates resolve true.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="eDirection"></param>
    /// <param name="afuncValidateCell"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentValidCells(ARX_Cell[] position, DIRECTION eDirection, CellValidate[] afuncValidateCell)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (ARX_Cell cell in position)
        {
            ARX_Cell oCell = GetAdjacentValidCell(cell, eDirection, afuncValidateCell);
            if (cell == null)
                continue;
            else oCellList.Add(cell);
        }
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns the cells adjacent to the given cells in the given directions if the CellValidates resolve true.
    /// Given position(0,0) and eDirection{NORTH, EAST, WEST}
    /// this function will return the cells (0, 1)(NORTH), (1, 0)(EAST) and (-1, 0)(WEST) 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="eDirection"></param>
    /// <param name="afuncValidateCell"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentValidCells(ARX_Cell[] position, DIRECTION[] eDirection, CellValidate[] afuncValidateCell)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (ARX_Cell cell in position)
        {
            foreach (DIRECTION dir in eDirection)
            {
                oCellList.Add(GetAdjacentValidCell(cell, dir, afuncValidateCell));
            }
        }
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns the cell adjacent to the given position in the given direction.   
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="eDirection"></param>
    /// <returns></returns>
    public ARX_Cell GetAdjacentCell(ARX_Cell cell, DIRECTION eDirection)
    {
        return GetCell(cell.Position + ToolBox.GetTranslation(eDirection));
    }

    /// <summary>
    /// Returns the cells adjacent to the given positions in the given direction.   
    /// </summary>
    /// <param name="position"></param>
    /// <param name="eDirection"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentCell(ARX_Cell[] positions, DIRECTION eDirection)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (ARX_Cell cell in positions)
            oCellList.Add(GetAdjacentCell(cell, eDirection));
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns the cells adjacent to the given position in the given directions.   
    /// Given position(0,0) and eDirection{NORTH, EAST, WEST}
    /// this function will return the cells (0, 1)(NORTH), (1, 0)(EAST) and (-1, 0)(WEST) 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="eDirection"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAdjacentCell(ARX_Cell[] positions, DIRECTION[] eDirection)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (ARX_Cell cell in positions)
            foreach (DIRECTION dir in eDirection)
                oCellList.Add(GetAdjacentCell(cell, dir));
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns the cells at the given positions.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public ARX_Cell[] GetCells(Vector3Int[] position)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (Vector3Int vec in position)
            oCellList.Add(GetCell(vec));
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns cells at the given positions that resolve true with the given CellValidates.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="afuncValidateCell"></param>
    /// <returns></returns>
    public ARX_Cell[] GetCells(Vector3Int[] position, CellValidate[] afuncValidateCell)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>(GetCells(position));
        for (int i = 0; i < oCellList.Count; i++)
        {
            ARX_Cell cell = oCellList[i];
            foreach (CellValidate v in afuncValidateCell)
                if (v(cell) == false)
                {
                    oCellList.Remove(cell);
                    i--;
                    break;
                }
        }
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns all cells on the given path.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAllCellsByPath(int path)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (ARX_Cell c in Cells)
            if (c.Path == path)
                oCellList.Add(c);
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns all cells on the given paths.
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAllCellsByPath(int[] paths)
    {

        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (int n in paths)
        {
            oCellList.AddRange(GetAllCellsByPath(n));
        }
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns arrays of cells on the given paths.
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    public ARX_Cell[][] GetCellArrayByPath(int[] paths)
    {
        List<ARX_Cell[]> oCellArrayList = new List<ARX_Cell[]>();

        foreach (int n in paths)
            oCellArrayList.Add(GetAllCellsByPath(n));

        return oCellArrayList.ToArray();
    }

    /// <summary>
    /// Returns all cells on the given path that resolve true with the given CellValidates.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="afuncValidateCell"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAllCellsByPath(int path, CellValidate[] afuncValidateCell)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>(GetAllCellsByPath(path));

        for (int i = 0; i < oCellList.Count; i++)
        {
            ARX_Cell oCell = oCellList[i];
            foreach (CellValidate v in afuncValidateCell)
                if (v(oCell) == false)
                {
                    oCellList.Remove(oCell);
                    i--;
                    break;
                }
        }

        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns all cells on the given paths that resolve true with the given CellValidates.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="afuncValidateCell"></param>
    /// <returns></returns>
    public ARX_Cell[] GetAllCellsByPath(int[] paths, CellValidate[] afuncValidateCell)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (int n in paths)
            oCellList.AddRange(GetAllCellsByPath(n, afuncValidateCell));
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns arrays of cells on the given paths.
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    public ARX_Cell[][] GetCellArrayByPath(int[] paths, CellValidate[] afuncValidateCell)
    {
        List<ARX_Cell[]> oCellArrayList = new List<ARX_Cell[]>();

        foreach (int n in paths)
        {
            oCellArrayList.Add(GetAllCellsByPath(n, afuncValidateCell));
        }

        return oCellArrayList.ToArray();
    }

    /// <summary>
    /// Returns calls with the given attribute
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public ARX_Cell[] GetCellsByAttribute(string attribute)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (ARX_Cell c in Cells)
            if (c.HasAttribute(attribute) == true)
                oCellList.Add(c);
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns calls with the given attributes
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public ARX_Cell[] GetCellsByAttribute(string[] attribute)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (string str in attribute)
            oCellList.AddRange(GetCellsByAttribute(str));

        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns calls with the given attribute that resolve true with the given CellValidates
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public ARX_Cell[] GetCellsByAttribute(string attribute, CellValidate[] afuncValidateCell)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>(GetCellsByAttribute(attribute));
        for (int i = 0; i < oCellList.Count; i++)
        {
            ARX_Cell cell = oCellList[i];

            foreach (CellValidate v in afuncValidateCell)
                if (v(cell) == false)
                {
                    oCellList.Remove(cell);
                    i--;
                    break;
                }
        }
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns calls with the given attributes that resolve true with the given CellValidates
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public ARX_Cell[] GetCellsByAttribute(string[] attribute, CellValidate[] afuncValidateCell)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>();
        foreach (string str in attribute)
            oCellList.AddRange(GetCellsByAttribute(str, afuncValidateCell));
        return oCellList.ToArray();
    }


    /// <summary>
    /// Returns calls with the given attribute.
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public ARX_Cell[] GetCellsByAttribute(ARX_Cell[] cells, string attribute)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>(cells);
        for (int i = 0; i < oCellList.Count; i++)
        {
            ARX_Cell cell = oCellList[i];
            if (cell.HasAttribute(attribute) == false)
            {
                oCellList.Remove(cell);
                i--;
                continue;
            }
        }
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns calls with the given attributes.
    /// </summary>
    /// <param name="attributes"></param>
    /// <returns></returns>
    public ARX_Cell[] GetCellsByAttribute(ARX_Cell[] cells, string[] attributes)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>(cells);
        for (int i = 0; i < oCellList.Count; i++)
        {
            ARX_Cell cell = oCellList[i];
            foreach(string attribute in attributes)
                if (cell.HasAttribute(attribute) == false)
                {
                    oCellList.Remove(cell);
                    i--;
                    break;
                }
        }
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns calls with the given attribute that resolve true with the given CellValidates
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public ARX_Cell[] GetCellsByAttribute(ARX_Cell[] cells, string attribute, CellValidate[] afuncValidateCell)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>(cells);
        for (int i = 0; i < oCellList.Count; i++)
        {
            ARX_Cell cell = oCellList[i];
            if (cell.HasAttribute(attribute) == false)
            {
                oCellList.Remove(cell);
                i--;
                continue;
            }

            foreach (CellValidate v in afuncValidateCell)
            {
                if (v(cell) == false)
                {
                    oCellList.Remove(cell);
                    i--;
                    break;
                }
            }
                
        }
        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns calls with the given attributes that resolve true with the given CellValidates
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public ARX_Cell[] GetCellsByAttribute(ARX_Cell[] cells, string[] attributes, CellValidate[] afuncValidateCell)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>(cells);
        for (int i = 0; i < oCellList.Count; i++)
        {
            ARX_Cell cell = oCellList[i];
            if (cell.HasAttributes(attributes) == false)
            {
                oCellList.Remove(cell);
                i--;
                continue;
            }

            foreach (CellValidate v in afuncValidateCell)
            {
                if (v(cell) == false)
                {
                    oCellList.Remove(cell);
                    i--;
                    break;
                }
            }
        }
        return oCellList.ToArray();
    }


    /// <summary>
    /// Returns calls with the given attribute.
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public ARX_Cell[] GetCellsByAttribute(Vector3Int[] cells, string attribute)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>(GetCells(cells));
        for (int i = 0; i < oCellList.Count; i++)
        {
            ARX_Cell cell = oCellList[i];
            if (cell.HasAttribute(attribute) == false)
            {

                oCellList.Remove(cell);
                i--;
                continue;
            }
        }

        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns calls with the given attributes.
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public ARX_Cell[] GetCellsByAttribute(Vector3Int[] cells, string[] attributes)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>(GetCells(cells));
        for (int i = 0; i < oCellList.Count; i++)
        {
            ARX_Cell cell = oCellList[i];
            foreach(string str in attributes)
            if (cell.HasAttribute(str) == false)
            {
                oCellList.Remove(cell);
                i--;
                break;
            }
        }

        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns calls with the given attribute that resolve true with the given CellValidates
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public ARX_Cell[] GetCellsByAttribute(Vector3Int[] cells, string attribute, CellValidate[] afuncValidateCell)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>(GetCellsByAttribute(cells, attribute));
        for (int i = 0; i < oCellList.Count; i++)
        {
            ARX_Cell oCell = oCellList[i];
            foreach (CellValidate c in afuncValidateCell)
                if (c(oCell) == false)
                {
                    oCellList.Remove(oCell);
                    i--;
                    break;
                }
        }

        return oCellList.ToArray();
    }

    /// <summary>
    /// Returns calls with the given attributes that resolve true with the given CellValidates
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public ARX_Cell[] GetCellsByAttribute(Vector3Int[] cells, string[] attribute, CellValidate[] afuncValidateCell)
    {
        List<ARX_Cell> oCellList = new List<ARX_Cell>(GetCellsByAttribute(cells, attribute));
        for (int i = 0; i < oCellList.Count; i++)
        {
            ARX_Cell oCell = oCellList[i];
            foreach (CellValidate c in afuncValidateCell)
                if (c(oCell) == false)
                {
                    oCellList.Remove(oCell);
                    i--;
                    break;
                }
        }

        return oCellList.ToArray();
    }

    #endregion

    #region Direction

    public DIRECTION GetDirection(ARX_Cell start, ARX_Cell end)
    {
        Vector3Int vecStart = start.Position;
        Vector3Int vecEnd = end.Position;
        return GetDirection(vecStart, vecEnd);
    }

    public DIRECTION GetDirection(Vector3Int start, Vector3Int end)
    {
        Vector3Int dif = end - start;

        if (dif.y > 0)
            return DIRECTION.UP;
        else if (dif.y < 0)
            return DIRECTION.DOWN;

        Vector2 vec = new Vector2(dif.x, dif.z);
        return ToolBox.GetDirection(vec);
    }
    #endregion

    #region Is

    /// <summary>
    /// Returns true if the given point lies within the current floor's bounds.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool IsInsideBounds(Vector3 point, int nCurrentFloor)
    {
        Vector2 buf = new Vector2(point.x, point.z);
        Rect bounds = GetLevelBounds(nCurrentFloor);

        bool bReturn = bounds.Contains(buf);
        //Debug.Log("It is " + bReturn + " that point " + buf.ToString() + " is within floor " + nCurrentFloor + " bound " + bounds.min.ToString() + " and " + bounds.max.ToString());
        return bReturn;
    }


    /// <summary>
    /// Returns true if the two positions are adjacent.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="position2"></param>
    /// <returns></returns>
    public bool IsAdjacent(Vector3Int position, Vector3Int position2) { return Vector3Int.Distance(position, position2) == 1; }

    /// <summary>
    /// Returns true if the two positions are adjacent.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="position2"></param>
    /// <returns></returns>
    public bool IsAdjacent(ARX_Cell position, Vector3Int position2) { return Vector3Int.Distance(position.Position, position2) == 1; }

    /// <summary>
    /// Returns true if the two positions are adjacent.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="position2"></param>
    /// <returns></returns>
    public bool IsAdjacent(Vector3Int position, ARX_Cell position2) { return Vector3Int.Distance(position, position2.Position) == 1; }
    #endregion

    #region Shape Making

    public ARX_Cell[] MakeBlob(ARX_Cell oStart, int nNumberOfRooms, ARX_CellSettings cs, CellValidate[] fCellValidates = null, bool includeVertical = false)
    {
        return MakeBlob(new ARX_Cell[] { oStart }, nNumberOfRooms, cs, fCellValidates, includeVertical);
    }

    public ARX_Cell[] MakeBlob(ARX_Cell[] oStart, int nNumberOfRooms, ARX_CellSettings cs, CellValidate[] fCellValidates = null, bool includeVertical = false)
    {
        if (oStart == null || oStart.Length == 0)
            return new ARX_Cell[] { };

        if (fCellValidates == null)
            fCellValidates = new CellValidate[] { IsEmptyCell};
        List<CellValidate> oValidateList = new List<CellValidate>(fCellValidates);

        List<ARX_Cell> oListOfExpandedCells = new List<ARX_Cell>(oStart);

        CellValidate IsNotAlreadyIncluded = new CellValidate(
            delegate(ARX_Cell c)
            {
                return oListOfExpandedCells.Contains(c) == false;
            }
            );
        oValidateList.Add(IsNotAlreadyIncluded);


        while(oListOfExpandedCells.Count < nNumberOfRooms)
        //for (int i = 0; i < nNumberOfRooms; i++)
        {
            ARX_Cell[] oExpandable = GetAdjacentCells(oListOfExpandedCells.ToArray(), oValidateList.ToArray(), includeVertical);
            if (oExpandable.Length == 0)
                break;
            
            ARX_Cell oRandomCell = GetRandomCell(oExpandable);
            if (oRandomCell == null)
            {
                Debug.LogError("Cell found was null. Breaking.");
                break;
            }
            oRandomCell.Set(cs);
            oListOfExpandedCells.Add(oRandomCell);
        }


        return oListOfExpandedCells.ToArray();
    }
    
    public ARX_Pathway MakePathObject(ARX_Cell oStart, ARX_Cell oEnd, bool bIncludeGiven, int nMinSectionLength, int nMaxSectionLength, ARX_CellSettings cs)
    {
        ARX_Pathway pathHelper = new ARX_Pathway();
        pathHelper.Run(oStart, oEnd, bIncludeGiven, nMinSectionLength, nMaxSectionLength, this, cs);
        return pathHelper;
    }

    public ARX_Pathway MakePathObject(Vector3Int vecStart, Vector3Int vecEnd, bool bIncludeGiven, int nMinSectionLength, int nMaxSectionLength, ARX_CellSettings cs)
    {
        ARX_Cell oStart = GetCell(vecStart);
        ARX_Cell oEnd = GetCell(vecEnd);
        return MakePathObject(oStart, oEnd, bIncludeGiven, nMinSectionLength, nMaxSectionLength, cs);
    }

    public ARX_Cell[] MakePath(Vector3Int vecStart, Vector3Int vecEnd, bool bIncludeGiven, int nMinSectionLength, int nMaxSectionLength, ARX_CellSettings cs)
    {
        return MakePathObject(vecStart, vecEnd, bIncludeGiven, nMinSectionLength, nMaxSectionLength, cs).moa_pathCells.ToArray();
    }

    public ARX_Cell[] MakePath(ARX_Cell oStart, ARX_Cell oEnd, bool bIncludeGiven, int nMinSectionLength, int nMaxSectionLength, ARX_CellSettings cs)
    {
        return MakePathObject(oStart, oEnd, bIncludeGiven, nMinSectionLength, nMaxSectionLength, cs).moa_pathCells.ToArray();
    }

    public ARX_Cell[] MakeLine(ARX_Cell oStart, DIRECTION dir, int nNumberOfRooms, ARX_CellSettings cs)
    {
        List<ARX_Cell> oList = new List<ARX_Cell>();

        ARX_Cell oLastCell = oStart;
        for (int i = 0; i < nNumberOfRooms; i++)
        {
            ARX_Cell cell = SetCell(oLastCell, dir, cs);
            oList.Add(cell);
            oLastCell = cell;
        }

        return oList.ToArray();
    }
    
    public class SimpleRect
    {
        public Vector3Int min, max;

        public int width
        {
            get
            {
                return Mathf.Abs(max.x - min.x);
            }
        }

        public int height
        {
            get
            {
                return Mathf.Abs(max.y - min.y);
            }
        }

        public int length
        {
            get
            {
                return Mathf.Abs(max.z - min.z);
            }
        }

        public SimpleRect(Vector3Int one, Vector3Int two)
        {
            if (one.x < two.x)
            {
                min.x = one.x;
                max.x = two.x;
            }
            else
            {
                min.x = two.x;
                max.x = one.x;
            }

            if (one.y < two.y)
            {
                min.y = one.y;
                max.y = two.y;
            }
            else
            {
                min.y = two.y;
                max.y = one.y;
            }

            if (one.z < two.z)
            {
                min.z = one.z;
                max.z = two.z;
            }
            else
            {
                min.z = two.z;
                max.z = one.z;
            }
        }
    }
    
    public ARX_Cell[] MakeSquare(ARX_Cell one, ARX_Cell two, bool bIncludeGiven, ARX_CellSettings cs, int nMinWidth = 2, int nMinLength = 2)
    {
        Vector3Int min = one.Position;
        Vector3Int max = two.Position;

        List<ARX_Cell> oList = new List<ARX_Cell>();
        SimpleRect simpleRect = new SimpleRect(min, max);

        min = simpleRect.min;
        max = simpleRect.max;

        int nWidth = simpleRect.width;
        int nLength = simpleRect.length;

        if (nWidth < nMinWidth)
            nWidth = nMinWidth;
        if (nLength < nMinLength)
            nLength = nMinLength;

        for (int z = 0; z < nLength; z++)
            for (int x = 0; x < nWidth; x++)
            {
                Vector3Int newVec = min + new Vector3Int(x, 0, z);
                ARX_Cell c = GetCell(newVec);
                if (c == null)
                    continue;

                if (x == 0 && z == 0 && bIncludeGiven == false)
                    continue;

                if (x == nWidth - 1 && z == nLength - 1 && bIncludeGiven == false)
                    continue;

                c.Set(cs);
                oList.Add(c);
            }


        return oList.ToArray();
    }

    public ARX_Cell[] MakeSquare(Vector3Int min, Vector3Int max, bool bIncludeGiven, ARX_CellSettings cs, int nMinWidth = 2, int nMinLength = 2)
    {
        ARX_Cell one = GetCell(min);
        ARX_Cell two = GetCell(max);
        return MakeSquare(one, two, bIncludeGiven, cs, nMinWidth, nMinLength);
    }
    
    //public ARX_Cell[][] Subdivide(ARX_Cell[] oaCells, int nSections)
    //{
    //    return null;
    //}
    //public ARX_Cell[] RandomExpand() { return null; }
    //public ARX_Cell[] RandomShrink() { return null; }

    public void MakeGrid(int nWidth, int nLength, int nHeight, string strCellPrefab)
    {
        DestroyCells();

        if (nHeight < 1)
            nHeight = 1;

        int wStart = -nWidth / 2;
        int lStart = -nLength / 2;

        int wEnd = nWidth / 2;
        int lEnd = nLength / 2;

        for (int w = wStart; w < wEnd; w++)
            for (int l = lStart; l < lEnd; l++)
                for (int h = 0; h < nHeight; h++)
                {
                    Vector3Int vecPosition = new Vector3Int(w, h, l);
                    ARX_Cell c = AddCell(vecPosition, strCellPrefab);
                    c.Recolor();
                }
    }

    public ARX_Cell AddCell(Vector3Int vecPosition, string strCellPrefab)
    {
        ARX_Cell cell = GetCell(vecPosition);
        if (cell != null)
            return cell;

        ARX_Cell c = new ARX_Cell(this, vecPosition, strCellPrefab);
        Cells.Add(c);
        bDirty = true;
        return c;
    }

    public Color GetPathColor(int nPath)
    {
        return AddNewPathColor(nPath);
    }

    void DestroyCells()
    {
        foreach (ARX_Cell c in Cells)
            c.Destroy();
        Cells.Clear();
    }

    Color AddNewPathColor(int nPath)
    {
        if (PathColors.ContainsKey(nPath) == false)
        {
            PathColors[nPath] = ToolBox.GetRandomColorOverOne();
        }

        return PathColors[nPath];
    }


    #endregion

    #region Helper

    public void Destroy()
    {
        foreach (ARX_Cell c in Cells)
            c.Destroy();
        Cells.Clear();
        ToolBox.DeleteChildren(transform);
    }
    
    void SpaceGridItems()
    {
        foreach (ARX_Cell cell in Cells)
        {
            cell.gameObject.transform.localPosition = mo_placementGrid.CellToLocal(cell.Position);
        }
    }

    public ARX_Cell GetRandomCell(ARX_Cell[] oArray)
    {
        if (oArray.Length == 0)
            return null;
        int nRandom = UnityEngine.Random.Range(0, oArray.Length);
        return oArray[nRandom];
    }

    #endregion

    #region Cell Validate

    public static CellValidate IsEmptyCell = new CellValidate(
           delegate (ARX_Cell cell)
           {
               return cell.HasAttribute(empty);
           }
           )
        ;

    #endregion

    #region Walls

    /// <summary>
    /// Opens or closes Cell Script's walls based on if they are flanked 
    /// or not.
    /// </summary>
    public void SetCellWalls()
    {
        foreach (ARX_Cell cell in Cells)
        {
            if (cell.HasAttribute(empty) || cell.HasAttribute(nullpanel))
            {
                cell.CellScript.gameObject.SetActive(false);
                continue;
            }

            cell.CellScript.SetAllToWalled();

            bool bAtLeastOneFlank = false;
            foreach (DIRECTION dir in Defines.CardinalDirections)
            {
                ARX_Cell oFlankingCell = GetAdjacentCell(cell, dir);

                //If the cell is flanked at direction dir AND
                //the cell settings allow 
                if (oFlankingCell != null && cell.mo_cellSettings.mb_enclosePathWalls == true &&
                    cell.Path == oFlankingCell.Path)
                {
                    cell.CellScript.SetFlankingGameObject(dir, ARX_Script_Cell.FLANKSTATUS.FLANKED);
                    bAtLeastOneFlank = true;
                }

            }
            
            if (!bAtLeastOneFlank && cell.CellScript.mo_none != null)
                cell.CellScript.mo_none.SetActive(true);
        }
    }

    #endregion
    
    #region Const

    public const string
    empty = "empty", nullpanel = "nullpanel";

    #endregion

    #region Bounds

    public Rect GetLevelBounds(int nFloor)
    {
        if (LevelBounds.ContainsKey(nFloor) == false)
            LevelBounds[nFloor] = new Rect();
        return LevelBounds[nFloor];
    }

    /// <summary>
    /// Widens the bounds of the floor that is saved in the LevelBounds Dictionary
    /// based on the cell's position.
    /// </summary>
    /// <param name="cell"></param>
    public void AddToBounds(ARX_Cell cell)
    {
        int nFloor = cell.Position.y;
        
        Rect rect = GetLevelBounds(cell.Position.y);

        Vector2 min = rect.min;
        Vector2 max = rect.max;

        if (cell.Position.x < min.x)
        {
            min.x = cell.Position.x;
        }

        if (cell.Position.z < min.y)
        {
            min.y = cell.Position.z;
        }

        if (cell.Position.x > max.x)
        {
            max.x = cell.Position.x;
        }

        if (cell.Position.z > max.y)
        {
            max.y = cell.Position.z;
        }

        rect.min = new Vector2(min.x, min.y);
        rect.max = new Vector2(max.x, max.y);

        LevelBounds[nFloor] = rect;
    }
    #endregion
}
