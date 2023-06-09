using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Taxonomy.Schedule.Wpf.Data;
using Taxonomy.Schedule.Wpf.Pages;

namespace Taxonomy.Schedule.Wpf.Windows
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        private MainViewModel viewModel;
        public StartWindow()
        {
            InitializeComponent();

            viewModel = new MainViewModel();
            DataContext = viewModel;
            StartUpdatingTime();

            DateTB.Text = DateTime.Now.ToShortDateString();

            if (db_connection.Employee.Post.Id == 1)
            {
                UserAddMItem.Visibility = Visibility.Visible;
                ScheduleMItem.Visibility = Visibility.Visible;
                ReqstMItem.Visibility = Visibility.Collapsed;
            }
            else
            {
                UserAddMItem.Visibility = Visibility.Collapsed;
                ScheduleMItem.Visibility = Visibility.Visible;
                ReqstMItem.Visibility = Visibility.Visible;
            }

            UserLb.Content = $"{db_connection.Employee.Name} {db_connection.Employee.Surname}";

            UpdateMsgCount();
        }

        public void UpdateMsgCount()
        {
            MsgCount.Content = db_connection.connection.Chat.Where(x => x.Owner_Id == db_connection.Employee.Id && x.IsReaded == false).Count();
        }

        private void StartUpdatingTime()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            viewModel.CurrentTime = DateTime.Now.ToString("HH:mm:ss");
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

        private void StateBtn_Click(object sender, RoutedEventArgs e)
        {
            if(this.WindowState == WindowState.Normal) this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;

            UpdateMsgCount();
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Minimized) this.WindowState = WindowState.Minimized;

            UpdateMsgCount();
        }

        private void UserMItem_Click(object sender, RoutedEventArgs e)
        {
            NavFrame.NavigationService.Navigate(new UserPage());
            UpdateMsgCount();
        }

        private void ScheduleMItem_Click(object sender, RoutedEventArgs e)
        {
            NavFrame.NavigationService.Navigate(new SchedulePage());
            UpdateMsgCount();
        }

        private void RequestMItem_Click(object sender, RoutedEventArgs e)
        {
            NavFrame.NavigationService.Navigate(new AdminPage());
            UpdateMsgCount();
        }

        private void SignOutMItem_Click(object sender, RoutedEventArgs e)
        {
            new AuthWindow().Show();
            this.Close();
        }

        private void CloseMItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavFrame.NavigationService.Navigate(new MsgPage());
        }

        private void ReqstMItem_Click(object sender, RoutedEventArgs e)
        {
            NavFrame.NavigationService.Navigate(new RequstPage());
        }

        private void UserAddMItem_Click(object sender, RoutedEventArgs e)
        {
            NavFrame.NavigationService.Navigate(new AddUserPage());
        }
    }
}

public class MainViewModel : INotifyPropertyChanged
{
    private string currentTime;

    public string CurrentTime
    {
        get { return currentTime; }
        set
        {
            currentTime = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
