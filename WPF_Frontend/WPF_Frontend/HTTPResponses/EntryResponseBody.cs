using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Frontend.HTTPResponses
{
    public class EntryResponseBody
    {
        public long EntryId
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

        public long UserId
        {
            get;
            set;
        }
    }
}
