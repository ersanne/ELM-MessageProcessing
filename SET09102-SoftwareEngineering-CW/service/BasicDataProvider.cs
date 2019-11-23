using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using SET09102_SoftwareEngineering_CW.bdo;

namespace SET09102_SoftwareEngineering_CW.service
{
    public class BasicDataProvider
    {
        private static BasicDataProvider _instance;

        private BasicDataProvider()
        {
            ReadTextSpeakFile();
        }

        public static BasicDataProvider GetInstance()
        {
            return _instance ?? (_instance = new BasicDataProvider());
        }

        public Dictionary<string, string> TextSpeakWords { get; set; }

        public TrendingList TrendingList { get; set; } = new TrendingList();

        public ObservableCollection<string> MentionList { get; } = new ObservableCollection<string>();

        public SirList SirList { get; set; } = new SirList();
        
        public void AddOrIncrementTrendingListItem(string item)
        {
            if (TrendingList.Any(elem => elem.HashTag.Equals(item)))
            {
                TrendingList.Single(elem => elem.HashTag.Equals(item)).Count += 1;
            }
            else
            {
                TrendingList.Add(new TrendingItem(item, 1));
            }

            TrendingList.Sort(o => o.Count);
        }

        private void ReadTextSpeakFile()
        {
            TextSpeakWords = new Dictionary<string, string>();
            var reader =
                //new StreamReader(File.OpenRead(Path.Combine(Environment.CurrentDirectory, @"Data\", "textwords.csv")));
                new StreamReader(File.OpenRead(@"../../Data/textwords.csv"));
            while (!reader.EndOfStream)
            {
                var parts = reader.ReadLine()?.Split(',');
                TextSpeakWords.Add(parts[0], parts[1]);
            }
        }
    }
}