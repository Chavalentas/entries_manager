using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WPF_Frontend.Model
{
    public class JsonTokenSerializer : ITokenSerializer
    {
        private string _path;

        private IConfiguration _configuration;

        public JsonTokenSerializer(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "Cannot be null!");
            _path = _configuration["tokenPath"];
        }

        public void Serialize(Token token)
        {
            if (!File.Exists(_path))
            {
                var file = File.Create(_path);
                file.Close();
            }

            if (token == null)
            {
                throw new ArgumentNullException(nameof(token), "Cannot be null!");
            }

            string jsonString = JsonSerializer.Serialize(token);
            File.WriteAllText(_path, jsonString);
        }

        public Token Deserialize()
        {
            if (!File.Exists(_path))
            {
                throw new FileNotFoundException($"The file under {nameof(_path)} does not exist!");
            }

            string jsonString = File.ReadAllText(_path);
            Token token = JsonSerializer.Deserialize<Token>(jsonString)!;
            return token;
        }
    }
}
