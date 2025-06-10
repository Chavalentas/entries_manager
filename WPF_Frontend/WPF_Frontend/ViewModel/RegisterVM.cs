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
    public class RegisterVM
    {
        private string _userName;

        private string _email;

        private string _password;

        private string _firstName;

        private string _lastName;

        private UsersService _userService;

        private IConnectivity _connectivity;

        private IPasswordValidator _passwordValidator;

        public RegisterVM(UsersService usersService, IConnectivity connectivity, IPasswordValidator passwordValidator)
        {
            _userService = usersService ?? throw new ArgumentNullException(nameof(usersService), "Cannot be null!");
            _connectivity = connectivity ?? throw new ArgumentNullException(nameof(connectivity), "Cannot be null!");
            _passwordValidator = passwordValidator ?? throw new ArgumentNullException(nameof(passwordValidator), "Cannot be null!");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler<OnLoginRequestEventArgs> OnLoginRequest;

        public event EventHandler<OnOccurredErrorEventArgs> OnOccurredError;

        public event EventHandler<OnInformationEventArgs> OnInformationRequest;

        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                _firstName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstName)));
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                _lastName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName)));
            }
        }

        public string Username
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
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

        public ICommand SubmitCommand
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

                        string username = Username;
                        string email = Email;
                        string password = Password;
                        string firstName = FirstName;
                        string lastName = LastName;

                        if (string.IsNullOrEmpty(firstName))
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("The first name must not be empty!"));
                            return;
                        }

                        if (string.IsNullOrEmpty(lastName))
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("The last name must not be empty!"));
                            return;
                        }

                        if (string.IsNullOrEmpty(username))
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("The username must not be empty!"));
                            return;
                        }

                        if (string.IsNullOrEmpty(email))
                        {
                            FireOnOccurredError(new OnOccurredErrorEventArgs("The email must not be empty!"));
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
                            Email = email,
                            Username = username,
                            FirstName = firstName,
                            LastName = lastName
                        };

                        var successMessage = await _userService.RegisterUserAsync(user);

                        if (!string.IsNullOrEmpty(successMessage))
                        {
                            FireOnInformation(new OnInformationEventArgs("The user was successfully registered!"));
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

        public ICommand LoginCommand
        {
            get
            {
                return new GenericCommand((obj) =>
                {
                    FireOnLoginRequest(new OnLoginRequestEventArgs());
                });
            }
        }

        protected virtual void FireOnLoginRequest(OnLoginRequestEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnLoginRequest?.Invoke(this, args);
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
    }
}
