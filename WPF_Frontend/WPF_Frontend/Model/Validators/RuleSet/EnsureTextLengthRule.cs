using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Frontend.Exceptions.ValidatorExceptions;

namespace WPF_Frontend.Model.Validators.RuleSet
{
    public class EnsureTextLengthRule : ITextRule
    {
        private int _length;

        public EnsureTextLengthRule(int length)
        {
            Length = length;
        }

        public int Length
        {
            get
            {
                return _length;
            }

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Length), "Cannot be negative!");
                }

                _length = value;
            }
        }

        public void Ensure(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text), "Cannot be null or empty!");
            }

            if (text.Length != Length)
            {
                throw new InvalidTextLengthException("Invalid text length detected!");
            }
        }
    }
}
