using DocumentFormat.OpenXml.Office2010.Excel;
using internet_connection_lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Frontend.Commands;
using WPF_Frontend.EventArguments;
using WPF_Frontend.Exceptions;
using WPF_Frontend.Model;
using WPF_Frontend.Services;

namespace WPF_Frontend.ViewModel
{
    public class UpdateEntryVM : INotifyPropertyChanged
    {
        private long _userId;

        private Entry _entryToUpdate;

        private UsersService _usersService;

        private EntriesService _entriesService;

        private IConnectivity _connectivity;

        private ITokenSerializer _tokenSerializer;

        public UpdateEntryVM(UsersService usersService, EntriesService entriesService, IConnectivity connectivity, ITokenSerializer tokenSerializer)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService), "Cannot be null!");
            _entriesService = entriesService ?? throw new ArgumentNullException(nameof(entriesService), "Cannot be null!");
            _tokenSerializer = tokenSerializer ?? throw new ArgumentNullException(nameof(tokenSerializer), "Cannot be null!");
            _connectivity = connectivity ?? throw new ArgumentNullException(nameof(connectivity), "Cannot be null!");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler<OnOccurredErrorEventArgs> OnOccurredError;

        public event EventHandler<OnInformationEventArgs> OnInformationRequest;

        public event EventHandler<OnGoBackRequestEventArgs> OnGoBackRequest;

        public event EventHandler<OnLoggedOutUserEventArgs> OnLoggedOutUser;

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

        public Entry EntryToUpdate
        {
            get
            {
                return _entryToUpdate;
            }

            set
            {
                _entryToUpdate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EntryToUpdate)));
            }
        }

        public ICommand GoBackCommand
        {
            get
            {
                return new GenericCommand((obj) =>
                {
                    FireOnGoBackRequest(new OnGoBackRequestEventArgs());
                });
            }
        }

        public ICommand SubmitCommand
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

                        if (string.IsNullOrEmpty(EntryToUpdate.EntryTitle))
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("Entry title must not be empty!"));
                            return;
                        }

                        if (string.IsNullOrEmpty(EntryToUpdate.EntryText))
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("Entry text must not be empty!"));
                            return;
                        }

                        if (EntryToUpdate.UserId != UserId)
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("Invalid operation! Try again!"));
                        }

                        string successMessage = await _entriesService.UpdateEntryAsync(EntryToUpdate, EntryToUpdate.EntryId);

                        if (!string.IsNullOrEmpty(successMessage))
                        {
                            FireOnInformation(new OnInformationEventArgs("The entry was successfully updated!"));
                            return;
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

        protected virtual void FireOnGoBackRequest(OnGoBackRequestEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnGoBackRequest?.Invoke(this, args);
        }

        protected virtual void FireOnLoggedOutUser(OnLoggedOutUserEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnLoggedOutUser?.Invoke(this, args);
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
