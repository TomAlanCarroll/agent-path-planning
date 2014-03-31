using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgentPathPlanning.SearchAlgorithms
{
    public static class ThreadSafeRandom
    {
        [ThreadStatic]
        private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }

    class QLearning
    {
        private Cell currentCell;
        private List<Direction> allowedDirections; // The allowed directions for the current cell
        private Cell rewardCell;
        private GridWorld gridWorld;
        private double[,,] qTable;
        private int stepCount = 0;
        private int episodeCount = 0;

        private bool training = true;

        private const int NUMBER_OF_MOVES = 4;
        private const int NUMBER_OF_EPISODES = 100;
        private const int MAX_SEARCH_STEPS = 150;
        private const double ALPHA = 0.2;
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

            if (stepCount > MAX_SEARCH_STEPS)
            {
                RestartEpisode();
            }

            if (episodeCount > NUMBER_OF_EPISODES)
            {
                training = false;
            }

            // Check to see if we found the reward
            if (currentCell.IsRewardCell())
            {
                System.Console.Out.WriteLine("Found the reward");
                return;
            }

            if (training)
            {
                Direction bestDirection = GetBestDirection();

                // In this stochastic implementation, use the transition function to get the actual direction to move
                Direction actualDirection = Transition(bestDirection);

                if (actualDirection == Direction.UP)
                {
                    currentCell = gridWorld.GetCells()[currentCell.GetRowIndex() - 1, currentCell.GetColumnIndex()];
                }
                else if (actualDirection == Direction.DOWN)
                {
                    currentCell = gridWorld.GetCells()[currentCell.GetRowIndex() + 1, currentCell.GetColumnIndex()];
                }
                else if (actualDirection == Direction.LEFT)
                {
                    currentCell = gridWorld.GetCells()[currentCell.GetRowIndex(), currentCell.GetColumnIndex() - 1];
                }
                else if (actualDirection == Direction.RIGHT)
                {
                    currentCell = gridWorld.GetCells()[currentCell.GetRowIndex(), currentCell.GetColumnIndex() + 1];
                }
            }
        }

        public LinkedList<Cell> GetBestPath()
        {
            // TODO: Implement
            return null;
        }

        public Direction GetBestDirection()
        {
            SortedList<Direction, double> directions = new SortedList<Direction, double>();
            allowedDirections = new List<Direction>();

            if (gridWorld.CanMove(Direction.UP, currentCell.GetRowIndex(), currentCell.GetColumnIndex()))
            {
                allowedDirections.Add(Direction.UP);
                directions.Add(Direction.UP, GetQValue(gridWorld.GetCells()[currentCell.GetRowIndex() - 1, currentCell.GetColumnIndex()]));
            }

            if (gridWorld.CanMove(Direction.DOWN, currentCell.GetRowIndex(), currentCell.GetColumnIndex()))
            {
                allowedDirections.Add(Direction.DOWN);
                directions.Add(Direction.DOWN, GetQValue(gridWorld.GetCells()[currentCell.GetRowIndex() + 1, currentCell.GetColumnIndex()]));
            }

            if (gridWorld.CanMove(Direction.LEFT, currentCell.GetRowIndex(), currentCell.GetColumnIndex()))
            {
                allowedDirections.Add(Direction.LEFT);
                directions.Add(Direction.LEFT, GetQValue(gridWorld.GetCells()[currentCell.GetRowIndex(), currentCell.GetColumnIndex() - 1]));
            }

            if (gridWorld.CanMove(Direction.RIGHT, currentCell.GetRowIndex(), currentCell.GetColumnIndex()))
            {
                allowedDirections.Add(Direction.RIGHT);
                directions.Add(Direction.RIGHT, GetQValue(gridWorld.GetCells()[currentCell.GetRowIndex(), currentCell.GetColumnIndex() + 1]));
            }

            // Get the direction with the highest QValue
            return directions.ElementAt(directions.Count - 1).Key;
        }

        public Direction Transition(Direction bestDirection)
        {
            Random random = new Random();
            double transitionValue = random.NextDouble();

            if (transitionValue <= 0.6)
            {
                return bestDirection;
            }
            else // Select a random direction
            {
                Shuffle(allowedDirections);
                
                return allowedDirections[0];
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

        public void Shuffle(List<Direction> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int randomNumber = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                Direction value = list[randomNumber];
                list[randomNumber] = list[n];
                list[n] = value;
            }
        }

        public void RestartEpisode()
        {
            // Reset the step count for the next episode
            stepCount = 0;

            // Increment the episode counter
            episodeCount++;

            List<Cell> availableCells = new List<Cell>();

            // Place the agent at a random tile (not an obstacle)
            foreach (Cell cell in gridWorld.GetCells())
            {
                if (!cell.IsObstacle() && !cell.IsRewardCell())
                {
                    availableCells.Add(cell);
                }
            }

            int nextCellIndex = (new Random()).Next(0, availableCells.Count - 1);

            currentCell = availableCells[nextCellIndex];
        }

        public bool IsTraining()
        {
            return training;
        }

        public Cell GetCurrentCell()
        {
            return currentCell;
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
