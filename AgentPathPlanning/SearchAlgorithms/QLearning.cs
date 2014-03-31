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
        private GridWorld gridWorld;
        private double[,,] qTable;
        private int stepCount = 0;
        private int episodeCount = 0;

        private const int NUMBER_OF_MOVES = 4;
        private const int NUMBER_OF_EPISODES = 100;
        private const double ALPHA = 0.8;
        private const double GAMMA = 0.99;

        public QLearning(GridWorld gridWorld, Cell startingCell, Cell rewardCell)
        {
            this.currentCell = startingCell;
            this.rewardCell = rewardCell;
            this.gridWorld = gridWorld;

            this.qTable = new double[gridWorld.GetCells().GetLength(0),gridWorld.GetCells().GetLength(1),NUMBER_OF_MOVES];
        }

        public void Run(object sender, EventArgs e)
        {
            // TODO: Implement
            System.Console.Out.WriteLine("Running Q-Learning; Step Count: " + stepCount++);
        }

        public void IlluminateCurrentCell(object sender, EventArgs e)
        {
            // TODO: Implement
        }

        public LinkedList<Cell> GetBestPath()
        {
            // TODO: Implement
            return null;
        }

        public Direction GetBestDirection()
        {
            SortedList<Direction, double> directions = new SortedList<Direction, double>();

            if (gridWorld.CanMove(Direction.UP, currentCell.GetRowIndex(), currentCell.GetColumnIndex()))
            {
                directions.Add(Direction.UP, GetQValue(gridWorld.GetCells()[currentCell.GetRowIndex() - 1, currentCell.GetColumnIndex()]));
            }

            if (gridWorld.CanMove(Direction.DOWN, currentCell.GetRowIndex(), currentCell.GetColumnIndex()))
            {
                directions.Add(Direction.DOWN, GetQValue(gridWorld.GetCells()[currentCell.GetRowIndex() + 1, currentCell.GetColumnIndex()]));
            }

            if (gridWorld.CanMove(Direction.LEFT, currentCell.GetRowIndex(), currentCell.GetColumnIndex()))
            {
                directions.Add(Direction.LEFT, GetQValue(gridWorld.GetCells()[currentCell.GetRowIndex(), currentCell.GetColumnIndex() - 1]));
            }

            if (gridWorld.CanMove(Direction.RIGHT, currentCell.GetRowIndex(), currentCell.GetColumnIndex()))
            {
                directions.Add(Direction.RIGHT, GetQValue(gridWorld.GetCells()[currentCell.GetRowIndex(), currentCell.GetColumnIndex() + 1]));
            }

            return Direction.RIGHT;
        }

        public Direction transition(Direction bestDirection)
        {
            Random random = new Random();
            double transitionValue = random.NextDouble();

            if (transitionValue <= 0.6)
            {
                return bestDirection;
            }
            else // Select a random direction
            {
                double randomTransitionValue = random.NextDouble();

                if (randomTransitionValue <= 0.25)
                {
                    return Direction.UP;
                }
                else if (randomTransitionValue <= 0.5)
                {
                    return Direction.DOWN;
                }
                else if (randomTransitionValue <= 0.75)
                {
                    return Direction.LEFT;
                }
                else
                {
                    return Direction.RIGHT;
                }
            }
        }

        public int Reward(Cell cell)
        {
            if (cell.IsRewardCell())
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }

        public double GetQValue(Cell cell)
        {
            // TODO: Implement
            return 0;
        }

        public int GetEpisodeCount()
        {
            return episodeCount;
        }

        public void SetEpisodeCount(int episodeCount)
        {
            this.episodeCount = episodeCount;
        }
    }
}
