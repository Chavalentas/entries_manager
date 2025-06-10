using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using WPF_Frontend.Config;
using WPF_Frontend.Exceptions;
using WPF_Frontend.HTTPBodies;
using WPF_Frontend.HTTPResponses;
using WPF_Frontend.Model;

namespace WPF_Frontend.Services
{
    public class UsersService
    {
        private string _usersURL;

        private HttpClient _httpClient;

        private IConfiguration _configuration;

        public UsersService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "Cannot be null!");
            _usersURL = _configuration["backendConfig:usersURL"];
        }

        public async Task<long> VerifyUserAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token), "Cannot be null or empty!");
            }

            string url = _usersURL + "/userid";

            var tokenRequest = new VerifyUserRequestBody()
            {
                Token = token
            };

            var response = await _httpClient.PostAsJsonAsync(url, tokenRequest);
            long decodedId = 0;

            if (response.IsSuccessStatusCode)
            {
                var idObj = await response.Content.ReadFromJsonAsync<UserIdResponseBody>();
                decodedId = idObj.UserId;
            }
            else
            {
                var obj = await response.Content.ReadFromJsonAsync<MessageResponseBody>();
                throw new UnsuccessfulHttpOperationException(obj.Message);
            }

            return decodedId;
        }

        public async Task<string> RegisterUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Cannot be null!");
            }

            var reqBody = new RegisterUserRequestBody()
            {
                UserName = user.Username,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            string url = _usersURL + "/register";

            var response = await _httpClient.PostAsJsonAsync(url, reqBody);
            string successMessage = null;

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadFromJsonAsync<MessageResponseBody>();
                successMessage = message.Message;
            }
            else
            {
                var obj = await response.Content.ReadFromJsonAsync<MessageResponseBody>();
                throw new UnsuccessfulHttpOperationException(obj.Message);
            }

            return successMessage;
        }

        public async Task<string> LoginUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Cannot be null!");
            }

            var reqBody = new LoginUserRequestBody()
            {
                UserName = user.Username,
                Password = user.Password,
                Email = user.Email
            };

            string url = _usersURL + "/login";

            var response = await _httpClient.PostAsJsonAsync(url, reqBody);
            string token = null;

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<TokenReponseBody>();
                token = res.Token;
            }
            else
            {
                var obj = await response.Content.ReadFromJsonAsync<MessageResponseBody>();
                throw new UnsuccessfulHttpOperationException(obj.Message);
            }

            return token;
        }

        public async Task<string> UpdateUserAsync(User user, long userId)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Cannot be null!");
            }

            if (user.UserId != userId)
            {
                throw new ArgumentException("The user IDs must be identical!", nameof(userId));
            }

            string url = _usersURL + $"/{Convert.ToString(userId)}";

            var reqBody = new UpdateUserRequestBody()
            {
                UserId = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Username,
                Password = user.Password,
                Email = user.Email
            };

            var response = await _httpClient.PutAsJsonAsync(url, reqBody);
            string successMessage = null;

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadFromJsonAsync<MessageResponseBody>();
                successMessage = message.Message;
            }
            else
            {
                var obj = await response.Content.ReadFromJsonAsync<MessageResponseBody>();
                throw new UnsuccessfulHttpOperationException(obj.Message);
            }

            return successMessage;
        }

        public async Task<string> DeleteUserAsync(long userId)
        {
            string url = _usersURL + $"/{Convert.ToString(userId)}";
            var response = await _httpClient.DeleteAsync(url);
            string successMessage = null;

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadFromJsonAsync<MessageResponseBody>();
                successMessage = message.Message;
            }
            else
            {
                var obj = await response.Content.ReadFromJsonAsync<MessageResponseBody>();
                throw new UnsuccessfulHttpOperationException(obj.Message);
            }

            return successMessage;
        }

        public async Task<string> UpdateUserPasswordAsync(long userId, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException(nameof(newPassword), "Cannot be null!");
            }

            string url = _usersURL + $"/pwd/{Convert.ToString(userId)}";

            var reqBody = new UpdatePasswordRequestBody()
            {
                UserId = userId,
                Password = newPassword
            };

            var response = await _httpClient.PutAsJsonAsync(url, reqBody);
            string successMessage = null;

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadFromJsonAsync<MessageResponseBody>();
                successMessage = message.Message;
            }
            else
            {
                var obj = await response.Content.ReadFromJsonAsync<MessageResponseBody>();
                throw new UnsuccessfulHttpOperationException(obj.Message);
            }

            return successMessage;
        }

        public async Task<User> GetUserInformationAsync(long userId)
        {
            string url = _usersURL + $"/{Convert.ToString(userId)}";

            var response = await _httpClient.GetAsync(url);
            User result = null;

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<UserResponseBody>();
                result = new User()
                {
                    UserId = res.UserId,
                    FirstName = res.FirstName,
                    LastName = res.LastName,
                    Username = res.Username,
                    Email = res.Email,
                    Password = res.Password
                };
            }
            else
            {
                var obj = await response.Content.ReadFromJsonAsync<MessageResponseBody>();
                throw new UnsuccessfulHttpOperationException(obj.Message);
            }

            return result;
        }
    }
}
