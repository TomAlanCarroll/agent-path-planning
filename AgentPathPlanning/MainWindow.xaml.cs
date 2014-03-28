using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace AgentPathPlanning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double UPDATE_FREQUENCY = 0.2;

        // Cell size
        private const int CELL_HEIGHT = 60;
        private const int CELL_WIDTH = 60;

        private GridWorld gridWorld;
        private Agent agent;
        private Reward reward;

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
                    agent = new Agent(grid, CELL_HEIGHT, CELL_WIDTH, gridWorld.GetAgentStartingPosition()[0], gridWorld.GetAgentStartingPosition()[1]);
                }
                else
                {
                    MessageBox.Show("Error: The agent starting position must be specified in the grid map file with the number 1. Please correct and try again.");
                }

                // Setup the reward
                if (gridWorld.GetRewardPosition() != null && gridWorld.GetRewardPosition().Length == 2)
                {
                    reward = new Reward(grid, CELL_HEIGHT, CELL_WIDTH, gridWorld.GetRewardPosition()[0], gridWorld.GetRewardPosition()[1]);
                }
                else
                {
                    MessageBox.Show("Error: The reward starting position must be specified in the grid map file with the number 2. Please correct and try again.");
                }
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Start the simulation; Update every UPDATE_FREQUENCY
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
