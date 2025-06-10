using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPF_Frontend.EventArguments;
using WPF_Frontend.Model;
using WPF_Frontend.ViewModel;

namespace WPF_Frontend.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public void RegisterRequestCallback(object sender, OnReqisterRequestEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            var registerWindow = App.CreateRegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        public void OccurredErrorCallback(object sender, OnOccurredErrorEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            MessageBox.Show(args.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void LoggedInUserCallback(object sender, OnLoggedInUserEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            var dashboardWindow = App.CreateDashboardWindow();
            dashboardWindow.Show();
            this.Close();
        }

        private void OnChangedPassword(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)e.Source;
            LoginVM loginVM = (LoginVM)this.DataContext;
            loginVM.Password = passwordBox.Password;
        }
    }
}
