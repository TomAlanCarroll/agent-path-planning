using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AgentPathPlanning
{
    class Agent
    {
        private Image agentImage;
        private Image agentWithRewardImage;
        private int height;
        private int width;
        private int rowIndex;
        private int columnIndex;

        public Agent(Grid grid, int height, int width, int rowIndex, int columnIndex)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.height = height;
            this.width = width;

            // Setup the agent image source
            BitmapImage agentImageSource = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            agentImageSource.BeginInit();
            agentImageSource.UriSource = new Uri("pack://application:,,,/Images/walle.png");
            agentImageSource.EndInit();

            agentImage = new Image();
            agentImage.Source = agentImageSource;

            agentImage.Height = height;
            agentImage.Width = width;
            agentImage.Stretch = Stretch.None;

            agentImage.Margin = new Thickness(columnIndex * width, rowIndex * height, 0, 0);

            agentImage.VerticalAlignment = VerticalAlignment.Top;
            agentImage.HorizontalAlignment = HorizontalAlignment.Left;

            agentImage.Visibility = Visibility.Visible;

            grid.Children.Add(agentImage);

            // Setup the agent with reward image source 
            BitmapImage agentWithRewardImageSource = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            agentWithRewardImageSource.BeginInit();
            agentWithRewardImageSource.UriSource = new Uri("pack://application:,,,/Images/walle_cooler.png");
            agentWithRewardImageSource.EndInit();

            agentWithRewardImage = new Image();
            agentWithRewardImage.Source = agentWithRewardImageSource;

            agentWithRewardImage.Height = height;
            agentWithRewardImage.Width = width;
            agentWithRewardImage.Stretch = Stretch.None;

            agentWithRewardImage.Margin = new Thickness(columnIndex * width, rowIndex * height, 0, 0);

            agentWithRewardImage.VerticalAlignment = VerticalAlignment.Top;
            agentWithRewardImage.HorizontalAlignment = HorizontalAlignment.Left;

            agentWithRewardImage.Visibility = Visibility.Hidden;

            grid.Children.Add(agentWithRewardImage);
        }

        public Image GetAgentImage()
        {
            return agentImage;
        }

        public void SetAgentImage(Image agentImage)
        {
            this.agentImage = agentImage;
        }

        public int GetRowIndex()
        {
            return rowIndex;
        }

        public void SetRowIndex(int rowIndex)
        {
            this.rowIndex = rowIndex;
        }

        public int GetColumnIndex()
        {
            return columnIndex;
        }

        public void SetColumnIndex(int columnIndex)
        {
            this.columnIndex = columnIndex;
        }

        /// <summary>
        /// Moves this agent in the grid world
        /// </summary>
        /// <param name="grid">The grid world</param>
        /// <param name="direction">The Direction to move towards</param>
        /// <returns>true if the move was successful; false otherwise</returns>
        public bool Move(GridWorld gridWorld, Direction direction)
        {
            // Move in the provided direction if there is no obstacle in that direction
            if (gridWorld.CanMove(direction, rowIndex, columnIndex))
            {
                switch (direction)
                {
                    case Direction.UP:
                        rowIndex--;
                        break;
                    case Direction.DOWN:
                        rowIndex++;
                        break;
                    case Direction.LEFT:
                        columnIndex--;
                        break;
                    case Direction.RIGHT:
                        columnIndex++;
                        break;
                }

                UpdatePosition();

                return true;
            }

            return false;
        }

        public void UpdatePosition()
        {
            agentImage.Margin = new Thickness(columnIndex * width, rowIndex * height, 0, 0);
            agentWithRewardImage.Margin = new Thickness(columnIndex * width, rowIndex * height, 0, 0);
        }

        public void ShowAgentWithReward()
        {
            agentImage.Visibility = Visibility.Hidden;
            agentWithRewardImage.Visibility = Visibility.Visible;
        }

        public void ShowAgentWithoutReward()
        {
            agentImage.Visibility = Visibility.Visible;
            agentWithRewardImage.Visibility = Visibility.Hidden;
        }
    }
}
