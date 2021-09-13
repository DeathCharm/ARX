using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ARX;


namespace ARX
{
    /// <summary>
    /// Base class for Editor-based drag-and-drop functionality on a grid.
    /// </summary>
    /// <typeparam name="DragItem"></typeparam>
    /// <typeparam name="DropZone"></typeparam>
    public abstract class ARX_EditorGrid<DragItem, DropZone> : ARX_DragAndDrop<DragItem, DropZone> where DragItem : DnDObject
    {
        #region Variables

        private int mn_rows = 3;
        private int mn_columns = 3;
        public int Rows { get { return mn_rows; } }
        public int Columns { get { return mn_columns; } }

        public float mnf_nodeWidth = 50, mnf_nodeHeight = 50, mnf_xPadding = 3, mnf_yPadding = 3;

        /// <summary>
        /// The dimensions of the grid and its location on the EditorWindow
        /// </summary>
        protected Rect mo_gridRect;
        #endregion

        #region Constructor and Initializers

        public ARX_EditorGrid(int rows, int columns, float nodeWidth, float nodeHeight, Rect windowRect)
        {
            mo_gridRect = windowRect;
            InitializeValues(rows, columns, nodeWidth, nodeHeight);
            InitializeGridItems(rows, columns);
        }

        /// <summary>
        /// Helper funcion to initialize helper variables
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="nodeWidth"></param>
        /// <param name="nodeHeight"></param>
        void InitializeValues(int rows, int columns, float nodeWidth, float nodeHeight)
        {
            mn_rows = rows;
            mn_columns = columns;
            mnf_nodeHeight = nodeHeight;
            mnf_nodeWidth = nodeWidth;
        }

        /// <summary>
        /// Creates the grid of default node items and dropzones
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        void InitializeGridItems(int rows, int columns)
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    int i = x * y;
                    Rect rect = GetNodeRect(x, y);

                    moa_dragItems.Add(I_GetNewDragItem(rect));
                    moa_dropZones.Add(I_GetNewDropZone(rect));
                }
            }

        }

        #endregion

        #region Abstracts

        public abstract DragItem I_GetNewDragItem(Rect rect);
        public abstract DropZone I_GetNewDropZone(Rect rect);

        public abstract void I_DrawItem(DragItem item, Rect rect);
        public abstract void I_DrawBackground(Rect rect);

        #endregion

        #region Helpers

        /// <summary>
        /// Returns the dimensions and position of the node at position (x,y)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        Rect GetNodeRect(int x, int y)
        {
            float xPosition = (x * mnf_xPadding) + (x * mnf_nodeWidth);
            float yPosition = (y + mnf_yPadding) + (y * mnf_nodeHeight);

            xPosition += mo_gridRect.x;
            yPosition += mo_gridRect.y;

            Rect rect = new Rect(xPosition, yPosition, mnf_nodeWidth, mnf_nodeHeight);
            return rect;
        }


        public DragItem GetItem(int x, int y)
        {
            int n = x * y;
            if (n >= moa_dragItems.Count)
                return default(DragItem);
            return moa_dragItems[n];
        }

        #endregion

        #region Draw


        /// <summary>
        /// Draws the Grid and its nodes.
        /// </summary>
        public void Draw()
        {
            I_DrawBackground(mo_gridRect);

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {

                    Rect rect = GetNodeRect(x, y);
                    DragItem t = GetItem(x, y);
                    if (t == default(DragItem))
                        continue;

                    t.rectNormal = rect;
                    I_DrawItem(t, rect);

                }
            }
        }

        #endregion

    }

}

