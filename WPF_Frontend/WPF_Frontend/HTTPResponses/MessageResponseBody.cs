using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Frontend.HTTPResponses
{
    public class MessageResponseBody
    {
        private string _message;

        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
            }
        }
    }
}
