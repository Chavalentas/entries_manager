using internet_connection_lib;
using Microsoft.Extensions.Configuration;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using WPF_Frontend.Model;
using WPF_Frontend.Model.Validators.PasswordValidators;
using WPF_Frontend.Services;
using WPF_Frontend.View;
using WPF_Frontend.ViewModel;

namespace WPF_Frontend
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IKernel Container;

        public static Window CreateLoginWindow()
        {
            LoginWindow loginWindow = new LoginWindow();
            var vm = Container.Get<LoginVM>();
            vm.OnRegisterRequest += loginWindow.RegisterRequestCallback;
            vm.OnOccurredError += loginWindow.OccurredErrorCallback;
            vm.OnLoggedInUser += loginWindow.LoggedInUserCallback;
            loginWindow.DataContext = vm;
            return loginWindow;
        }

        public static Window CreateRegisterWindow()
        {
            RegisterWindow registerWindow = new RegisterWindow();
            var vm = App.Container.Get<RegisterVM>();
            vm.OnLoginRequest += registerWindow.LoginRequestCallback;
            vm.OnOccurredError += registerWindow.OccurredErrorCallback;
            vm.OnInformationRequest += registerWindow.OccurredInformationCallback;
            registerWindow.DataContext = vm;
            return registerWindow;
        }

        public static Window CreateDashboardWindow()
        {
            DashboardWindow dashboardWindow = new DashboardWindow();
            var vm = App.Container.Get<DashboardVM>();
            vm.OnOccurredError += dashboardWindow.OccurredErrorCallback;
            vm.OnInformationRequest += dashboardWindow.OccurredInformationCallback;
            vm.OnAddEntryRequest += dashboardWindow.AddEntryRequestCallback;
            vm.OnUpdateEntryRequest += dashboardWindow.UpdateEntryRequestCallback;
            vm.OnViewEntryRequest += dashboardWindow.ViewEntryRequestCallback;
            vm.OnChangeProfileRequest += dashboardWindow.ChangeProfileRequestCallback;
            vm.OnLoggedOutUser += dashboardWindow.LoggedOutUserCallback;
            dashboardWindow.DataContext = vm;
            return dashboardWindow;
        }

        public static Window CreateAddEntryWindow()
        {
            AddEntryWindow addEntryWindow = new AddEntryWindow();
            var vm = App.Container.Get<AddEntryVM>();
            addEntryWindow.DataContext = vm;
            vm.OnOccurredError += addEntryWindow.OccurredErrorCallback;
            vm.OnInformationRequest += addEntryWindow.OccurredInformationCallback;
            vm.OnGoBackRequest += addEntryWindow.GoBackRequestCallback;
            vm.OnLoggedOutUser += addEntryWindow.LoggedOutUserCallback;
            return addEntryWindow;
        }

        public static Window CreateUpdateEntryWindow(Entry entryToUpdate)
        {
            if (entryToUpdate == null)
            {
                throw new ArgumentNullException(nameof(entryToUpdate), "Cannot be null!");
            }

            UpdateEntryWindow updateEntryWindow = new UpdateEntryWindow();
            var vm = App.Container.Get<UpdateEntryVM>();
            vm.EntryToUpdate = entryToUpdate;
            updateEntryWindow.DataContext = vm;
            vm.OnOccurredError += updateEntryWindow.OccurredErrorCallback;
            vm.OnInformationRequest += updateEntryWindow.OccurredInformationCallback;
            vm.OnGoBackRequest += updateEntryWindow.GoBackRequestCallback;
            vm.OnLoggedOutUser += updateEntryWindow.LoggedOutUserCallback;
            return updateEntryWindow;
        }

        public static Window CreateProfileWindow()
        {
            ProfileWindow profileWindow = new ProfileWindow();
            var vm = App.Container.Get<ProfileVM>();
            vm.OnOccurredError += profileWindow.OccurredErrorCallback;
            vm.OnInformationRequest += profileWindow.OccurredInformationCallback;
            vm.OnGoBackRequest += profileWindow.GoBackRequestCallback;
            vm.OnLoggedOutUser += profileWindow.LoggedOutUserCallback;
            profileWindow.DataContext = vm;
            return profileWindow;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Container = new StandardKernel();
            Container.Bind<UsersService>().To<UsersService>().InTransientScope();
            Container.Bind<EntriesService>().To<EntriesService>().InTransientScope();
            Container.Bind<IConnectivity>().To<Connectivity>().InTransientScope();
            Container.Bind<ITokenSerializer>().To<JsonTokenSerializer>().InTransientScope();
            Container.Bind<IPasswordValidator>().To<PasswordValidator>().InTransientScope();
            Container.Bind<IConfiguration>().ToMethod(c =>
            {
                var config = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("settings.json")
                                 .Build();
                return config;
            });

            var dashboardWindow = CreateDashboardWindow();
            dashboardWindow.Show();
        }
    }
}
