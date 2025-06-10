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
    public class LoginVM : INotifyPropertyChanged
    {
        private UsersService _userService;

        private IConnectivity _connectivity;

        private ITokenSerializer _tokenSerializer;

        private string _usernameOrEmail;

        private string _password;

        private IPasswordValidator _passwordValidator;

        public LoginVM(UsersService usersService, IConnectivity connectivity, ITokenSerializer tokenSerializer, IPasswordValidator passwordValidator)
        {
            _userService = usersService ?? throw new ArgumentNullException(nameof(usersService), "Cannot be null!");
            _connectivity = connectivity ?? throw new ArgumentNullException(nameof(connectivity), "Cannot be null!");
            _tokenSerializer = tokenSerializer ?? throw new ArgumentNullException(nameof(tokenSerializer), "Cannot be null!");
            _passwordValidator = passwordValidator ?? throw new ArgumentNullException(nameof(passwordValidator), "Cannot be null!");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler<OnReqisterRequestEventArgs> OnRegisterRequest;

        public event EventHandler<OnOccurredErrorEventArgs> OnOccurredError;

        public event EventHandler<OnLoggedInUserEventArgs> OnLoggedInUser;

        public string UsernameOrEmail
        {
            get
            {
                return _usernameOrEmail;
            }

            set
            {
                _usernameOrEmail = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsernameOrEmail)));
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        public ICommand RegisterCommand
        {
            get
            {
                return new GenericCommand((obj) =>
                {
                    FireOnRegisterRequest(new OnReqisterRequestEventArgs());
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

                        string userNameOrEmail = UsernameOrEmail;
                        string password = Password;

                        if (string.IsNullOrEmpty(userNameOrEmail))
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("The username must not be empty!"));
                            return;
                        }

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

                        var user = new User()
                        {
                            Password = password,
                            Email = userNameOrEmail,
                            Username = userNameOrEmail
                        };

                        var token = await _userService.LoginUserAsync(user);

                        if (!string.IsNullOrEmpty(token))
                        {
                            Token t = new Token()
                            {
                                TokenValue = token
                            };

                            _tokenSerializer.Serialize(t);
                            FireOnLoggedInUser(new OnLoggedInUserEventArgs());
                        }
                        else
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("Some error occurred during the login! Try again later!"));
                        }
                    }
                    catch (UnsuccessfulHttpOperationException ex)
                    {
                        FireOnOccurredError(new OnOccurredErrorEventArgs(ex.Message));
                        return;
                    }
                    catch (Exception ex)
                    {
                        FireOnOccurredError(new OnOccurredErrorEventArgs("Some error occurred during the login! Try again later!"));
                        return;
                    }
                });
            }
        }

        protected virtual void FireOnRegisterRequest(OnReqisterRequestEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnRegisterRequest?.Invoke(this, args);
        }

        protected virtual void FireOnOccurredError(OnOccurredErrorEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnOccurredError?.Invoke(this, args);
        }

        protected virtual void FireOnLoggedInUser(OnLoggedInUserEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnLoggedInUser?.Invoke(this, args);
        }
    }
}
