using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Frontend.Model
{
    public interface ITokenSerializer
    {
        void Serialize(Token token);

        Token Deserialize();
    }
}
