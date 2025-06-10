using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Frontend.HTTPBodies
{
    public class UpdateEntryRequestBody
    {
        public long EntryId
        {
            get;
            set;
        }

        public long UserId
        {
            get;
            set;
        }

        public string EntryTitle
        {
            get;
            set;
        }

        public string EntryText
        {
            get;
            set;
        }
    }
}
