using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Win32;
using SET09102_SoftwareEngineering_CW.bdo;
using SET09102_SoftwareEngineering_CW.exceptions;
using SET09102_SoftwareEngineering_CW.service;

namespace SET09102_SoftwareEngineering_CW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //Get access to data provider / data store
            var basicDataProvider = BasicDataProvider.GetInstance();

            //Initialize Window
            InitializeComponent();

            //Set ItemsSource for ListBoxes
            TrendingList.ItemsSource = basicDataProvider.TrendingList;
            TrendingList.Items.SortDescriptions.Add(new SortDescription("Count", ListSortDirection.Descending));
            MentionList.ItemsSource = basicDataProvider.MentionList;
        }

        private void UploadFileBtn_Click(object sender, RoutedEventArgs e)
        {
            //File dialog to open input file
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".txt";
            fileDialog.Filter = "Text documents (.txt)|*.txt";
            var result = fileDialog.ShowDialog();

            //If file selected, get the file
            if (!result.HasValue || !result.Value) return;
            var file = new System.IO.StreamReader(fileDialog.FileName);

            //Variables required
            string line;
            var count = 0;
            var header = "";
            var body = "";
            ProcessingResult resultWindow;

            //Read file
            while ((line = file.ReadLine()) != null)
            {
                if (line == "---" && !header.Equals(""))
                {
                    //Process message and display result in a new window (As there may be multiple messages open one window for each message)
                    resultWindow = new ProcessingResult(ProcessMessage(new RawMessage(header, body)));
                    resultWindow.Show();
                    
                    count = 0; //Reset count to start a new message
                    header = ""; //Reset message header
                    body = ""; //Reset message body
                    continue; //Skip rest of iteration as this message is finished
                }
                if (count == 0) //New message, first line is the header
                {
                    header = line;
                }
                else //Build message body
                {
                    body = string.Concat(body, line, Environment.NewLine); //Added NewLine as it is not included in line
                }

                count++; //Increment count
            }

            if (count == 0) return; //No message in progress
            //Process message in progress if not ended by message divider (---)
            resultWindow = new ProcessingResult(ProcessMessage(new RawMessage(header, body)));
            resultWindow.Show();
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultBox.Text = ProcessMessage(new RawMessage(InputTextHeader.Text, InputTextBody.Text));
        }

        private static string ProcessMessage(RawMessage message)
        {
            try
            {
                return MessageProcessor.GetInstance().ProcessMessage(message);
            }
            catch (InputException except)
            {
                MessageBox.Show(except.Message, "Error parsing Input");
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error");
            }
            return null;
        }
    }
}