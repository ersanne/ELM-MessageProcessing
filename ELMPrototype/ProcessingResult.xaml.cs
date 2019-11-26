using System.Windows;

namespace ELMPrototype
{
    /// <summary>
    ///     Interaction logic for ProcessingResult.xaml
    /// </summary>
    public partial class ProcessingResult : Window
    {
        public ProcessingResult(string resultText)
        {
            InitializeComponent();
            ResultTextBox.Text = resultText;
        }

        private void SaveToFileBtn_Click(object sender, RoutedEventArgs e)
        {
            //Save to output.json
            MainWindow.SaveToOutputFile(ResultTextBox.Text);
        }
    }
}