using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Frontend.Model;

namespace WPF_Frontend.EventArguments
{
    public class OnUpdateEntryEventArgs : EventArgs
    {
        private Entry _entry;

        public OnUpdateEntryEventArgs(Entry entry)
        {
            EntryToUpdate = entry;
        }

        public Entry EntryToUpdate
        {
            get
            {
                return _entry;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(EntryToUpdate), "Cannot be null!");
                }

                _entry = value;
            }
        }
    }
}
