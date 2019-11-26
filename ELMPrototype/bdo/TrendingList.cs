using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ELMPrototype.bdo
{
    /// <summary>
    ///     Custom ObservableCollection for Trending list items
    /// </summary>
    public class TrendingList : ObservableCollection<TrendingItem>
    {
        //AddOrIncrement method
        public void AddOrIncrement(string item)
        {
            //If item exists in list increment count
            if (this.Any(elem => elem.HashTag.Equals(item)))
                this.Single(elem => elem.HashTag.Equals(item)).Count += 1;
            //If item doesnt exist add item
            else
                Add(new TrendingItem(item, 1));

            //Resort list
            this.Sort(o => o.Count);
        }
    }

    //Custom item for Trending List
    public class TrendingItem : INotifyPropertyChanged
    {
        private int _count;

        public TrendingItem(string hashTag, int count)
        {
            HashTag = hashTag;
            Count = count;
        }

        public string HashTag { get; }

        public int Count
        {
            get => _count;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                _count = value;
                //Notify UI of change
                OnPropertyChanged(this, "Count");
            }
        }

        //Event to notify UI
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}