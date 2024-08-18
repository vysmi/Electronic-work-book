using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EWB
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }
        private void windowMouseDown(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
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
        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {           
            string username = txtUser.Text;
            string password = txtPassword.Password;
            using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True"))
            {
                connection.Open();

                string query = "SELECT * FROM Users WHERE Login = @Login AND Password = @Password;";
                if(txtUser.Text == "Admin")
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", username);
                        command.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DoubleAnimation animationOpacity = new DoubleAnimation();
                                animationOpacity.Duration = TimeSpan.FromSeconds(1);
                                animationOpacity.To = 0;
                                animationOpacity.Completed += (s, ea) =>
                                {
                                    this.Close();
                                };
                                mainGrid.BeginAnimation(Grid.OpacityProperty, animationOpacity);
                                WindowMainPanelAdmin windowMainPanelAdmin = new WindowMainPanelAdmin();
                                windowMainPanelAdmin.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Неправильный логин или пароль. Пожалуйста, повторите попытку.", "Сообщение");
                            }
                        }
                    }
                }
                else
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", username);
                        command.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DataBank.Id = (int)reader["IDuser"];
                                DoubleAnimation animationOpacity = new DoubleAnimation();
                                animationOpacity.Duration = TimeSpan.FromSeconds(1);
                                animationOpacity.To = 0;
                                animationOpacity.Completed += (s, ea) =>
                                {
                                    this.Close();
                                };
                                mainGrid.BeginAnimation(Grid.OpacityProperty, animationOpacity);
                                workWindow workWindow = new workWindow();
                                workWindow.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Неправильный логин или пароль. Пожалуйста, повторите попытку.", "Сообщение");
                            }
                        }
                    }
                }
            }
        }
    }
}
