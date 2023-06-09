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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Taxonomy.Schedule.Wpf.Data;

namespace Taxonomy.Schedule.Wpf.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(LoginTB.Text) && string.IsNullOrEmpty(PasswordTB.Password))
            {
                MsgLb.Content = "Заполните все поля";
                MsgWP.Visibility = Visibility.Visible;
                return;
            }

            var employee = db_connection.connection.Employee.FirstOrDefault(x => x.Login == LoginTB.Text && x.Password == PasswordTB.Password);
            if (employee != null)
            {
                db_connection.Employee = employee;
                new StartWindow().Show();
                this.Close();
            }
            else
            {
                MsgLb.Content = "Пользователь не найден";
                MsgWP.Visibility = Visibility.Visible;
            }
        }
    }
}
