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
    /// Логика взаимодействия для AddSchedulePage.xaml
    /// </summary>
    public partial class AddSchedulePage : Page
    {

        List<Shedule> shedules;
        List<Brigade> brigades;
        List<TypeChange> typeChanges;
        List<BrigadeShedule> brigadeShedules;
        BrigadeShedule curr = new BrigadeShedule();
        bool add;
        public AddSchedulePage()
        {
            InitializeComponent();

            shedules = db_connection.connection.Shedule.ToList();
            brigades = db_connection.connection.Brigade.ToList();
            typeChanges = db_connection.connection.TypeChange.ToList();

            dayCB.SelectedDate = DateTime.Now;

            foreach (var brigade in brigades)
            {
                brigadeCB.Items.Add(brigade.Name);
            }
            foreach (var typeChange in typeChanges)
            {
                TypeCB.Items.Add(typeChange.Name);
            }
            add = true;
        }

        public AddSchedulePage(BrigadeShedule brigadeShedule)
        {
            InitializeComponent();
            shedules = db_connection.connection.Shedule.ToList();
            brigades = db_connection.connection.Brigade.ToList();
            typeChanges = db_connection.connection.TypeChange.ToList();
            add = false;

            foreach (var brigade in brigades)
            {
                brigadeCB.Items.Add(brigade.Name);
            }
            foreach (var typeChange in typeChanges)
            {
                TypeCB.Items.Add(typeChange.Name);
            }
            add = false;
            curr = brigadeShedule;
            dayCB.SelectedDate = brigadeShedule.Shedule.DayOfWeek;
            brigadeCB.SelectedIndex = (int)brigadeShedule.Brigade.Id - 1;
            TypeCB.SelectedIndex = (int)brigadeShedule.Id_TypeChange - 1;
            dayCB.IsEnabled = false;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            brigadeShedules = db_connection.connection.BrigadeShedule.ToList();
            try
            {
                BrigadeShedule check = brigadeShedules.Where(x => x.Id_Brigade == brigadeCB.SelectedIndex + 1 && x.Shedule.DayOfWeek == dayCB.SelectedDate.Value).FirstOrDefault();
                if (check == null && add)
                {
                    if (add)
                    {
                        curr.Id_Brigade = brigadeCB.SelectedIndex + 1;
                        curr.Id_TypeChange = TypeCB.SelectedIndex + 1;
                        db_connection.connection.Shedule.Add(new Shedule { DayOfWeek = dayCB.SelectedDate.Value.Date });
                        db_connection.connection.SaveChanges();
                        curr.Id_Shedule = db_connection.connection.Shedule.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                        db_connection.connection.BrigadeShedule.Add(curr);
                        db_connection.connection.SaveChanges();
                    }
                    else
                    {


                    }
                }
                else if (check == null && !add)
                {
                    db_connection.connection.Shedule.Add(new Shedule { DayOfWeek = dayCB.SelectedDate.Value.Date });
                    db_connection.connection.SaveChanges();
                    curr.Id_Shedule = db_connection.connection.Shedule.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                    curr.Id_Brigade = brigadeCB.SelectedIndex + 1;
                    curr.Id_TypeChange = TypeCB.SelectedIndex + 1;
                    db_connection.connection.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Бригада уже работает в этот день");
                }
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так");
            }
            NavigationService.Navigate(new SchedulePage());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SchedulePage());
        }
    }
}
