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
    /// Логика взаимодействия для GeneratePage.xaml
    /// </summary>
    public partial class GeneratePage : Page
    {
        public DateTime begin;
        public DateTime end;
        public int valueOfBridatePerChange;
        public List<Shedule> shedules;
        public List<Brigade> brigade;
        public List<BrigadeShedule> brigadeShedules;
        public GeneratePage()
        {
            InitializeComponent();

            shedules = db_connection.connection.Shedule.ToList();
            brigade = db_connection.connection.Brigade.ToList();
            brigadeShedules = db_connection.connection.BrigadeShedule.ToList();
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                begin = BeginDP.SelectedDate.Value.Date;
                end = EndDP.SelectedDate.Value.Date;
                valueOfBridatePerChange = Convert.ToInt32(valueBrigade.Text);
                if (begin <= end && begin >= DateTime.Now.Date)
                {
                    List<BrigadeShedule> toDelBrigadeShedule = db_connection.connection.BrigadeShedule.Where(x => x.Shedule.DayOfWeek >= begin && x.Shedule.DayOfWeek <= end).ToList();
                    List<Shedule> toDelShedule = db_connection.connection.Shedule.Where(x => x.DayOfWeek >= begin && x.DayOfWeek <= end).ToList();
                    foreach (var brigShed in toDelBrigadeShedule)
                    {
                        db_connection.connection.BrigadeShedule.Remove(brigShed);
                    }
                    foreach (var shedule in toDelShedule) //удаление пред расписания
                    {
                        db_connection.connection.Shedule.Remove(shedule);
                    }
                    db_connection.connection.SaveChanges();
                    int brigadeCount = brigade.Count();
                    int curBrigade = 1;
                    while (begin <= end)
                    {
                        Shedule newShed = new Shedule { DayOfWeek = begin };
                        db_connection.connection.Shedule.Add(newShed);
                        db_connection.connection.SaveChanges();
                        int idnewShed = db_connection.connection.Shedule.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                        for (int i = 0; i < valueOfBridatePerChange; i++)
                        {
                            BrigadeShedule newBrigShed = new BrigadeShedule
                            {
                                Id_Brigade = curBrigade,
                                Id_Shedule = idnewShed,
                                Id_TypeChange = 1
                            };
                            db_connection.connection.BrigadeShedule.Add(newBrigShed);
                            curBrigade++;
                            if (curBrigade >= brigadeCount) curBrigade = 1;
                        }
                        for (int i = 0; i < valueOfBridatePerChange; i++)
                        {
                            BrigadeShedule newBrigShed = new BrigadeShedule
                            {
                                Id_Brigade = curBrigade,
                                Id_Shedule = idnewShed,
                                Id_TypeChange = 2
                            };
                            db_connection.connection.BrigadeShedule.Add(newBrigShed);
                            curBrigade++;
                            if (curBrigade >= brigadeCount) curBrigade = 1;
                        }
                        begin = begin.AddDays(1);
                    }
                    db_connection.connection.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Не правильно заполнены даты");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ЧТо-то пошло не так!");
            }
            NavigationService.Navigate(new SchedulePage());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SchedulePage());
        }
    }
}
