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
    class Reward
    {
        private Image rewardImage;
        private int height;
        private int width;
        private int rowIndex;
        private int columnIndex;

        public Reward(Grid grid, int height, int width, int rowIndex, int columnIndex)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.height = height;
            this.width = width;

            // Setup the agent image source
            BitmapImage rewardImageSource = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            rewardImageSource.BeginInit();
            rewardImageSource.UriSource = new Uri("pack://application:,,,/Images/cooler.gif");
            rewardImageSource.EndInit();

            rewardImage = new Image();
            rewardImage.Source = rewardImageSource;

            rewardImage.Height = height;
            rewardImage.Width = width;
            rewardImage.Stretch = Stretch.None;

            rewardImage.Margin = new Thickness(columnIndex * width, rowIndex * height, 0, 0);

            rewardImage.VerticalAlignment = VerticalAlignment.Top;
            rewardImage.HorizontalAlignment = HorizontalAlignment.Left;

            rewardImage.Visibility = Visibility.Visible;

            grid.Children.Add(rewardImage);
        }

        public Image GetRewardImage()
        {
            return rewardImage;
        }

        public void SetRewardImage(Image rewardImage)
        {
            this.rewardImage = rewardImage;
        }

        public int GetRowIndex()
        {
            return rowIndex;
        }

        public int GetColumnIndex()
        {
            return columnIndex;
        }

        public void UpdatePosition()
        {
            rewardImage.Margin = new Thickness(columnIndex * width, rowIndex * height, 0, 0);
        }
    }
}
