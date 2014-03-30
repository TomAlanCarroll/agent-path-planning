using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentPathPlanning.SearchAlgorithms
{
    class QLearning
    {
        private Cell currentCell;
        private Cell rewardCell;
        private int stepCount = 0;

        public QLearning(Cell startingCell, Cell rewardCell)
        {
            this.currentCell = startingCell;
            this.rewardCell = rewardCell;
            // TODO: Implement
        }

        public void Run(object sender, EventArgs e)
        {
            // TODO: Implement
            System.Console.Out.WriteLine("Running Q-Learning; Step Count: " + stepCount++);
        }
    }
}
