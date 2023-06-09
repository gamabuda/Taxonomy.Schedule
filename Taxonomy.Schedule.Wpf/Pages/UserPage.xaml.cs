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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public Employee Employee { get; set; }
        public UserPage()
        {
            InitializeComponent();

            Employee = db_connection.Employee;
            PasswordTB.Password = Employee.Password;
            this.DataContext = Employee;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Employee = db_connection.Employee;
            PasswordTB.Password = Employee.Password;
            MsgWP.Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(PasswordTB.Password) && string.IsNullOrEmpty(LoginTB.Text))
            {
                MsgWP.Visibility = Visibility.Visible;
                return;
            }

            Employee.Password = PasswordTB.Password;

            try
            {
                db_connection.connection.SaveChanges();
                MsgWP.Visibility = Visibility.Hidden;
            }
            catch
            {
                MsgWP.Visibility = Visibility.Visible;
            }
        }
    }
}
