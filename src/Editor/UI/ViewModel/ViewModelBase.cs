using System;
using System.ComponentModel;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected virtual void RaisePropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}