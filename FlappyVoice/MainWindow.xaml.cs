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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;



namespace FlappyVoice
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        double topScore = Player.Score;

        bool mousedown = false;
        double ob_gap_end = 60; //minimum der größe der Lücke

        double ob_width = 50.0; //Breite des Obstacles
        double ob_gap_base = 200.0; //Größe der Lücke
        double ob_speed = 4.0; //Geschwindigkeit der Obstacles
        double partitions = 4.0; //Abstände zwischen den Obstaclen

        double player_forward = 100.0; //margin left
        double player_size = 10.0; //größe
        double player_lift = 200.0; //margin bottom
        double player_speed = 0.0; //wie schnell es runter "fallen" soll

        DispatcherTimer timer;
        int counter = 0;

        Random r = new Random();
        StartWindow startWin = new StartWindow();

        List<Obstacles> obstactles = new List<Obstacles>();
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

        }



        void timer_Tick(object sender, EventArgs e)
        {
            counter++;
            if (counter > topScore)
            {
                topScore = counter;
            }

            canvas_base.Children.Clear();



            //Start - Dieser Block kreiert Textblkc für aktuellen Score, Highscore und Name

            //Current PLayername

            TextBlock playerName = new TextBlock();

            playerName.Background = new SolidColorBrush(Colors.White);
            playerName.Margin = new Thickness(5, 2, 0, 0); //Left,Top,Right,Bottom
            playerName.FontSize = 20.0;

            playerName.Text = $"{Player.Name}";

            //current score

            TextBlock scores = new TextBlock();


            scores.Background = new SolidColorBrush(Colors.White);
            scores.Margin = new Thickness(5, 35, 5, 5);
            scores.FontSize = 20.0;
            scores.Text = "  " + counter.ToString() + "  ";

            //Highscore

            TextBlock topScoretext = new TextBlock(); //neuer Textblock für Highscore
            topScoretext.FontWeight = FontWeights.Bold;

            topScoretext.Background = new SolidColorBrush(Colors.White);
            topScoretext.Margin = new Thickness(5, 65, 5, 5);
            topScoretext.FontSize = 20.0;

            topScoretext.Text = "  " + topScore.ToString() + "  ";
            if (topScore == counter)
                topScoretext.Text = "  " + topScore.ToString() + " ! ";

            //current click count

            TextBlock clicks = new TextBlock();


            clicks.Background = new SolidColorBrush(Colors.White);
            clicks.Margin = new Thickness(5, 95, 5, 5);
            clicks.FontSize = 20.0;
            clicks.Text = "  " + Player.ClickCounter.ToString() + "  ";




            //End - Hier endet dieser Block

            double height = canvas_base.ActualHeight;
            double width = canvas_base.ActualWidth;

            //Wenn die Maus nicht gedrückt ist, soll der Player schneller fallen
            if (!mousedown || (mousedown && counter <= 0))
            {
                player_speed += 0.3;
                player_lift -= player_speed;
            }


            Rectangle you = new Rectangle() //Player
            {
                Width = player_size,
                Height = player_size,
                StrokeThickness = 2.0,
                Fill = new SolidColorBrush(Colors.Yellow)
            };

            //hier wird die Position des Players gesetzt
            you.SetValue(Canvas.TopProperty, height - player_lift);
            you.SetValue(Canvas.LeftProperty, player_forward);


            //Obstacles erstellen
            foreach (Obstacles ob in obstactles)
            {
                //hier wird der Abstand der Lücke verändert (also sie wird kleiner und kleiner)
                double ob_gap = ob_gap_base * ob.left / canvas_base.ActualWidth + ob_gap_end;
                double top_height = (height - ob_gap) * Math.Pow(Math.Sin((ob.height + ob.neg * 2 * ob.left / canvas_base.ActualWidth)), 2.0);

                Color colors = Colors.Green;

                Rectangle top = new Rectangle()
                {
                    Width = ob_width,
                    Height = top_height,
                    StrokeThickness = 2.0,
                    Fill = new SolidColorBrush(colors)
                };
                //Setzten der Values des oberen Teil des Obstacles
                top.SetValue(Canvas.TopProperty, 0.0);
                top.SetValue(Canvas.LeftProperty, ob.left);

                Rectangle bottom = new Rectangle()
                {
                    Width = ob_width,
                    Height = height - top_height - ob_gap,
                    Fill = new SolidColorBrush(colors)
                };

                //Setzten der Values des unteren Teil des Obstacles
                bottom.SetValue(Canvas.TopProperty, top_height + ob_gap);
                bottom.SetValue(Canvas.LeftProperty, ob.left);

                ob.visual_rect_top = top;
                ob.visual_rect_bottom = bottom;

                //fügt zum GUI hinzu
                canvas_base.Children.Add(top);
                canvas_base.Children.Add(bottom);

                ob.left -= ob_speed;

                //respawned praktisch wieder die obstacles (max. 4 ob und dann respawn neue obs mit anderen values)
                if (ob.left + ob_width < 0.0)
                {
                    ob.left = width;
                    ob.height = r.NextDouble();
                    ob.neg = (r.Next() % 2) * 2 - 1;
                    ob.hit = false;
                }


            }


            //fügt zum GUI hinzu (Name, Score, HIghscore, Player)
            canvas_base.Children.Add(playerName);
            canvas_base.Children.Add(scores);
            canvas_base.Children.Add(topScoretext);
            canvas_base.Children.Add(clicks);
            canvas_base.Children.Add(you);
            



            foreach (Obstacles obstacle in obstactles)
            {
                //schaut ob es collision gab
                if (obstacle.hit == false && collision(you, obstacle.visual_rect_top) || collision(you, obstacle.visual_rect_bottom))
                {

                    //MessageBox.Show("Game Over");

                    obstacle.hit = true;

                    //if (obstacle.hit == true)
                    //{
                    //    if (MessageBox.Show("Try again?", "GAME OVER", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    //    {
                    //        resetAll();
                    //        startWin.Show();
                    //        Close();
                    //        obstacle.hit = false;
                    //    }
                    //    break;
                    //}

                    if (topScore > Player.Score)
                    {
                        InfoList.WriteInfoList(Player.Name, Convert.ToInt16(topScore), Convert.ToInt16(Player.ClickCounter));
                    }
                    Player.ClickCounter = 0;
                    resetAll();
                    return;
                }

            }

        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {


            MouseDown += canvas_base_MouseDown;

            //damit das Spiel flüssig läuft
            timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.02) };
            timer.Tick += timer_Tick;
            timer.Start();

            resetAll();
        }



        private void resetAll()
        {
            counter = 0;

            obstactles.Clear();
            for (int i = 0; i < partitions; i++)
                obstactles.Add(new Obstacles() { height = r.NextDouble(), left = 500 + (canvas_base.ActualWidth + ob_width) * (i / partitions), neg = (r.Next() % 2) * 2 - 1 });


            player_lift = 200.0;
            player_speed = 0.0;

        }

        void canvas_base_MouseDown(object sender, MouseButtonEventArgs e)
        {
            player_speed = -5; //block geht um 5 "rauf"; 
            Player.ClickCounter++;
        }

        bool collision(Rectangle player, Rectangle obstacle)
        {
            double playerL = Convert.ToDouble(player.GetValue(Canvas.LeftProperty));
            double playerT = Convert.ToDouble(player.GetValue(Canvas.TopProperty));
            double playerR = playerL + player.Width;
            double playerB = playerT + player.Height;

            double obL = Convert.ToDouble(obstacle.GetValue(Canvas.LeftProperty));
            double obT = Convert.ToDouble(obstacle.GetValue(Canvas.TopProperty));
            double obR = obL + obstacle.Width;
            double obB = obT + obstacle.Height;

            //wenn player oben ankommt
            if (playerT < 0)
            {
                return true;

            }
            //wenn player unten ankommt
            if (playerB > canvas_base.ActualHeight)
            {
                return true;

            }

            //wenn player am obstacle ankommt
            return playerR > obL && playerL < obR && playerB > obT && playerT < obB;
        }

        
    }
}

