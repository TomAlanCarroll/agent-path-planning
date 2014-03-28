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
        private Rectangle rectangle;

        public Cell(int rowIndex, int columnIndex, bool isObstacle)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.isObstacle = isObstacle;
        }

        public bool IsObstacle()
        {
            return isObstacle;
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
