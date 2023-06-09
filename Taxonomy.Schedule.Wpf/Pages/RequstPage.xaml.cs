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
    /// Логика взаимодействия для RequstPage.xaml
    /// </summary>
    public partial class RequstPage : Page
    {
        public RequstPage()
        {
            InitializeComponent();

            OutlinedComboBox.ItemsSource = db_connection.connection.Request.Select(x => x.Title).ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(MsgTB.Text) || string.IsNullOrEmpty(OutlinedComboBox.Text))
            {
                return;
            }

            try
            {
                db_connection.connection.Chat.Add(new Chat()
                {
                    Request = db_connection.connection.Request.FirstOrDefault(x => x.Title == OutlinedComboBox.Text),
                    Message= MsgTB.Text,
                    Employee = db_connection.Employee,
                    Employee1 = db_connection.connection.Employee.FirstOrDefault(x => x.Post.Id == 1),
                    Date = DateTime.Now,
                    IsReaded= false
                });

                db_connection.connection.SaveChanges();

                MsgTB.Text = String.Empty;
                OutlinedComboBox.Text = String.Empty;

                MsgCard.Visibility = Visibility.Visible;
            }
            catch { }
        }
    }
}
