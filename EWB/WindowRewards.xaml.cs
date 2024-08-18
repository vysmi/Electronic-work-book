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
using OfficeOpenXml.Style;

namespace EWB
{
    public partial class WindowRewards : Window
    {
        public WindowRewards()
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
            string query = $"SELECT * FROM DataReward WHERE {DataBank.Id} = IDuser;";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable("DataReward");
            adapter.Fill(table);
            ConvertAdminDataGrid.ItemsSource = table.DefaultView;
            connection.Close();
        }
        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (idTxt.Text == "" || reportRecord.Text == "" || rewardInfo.Text == "" || dateRec.Text == "" || numberRecord.Text == "")
            {
                MessageBox.Show("Одно или несколько полей не заполнены! Все поля должны содержать значения.", "Ошибка");
                return;
            }
            using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True"))
            {
                connection.Open();
                string query = "INSERT INTO DataReward (DateOfEntryRew, RewardDetails, DocEntryMadeRew, IDuser, IDorder) VALUES (@DateOfEntryRew, @RewardDetails, @DocEntryMadeRew, @IDuser, @IDorder)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IDorder", numberRecord.Text);
                command.Parameters.AddWithValue("@DateOfEntryRew", dateRec.Text);
                command.Parameters.AddWithValue("@RewardDetails", rewardInfo.Text);
                command.Parameters.AddWithValue("@DocEntryMadeRew", reportRecord.Text);
                command.Parameters.AddWithValue("@IDuser", idTxt.Text);

                command.ExecuteNonQuery();
            }
            ButtonReward();
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            int selectedId = GetSelectedId();
            if (selectedId != -1)
            {
                string newNumberRecord = numberRecord.Text;
                string newDateRecord = dateRec.Text;
                string newRewardInfo = rewardInfo.Text;
                string newReportRecord = reportRecord.Text;
                string newIdTxt = idTxt.Text;

                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True"))
                {
                    connection.Open();
                    string sql = "UPDATE DataReward SET DateOfEntryRew = @DateOfEntryRew, RewardDetails = @RewardDetails, DocEntryMadeRew = @DocEntryMadeRew, IDuser = @IDuser, IDorder = @IDorder WHERE IDentryReward = @Id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", selectedId);
                        command.Parameters.AddWithValue("@IDorder", newNumberRecord);
                        command.Parameters.AddWithValue("@DateOfEntryRew", newDateRecord);
                        command.Parameters.AddWithValue("@RewardDetails", newRewardInfo);
                        command.Parameters.AddWithValue("@DocEntryMadeRew", newReportRecord);
                        command.Parameters.AddWithValue("@IDuser", newIdTxt);
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
                int selectedId = (int)row["IDentryReward"];
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
                string sql = "DELETE FROM DataReward WHERE IDentryReward = @Id";
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
                    numberRecord.Text = selectedRow["IDorder"].ToString();
                    dateRec.Text = selectedRow["DateOfEntryRew"].ToString();
                    rewardInfo.Text = selectedRow["RewardDetails"].ToString();
                    reportRecord.Text = selectedRow["DocEntryMadeRew"].ToString();
                    idTxt.Text = selectedRow["IDuser"].ToString();

                }
            }
        }
        private void searchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            string sText = searchText.Text;
            SearchAndMoveToTop(sText);
        }
        private void btnAddId_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(idTxt.Text);
            DataBank.Id = id;
            ButtonReward();
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
