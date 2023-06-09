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

namespace Taxonomy.Schedule.Wpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        List<BrigadeShedule> brigadeShedules;
        List<Shedule> shedules;
        public AdminPage()
        {
            InitializeComponent();

            brigadeShedules = db_connection.connection.BrigadeShedule.OrderBy(x => x.Id_Shedule).ToList();
            shedules = db_connection.connection.Shedule.ToList();
            //dayCB.Items.Add("Все");
            //foreach (var shedule in shedules)
            //{
            //    dayCB.Items.Add(shedule.DayOfWeek);
            //}
            LV.ItemsSource = brigadeShedules;

            DataContext = this;
        }

        private void LV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LV.SelectedItem != null)
            {
                BrigadeShedule sand = sender as BrigadeShedule;
                this.NavigationService.Navigate(new AdminSubPage(sand));
            }

        }

        void Filter()
        {
            List<BrigadeShedule> filterL = brigadeShedules;
            //if (dayCB.SelectedIndex != 0)
            //{
            //    filterL = brigadeShedules.Where(x => x.Id_Shedule == dayCB.SelectedIndex).ToList();
            //}
            LV.ItemsSource = filterL;
        }

        private void dayCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AdminSubPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AdminSubPage());
        }
    }
}
