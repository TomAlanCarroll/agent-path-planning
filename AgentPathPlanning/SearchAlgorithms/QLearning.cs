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
        private Cell currentCell; // The cell where the agent is located in the simulation
        private List<Direction> allowedDirections; // The allowed directions for the current cell
        private Cell rewardCell; // The cell where the reward is located in the simulation
        private GridWorld gridWorld; // The grid world simulation
        private double[,,] qTable; // The Q-Table
        private int stepCount = 0; // The number of steps take in each episode
        private int episodeCount = 0; // The total number of episodes that have occured

        private bool training = true; // boolean flag to indicate training versus testing
        private bool favorUnexploredCells = true; // boolean flag to favor unexplored cells while training

        private const int NUMBER_OF_MOVES = 4; // Number of possible moves (up, down, left, and right)
        private const int NUMBER_OF_EPISODES = 100; // Number of episodes to train the agent
        private const int MAX_SEARCH_STEPS = 150; // Max number of steps to take in an episode without finding the reward
        private const int REWARD = 100; // The reward value
        private const double ALPHA = 0.2; // The value of alpha to use in the Q-Learning rule
        private const double GAMMA = 0.99; // The value of gamma to use in the Q-Learning rule

        public QLearning(GridWorld gridWorld, Cell startingCell, Cell rewardCell)
        {
            this.currentCell = startingCell;
            this.rewardCell = rewardCell;
            this.gridWorld = gridWorld;

            this.qTable = new double[gridWorld.GetCells().GetLength(0),gridWorld.GetCells().GetLength(1),NUMBER_OF_MOVES];
        }

        /// <summary>
        /// Runs a tick of the Q-Learning search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Gets the best direction to go from the Q-Table for a given cell
        /// </summary>
        /// <param name="cell">The originating cell</param>
        /// <returns>The best direction for the cell. If all directions have no Q-values > 0 then Direction.NONE will be returned</returns>
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

        /// <summary>
        /// The transition function for Q-Learning.
        /// This function has two modes:
        /// 
        /// 1. Training - 60% chance of using the provided bestDirection; 
        ///               20% chance of favoring a random unexplored cell direction;
        ///               20% chance of a completely random cell direction;
        /// 2. Testing - 100% chance of the best direction from the Q-Table
        /// 
        /// If Direction.None is provided for training and testing, 
        /// then a direction leading to a random cell will be returned (with unexplored having highest priority).
        /// </summary>
        /// <param name="currentCell">The originating cell</param>
        /// <param name="bestDirection">The best direction from the Q-Table</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets an unexplored direction by shuffling the directions available to move towards.
        /// </summary>
        /// <returns>An unexplored direction. If all directions have been explored then a random direction is returned.</returns>
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

        /// <summary>
        /// Gets the reward for a cell.
        /// </summary>
        /// <param name="cell">The cell with the reward.</param>
        /// <returns>REWARD or 0</returns>
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

        public double[, ,] GetQTable()
        {
            return qTable;
        }

        /// <summary>
        /// Gets the Q-Value in the Q-Table for a cell and a direction
        /// </summary>
        /// <param name="cell">The originating cell</param>
        /// <param name="direction">The direction of travel</param>
        /// <returns>The current Q-Value in the Q-Table</returns>
        public double GetQValue(Cell cell, Direction direction)
        {
            return qTable[cell.GetRowIndex(),cell.GetColumnIndex(),(int)direction];
        }

        /// <summary>
        /// Convenience method to sum to the Q-Values for all directions for a given cell
        /// </summary>
        /// <param name="cell">The cell to sum the Q-Values</param>
        /// <returns>The sum of Q-Values</returns>
        public double GetSumQValue(Cell cell)
        {
            return qTable[cell.GetRowIndex(), cell.GetColumnIndex(), (int)Direction.UP] +
                qTable[cell.GetRowIndex(), cell.GetColumnIndex(), (int)Direction.DOWN] +
                qTable[cell.GetRowIndex(), cell.GetColumnIndex(), (int)Direction.LEFT] +
                qTable[cell.GetRowIndex(), cell.GetColumnIndex(), (int)Direction.RIGHT];
        }

        /// <summary>
        /// Updates the Q-Value in the Q-Table for a given cell and direction
        /// </summary>
        /// <param name="cell">The originating cell</param>
        /// <param name="direction">The direction of travel</param>
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

        /// <summary>
        /// The Q-Learning rule. This calculates the Q-Value 
        /// </summary>
        /// <param name="currentQValue"> The current Q-Value at this cell in this direction</param>
        /// <param name="alpha">The learning rate</param>
        /// <param name="reward">The reward at the cell</param>
        /// <param name="gamma">The discount factor</param>
        /// <param name="maxEstimate">The estimate of optimal future value</param>
        /// <returns>A calculated Q-Value</returns>
        public double QLearningRule(double currentQValue, double alpha, double reward, double gamma, double maxEstimate)
        {
            return currentQValue + (alpha * (reward + (gamma * maxEstimate) - currentQValue));
        }

        /// <summary>
        /// Shuffles a list of directions in a random order
        /// </summary>
        /// <param name="list">The shuffled list</param>
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

        /// <summary>
        /// Restarts an episode of Q-Learning and moves the current position to the provided starting position if one is provided.
        /// </summary>
        /// <param name="startingPosition">The starting position for the episode. If null is provided a random starting position will be determined.</param>
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
