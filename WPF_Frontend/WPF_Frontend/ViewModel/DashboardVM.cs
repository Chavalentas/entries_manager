using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using internet_connection_lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Frontend.Commands;
using WPF_Frontend.EventArguments;
using WPF_Frontend.Exceptions;
using WPF_Frontend.HTTPResponses;
using WPF_Frontend.Model;
using WPF_Frontend.Services;

namespace WPF_Frontend.ViewModel
{
    public class DashboardVM : INotifyPropertyChanged
    {
        private UsersService _usersService;

        private EntriesService _entriesService;

        private IConnectivity _connectivity;

        private ITokenSerializer _tokenSerializer;

        private ObservableCollection<EntryVM> _entriesVM;

        private long _userId;

        public DashboardVM(UsersService usersService, EntriesService entriesService, IConnectivity connectivity, ITokenSerializer tokenSerializer)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService), "Cannot be null!");
            _entriesService = entriesService ?? throw new ArgumentNullException(nameof(entriesService), "Cannot be null!");
            _connectivity = connectivity ?? throw new ArgumentNullException(nameof(connectivity), "Cannot be null!");
            _tokenSerializer = tokenSerializer ?? throw new ArgumentNullException(nameof(tokenSerializer), "Cannot be null!");
            EntriesVM = new ObservableCollection<EntryVM>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler<OnOccurredErrorEventArgs> OnOccurredError;

        public event EventHandler<OnInformationEventArgs> OnInformationRequest;

        public event EventHandler<OnAddEntryRequestEventArgs> OnAddEntryRequest;

        public event EventHandler<OnUpdateEntryEventArgs> OnUpdateEntryRequest;

        public event EventHandler<OnViewEntryRequestEventArgs> OnViewEntryRequest;

        public event EventHandler<OnLoggedOutUserEventArgs> OnLoggedOutUser;

        public event EventHandler<OnChangeProfileRequestEventArgs> OnChangeProfileRequest;

        public long UserId
        {
            get
            {
                return _userId;
            }

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(UserId), "Cannot be out of range!");
                }

                _userId = value;
            }
        }

        public ObservableCollection<EntryVM> EntriesVM
        {
            get
            {
                return _entriesVM;
            }

            set
            {
                _entriesVM = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EntriesVM)));
            }
        }

        public ICommand ChangeProfileCommand
        {
            get
            {
                return new GenericCommand((obj) =>
                {
                    FireOnChangeProfileRequest(new OnChangeProfileRequestEventArgs());
                });
            }
        }

        public ICommand LogOutCommand
        {
            get
            {
                return new GenericCommand((obj) =>
                {
                    LogOut();
                });
            }
        }

        public ICommand AddEntryCommand
        {
            get
            {
                return new GenericCommand((obj) =>
                {
                    FireOnAddEntryRequest(new OnAddEntryRequestEventArgs());
                });
            }
        }

        public ICommand UpdateEntryCommand
        {
            get
            {
                return new GenericCommand((obj) =>
                {
                    if (obj == null)
                    {
                        throw new ArgumentNullException(nameof(obj), "Cannot be null!");
                    }

                    EntryVM entryVM = obj as EntryVM;
                    var entry = entryVM.GetEntry();
                    FireOnUpdateEntryRequest(new OnUpdateEntryEventArgs(entry));
                });
            }
        }

        public ICommand DeleteEntryCommand
        {
            get
            {
                return new GenericCommand(async(obj) =>
                {
                    try
                    {
                        var connectedToInternet = await _connectivity.IsConnectedAsync("testmy.net");

                        if (!connectedToInternet)
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("No internet connection! Try again later!"));
                            return;
                        }

                        if (obj == null)
                        {
                            throw new ArgumentNullException(nameof(obj), "Cannot be null!");
                        }

                        EntryVM entryVM = obj as EntryVM;
                        var entry = entryVM.GetEntry();

                        var successMessage = await _entriesService.DeleteEntryAsync(entry.EntryId);

                        if (!string.IsNullOrEmpty(successMessage))
                        {
                            await SetEntriesAsync();
                            FireOnInformation(new OnInformationEventArgs("The entry was successfully deleted!"));
                        }
                        else
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("Some error occurred during the process! Try again later!"));
                        }
                    }
                    catch (UnsuccessfulHttpOperationException ex)
                    {
                        FireOnOccurredError(new OnOccurredErrorEventArgs(ex.Message));
                        return;
                    }
                    catch (Exception ex)
                    {
                        FireOnOccurredError(new OnOccurredErrorEventArgs("Some error occurred during the process! Try again later!"));
                        return;
                    }
                });
            }
        }

        public ICommand ViewEntryCommand
        {
            get
            {
                return new GenericCommand((obj) =>
                {
                    if (obj == null)
                    {
                        throw new ArgumentNullException(nameof(obj), "Cannot be null!");
                    }

                    EntryVM entryVM = obj as EntryVM;
                    var entry = entryVM.GetEntry();
                    FireOnViewEntryRequest(new OnViewEntryRequestEventArgs(entry));
                });
            }
        }

        protected virtual void FireOnOccurredError(OnOccurredErrorEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnOccurredError?.Invoke(this, args);
        }

        protected virtual void FireOnInformation(OnInformationEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnInformationRequest?.Invoke(this, args);
        }

        protected virtual void FireOnUpdateEntryRequest(OnUpdateEntryEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnUpdateEntryRequest?.Invoke(this, args);
        }

        protected virtual void FireOnAddEntryRequest(OnAddEntryRequestEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnAddEntryRequest?.Invoke(this, args);
        }

        protected virtual void FireOnViewEntryRequest(OnViewEntryRequestEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnViewEntryRequest?.Invoke(this, args);
        }

        protected virtual void FireOnChangeProfileRequest(OnChangeProfileRequestEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnChangeProfileRequest?.Invoke(this, args);
        }

        protected virtual void FireOnLoggedOutUser(OnLoggedOutUserEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnLoggedOutUser?.Invoke(this, args);
        }

        public async Task ValidateUserAsync()
        {
            try
            {
                var connectedToInternet = await _connectivity.IsConnectedAsync("testmy.net");

                if (!connectedToInternet)
                {
                    FireOnOccurredError(new OnOccurredErrorEventArgs("No internet connection! Try again later!"));
                    return;
                }

                var token = _tokenSerializer.Deserialize();

                if (string.IsNullOrEmpty(token.TokenValue))
                {
                    LogOut();
                    return;
                }

                long userId = await _usersService.VerifyUserAsync(token.TokenValue);
                UserId = userId;
            }
            catch (UnsuccessfulHttpOperationException ex)
            {
                LogOut();
                return;
            }
            catch (Exception ex)
            {
                LogOut();
                return;
            }
        }

        public async Task SetEntriesAsync()
        {
            try
            {
                var connectedToInternet = await _connectivity.IsConnectedAsync("testmy.net");

                if (!connectedToInternet)
                {
                    FireOnOccurredError(new OnOccurredErrorEventArgs("No internet connection! Try again later!"));
                    return;
                }

                var entries = await _entriesService.GetEntriesAsync(UserId);
                entries = entries.OrderByDescending(e => e.EntryDate).ToList();
                EntriesVM.Clear();
                entries.ForEach(e => EntriesVM.Add(new EntryVM(e)));
            }
            catch (UnsuccessfulHttpOperationException ex)
            {
                FireOnOccurredError(new OnOccurredErrorEventArgs(ex.Message));
                return;
            }
            catch (Exception ex)
            {
                FireOnOccurredError(new OnOccurredErrorEventArgs("Some error occurred during the loading!"));
                return;
            }
        }

        private void LogOut()
        {
            Token token = new Token()
            {
                TokenValue = string.Empty
            };

            _tokenSerializer.Serialize(token);
            FireOnLoggedOutUser(new OnLoggedOutUserEventArgs());
        }
    }
}
