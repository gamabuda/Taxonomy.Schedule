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
    /// Логика взаимодействия для AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        Employee Employee = new Employee();
        List<Brigade> Brigades = db_connection.connection.Brigade.ToList();
        List<Post> Posts = db_connection.connection.Post.ToList();
        public AddUserPage()
        {
            InitializeComponent();

            postCB.ItemsSource = Posts;
            brigadeCB.ItemsSource = Brigades;
            DataContext = Employee;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if(this.NavigationService.CanGoBack)
                this.NavigationService.GoBack();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db_connection.connection.Employee.Add(Employee);
                if (this.NavigationService.CanGoBack)
                    this.NavigationService.GoBack();
            }
            catch
            {
                MessageBox.Show("Данные некорректны! Проверьте их и пропробуйте снова");
            }
        }
    }
}
