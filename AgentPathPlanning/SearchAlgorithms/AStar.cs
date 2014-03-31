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
        private LinkedList<Cell> visitedCells;
        private LinkedList<Cell> unvisitedCells;

        private LinkedList<Cell> bestPath;

        private GridWorld gridWorld;

        private Cell currentCell;

        private Cell rewardCell;

        private int stepCount = 0;

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

        public double GetHeuristicEstimate (Cell neighbor, Cell reward)
        {
            // Return the euclidean distance
            return Math.Sqrt(Math.Pow(rewardCell.GetColumnIndex() - neighbor.GetColumnIndex(), 2) +
                             Math.Pow(rewardCell.GetRowIndex() - neighbor.GetRowIndex(), 2));
        }

        public Cell GetCurrentCell()
        {
            return currentCell;
        }
    }
}
