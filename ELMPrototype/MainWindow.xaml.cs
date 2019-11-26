using System;
using System.IO;
using System.Windows;
using ELMPrototype.bdo;
using ELMPrototype.exceptions;
using ELMPrototype.service;
using Microsoft.Win32;

namespace ELMPrototype
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
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
            //TrendingList.Items.SortDescriptions.Add(new SortDescription("Count", ListSortDirection.Descending));
            MentionList.ItemsSource = basicDataProvider.MentionList;
            SirList.ItemsSource = basicDataProvider.SirList;
            QuarantineList.ItemsSource = basicDataProvider.QuarantineList;
        }

        private void UploadFileBtn_Click(object sender, RoutedEventArgs e)
        {
            //File dialog to open input file
            var fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".txt";
            fileDialog.Filter = "Text documents (.txt)|*.txt";
            var result = fileDialog.ShowDialog();

            //If file selected, get the file
            if (!result.HasValue || !result.Value) return;
            var file = new StreamReader(fileDialog.FileName);

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
                    header = line;
                else //Build message body
                    body = string.Concat(body, line, Environment.NewLine); //Added NewLine as it is not included in line

                count++; //Increment count
            }

            if (count == 0) return; //No message in progress
            //Process message in progress if not ended by message divider (---)
            resultWindow = new ProcessingResult(ProcessMessage(new RawMessage(header, body)));
            resultWindow.Show();
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            //Process text from text boxes and output to result textbox
            ResultBox.Text = ProcessMessage(new RawMessage(InputTextHeader.Text, InputTextBody.Text));
        }

        private void SaveResultBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveToOutputFile(ResultBox.Text);
        }

        private static string ProcessMessage(RawMessage message)
        {
            try
            {
                //Process Message
                return MessageProcessor.GetInstance().ProcessMessageIndentedJson(message);
            }
            catch (InputException except)
            {
                //If InputException thrown open show dialog with error message
                MessageBox.Show(except.Message, "Error parsing Input");
            }
            catch (Exception except)
            {
                //If Exception thrown open show dialog with error message
                MessageBox.Show(except.Message, "Error");
            }

            //Should never reach this point but return null if it does.
            return null;
        }

        public static void SaveToOutputFile(string text)
        {
            var outputFilePath = @"./output.json";
            if (File.Exists(outputFilePath))
                //Append text to existing file
                File.AppendAllText(outputFilePath, Environment.NewLine + text);
            else
                //Write to output file
                File.WriteAllText(outputFilePath, text);
        }
    }
}