using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using EWB;
using static EWB.workWindow;

namespace EWB
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            ButtonWork();
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
        private void ButtonWork()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True");
            connection.Open();
            string query = $"SELECT * FROM DataWork WHERE {DataBank.Id} = IDuser;";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable("DataWork");
            adapter.Fill(table);
            ConvertAdminDataGrid.ItemsSource = table.DefaultView;
            connection.Close();
        }
        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (numberRecord.Text == "" || dateRec.Text == "" || infWork.Text == "" || reportRecord.Text == "" || idTxt.Text == "")
            {
                MessageBox.Show("Одно или несколько полей не заполнены! Все поля должны содержать значения.", "Ошибка");
                return;
            }
            using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True"))
            {
                connection.Open();
                string query = "INSERT INTO DataWork (IDuser, DateOfEntry, JobDetails, DocEntryMade, IDorder) VALUES (@IDuser, @DateofEntry, @Jobdetails, @DocentryMade, @Idorder)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IDuser", idTxt.Text);
                command.Parameters.AddWithValue("@Idorder", numberRecord.Text);
                command.Parameters.AddWithValue("@DateofEntry", dateRec.Text);
                command.Parameters.AddWithValue("@Jobdetails", infWork.Text);
                command.Parameters.AddWithValue("@DocentryMade", reportRecord.Text);
                command.ExecuteNonQuery();
            }
            ButtonWork();
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            int selectedId = GetSelectedId();
            if (selectedId != -1)
            {
                string newNumberRec = numberRecord.Text;
                string newDateRec = dateRec.Text;
                string newinfWork = infWork.Text;
                string newReportRecord = reportRecord.Text;

                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True"))
                {
                    connection.Open();
                    string sql = "UPDATE DataWork SET DateOfEntry = @DateofEntry, JobDetails = @Jobdetails, DocEntryMade = @DocentryMade, IDorder = @Idorder WHERE IDdataWork = @Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", selectedId);
                        command.Parameters.AddWithValue("@Idorder", newNumberRec);
                        command.Parameters.AddWithValue("@DateofEntry", newDateRec);
                        command.Parameters.AddWithValue("@Jobdetails", newinfWork);
                        command.Parameters.AddWithValue("@DocentryMade", newReportRecord);
                        command.ExecuteNonQuery();
                        ButtonWork();
                    }
                }
            }
        }
        private int GetSelectedId()
        {
            if (ConvertAdminDataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)ConvertAdminDataGrid.SelectedItem;
                int selectedId = (int)row["IDdataWork"];
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
                string sql = "DELETE FROM DataWork WHERE IDdataWork = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", selectedId);
                    command.ExecuteNonQuery();
                    ButtonWork();
                }

            }
        }
        private void btnAddId_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(idTxt.Text);
            DataBank.Id = id;
            ButtonWork();
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
        private void ConvertAdminDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ConvertAdminDataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = ConvertAdminDataGrid.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    numberRecord.Text = selectedRow["IDorder"].ToString();
                    dateRec.Text = selectedRow["DateOfEntry"].ToString();
                    infWork.Text = selectedRow["JobDetails"].ToString();
                    reportRecord.Text = selectedRow["DocEntryMade"].ToString();
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
