using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WPF_Frontend.Config;
using WPF_Frontend.Exceptions;
using WPF_Frontend.HTTPBodies;
using WPF_Frontend.HTTPResponses;
using WPF_Frontend.Model;

namespace WPF_Frontend.Services
{
    public class EntriesService
    {
        private string _entriesURL;

        private HttpClient _httpClient;

        private IConfiguration _configuration;

        public EntriesService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "Cannot be null!");
            _entriesURL = _configuration["backendConfig:entriesURL"];
        }

        public async Task<string> CreateEntryAsync(Entry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry), "Cannot be null!");
            }

            var reqBody = new CreateEntryRequestBody()
            {
                EntryDate = entry.EntryDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EntryText = entry.EntryText,
                EntryTitle = entry.EntryTitle,
                UserId = entry.UserId
            };

            string url = _entriesURL + "/create";

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

        public async Task<string> UpdateEntryAsync(Entry entry, long entryId)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry), "Cannot be null!");
            }

            if (entry.EntryId != entryId)
            {
                throw new ArgumentException("The entry IDs must be identical!", nameof(entryId));
            }

            string url = _entriesURL + $"/{Convert.ToString(entryId)}";

            var reqBody = new UpdateEntryRequestBody()
            {
                EntryId = entryId,
                EntryTitle = entry.EntryTitle,
                EntryText = entry.EntryText,
                UserId = entry.UserId
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

        public async Task<string> DeleteEntryAsync(long entryId)
        {
            string url = _entriesURL + $"/{Convert.ToString(entryId)}";
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

        public async Task<List<Entry>> GetEntriesAsync(long userId)
        {
            string url = _entriesURL + $"/{Convert.ToString(userId)}";
            var response = await _httpClient.GetAsync(url);
            List<Entry> result = null;

            if (response.IsSuccessStatusCode)
            {
                var entries = await response.Content.ReadFromJsonAsync<List<EntryResponseBody>>();
                result = entries.Select(e => new Entry()
                {
                    EntryDate = Convert.ToDateTime(e.EntryDate).ToUniversalTime(),
                    EntryId = e.EntryId,
                    UserId = e.UserId,
                    EntryTitle = e.EntryTitle,
                    EntryText = e.EntryText
                }).ToList();
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
