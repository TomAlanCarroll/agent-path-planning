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
        private int xOffset;
        private int yOffset;

        public Agent(Grid grid, int height, int width, int rowIndex, int columnIndex)
        {
            // Setup the agent image source
            BitmapImage agentImageSource = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            agentImageSource.BeginInit();
            agentImageSource.UriSource = new Uri("pack://application:,,,/Images/walle.png");
            agentImageSource.EndInit();

            Image agentImage = new Image();
            agentImage.Source = agentImageSource;

            agentImage.Height = height;
            agentImage.Width = width;
            agentImage.Stretch = Stretch.None;

            agentImage.Margin = new Thickness(columnIndex * width, rowIndex * height, 0, 0);

            agentImage.VerticalAlignment = VerticalAlignment.Top;
            agentImage.HorizontalAlignment = HorizontalAlignment.Left;

            agentImage.Visibility = Visibility.Visible;

            grid.Children.Add(agentImage);
        }

        public Image GetAgentImage()
        {
            return agentImage;
        }

        public void SetAgentImage(Image agentImage)
        {
            this.agentImage = agentImage;
        }
    }
}
