using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgentPathPlanning.SearchAlgorithms
{
    class AStar
    {
        private LinkedList<Cell> visitedCells; // The set of visited cells in the grid simulation
        private LinkedList<Cell> unvisitedCells; // The set of unvisted cells in the grid simulation
        private LinkedList<Cell> bestPath; // The best path between the starting position and the reward

        private GridWorld gridWorld; // The grid world in the simulation

        private Cell currentCell; // The cell where the agent is located in the simulation
        private Cell rewardCell; // The cell where the reward is located in the simulation

        private int stepCount = 0; // The number of steps take in each episode

        public AStar(GridWorld gridWorld, Cell startingCell, Cell rewardCell)
        {
            visitedCells = new LinkedList<Cell>();
            unvisitedCells = new LinkedList<Cell>();
            bestPath = new LinkedList<Cell>();
            this.currentCell = startingCell;
            this.rewardCell = rewardCell;
            this.gridWorld = gridWorld;

            this.currentCell.SetGScore(0);

            double fScore = this.currentCell.GetGScore() + GetHeuristicEstimate(this.currentCell, rewardCell);

            this.currentCell.SetFScore(fScore);

            unvisitedCells.AddFirst(this.currentCell);
        }

        /// <summary>
        /// Runs a tick of the A* search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Run(object sender, EventArgs e)
        {

            System.Console.Out.WriteLine("Running A*; Step Count: " + stepCount++);

            while (true)
            {
                if (unvisitedCells.Count == 0)
                {
                    return;
                }

                currentCell = unvisitedCells.Last.Value;
                unvisitedCells.RemoveLast();

                if (currentCell.HasBeenSearched())
                {
                    continue;
                }

                break;
            }

            currentCell.SetHasBeenSearched(true);

            visitedCells.AddLast(currentCell);

            if (currentCell.IsRewardCell())
            {
                System.Console.Out.WriteLine("Found the reward");
                return;
            }

            // Check if cell is a neighbor of the currentCell
            // If so, set the cost and add to unvisitedCells in ProcessNeighbor()

            if (gridWorld.CanMove(Direction.UP, currentCell.GetRowIndex(), currentCell.GetColumnIndex()))
            {
                ProcessNeighbor(gridWorld.GetCells()[currentCell.GetRowIndex() - 1, currentCell.GetColumnIndex()]);
            }

            if (gridWorld.CanMove(Direction.DOWN, currentCell.GetRowIndex(), currentCell.GetColumnIndex()))
            {
                ProcessNeighbor(gridWorld.GetCells()[currentCell.GetRowIndex() + 1, currentCell.GetColumnIndex()]);
            }

            if (gridWorld.CanMove(Direction.LEFT, currentCell.GetRowIndex(), currentCell.GetColumnIndex()))
            {
                ProcessNeighbor(gridWorld.GetCells()[currentCell.GetRowIndex(), currentCell.GetColumnIndex() - 1]);
            }

            if (gridWorld.CanMove(Direction.RIGHT, currentCell.GetRowIndex(), currentCell.GetColumnIndex()))
            {
                ProcessNeighbor(gridWorld.GetCells()[currentCell.GetRowIndex(), currentCell.GetColumnIndex() + 1]);
            }
        }

        /// <summary>
        /// Processes an available neighbor by calculating the g, f, and h scores and adding to the appropriate array
        /// </summary>
        /// <param name="neighbor">The neighboring cell</param>
        public void ProcessNeighbor(Cell neighbor)
        {
            // Set the parent of the neighboring cell to the current cell
            if (!visitedCells.Contains(neighbor) && !unvisitedCells.Contains(neighbor))
            {
                neighbor.SetParent(currentCell);
            }

            // Calculate the scores
            double gScore = currentCell.GetGScore() + 1;

            neighbor.SetGScore(gScore);

            double fScore = neighbor.GetGScore() + GetHeuristicEstimate(neighbor, rewardCell);

            neighbor.SetFScore(fScore);

            Cell[] unvisitedCellsArray = new Cell[unvisitedCells.Count];
            unvisitedCells.CopyTo(unvisitedCellsArray, 0);
            // Add to unvisited cells for further exploration
            foreach (Cell unvisitedCell in unvisitedCellsArray)
            {
                if (unvisitedCell.GetFScore() < fScore)
                {
                    unvisitedCells.AddBefore(unvisitedCells.Find(unvisitedCell), neighbor);
                    return;
                }
            }

            // This cell has the lowest cost; Add to the last position in the linked list

            unvisitedCells.AddLast(neighbor);
        }

        // Gets the best path by back-tracing from the reward to the starting position for each cell
        public LinkedList<Cell> GetBestPath()
        {
            if (currentCell != null)
            {
                Cell nextCell = currentCell;

                while (nextCell.GetRowIndex() != gridWorld.GetAgentStartingPosition()[0] ||
                       nextCell.GetColumnIndex() != gridWorld.GetAgentStartingPosition()[1])
                {
                    bestPath.AddFirst(nextCell);

                    nextCell = nextCell.GetParent();

                    if (nextCell == null)
                    {
                        throw new Exception("Unable to find starting cell in best path");
                    }
                }

                // Add the starting cell
                bestPath.AddFirst(nextCell);

                return bestPath;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Calculates the Euclidean distance between two cells
        /// </summary>
        /// <param name="neighbor">The neighbor cell</param>
        /// <param name="reward">The reward cell</param>
        /// <returns>The Euclidean distance between the two cells</returns>
        public double GetHeuristicEstimate (Cell neighbor, Cell reward)
        {
            // Return the Euclidean distance
            return Math.Sqrt(Math.Pow(rewardCell.GetColumnIndex() - neighbor.GetColumnIndex(), 2) +
                             Math.Pow(rewardCell.GetRowIndex() - neighbor.GetRowIndex(), 2));
        }

        public Cell GetCurrentCell()
        {
            return currentCell;
        }
    }
}
