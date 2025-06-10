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
using System.Windows.Shapes;
using WPF_Frontend.EventArguments;
using WPF_Frontend.ViewModel;

namespace WPF_Frontend.View
{
    /// <summary>
    /// Interaction logic for AddEntryWindow.xaml
    /// </summary>
    public partial class AddEntryWindow : Window
    {
        public AddEntryWindow()
        {
            InitializeComponent();
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

        public void GoBackRequestCallback(object sender, OnGoBackRequestEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            var window = App.CreateDashboardWindow();
            window.Show();
            this.Close();
        }

        public void LoggedOutUserCallback(object sender, OnLoggedOutUserEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            var window = App.CreateLoginWindow();
            window.Show();
            this.Close();
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            AddEntryVM addEntryVM = (AddEntryVM)DataContext;
            await addEntryVM.ValidateUserAsync();
        }
    }
}
