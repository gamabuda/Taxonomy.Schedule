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
using Taxonomy.Schedule.Wpf.Windows;

namespace Taxonomy.Schedule.Wpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для MsgPage.xaml
    /// </summary>
    public partial class MsgPage : Page
    {

        private List<Chat> Chats;
        private Chat SelectedChat;
        public MsgPage()
        {
            InitializeComponent();

            Chats = db_connection.connection.Chat.Where(x => x.Owner_Id == db_connection.Employee.Id).ToList();
            Chats.Reverse();

            if (Chats.Count > 0 )
            {

                AlertCard.Visibility = Visibility.Hidden;
                InfoCard.Visibility = Visibility.Hidden;
                UnselectedCard.Visibility = Visibility.Visible;
            }
            else
            {
                AlertCard.Visibility = Visibility.Visible;
                InfoCard.Visibility = Visibility.Hidden;
            }

            MsgIC.ItemsSource = Chats;
            this.DataContext = this;

            if(db_connection.Employee.Post.Id != 1)
            {
                StatusCB.Visibility = Visibility.Hidden;
                ActionWP.Visibility = Visibility.Hidden;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MsgIC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MsgIC.SelectedItem == null) return;

            SelectedChat = MsgIC.SelectedItem as Chat;
            InfoCard.DataContext = SelectedChat;

            InfoCard.Visibility = Visibility.Visible;
            UnselectedCard.Visibility = Visibility.Collapsed;

            if (SelectedChat.IsReaded == false)
            {
                SelectedChat.IsReaded = true;
                db_connection.connection.SaveChanges();

                Chats = db_connection.connection.Chat.Where(x => x.Owner_Id == db_connection.Employee.Id).ToList();
                Chats.Reverse();
                MsgIC.ItemsSource = Chats;

            }

            if (db_connection.Employee.Post.Id != 1)
                return;

            if (SelectedChat.IsAllowed == true)
            {
                ActionWP.Visibility = Visibility.Visible;
                StatusCB.SelectedIndex = 0;
            }
            else if (SelectedChat.IsAllowed == false)
            {
                ActionWP.Visibility = Visibility.Visible;
                StatusCB.SelectedIndex = 1;
            }
            else
            {
                ActionWP.Visibility = Visibility.Hidden;
                StatusCB.Text = string.Empty;
            }
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;

            if (string.IsNullOrEmpty(cb.Text)) return;

            if(cb.Text == "Одобрено")
            {
                SelectedChat.IsAllowed = true;
                db_connection.connection.SaveChanges();
            }
            else
            {
                SelectedChat.IsAllowed = false;
                db_connection.connection.SaveChanges();
            }

            ActionWP.Visibility = Visibility.Visible;
        }

        private void DocBtn_Click(object sender, RoutedEventArgs e)
        {
            new ReqstDocumentWindow(SelectedChat).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            db_connection.connection.Chat.Add(new Chat()
            {
                Employee = db_connection.Employee,
                Employee1 = SelectedChat.Employee,
                Request = SelectedChat.Request,
                Date = DateTime.Now,
                IsReaded = false,
                IsAllowed= false,
                Message = $"Дорогой {SelectedChat.Employee.Surname} {SelectedChat.Employee.Name}, мы обработали вашу заявку на {SelectedChat.Request.Title} и вынесли итог. Вердикт по вашему запросу: {((bool)SelectedChat.IsAllowed ? "Одобрен" : "Отказ")}"
            });

            db_connection.connection.SaveChanges();
        }
    }
}
