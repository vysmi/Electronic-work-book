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
    public partial class WindowPersonal : Window
    {
        public WindowPersonal()
        {
            InitializeComponent();
            ButtonReward();
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
        private void ButtonReward()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True");
            connection.Open();
            string query = $"SELECT * FROM PersonInformation;";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable("PersonInformation");
            adapter.Fill(table);
            ConvertAdminDataGrid.ItemsSource = table.DefaultView;
            connection.Close();
        }
        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (idRecord.Text == "" || firstName.Text == "" || userName.Text == "" || lastName.Text == "" || dateBirthday.Text == "" || education.Text == "" || profession.Text == "" || userID.Text == "" || dateInput.Text == "")
            {
                MessageBox.Show("Одно или несколько полей не заполнены! Все поля должны содержать значения.", "Ошибка");
                return;
            }
            using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True"))
            {
                connection.Open();
                string query = "INSERT INTO PersonInformation (FirstName, Name, LastName, Education, Profession, IDUser, DateOfBirthday, DateOfInput) VALUES (@FirstName, @Name, @LastName, @Education, @Profession, @IDUser, @DateOfBirthday, @DateOfInput)";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@FirstName", firstName.Text);
                command.Parameters.AddWithValue("@Name", userName.Text);
                command.Parameters.AddWithValue("@LastName", lastName.Text);
                command.Parameters.AddWithValue("@DateOfBirthday", dateBirthday.Text);
                command.Parameters.AddWithValue("@Education", education.Text);
                command.Parameters.AddWithValue("@Profession", profession.Text);
                command.Parameters.AddWithValue("@IDUser", userID.Text);
                command.Parameters.AddWithValue("@DateOfInput", dateInput.Text);

                command.ExecuteNonQuery();
            }
            ButtonReward();
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            int selectedId = GetSelectedId();
            if (selectedId != -1)
            {
                string newFirstName = firstName.Text;
                string newUserName = userName.Text;
                string newLastName = lastName.Text;
                string newDateBirthday = dateBirthday.Text;
                string newEducation = education.Text;
                string newProfession = profession.Text;
                string newUserID = userID.Text;
                string newDateInput = dateInput.Text;

                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True"))
                {
                    connection.Open();
                    string sql = "UPDATE PersonInformation SET FirstName = @FirstName, Name = @Name, LastName = @LastName, Education = @Education, Profession = @Profession, IDUser = @IDUser, DateOfBirthday = @DateOfBirthday, DateOfInput = DateOfInput WHERE IDpersonInf = @Id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", selectedId);
                        command.Parameters.AddWithValue("@FirstName", newFirstName);
                        command.Parameters.AddWithValue("@Name", newUserName);
                        command.Parameters.AddWithValue("@LastName", newLastName);
                        command.Parameters.AddWithValue("@DateOfBirthday", newDateBirthday);
                        command.Parameters.AddWithValue("@Education", newEducation);
                        command.Parameters.AddWithValue("@Profession", newProfession);
                        command.Parameters.AddWithValue("@IDUser", newUserID);
                        command.Parameters.AddWithValue("@DateOfInput", newDateInput);
                        command.ExecuteNonQuery();
                        ButtonReward();
                    }
                }
            }
        }
        private int GetSelectedId()
        {
            if (ConvertAdminDataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)ConvertAdminDataGrid.SelectedItem;
                int selectedId = (int)row["IDpersonInf"];
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
                string sql = "DELETE FROM PersonInformation WHERE IDpersonInf = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", selectedId);
                    command.ExecuteNonQuery();
                    ButtonReward();
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
                    idRecord.Text = selectedRow["IDpersonInf"].ToString();
                    firstName.Text = selectedRow["FirstName"].ToString();
                    userName.Text = selectedRow["Name"].ToString();
                    lastName.Text = selectedRow["LastName"].ToString();
                    dateBirthday.Text = selectedRow["DateOfBirthday"].ToString();
                    education.Text = selectedRow["Education"].ToString();
                    profession.Text = selectedRow["Profession"].ToString();
                    userID.Text = selectedRow["IDUser"].ToString();
                    dateInput.Text = selectedRow["DateOfInput"].ToString();
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
