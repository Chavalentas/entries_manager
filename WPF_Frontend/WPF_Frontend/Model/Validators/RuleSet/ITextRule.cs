using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Frontend.Model.Validators.RuleSet
{
    public interface ITextRule
    {
        void Ensure(string text);
    }
}
