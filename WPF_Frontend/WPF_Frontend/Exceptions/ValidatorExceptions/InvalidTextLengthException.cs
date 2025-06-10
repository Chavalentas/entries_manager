using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Frontend.Exceptions.ValidatorExceptions
{
    public class InvalidTextLengthException : Exception
    {
        public InvalidTextLengthException(string message): base(message)
        {
        }
    }
}
