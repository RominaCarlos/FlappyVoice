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

namespace FlappyVoice
{
    /// <summary>
    /// Interaktionslogik für StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(txtInputName.Text))
            {
                MessageBox.Show("Bitte gib einen Namen ein oder wähle eines der gespeicherten");

            }
            else
            {
                MainWindow mainWin = new MainWindow();
                mainWin.Show();
                Close();
                
            }
            
        }

        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
