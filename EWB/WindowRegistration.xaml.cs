using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    public partial class WindowRegistration : Window
    {
        public WindowRegistration()
        {
            InitializeComponent();
            ButtonPerson();
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
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            WindowMainPanelAdmin windowMainPanelAdmin = new WindowMainPanelAdmin();

            DoubleAnimation animationOpacity = new DoubleAnimation();
            animationOpacity.Duration = TimeSpan.FromSeconds(1);
            animationOpacity.To = 0;
            animationOpacity.Completed += (s, ea) =>
            {
                this.Close();
            };
            mainGrid.BeginAnimation(Grid.OpacityProperty, animationOpacity);
            windowMainPanelAdmin.Show();
        }
        private void ButtonPerson()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True");
            connection.Open();
            string query = $"SELECT * FROM Users;";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable("Users");
            adapter.Fill(table);
            ConvertAdminDataGrid.ItemsSource = table.DefaultView;
            connection.Close();
        }
        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (userID.Text == "" || userLogin.Text == "" || userPassword.Text == "" || firstName.Text == "" || userName.Text == "" || lastName.Text == "" || userPosition.Text == "")
            {
                MessageBox.Show("Одно или несколько полей не заполнены! Все поля должны содержать значения.", "Ошибка");
                return;
            }
            using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True"))
            {
                connection.Open();
                string query = "INSERT INTO Users (Login, Password, FirstName, Name, LastName, Position) VALUES (@Login, @Password, @FirstName, @Name, @LastName, @Position)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Login", userLogin.Text);
                command.Parameters.AddWithValue("@Password", userPassword.Text);
                command.Parameters.AddWithValue("@FirstName", firstName.Text);
                command.Parameters.AddWithValue("@Name", userName.Text);
                command.Parameters.AddWithValue("@LastName", lastName.Text);
                command.Parameters.AddWithValue("@Position", userPosition.Text);
                command.ExecuteNonQuery();
            }
            ButtonPerson();
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            int selectedId = GetSelectedId();
            if (selectedId != -1)
            {
                string newUserID = userID.Text;
                string newUserLogin = userLogin.Text;
                string newUserPassword = userPassword.Text;
                string newFirstName = firstName.Text;
                string newName = userName.Text;
                string newLastName = lastName.Text;
                string newUserPosition = userPosition.Text;

                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True"))
                {
                    connection.Open();
                    string sql = "UPDATE Users SET Login = @Login, Password = @Password, FirstName = @FirstName, Name = @Name, LastName = @LastName, Position = @Position WHERE IDuser = @Id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", selectedId);
                        command.Parameters.AddWithValue("@Login", userLogin.Text);
                        command.Parameters.AddWithValue("@Password", userPassword.Text);
                        command.Parameters.AddWithValue("@FirstName", firstName.Text);
                        command.Parameters.AddWithValue("@Name", userName.Text);
                        command.Parameters.AddWithValue("@LastName", lastName.Text);
                        command.Parameters.AddWithValue("@Position", userPosition.Text);
                        command.ExecuteNonQuery();
                        ButtonPerson();
                    }
                }
            }
        }
        private int GetSelectedId()
        {
            if (ConvertAdminDataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)ConvertAdminDataGrid.SelectedItem;
                int selectedId = (int)row["IDuser"];
                return selectedId;
            }
            return -1;
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int selectedId = GetSelectedId();
            using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True"))
            {
                connection.Open();
                string sql = "DELETE FROM Users WHERE IDuser = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", selectedId);
                    command.ExecuteNonQuery();
                    ButtonPerson();
                }

            }
        }
        private void ConvertAdminDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ConvertAdminDataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = ConvertAdminDataGrid.SelectedItem as DataRowView;
                if (selectedRow != null)
                {                   
                    userID.Text = selectedRow["IDuser"].ToString();
                    userLogin.Text = selectedRow["Login"].ToString();
                    userPassword.Text = selectedRow["Password"].ToString();
                    firstName.Text = selectedRow["FirstName"].ToString();
                    userName.Text = selectedRow["Name"].ToString();
                    lastName.Text = selectedRow["LastName"].ToString();
                    userPosition.Text = selectedRow["Position"].ToString();
                }
            }
        }
        private void searchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            string sText = searchText.Text;
            SearchAndMoveToTop(sText);
        }

        private void SearchAndMoveToTop(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText) || ConvertAdminDataGrid.Items.Count == 0)
                return;

            DataRowView foundRow = null;
            foreach (DataRowView row in ConvertAdminDataGrid.Items)
            {
                foreach (var item in row.Row.ItemArray)
                {
                    if (item.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        foundRow = row;
                        break;
                    }
                }
                if (foundRow != null)
                    break;
            }

            if (foundRow != null)
            {
                ConvertAdminDataGrid.ScrollIntoView(foundRow);
                ConvertAdminDataGrid.SelectedItem = foundRow;
            }
        }
    }
}
