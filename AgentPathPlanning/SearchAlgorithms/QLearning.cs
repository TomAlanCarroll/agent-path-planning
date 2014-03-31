using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgentPathPlanning.SearchAlgorithms
{
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
        private bool favorUnexploredCells = true;

        private const int NUMBER_OF_MOVES = 4;
        private const int NUMBER_OF_EPISODES = 100;
        private const int MAX_SEARCH_STEPS = 150;
        private const int REWARD = 100;
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
                RestartEpisode(null);
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

            Direction bestDirection = GetBestDirection(currentCell);

            // In this stochastic implementation, use the transition function to get the actual direction to move
            Direction actualDirection = Transition(currentCell, bestDirection);

            currentCell = gridWorld.GetCell(currentCell, actualDirection);
            currentCell.SetHasBeenSearched(true);

            if (training)
            {
                // Update the Q-Table with the current cell
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    if (direction != Direction.NONE &&
                        gridWorld.CanMove(direction, currentCell.GetRowIndex(), currentCell.GetColumnIndex()))
                    {
                        UpdateQValue(currentCell, direction);
                    }
                }
            }
        }

        public Direction GetBestDirection(Cell cell)
        {
            allowedDirections = new List<Direction>();
            Direction bestDirection = Direction.NONE;
            double maxQValue = 0;

            if (gridWorld.CanMove(Direction.UP, cell.GetRowIndex(), cell.GetColumnIndex()))
            {
                allowedDirections.Add(Direction.UP);
                
                if (GetQValue(cell, Direction.UP) > maxQValue)
                {
                    maxQValue = GetQValue(cell, Direction.UP);
                    bestDirection = Direction.UP;
                }
            }

            if (gridWorld.CanMove(Direction.DOWN, cell.GetRowIndex(), cell.GetColumnIndex()))
            {
                allowedDirections.Add(Direction.DOWN);
                
                if (GetQValue(cell, Direction.DOWN) > maxQValue)
                {
                    maxQValue = GetQValue(cell, Direction.DOWN);
                    bestDirection = Direction.DOWN;
                }
            }

            if (gridWorld.CanMove(Direction.LEFT, cell.GetRowIndex(), cell.GetColumnIndex()))
            {
                allowedDirections.Add(Direction.LEFT);
                
                if (GetQValue(cell, Direction.LEFT) > maxQValue)
                {
                    maxQValue = GetQValue(cell, Direction.LEFT);
                    bestDirection = Direction.LEFT;
                }
            }

            if (gridWorld.CanMove(Direction.RIGHT, cell.GetRowIndex(), cell.GetColumnIndex()))
            {
                allowedDirections.Add(Direction.RIGHT);
                
                if (GetQValue(cell, Direction.RIGHT) > maxQValue)
                {
                    maxQValue = GetQValue(cell, Direction.RIGHT);
                    bestDirection = Direction.RIGHT;
                }
            }

            // Get the direction with the highest QValue
            return bestDirection;
        }

        public Direction Transition(Cell currentCell, Direction bestDirection)
        {
            Random random = new Random();
            double transitionValue = random.NextDouble();
            if (bestDirection == Direction.NONE) // Explore unexplored cells
            {
                return UnexploredDirection();
            }
            else // Use the best direction / unexplored / random
            {
                if (training)
                {
                    if (transitionValue <= 0.6)
                    {
                        return bestDirection;
                    }
                    else if (transitionValue <= 0.8 && favorUnexploredCells)  // Favor unexplored cells to improve the Q-Learning search
                    {
                        return UnexploredDirection();
                    }
                    else // Select a random direction
                    {
                        Shuffle(allowedDirections);

                        return allowedDirections[0];
                    }
                }
                else // Testing: no stochastic transitions
                {
                    return bestDirection;
                }
            }
        }

        public Direction UnexploredDirection()
        {
            Shuffle(allowedDirections);

            foreach (Direction direction in allowedDirections)
            {
                if (!gridWorld.GetCell(currentCell, direction).HasBeenSearched())
                {
                    return direction;
                }
            }

            // All directions have been searched; Return the first (random) direction
            return allowedDirections[0];
        }

        public int Reward(Cell cell)
        {
            if (cell.IsRewardCell())
            {
                return REWARD;
            }
            else
            {
                return 0;
            }
        }

        public double GetQValue(Cell cell, Direction direction)
        {
            return qTable[cell.GetRowIndex(),cell.GetColumnIndex(),(int)direction];
        }

        public double GetSumQValue(Cell cell)
        {
            return qTable[cell.GetRowIndex(), cell.GetColumnIndex(), (int)Direction.UP] +
                qTable[cell.GetRowIndex(), cell.GetColumnIndex(), (int)Direction.DOWN] +
                qTable[cell.GetRowIndex(), cell.GetColumnIndex(), (int)Direction.LEFT] +
                qTable[cell.GetRowIndex(), cell.GetColumnIndex(), (int)Direction.RIGHT];
        }


        public void UpdateQValue(Cell cell, Direction direction)
        {
            double currentQValue = GetQValue(cell, direction);

            Cell nextCell = gridWorld.GetCell(cell, direction);

            double nextCellReward = Reward(nextCell);

            double maxEstimate = 0;
            if (GetBestDirection(nextCell) != Direction.NONE)
            {
                maxEstimate = GetQValue(nextCell, GetBestDirection(nextCell));
            }

            qTable[cell.GetRowIndex(), cell.GetColumnIndex(), (int)direction] = QLearningRule(currentQValue, ALPHA, nextCellReward, GAMMA, maxEstimate);
        }

        public double QLearningRule(double currentQValue, double alpha, double reward, double gamma, double maxEstimate)
        {
            return currentQValue + (alpha * (reward + (gamma * maxEstimate) - currentQValue));
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

        public void RestartEpisode(Cell startingPosition)
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

            if (startingPosition == null)
            {
                int nextCellIndex = (new Random()).Next(0, availableCells.Count - 1);
                currentCell = availableCells[nextCellIndex];
            }
            else
            {
                currentCell = startingPosition;
            }
        }

        public bool IsTraining()
        {
            return training;
        }

        public Cell GetCurrentCell()
        {
            return currentCell;
        }

        public void SetCurrentCell(Cell currentCell)
        {
            this.currentCell = currentCell;
        }

        public int GetEpisodeCount()
        {
            return episodeCount;
        }

        public void SetEpisodeCount(int episodeCount)
        {
            this.episodeCount = episodeCount;
        }

        public int GetReward()
        {
            return REWARD;
        }
    }

    public static class ThreadSafeRandom
    {
        [ThreadStatic]
        private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }
}
