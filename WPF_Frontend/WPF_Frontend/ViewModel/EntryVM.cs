using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Frontend.Model;

namespace WPF_Frontend.ViewModel
{
    public class EntryVM : INotifyPropertyChanged
    {
        private Entry _entry;

        private string _entryDateText;

        private string _entryTitle;

        private string _entryText;

        public EntryVM(Entry entry)
        {
            _entry = entry ?? throw new ArgumentNullException(nameof(entry), "Cannot be null!");
            EntryDateText = _entry.EntryDate.ToString("yyyy-MM-dd HH:mm:ss");
            EntryTitle = entry.EntryTitle;
            EntryText = entry.EntryText.Length > 10 ? entry.EntryText.Substring(0, 10) + "..." : entry.EntryText;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string EntryDateText
        {
            get
            {
                return _entryDateText;
            }

            set
            {

                _entryDateText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EntryDateText)));
            }
        }

        public string EntryTitle
        {
            get
            {
                return _entryTitle;
            }

            set
            {
                _entryTitle = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EntryTitle)));
            }
        }

        public string EntryText
        {
            get
            {
                return _entryText;
            }

            set
            {
                _entryText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EntryText)));
            }
        }

        public Entry GetEntry()
        {
            var result = new Entry()
            {
                EntryId = _entry.EntryId,
                EntryTitle = _entry.EntryTitle,
                UserId = _entry.UserId,
                EntryDate = _entry.EntryDate,
                EntryText = _entry.EntryText
            };

            return result;
        }
    }
}
