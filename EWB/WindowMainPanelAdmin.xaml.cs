using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EWB
{
    public partial class WindowMainPanelAdmin : Window
    {
        public WindowMainPanelAdmin()
        {
            InitializeComponent();
        }
        private void windowMouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animationOpacity = new DoubleAnimation();
            animationOpacity.Duration = TimeSpan.FromSeconds(0.1);
            animationOpacity.To = 0;
            animationOpacity.Completed += (s, ea) =>
            {
                Application.Current.Shutdown();
            };
            mainGrid.BeginAnimation(Grid.OpacityProperty, animationOpacity);
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animationOpacity = new DoubleAnimation();
            animationOpacity.Duration = TimeSpan.FromSeconds(1);
            animationOpacity.To = 0;
            animationOpacity.Completed += (s, ea) =>
            {
                this.Close();
            };
            mainGrid.BeginAnimation(Grid.OpacityProperty, animationOpacity);
            WindowRegistration windowRegistration = new WindowRegistration();
            windowRegistration.Show();
            this.Close();
        }
        private void btnLeave_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();

            DoubleAnimation animationOpacity = new DoubleAnimation();
            animationOpacity.Duration = TimeSpan.FromSeconds(1);
            animationOpacity.To = 0;
            animationOpacity.Completed += (s, ea) =>
            {
                this.Close();
            };
            mainGrid.BeginAnimation(Grid.OpacityProperty, animationOpacity);
            authWindow.Show();
        }

        private void btnWorkInf_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animationOpacity = new DoubleAnimation();
            animationOpacity.Duration = TimeSpan.FromSeconds(1);
            animationOpacity.To = 0;
            animationOpacity.Completed += (s, ea) =>
            {
                this.Close();
            };
            mainGrid.BeginAnimation(Grid.OpacityProperty, animationOpacity);
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
            this.Close();
        }

        private void btnPersonInf_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animationOpacity = new DoubleAnimation();
            animationOpacity.Duration = TimeSpan.FromSeconds(1);
            animationOpacity.To = 0;
            animationOpacity.Completed += (s, ea) =>
            {
                this.Close();
            };
            mainGrid.BeginAnimation(Grid.OpacityProperty, animationOpacity);
            WindowPersonal windowPersonal = new WindowPersonal();
            windowPersonal.Show();
            this.Close();
        }

        private void btnReward_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animationOpacity = new DoubleAnimation();
            animationOpacity.Duration = TimeSpan.FromSeconds(1);
            animationOpacity.To = 0;
            animationOpacity.Completed += (s, ea) =>
            {
                this.Close();
            };
            mainGrid.BeginAnimation(Grid.OpacityProperty, animationOpacity);
            WindowRewards windowRewards = new WindowRewards();
            windowRewards.Show();
            this.Close();
        }
    }
}
