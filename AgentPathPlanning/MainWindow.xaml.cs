using AgentPathPlanning.SearchAlgorithms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AgentPathPlanning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const long UPDATE_FREQUENCY = 100; // Run the search steps this many milliseconds

        // Cell size
        private const int CELL_HEIGHT = 60;
        private const int CELL_WIDTH = 60;

        private GridWorld gridWorld;
        private Cell startingCell;
        private Cell rewardCell;

        private AStar aStarSearch;
        private QLearning qLearningSearch;

        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
        }

        private void LoadGridButton_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Set the filter to CSV
            fileDialog.DefaultExt = ".csv";
            fileDialog.Filter = "CSV Files (*.csv)|*.*";

            Nullable<bool> result = fileDialog.ShowDialog();

            // Get the selected filename
            if (result == true)
            {
                // Setup the grid world
                gridWorld = new GridWorld(grid, GridMapParser.Parse(fileDialog.FileName), CELL_HEIGHT, CELL_WIDTH);

                // Setup the agent
                if (gridWorld.GetAgentStartingPosition() != null && gridWorld.GetAgentStartingPosition().Length == 2)
                {
                    int agentRowIndex = gridWorld.GetAgentStartingPosition()[0], agentColumnIndex = gridWorld.GetAgentStartingPosition()[1];

                    gridWorld.SetAgent(new Agent(grid, CELL_HEIGHT, CELL_WIDTH, agentRowIndex, agentColumnIndex));

                    // Get the starting cell
                    startingCell = gridWorld.GetCells()[agentRowIndex, agentColumnIndex];
                }
                else
                {
                    MessageBox.Show("Error: The agent starting position must be specified in the grid map file with the number 1. Please correct and try again.");
                    return;
                }

                // Setup the reward
                if (gridWorld.GetRewardPosition() != null && gridWorld.GetRewardPosition().Length == 2)
                {
                    int rewardRowIndex = gridWorld.GetRewardPosition()[0], rewardColumnIndex = gridWorld.GetRewardPosition()[1];

                    gridWorld.SetReward(new Reward(grid, CELL_HEIGHT, CELL_WIDTH, rewardRowIndex, rewardColumnIndex));

                    // Get the reward cell
                    rewardCell = gridWorld.GetCells()[rewardRowIndex, rewardColumnIndex];
                }
                else
                {
                    MessageBox.Show("Error: The reward starting position must be specified in the grid map file with the number 2. Please correct and try again.");
                    return;
                }

                // Make the start button active
                StartButton.IsEnabled = true;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Make the start button inactive
            StartButton.IsEnabled = false;

            // Make the stop button active
            StopButton.IsEnabled = true;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(UPDATE_FREQUENCY);



            if ((bool)AStarRadioButton.IsChecked)
            {
                if (aStarSearch == null)
                {
                    aStarSearch = new AStar(gridWorld, startingCell, rewardCell);
                }

                timer.Tick += new EventHandler(aStarSearch.Run);
                timer.Tick += new EventHandler(UpdateAgentPosition);
            }
            else // Q-Learning is checked
            {
                if (qLearningSearch == null)
                {
                    qLearningSearch = new QLearning(startingCell, rewardCell);
                }

                timer.Tick += new EventHandler(qLearningSearch.Run);
            }


            timer.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            timer.Stop();

            // Make the start button active
            StartButton.IsEnabled = true;

            // Make the stop button inactive
            StopButton.IsEnabled = false;
        }

        public void UpdateAgentPosition(object sender, EventArgs e)
        {
            gridWorld.GetAgent().SetRowIndex(aStarSearch.GetCurrentCell().GetRowIndex());
            gridWorld.GetAgent().SetColumnIndex(aStarSearch.GetCurrentCell().GetColumnIndex());

            gridWorld.GetAgent().UpdatePosition();
        }
    }
}
