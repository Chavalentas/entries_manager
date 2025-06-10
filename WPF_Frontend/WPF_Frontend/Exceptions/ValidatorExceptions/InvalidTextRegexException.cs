using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Frontend.Exceptions.ValidatorExceptions
{
    public class InvalidTextRegexException : Exception
    {
        public InvalidTextRegexException(string message) : base(message)
        {
        }
    }
}
