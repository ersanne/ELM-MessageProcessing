using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using SET09102_SoftwareEngineering_CW.bdo;
using SET09102_SoftwareEngineering_CW.exceptions;

namespace SET09102_SoftwareEngineering_CW.service
{
    /// <summary>
    /// Singleton class to provide various data
    /// This class contains the textspeak words as well the various lists,
    /// outside the prototype this should be persisted properly.
    /// </summary>
    public class BasicDataProvider
    {
        private static BasicDataProvider _instance;

        private BasicDataProvider()
        {
            //Read textspeak file when initialising class 
            ReadTextSpeakFile();
        }

        public static BasicDataProvider GetInstance()
        {
            return _instance ?? (_instance = new BasicDataProvider());
        }

        //Dictionary containing textspeak words. <Short, Expanded> i.e. <LOL, Laughing out loud>
        public Dictionary<string, string> TextSpeakWords { get; private set; }

        //Custom ObservableCollection for trending hashtags
        public TrendingList TrendingList { get; } = new TrendingList();

        //ObservableCollection for mentions
        public ObservableCollection<string> MentionList { get; } = new ObservableCollection<string>();

        //Custom ObservableCollection for SIR items
        public SirList SirList { get; set; } = new SirList();

        //ObservableCollection for quarantined urls
        public ObservableCollection<string> QuarantineList { get; set; } = new ObservableCollection<string>();

        //Read textspeak file
        private void ReadTextSpeakFile()
        {
            TextSpeakWords = new Dictionary<string, string>(); //Create dictionary for textspeak words 
            
            StreamReader reader = null;
            //Try reading file from current directory or project directory
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, @"Data\", "textwords.csv")))
            {
                reader = new StreamReader(File.OpenRead(Path.Combine(Environment.CurrentDirectory, @"Data\",
                    "textwords.csv")));
            }
            else if (File.Exists(@"../../Data/textwords.csv"))
            {
                reader = new StreamReader(File.OpenRead(@"../../Data/textwords.csv"));
            }

            //If file not found throw exception
            if (reader == null)
            {
                throw new FileNotFoundException(
                    "Could not locate textspeak file. Please ensure it exists in the same directory as the program and is called textwords.csv");
            }

            //Read the file
            while (!reader.EndOfStream)
            {
                var parts = reader.ReadLine()?.Split(',');
                if (parts != null) TextSpeakWords.Add(parts[0], parts[1]);
            }
        }
    }
}