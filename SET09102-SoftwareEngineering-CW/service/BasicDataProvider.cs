using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;

namespace SET09102_SoftwareEngineering_CW.service
{
    public class BasicDataProvider
    {
        private static BasicDataProvider instance;
        private Dictionary<string, string> textSpeakWords = new Dictionary<string, string>();

        private BasicDataProvider()
        {
            ReadTextSpeakFile();
        }

        public static BasicDataProvider GetInstance()
        {
            return instance ?? (instance = new BasicDataProvider());
        }

        public Dictionary<string, string> TextSpeakWords
        {
            get => textSpeakWords;
            set => textSpeakWords = value;
        }

        private void ReadTextSpeakFile()
        {
            var words = new Dictionary<string, string>();

            var reader = new StreamReader(File.OpenRead(Path.Combine(Environment.CurrentDirectory, @"Data\", "textwords.csv")));
            while (!reader.EndOfStream)
            {
                var parts = reader.ReadLine().Split(',');
                words.Add(parts[0], parts[1]);
            }

        }
    }
}