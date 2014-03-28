using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AgentPathPlanning
{
    class GridWorld
    {
        private Cell[,] cells;

        // Number of rows and columns (default is 10x10)
        private int ROWS = 10;
        private int COLUMNS = 10;

        // Cell colors (RGB byte arrays)
        private SolidColorBrush UNOCCUPIED_CELL_BACKGROUND_COLOR = new SolidColorBrush(Color.FromRgb(244, 244, 244));
        private SolidColorBrush OCCUPIED_CELL_BACKGROUND_COLOR = new SolidColorBrush(Color.FromRgb(218, 164, 160));
        private SolidColorBrush OBSTACLE_CELL_BACKGROUND_COLOR = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private SolidColorBrush CELL_STROKE_COLOR = new SolidColorBrush(Color.FromRgb(0, 0, 0));

        private int currentRowIndex = 0;
        private int currentColumnIndex = 0;

        public GridWorld(Grid grid, Cell[,] cells, int cellHeight, int cellWidth)
        {
            ROWS = cells.GetLength(0);
            COLUMNS = cells.GetLength(1);
            this.cells = cells;
            
            // Build the cells
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    Rectangle rectangle = new Rectangle();

                    rectangle.Name = "cell" + i + j;

                    rectangle.Height = cellHeight;
                    rectangle.Width = cellWidth;

                    rectangle.VerticalAlignment = VerticalAlignment.Top;
                    rectangle.HorizontalAlignment = HorizontalAlignment.Left;

                    rectangle.Margin = new Thickness(j * cellWidth, i * cellHeight, 0, 0);

                    rectangle.Stroke = CELL_STROKE_COLOR;

                    if (this.cells[i, j].IsObstacle())
                    {
                        rectangle.Fill = OBSTACLE_CELL_BACKGROUND_COLOR;
                    }
                    else
                    {
                        rectangle.Fill = UNOCCUPIED_CELL_BACKGROUND_COLOR;
                    }

                    this.cells[i, j].SetRectangle(rectangle);

                    grid.Children.Add(this.cells[i, j].GetRectangle());
                }
            }
        }

        private int GetRowIndexFromId(String id)
        {
            return id.Replace("cell", "")[0];
        }

        private int GetColumnIndexFromId(String id)
        {
            return id.Replace("cell", "")[1];
        }

        public int GetCurrentRowIndex()
        {
            return currentRowIndex;
        }

        public int GetCurrentColumnIndex()
        {
            return currentColumnIndex;
        }

        /// <summary>
        /// Determines if a cell specified by row and column indices is available to move towards.
        /// </summary>
        /// <param name="rowIndex">The row index</param>
        /// <param name="columnIndex">The column index</param>
        /// <returns>true if the cell can be moved to by an agent; false otherwise</returns>
        public bool CanMove(int rowIndex, int columnIndex)
        {
            // Check for out of bounds indices
            if (rowIndex < 0 || ROWS - 1 < rowIndex || columnIndex < 0 || COLUMNS - 1 < columnIndex)
            {
                return false;
            }

            // Check if this index is an obstacle
            if (cells[rowIndex, columnIndex].IsObstacle())
            {
                return false;
            }

            // Inbound and not an obstacle: return true
            return true;
        }
    }
}
