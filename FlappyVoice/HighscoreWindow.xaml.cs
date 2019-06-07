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
    /// Interaktionslogik für HighscoreWindow.xaml
    /// </summary>
    public partial class HighscoreWindow : Window
    {
        public HighscoreWindow()
        {
            InitializeComponent();

            string[] player = null;
            string[] AllLines = InfoList.AllLines;
            txtbPlayerList.Inlines.Add($"Name-Score-Clicks");
            foreach (string playerInfo in AllLines)
            {
                player = playerInfo.Split(';');
                txtbPlayerList.Inlines.Add($"\n{player[0]};{player[1]};{player[2]}");
            }
        }

        private void Highscores_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                MainWindow mainWin = new MainWindow();
                mainWin.Show();
                Close();
            }
        }
    }
}
