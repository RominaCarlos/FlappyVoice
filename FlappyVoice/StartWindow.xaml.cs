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

            string[] bestPlayer = InfoList.BestPlayer();

            lblHighestScore.Content = $"Name: {bestPlayer[0]} \n" +
                $"Punkte: {bestPlayer[1]}\n" +
                $"Clicks:{bestPlayer[2]}";

            string[] player = null;
            string[] AllLines = InfoList.AllLines;
            foreach (string playerInfo in AllLines)
            {
                player = playerInfo.Split(';');
                cboxNameSelection.Items.Add(player[0]);
            }
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            string[] playerInfo = null;

            try
            {
                Player.Name = cboxNameSelection.Text;
                InfoList.WriteInfoList(cboxNameSelection.Text, 0, 0);

                playerInfo = InfoList.ReadInfoList(cboxNameSelection.Text);
                Player.Score = Int16.Parse(playerInfo[1]);
                Player.ClickCounter = Int16.Parse(playerInfo[2]);

                MainWindow mainWin = new MainWindow();
                mainWin.Show();
                Close();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Bitte gib einen Namen ein oder wähle eines der gespeicherten");
            }

        }

        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnHighscores_Click(object sender, RoutedEventArgs e)
        {
            HighscoreWindow highscores = new HighscoreWindow();
            highscores.Show();
            Close();
        }
    }
}



