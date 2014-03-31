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

        private Agent agent;
        private Reward reward;

        // Number of rows and columns (default is 10x10)
        private int ROWS = 10;
        private int COLUMNS = 10;

        // Cell colors (RGB byte arrays)
        private SolidColorBrush UNOCCUPIED_CELL_BACKGROUND_COLOR = new SolidColorBrush(Color.FromRgb(244, 244, 244));
        private SolidColorBrush OCCUPIED_CELL_BACKGROUND_COLOR = new SolidColorBrush(Color.FromRgb(218, 164, 160));
        private SolidColorBrush OBSTACLE_CELL_BACKGROUND_COLOR = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private SolidColorBrush CELL_STROKE_COLOR = new SolidColorBrush(Color.FromRgb(0, 0, 0));

        private int[] agentStartingPosition;
        private int[] rewardPosition;

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
                    // Check if this is an agent/reward
                    if (this.cells[i, j].IsAgentStartingCell())
                    {
                        this.agentStartingPosition = new int[2];
                        this.agentStartingPosition[0] = i;
                        this.agentStartingPosition[1] = j;
                    }

                    if (this.cells[i, j].IsRewardCell())
                    {
                        this.rewardPosition = new int[2];
                        this.rewardPosition[0] = i;
                        this.rewardPosition[1] = j;
                    }

                    // Make a rectangle for the grid
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

        public Cell[,] GetCells()
        {
            return cells;
        }

        public Agent GetAgent()
        {
            return agent;
        }

        public void SetAgent(Agent agent)
        {
            this.agent = agent;
        }

        public Reward GetReward()
        {
            return reward;
        }

        public void SetReward(Reward reward)
        {
            this.reward = reward;
        }

        public int[] GetAgentStartingPosition()
        {
            return agentStartingPosition;
        }

        public int[] GetRewardPosition()
        {
            return rewardPosition;
        }

        /// <summary>
        /// Determines if a cell specified by row and column indices is available to move towards the specified direction.
        /// </summary>
        /// <param name="direction">The direction to move</param>
        /// <returns>true if the cell can be moved to by an agent; false otherwise</returns>
        public bool CanMove(Direction direction, int rowIndex, int columnIndex)
        {// Move in the provided direction if there is no obstacle in that direction
            int newRowIndex = rowIndex, newColumnIndex = columnIndex;
            switch (direction)
            {
                case Direction.UP:
                    newRowIndex = rowIndex - 1;
                    break;
                case Direction.DOWN:
                    newRowIndex = rowIndex + 1;
                    break;
                case Direction.LEFT:
                    newColumnIndex = newColumnIndex - 1;
                    break;
                case Direction.RIGHT:
                    newColumnIndex = newColumnIndex + 1;
                    break;
            }

            // Check for out of bounds indices
            if (newRowIndex < 0 || ROWS - 1 < newRowIndex || newColumnIndex < 0 || COLUMNS - 1 < newColumnIndex)
            {
                return false;
            }

            // Check if this index is an obstacle
            if (cells[newRowIndex, newColumnIndex].IsObstacle())
            {
                return false;
            }

            // Inbound and not an obstacle: return true
            return true;
        }

        /// <summary>
        /// Gets the cell in the specified direction from the provided starting cell
        /// </summary>
        /// <param name="startingCell">The starting cell</param>
        /// <param name="direction">The direction to get the cell</param>
        /// <returns></returns>
        public Cell GetCell(Cell startingCell, Direction direction)
        {
            if (direction == Direction.UP)
            {
                return GetCells()[startingCell.GetRowIndex() - 1, startingCell.GetColumnIndex()];
            }
            else if (direction == Direction.DOWN)
            {
                return GetCells()[startingCell.GetRowIndex() + 1, startingCell.GetColumnIndex()];
            }
            else if (direction == Direction.LEFT)
            {
                return GetCells()[startingCell.GetRowIndex(), startingCell.GetColumnIndex() - 1];
            }
            else if (direction == Direction.RIGHT)
            {
                return GetCells()[startingCell.GetRowIndex(), startingCell.GetColumnIndex() + 1];
            }
            else
            {
                throw new Exception("Unknown direction encountered.");
            }
        }
    }
}
