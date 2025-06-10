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
using System.Windows.Shapes;
using WPF_Frontend.EventArguments;
using WPF_Frontend.ViewModel;

namespace WPF_Frontend.View
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        public void LoginRequestCallback(object sender, OnLoginRequestEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            var loginWindow = App.CreateLoginWindow();
            loginWindow.Show();
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

        public void OccurredInformationCallback(object sender, OnInformationEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            MessageBox.Show(args.Message, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnChangedPassword(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)e.Source;
            RegisterVM registerVM = (RegisterVM)this.DataContext;
            registerVM.Password = passwordBox.Password;
        }
    }
}
