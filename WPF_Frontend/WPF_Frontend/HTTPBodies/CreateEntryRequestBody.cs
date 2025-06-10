using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Frontend.HTTPBodies
{
    public class CreateEntryRequestBody
    {
        public long UserId
        {
            get;
            set;
        }

        public string EntryText
        {
            get;
            set;
        }

        public string EntryTitle
        {
            get;
            set;
        }

        public string EntryDate
        {
            get;
            set;
        }
    }
}
