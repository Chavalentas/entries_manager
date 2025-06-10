using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Frontend.HTTPBodies
{
    public class VerifyUserRequestBody
    {
        private string _token;

        public string Token
        {
            get
            {
                return _token;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(Token), "Cannot be null!");
                }

                _token = value;
            }
        }
    }
}
