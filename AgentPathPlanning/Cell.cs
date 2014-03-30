using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace AgentPathPlanning
{
    class Cell
    {
        private int rowIndex;
        private int columnIndex;
        private double gScore;
        private double fScore;
        private bool isObstacle;
        private bool isAgentStartingCell;
        private bool isRewardCell;
        private bool hasBeenSearched;
        private Rectangle rectangle;

        public Cell(int rowIndex, int columnIndex, bool isObstacle, bool isAgentStartingCell, bool isRewardCell)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.isObstacle = isObstacle;
            this.isAgentStartingCell = isAgentStartingCell;
            this.isRewardCell = isRewardCell;
        }

        public int GetRowIndex()
        {
            return rowIndex;
        }

        public int GetColumnIndex()
        {
            return columnIndex;
        }

        public double GetGScore()
        {
            return gScore;
        }

        public void SetGScore(double gScore)
        {
            this.gScore = gScore;
        }

        public double GetFScore()
        {
            return fScore;
        }

        public void SetFScore(double fScore)
        {
            this.fScore = fScore;
        }

        public bool IsObstacle()
        {
            return isObstacle;
        }

        public bool IsAgentStartingCell()
        {
            return isAgentStartingCell;
        }

        public bool IsRewardCell()
        {
            return isRewardCell;
        }

        public void SetHasBeenSearched(bool hasBeenSearched)
        {
            this.hasBeenSearched = hasBeenSearched;
        }

        public bool HasBeenSearched()
        {
            return hasBeenSearched;
        }

        public Rectangle GetRectangle()
        {
            return rectangle;
        }

        public void SetRectangle(Rectangle rectangle)
        {
            this.rectangle = rectangle;
        }
    }
}
