using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Frontend.HTTPBodies
{
    public class UpdatePasswordRequestBody
    {
        public long UserId
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
    }
}
