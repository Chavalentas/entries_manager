using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WPF_Frontend.Exceptions.ValidatorExceptions;

namespace WPF_Frontend.Model.Validators.RuleSet
{
    public class EnsureTextRegexRule : ITextRule
    {
        private string _pattern;

        public EnsureTextRegexRule(string pattern)
        {
            Pattern = pattern;
        }

        public string Pattern
        {
            get
            {
                return _pattern;
            }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(Pattern), "Cannot be null or empty!");
                }

                _pattern = value;
            }
        }

        public void Ensure(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text), "Cannot be null or empty!");
            }

            var regex = new Regex(Pattern);

            if (!regex.IsMatch(text))
            {
                throw new InvalidTextRegexException("No regex match detected!");
            }
        }
    }
}
