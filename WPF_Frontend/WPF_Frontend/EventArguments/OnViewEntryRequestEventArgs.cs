using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Frontend.Model;

namespace WPF_Frontend.EventArguments
{
    public class OnViewEntryRequestEventArgs : EventArgs
    {
        private Entry _entry;

        public OnViewEntryRequestEventArgs(Entry entry)
        {
            EntryToView = entry;
        }

        public Entry EntryToView
        {
            get
            {
                return _entry;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(EntryToView), "Cannot be null!");
                }

                _entry = value;
            }
        }
    }
}
