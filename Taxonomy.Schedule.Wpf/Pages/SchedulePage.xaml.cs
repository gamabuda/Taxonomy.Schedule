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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Taxonomy.Schedule.Wpf.Data;
using OfficeOpenXml;
using System.IO;
using System.Windows.Forms;
using System.Data;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace Taxonomy.Schedule.Wpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        List<BrigadeShedule> brigadeShedules;
        public SchedulePage()
        {
            InitializeComponent();

            if(db_connection.Employee.Id == 1)
            {
                brigadeShedules = db_connection.connection.BrigadeShedule.OrderBy(x => x.Shedule.DayOfWeek).ToList();
            }
            else
            {
                var lst = db_connection.connection.BrigadeShedule.OrderBy(x => x.Shedule.DayOfWeek).ToList();
                brigadeShedules = lst.Where(x => x.Id_Brigade == db_connection.Employee.Id_Brigade).ToList();

                actionWP.Visibility = Visibility.Collapsed;
            }
            LV.ItemsSource = brigadeShedules;

            DataContext = this;
        }

        private void LV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LV.SelectedItem != null)
            {
                BrigadeShedule toRedact = LV.SelectedItem as BrigadeShedule;
                NavigationService.Navigate(new AddSchedulePage(toRedact));
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddSchedulePage());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GeneratePage());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ExportToExcel();
        }

        private void ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var itemList = brigadeShedules;
            // Создаем новый пакет Excel
            ExcelPackage excelPackage = new ExcelPackage();

            // Добавляем рабочую книгу
            ExcelWorkbook workbook = excelPackage.Workbook;
            ExcelWorksheet worksheet = workbook.Worksheets.Add("Sheet1");

            // Заполняем заголовки столбцов
            worksheet.Cells[1, 1].Value = "Дата";
            worksheet.Cells[1, 2].Value = "Штаб";
            worksheet.Cells[1, 3].Value = "Смена";

            // Заполняем ячейки данными из списка
            for (int i = 0; i < itemList.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = itemList[i].Shedule.DayOfWeek.Value.ToShortDateString();
                worksheet.Cells[i + 2, 2].Value = itemList[i].Brigade.Name;
                worksheet.Cells[i + 2, 3].Value = itemList[i].TypeChange.Name;
            }

            System.Windows.Forms.FolderBrowserDialog dialog = new FolderBrowserDialog();

            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = dialog.SelectedPath;
                // Выбранный путь к папке
                selectedPath += @"\\list.xlsx";

                // Сохраняем файл Excel
                FileInfo excelFile = new FileInfo(selectedPath);
                excelPackage.SaveAs(excelFile);
            }
        }

    }
}
