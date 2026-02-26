using System;
using System.Collections.Generic;
using System.Text;

namespace CabarlesWpf.ViewModel
{
    internal class ViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
