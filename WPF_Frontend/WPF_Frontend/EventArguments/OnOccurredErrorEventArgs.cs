using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Frontend.EventArguments
{
    public class OnOccurredErrorEventArgs : EventArgs
    {
        private string _message;

        public OnOccurredErrorEventArgs(string message)
        {
            Message = message;
        }

        public string Message
        {
            get
            {
                return _message;
            }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(Message), "Cannot be null!");
                }

                _message = value;
            }
        }
    }
}
