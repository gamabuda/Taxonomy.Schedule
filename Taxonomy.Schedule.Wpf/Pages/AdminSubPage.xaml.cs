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
    /// Логика взаимодействия для AdminSubPage.xaml
    /// </summary>
    public partial class AdminSubPage : Page
    {
        List<Shedule> shedules;
        List<Brigade> brigades;
        List<TypeChange> typeChanges;
        List<BrigadeShedule> brigadeShedules;
        BrigadeShedule curr = new BrigadeShedule();
        bool add;
        public AdminSubPage()
        {
            InitializeComponent();
            this.DataContext= this;
            shedules = db_connection.connection.Shedule.ToList();
            brigades = db_connection.connection.Brigade.ToList();
            typeChanges = db_connection.connection.TypeChange.ToList();

            dayCB.ItemsSource = shedules.ToList();
            brigadeCB.ItemsSource = brigades.ToList();
            TypeCB.ItemsSource = typeChanges.ToList();

            add = true;
            
        }

        public AdminSubPage(BrigadeShedule brigadeShedule)
        {
            InitializeComponent();
            curr = brigadeShedule;
            shedules = db_connection.connection.Shedule.ToList();
            brigades = db_connection.connection.Brigade.ToList();
            typeChanges = db_connection.connection.TypeChange.ToList();
            add = false;

            dayCB.ItemsSource = shedules.Select(x => x.DayOfWeek).ToList();
            brigadeCB.ItemsSource = brigades.Select(x => x.Name).ToList();
            TypeCB.ItemsSource = brigades.Select(x => x.Name).ToList();
            //foreach (var shedule in shedules)
            //{
            //    dayCB.Items.Add(shedule.DayOfWeek);
            //}
            //foreach (var brigade in brigades)
            //{
            //    brigadeCB.Items.Add(brigade.Name);
            //}
            //foreach (var typeChange in typeChanges)
            //{
            //    TypeCB.Items.Add(typeChange.typeChange);
            //}
            dayCB.SelectedIndex = (int)brigadeShedule.Id_Shedule - 1;
            brigadeCB.SelectedIndex = (int)brigadeShedule.Brigade.Id - 1;
            TypeCB.SelectedIndex = (int)brigadeShedule.Id_TypeChange - 1;
           
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            brigadeShedules = db_connection.connection.BrigadeShedule.ToList();
            if (curr.Id_Shedule == null || curr.Id_TypeChange == null || curr.Id_Brigade == null)
            {
                MessageBox.Show("Что-то пошло не так");
            }
            else
            {
                BrigadeShedule check = brigadeShedules.Where(x => x.Id_Brigade == brigadeCB.SelectedIndex - 1 && x.Id_Shedule == dayCB.SelectedIndex - 1).FirstOrDefault();
                if (check == null)
                {
                    if (add)
                    {
                        db_connection.connection.BrigadeShedule.Add(curr);
                        db_connection.connection.SaveChanges();
                    }
                    else
                    {
                        check = db_connection.connection.BrigadeShedule.Where(x => x.Id_Brigade == curr.Id_Brigade && x.Id_Shedule == curr.Id_Shedule).FirstOrDefault() as BrigadeShedule;
                        check.Id_Shedule = dayCB.SelectedIndex - 1;
                        check.Id_Brigade = brigadeCB.SelectedIndex - 1;
                        db_connection.connection.SaveChanges();

                    }
                }
                else
                {
                    MessageBox.Show("Бригада уже работает в этот день");
                }
            }
            NavigationService.GoBack();
        }

        private void dayCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //curr.Id_Shedule = dayCB.SelectedIndex - 1;
        }

        private void brigadeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //curr.Id_Brigade = brigadeCB.SelectedIndex - 1;
        }

        private void TypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //curr.Id_TypeChange = TypeCB.SelectedIndex - 1;
        }
    }
}
