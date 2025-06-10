using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
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
using WPF_Frontend.Model.Validators.PasswordValidators;
using WPF_Frontend.Services;

namespace WPF_Frontend.ViewModel
{
    public class ProfileVM : INotifyPropertyChanged
    {
        private long _userId;

        private User _user;

        private string _newPassword;

        private UsersService _usersService;

        private IConnectivity _connectivity;

        private IPasswordValidator _passwordValidator;

        private ITokenSerializer _tokenSerializer;

        public ProfileVM(UsersService usersService, IConnectivity connectivity, IPasswordValidator passwordValidator, ITokenSerializer tokenSerializer)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService), "Cannot be null!");
            _connectivity = connectivity ?? throw new ArgumentNullException(nameof(connectivity), "Cannot be null!");
            _tokenSerializer = tokenSerializer ?? throw new ArgumentNullException(nameof(tokenSerializer), "Cannot be null!");
            _passwordValidator = passwordValidator ?? throw new ArgumentNullException(nameof(passwordValidator), "Cannot be null!");
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

        public User User
        {
            get
            {
                return _user;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(User), "Cannot be null!");
                }

                _user = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(User)));
            }
        }

        public string NewPassword
        {
            get
            {
                return _newPassword;
            }

            set
            {
                _newPassword = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewPassword)));
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

        public ICommand ChangeProfileCommand
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

                        if (string.IsNullOrEmpty(User.FirstName))
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("The first name must not be empty!"));
                            return;
                        }

                        if (string.IsNullOrEmpty(User.LastName))
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("The last name must not be empty!"));
                            return;
                        }

                        if (string.IsNullOrEmpty(User.Username))
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("The username must not be empty!"));
                            return;
                        }

                        if (string.IsNullOrEmpty(User.Email))
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("The email must not be empty!"));
                            return;
                        }

                        var successMessage = await _usersService.UpdateUserAsync(User, UserId);

                        if (!string.IsNullOrEmpty(successMessage))
                        {
                            FireOnInformation(new OnInformationEventArgs("The user was successfully updated!"));
                        }
                        else
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("Some error occurred during the registration! Try again later!"));
                        }
                    }
                    catch (UnsuccessfulHttpOperationException ex)
                    {
                        FireOnOccurredError(new OnOccurredErrorEventArgs(ex.Message));
                        return;
                    }
                    catch (Exception ex)
                    {
                        FireOnOccurredError(new OnOccurredErrorEventArgs("Some error occurred during the registration! Try again later!"));
                        return;
                    }
                });
            }
        }

        public ICommand ChangePasswordCommand
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

                        string password = NewPassword;

                        if (string.IsNullOrEmpty(password))
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("The password must not be empty!"));
                            return;
                        }


                        if (!_passwordValidator.Validate(password))
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("The password must match criteria! (length >=8, uppercase, lowercase, digit, special char!"));
                            return;
                        }

                        var successMessage = await _usersService.UpdateUserPasswordAsync(UserId, password);

                        if (!string.IsNullOrEmpty(successMessage))
                        {
                            FireOnInformation(new OnInformationEventArgs("The password was successfully updated!"));
                        }
                        else
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("Some error occurred during the password update! Try again later!"));
                        }
                    }
                    catch (UnsuccessfulHttpOperationException ex)
                    {
                        FireOnOccurredError(new OnOccurredErrorEventArgs(ex.Message));
                        return;
                    }
                    catch (Exception ex)
                    {
                        FireOnOccurredError(new OnOccurredErrorEventArgs("Some error occurred during the password update! Try again later!"));
                        return;
                    }
                });
            }
        }

        public ICommand DeleteProfileCommand
        {
            get
            {
                return new GenericCommand(async (obj) =>
                {
                    try
                    {
                        var connectedToInternet = await _connectivity.IsConnectedAsync("testmy.net");

                        if (!connectedToInternet)
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("No internet connection! Try again later!"));
                            return;
                        }

                        var successMessage = await _usersService.DeleteUserAsync(UserId);

                        if (!string.IsNullOrEmpty(successMessage))
                        {
                            LogOut();
                        }
                        else
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("Some error occurred during the account deletion! Try again later!"));
                        }
                    }
                    catch (UnsuccessfulHttpOperationException ex)
                    {
                        FireOnOccurredError(new OnOccurredErrorEventArgs(ex.Message));
                        return;
                    }
                    catch (Exception ex)
                    {
                        FireOnOccurredError(new OnOccurredErrorEventArgs("Some error occurred during the registration! Try again later!"));
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

        public async Task FetchUserDataAsync()
        {
            try
            {
                var connectedToInternet = await _connectivity.IsConnectedAsync("testmy.net");

                if (!connectedToInternet)
                {
                    FireOnOccurredError(new OnOccurredErrorEventArgs("No internet connection! Try again later!"));
                    return;
                }

                var user = await _usersService.GetUserInformationAsync(UserId);
                User = user;
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
