using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SET09102_SoftwareEngineering_CW.bdo
{
    public class TrendingList : ObservableCollection<TrendingItem>
    {
        public TrendingList() : base()
        {
        }
    }

    public class TrendingItem : INotifyPropertyChanged
    {
        private int _count = 0;

        public TrendingItem(string hashTag, int count)
        {
            HashTag = hashTag;
            Count = count;
        }

        public string HashTag { get; set; }

        public int Count
        {
            get => _count;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                _count = value;
                OnPropertyChanged(this, "Count");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}