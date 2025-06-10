using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Frontend.Model.Validators.PasswordValidators
{
    public interface IPasswordValidator
    {
        bool Validate(string password);
    }
}
