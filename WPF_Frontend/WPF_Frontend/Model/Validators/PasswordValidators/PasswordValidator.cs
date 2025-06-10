using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Frontend.Model.Validators.RuleSet;

namespace WPF_Frontend.Model.Validators.PasswordValidators
{
    public class PasswordValidator : IPasswordValidator
    {
        private List<ITextRule> _rules;

        public PasswordValidator()
        {
            _rules = new List<ITextRule>
            {
                new EnsureMinTextLengthRule(8),
                new EnsureTextRegexRule("[A-Z]"),
                new EnsureTextRegexRule("[a-z]"),
                new EnsureTextRegexRule("\\d"),
                new EnsureTextRegexRule("[@_!#$%^&*()<>?/|}{~:]")
            };
        }

        public bool Validate(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Cannot be null or empty!");
            }

            try
            {
                _rules.ForEach(r => r.Ensure(password));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
