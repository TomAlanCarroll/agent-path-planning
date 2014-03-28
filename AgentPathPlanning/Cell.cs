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
        private bool isObstacle;
        private bool isAgentStartingCell;
        private bool isRewardCell;
        private Rectangle rectangle;

        public Cell(int rowIndex, int columnIndex, bool isObstacle, bool isAgentStartingCell, bool isRewardCell)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.isObstacle = isObstacle;
            this.isAgentStartingCell = isAgentStartingCell;
            this.isRewardCell = isRewardCell;
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
