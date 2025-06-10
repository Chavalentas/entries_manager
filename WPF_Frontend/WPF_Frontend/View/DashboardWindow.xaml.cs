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
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
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

        public void AddEntryRequestCallback(object sender, OnAddEntryRequestEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            var addEntryWindow = App.CreateAddEntryWindow();
            addEntryWindow.Show();
            this.Close();
        }

        public void UpdateEntryRequestCallback(object sender, OnUpdateEntryEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            var addEntryWindow = App.CreateUpdateEntryWindow(args.EntryToUpdate);
            addEntryWindow.Show();
            this.Close();
        }

        public void ViewEntryRequestCallback(object sender, OnViewEntryRequestEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            string toDisplay = args.EntryToView.EntryText;
            string title = args.EntryToView.EntryDate.ToString("yyyy-MM-dd HH:mm:ss") + " " + args.EntryToView.EntryTitle;
            MessageBox.Show(toDisplay, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ChangeProfileRequestCallback(object sender, OnChangeProfileRequestEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            var window = App.CreateProfileWindow();
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
            DashboardVM dashboardVM = (DashboardVM)DataContext;
            await dashboardVM.ValidateUserAsync();
            await dashboardVM.SetEntriesAsync();
        }
    }
}
