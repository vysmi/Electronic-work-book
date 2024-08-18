using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Policy;
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
using OfficeOpenXml;
using System.IO;

namespace EWB
{
    public partial class workWindow : Window
    {
        public workWindow()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            InitializeComponent();
            ButtonPersonal();
            ActiveUser();
        }
        private void windowMoused(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        
        private void btnMinimize2_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose2_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animationOpacity = new DoubleAnimation();
            animationOpacity.Duration = TimeSpan.FromSeconds(0.1);
            animationOpacity.To = 0;
            animationOpacity.Completed += (s, ea) =>
            {
                Application.Current.Shutdown();
            };
            mainWorkGrid.BeginAnimation(Grid.OpacityProperty, animationOpacity);            
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
            mainWorkGrid.BeginAnimation(Grid.OpacityProperty, animationOpacity);
            authWindow.Show();
        }
        public class UserData
        {
            public string Name { get; set; }
            public string LastName { get; set; }
            public string Position { get; set; }
        }
        private void ActiveUser()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True");
            connection.Open();
            string query = $"SELECT * FROM Users WHERE {DataBank.Id} = IDuser;";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable("Users");
            adapter.Fill(table);
            connection.Close();
            UserData personalData = new UserData()
            {
                Name = table.Rows[0]["Name"].ToString(),
                LastName = table.Rows[0]["LastName"].ToString(),
                Position = table.Rows[0]["Position"].ToString(),
            };
            actName.Text = personalData.Name;
            actLastName.Text = personalData.LastName;
            actPosition.Text = personalData.Position;
        }
        public class PersonalData
        {
            public string FirstName { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string DateOfBirthday { get; set; }
            public string Education { get; set; }
            public string Profession { get; set; }
            public string DateOfInput { get; set; }
            public string EDSowner { get; set; }
            public string EDSinput { get; set; }
        }
        private void ButtonPersonal()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True");
            connection.Open();
            string query = $"SELECT * FROM PersonInformation WHERE {DataBank.Id} = IDuser;";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable("PersonInformation");
            adapter.Fill(table);
            connection.Close();
            PersonalData personalData = new PersonalData()
            {
                FirstName = table.Rows[0]["FirstName"].ToString(),
                Name = table.Rows[0]["Name"].ToString(),
                LastName = table.Rows[0]["LastName"].ToString(),
                DateOfBirthday = Convert.ToDateTime(table.Rows[0]["DateOfBirthday"]).ToString("yyyy-MM-dd"),
                Education = table.Rows[0]["Education"].ToString(),
                Profession = table.Rows[0]["Profession"].ToString(),
                DateOfInput = Convert.ToDateTime(table.Rows[0]["DateOfInput"]).ToString("yyyy-MM-dd"),
                EDSowner = table.Rows[0]["EDSowner"].ToString(),
                EDSinput = table.Rows[0]["EDSinput"].ToString()
            };
            txtFirstName.Text = personalData.FirstName;
            txtPersonalName.Text = personalData.Name;
            txtLastName.Text = personalData.LastName;
            txtDateOfBirthday.Text = personalData.DateOfBirthday;
            txtEducation.Text = personalData.Education;
            txtProfession.Text = personalData.Profession;
            txtDateOfInput.Text = personalData.DateOfInput;
        }
        private void ButtonPersonal_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animationOpacity = new DoubleAnimation();
            animationOpacity.Duration = TimeSpan.FromSeconds(0);
            animationOpacity.To = 1;
            animationOpacity.Completed += (s, ea) =>
            {
                gridPersonal.Visibility = Visibility.Visible;
                gridData.Visibility = Visibility.Hidden;
                RewardGridData.Visibility = Visibility.Hidden;
                search.Visibility = Visibility.Hidden;
                searchText.Visibility = Visibility.Hidden;
            };
            gridPersonal.BeginAnimation(Grid.OpacityProperty, animationOpacity);           
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
            MainDataGrid.ItemsSource = table.DefaultView;
            connection.Close();
        }
        private void ButtonInfWork_Click(object sender, RoutedEventArgs e)
        {                        
            DoubleAnimation animationOpacity = new DoubleAnimation();
            animationOpacity.Duration = TimeSpan.FromSeconds(0);
            animationOpacity.To = 1;
            animationOpacity.Completed += (s, ea) =>
            {
                gridData.Visibility = Visibility.Visible;
                ButtonWork();
                gridPersonal.Visibility = Visibility.Hidden;
                RewardGridData.Visibility = Visibility.Hidden;
                search.Visibility = Visibility.Visible;
                searchText.Visibility = Visibility.Visible;
            };
            gridData.BeginAnimation(Grid.OpacityProperty, animationOpacity);

           
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
            RewardDataGrid.ItemsSource = table.DefaultView;
            connection.Close();
        }
        private void ButtonReward_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animationOpacity3 = new DoubleAnimation();
            animationOpacity3.Duration = TimeSpan.FromSeconds(0);
            animationOpacity3.To = 1;
            animationOpacity3.Completed += (s, ea) =>
            {
                RewardGridData.Visibility = Visibility.Visible;
                ButtonReward();
                gridPersonal.Visibility = Visibility.Hidden;
                gridData.Visibility = Visibility.Hidden;
                search.Visibility = Visibility.Visible;
                searchText.Visibility = Visibility.Visible;
            };
            RewardGridData.BeginAnimation(Grid.OpacityProperty, animationOpacity3);

        }

        private void ExportTextBlockDataToExcel(TextBlock textBlock, ExcelWorksheet worksheet, int rowIndex, int columnIndex)
        {
            string text = textBlock.Text;

            // Проверяем содержание даты
            DateTime dateValue;
            if (DateTime.TryParse(text, out dateValue))
            {
                worksheet.Cells[rowIndex, columnIndex].Value = dateValue;
                worksheet.Cells[rowIndex, columnIndex].Style.Numberformat.Format = "yyyy-mm-dd"; // Устанавливаем формат даты
            }
            else
            {
                worksheet.Cells[rowIndex, columnIndex].Value = text;
            }
        }
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if(gridPersonal.Visibility == Visibility.Visible)
            {
                var excelPackagePer = new ExcelPackage();
                var worksheetPer = excelPackagePer.Workbook.Worksheets.Add("Data");

                //Bндекс строки и столбца, с которого начинается запись в Excel
                int rowIndex = 1;
                int columnIndex = 1;

                ExportTextBlockDataToExcel(columnFirstName, worksheetPer, rowIndex, columnIndex);
                ExportTextBlockDataToExcel(columnName, worksheetPer, rowIndex, columnIndex + 1);
                ExportTextBlockDataToExcel(columnLastName, worksheetPer, rowIndex, columnIndex + 2);
                ExportTextBlockDataToExcel(columnDateOfBirthday, worksheetPer, rowIndex, columnIndex + 3);
                ExportTextBlockDataToExcel(columnEducation, worksheetPer, rowIndex, columnIndex + 4);
                ExportTextBlockDataToExcel(columnProfession, worksheetPer, rowIndex, columnIndex + 5);
                ExportTextBlockDataToExcel(columnDateOfInput, worksheetPer, rowIndex, columnIndex + 6);
                ExportTextBlockDataToExcel(txtFirstName, worksheetPer, rowIndex + 1, columnIndex);
                ExportTextBlockDataToExcel(txtPersonalName, worksheetPer, rowIndex + 1, columnIndex + 1);
                ExportTextBlockDataToExcel(txtLastName, worksheetPer, rowIndex + 1, columnIndex + 2);
                ExportTextBlockDataToExcel(txtDateOfBirthday, worksheetPer, rowIndex + 1, columnIndex + 3);
                ExportTextBlockDataToExcel(txtEducation, worksheetPer, rowIndex + 1, columnIndex + 4);
                ExportTextBlockDataToExcel(txtProfession, worksheetPer, rowIndex + 1, columnIndex + 5);
                ExportTextBlockDataToExcel(txtDateOfInput, worksheetPer, rowIndex + 1, columnIndex + 6);
                worksheetPer.Cells[worksheetPer.Dimension.Address].AutoFitColumns();
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    var file = new FileInfo(saveFileDialog.FileName);
                    excelPackagePer.SaveAs(file);
                    MessageBox.Show("Экспорт успешно завершен!", "Сообщение");
                }
            }
            else
            {
                var excelPackage = new ExcelPackage();
                var worksheet = excelPackage.Workbook.Worksheets.Add("Data");

                DataGrid activeDataGrid = GetActiveDataGrid(); //Получаем активную таблицу

                if (activeDataGrid != null)
                {
                    //Столбцы
                    for (int columnIndex = 0; columnIndex < activeDataGrid.Columns.Count; columnIndex++)
                    {
                        worksheet.Cells[1, columnIndex + 1].Value = activeDataGrid.Columns[columnIndex].Header;
                    }

                    //Строки
                    for (int rowIndex = 0; rowIndex < activeDataGrid.Items.Count; rowIndex++)
                    {
                        for (int columnIndex = 0; columnIndex < activeDataGrid.Columns.Count; columnIndex++)
                        {
                            var cellValue = (activeDataGrid.Items[rowIndex] as DataRowView).Row[columnIndex];
                            if (activeDataGrid.Columns[columnIndex].Header.ToString() == "Дата") // Проверяем, что это столбец с датами
                            {
                                DateTime dateValue;
                                if (DateTime.TryParse(cellValue.ToString(), out dateValue)) // Проверяем, что значение действительно дата
                                {
                                    worksheet.Cells[rowIndex + 2, columnIndex + 1].Value = dateValue;
                                    worksheet.Cells[rowIndex + 2, columnIndex + 1].Style.Numberformat.Format = "yyyy-mm-dd";
                                }
                                else
                                {
                                    worksheet.Cells[rowIndex + 2, columnIndex + 1].Value = cellValue;
                                }
                            }
                            else
                            {
                                worksheet.Cells[rowIndex + 2, columnIndex + 1].Value = cellValue;
                            }
                        }
                    }
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        var file = new FileInfo(saveFileDialog.FileName);
                        excelPackage.SaveAs(file);
                        MessageBox.Show("Экспорт успешно завершен!", "Сообщение");
                    }
                }
            }           
        }
        //Получение активной таблицы
        private DataGrid GetActiveDataGrid()
        {
            if (gridData.Visibility == Visibility.Visible)
            {
                SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True");
                connection.Open();
                string query = $"SELECT IDorder, DateOfEntry,JobDetails, DocEntryMade FROM DataWork WHERE {DataBank.Id} = IDuser;";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable("DataWork");
                adapter.Fill(table);
                connection.Close();
                ConvertMainDataGrid.ItemsSource = table.DefaultView;
                return ConvertMainDataGrid;
            }
            else if (RewardGridData.Visibility == Visibility.Visible)
            {
                SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-R5ART7A\SQLEXPRESS; Initial Catalog=WorkBooksDB; Integrated Security=True");
                connection.Open();
                string query = $"SELECT IDorder, DateOfEntryRew,RewardDetails, DocEntryMadeRew FROM DataReward WHERE {DataBank.Id} = IDuser;";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable("DataReward");
                adapter.Fill(table);
                connection.Close();
                ConvertRewardDataGrid.ItemsSource = table.DefaultView;
                return ConvertRewardDataGrid;
            }
            else
            {
                return null;
            }
        }
        private void searchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            string sText = searchText.Text;
            SearchAndMoveToTop(sText);
        }
        private void SearchAndMoveToTop(string searchText)
        {
            if(gridData.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrWhiteSpace(searchText) || MainDataGrid.Items.Count == 0)
                {
                    return;
                }

                DataRowView foundRow = null;
                foreach (DataRowView row in MainDataGrid.Items)
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
                    MainDataGrid.ScrollIntoView(foundRow);
                    MainDataGrid.SelectedItem = foundRow;
                }
            }
            else if (RewardGridData.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrWhiteSpace(searchText) || RewardDataGrid.Items.Count == 0)
                {
                    return;
                }

                DataRowView foundRow = null;
                foreach (DataRowView row in RewardDataGrid.Items)
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
                    RewardDataGrid.ScrollIntoView(foundRow);
                    RewardDataGrid.SelectedItem = foundRow;
                }
            }
        }
    }
}
