using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ELMPrototype.Tests.bdo;

namespace ELMPrototype.Tests
{
    /// <summary>
    /// Interaction logic for ProcessingResult.xaml
    /// </summary>
    public partial class ProcessingResult : Window
    {
        public ProcessingResult(string resultText)
        {
            InitializeComponent();
            ResultTextBox.Text = resultText;
        }
    }
}
