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

        double topScore = 0;

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

            playerName.Text = "Player: Romina";

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

/*
namespace FlappyVoice
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        Random r = new Random();

        int straightCounter = 0;

        double topScore = 0;    //Top Score des Spielers

        bool spaceDown = false;  //Betätigung der Space-Taste
        double obGapEnd = 60;

        int[] xs = new int[30];
        int[] ys = new int[30];

        int mouseUpThreshold = 30;

        double obWidth = 50.0;   //Obstacle Breite
        double obGapBase = 200.0;    //Obstacle
        double obSpeed = 4.0;   //Veränderung der Obstacle Größe
        double partitions = 3.0;

        double playerForward = 100.0;   //Position des Spielers
        double playerSize = 10.0;   //Größe des Spielcharakters
        double playerLift = 200.0;
        double playerSpeed = 0.0;   //Geschwindigkeit, die der Spieler nach vorne springt
         
        DispatcherTimer timer;   //Spieltimer (?)
        int counter = 0;
        int lastMouseCounter = 0;

        List<Obstacles> obstacleList = new List<Obstacles>();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindowLoaded;
        }

        void timerTick(object sender, EventArgs e)
        {
            /*Diese Methode updated Daten, die pro Tick verändert werden´:
             * Spieler Bewegung
             * Score
             * Obstacles

straightCounter++;
            counter++;
            if ((counter - lastMouseCounter) > mouseUpThreshold)
            {
                counter += 5;
            }
            if (spaceDown)
            {
                counter -= 10;
            }
            if (counter > topScore)
            {
                topScore = counter;
            }

            canvasBase.Children.Clear();


            //Start - Dieser Block kreiert Textblkc für aktuellen Score, Highscore und Name

            TextBlock scores = new TextBlock(); //kreiirt ein Textblock für scores
            if ((counter - lastMouseCounter) > mouseUpThreshold)
            {
                scores.FontWeight = FontWeights.Bold;
            }


            scores.Background = new SolidColorBrush(Colors.White);
scores.Margin = new Thickness(5.0);
scores.FontSize = 20.0;
            scores.Text = "  " + counter.ToString() + "  ";

            TextBlock topScoretext = new TextBlock(); //neuer Textblock für Highscore
topScoretext.FontWeight = FontWeights.Bold;

            topScoretext.Background = new SolidColorBrush(Colors.White);
topScoretext.Margin = new Thickness(5, 35, 5, 5);
topScoretext.FontSize = 20.0;
            topScoretext.Text = "  " + topScore.ToString() + "  ";
            if (topScore == counter)// && counter % 20 < 10
                topScoretext.Text = "  " + topScore.ToString() + " ! ";

            TextBlock playerName = new TextBlock();
playerName.Text = "";

            //End - Hier endet dieser Block

            double height = canvasBase.ActualHeight;
double width = canvasBase.ActualWidth;

            if (!spaceDown || (spaceDown && counter <= 0))
            {
                playerSpeed += 0.3;
                playerLift -= playerSpeed;
            }

            Rectangle gameCharacter = new Rectangle() //Spielcharakter
            {
                Width = playerSize,
                Height = playerSize,
                StrokeThickness = 2.0,
                Fill = new SolidColorBrush(Colors.Yellow)
            };

gameCharacter.SetValue(Canvas.TopProperty, height - playerLift);
            gameCharacter.SetValue(Canvas.LeftProperty, playerForward);

            foreach (Obstacles ob in obstacleList) //Obstacles erstellen
            {
                double obGap = obGapBase * ob.left / canvasBase.ActualWidth + obGapEnd;
double topHeight = (height - obGap) * Math.Pow(Math.Sin((ob.height + ob.neg * 2 * ob.left / canvasBase.ActualWidth)), 2.0);
Color colors = ob.hit ? Colors.Red : Colors.Green;

Rectangle obTop = new Rectangle() //obere Teil des Obstacles
{
    Width = obWidth,
    Height = topHeight,
    StrokeThickness = 2.0,
    Fill = new SolidColorBrush(colors)
};
obTop.SetValue(Canvas.TopProperty, 0.0);
                obTop.SetValue(Canvas.LeftProperty, ob.left);

                Rectangle obBottom = new Rectangle()
                {
                    Width = obWidth,
                    Height = height - topHeight - obGap,
                    Fill = new SolidColorBrush(colors)
                };
obBottom.SetValue(Canvas.TopProperty, topHeight + obGap);
                obBottom.SetValue(Canvas.LeftProperty, ob.left);

                ob.visual_rect_top = obTop;
                ob.visual_rect_bottom = obBottom;
                canvasBase.Children.Add(obTop);
                canvasBase.Children.Add(obBottom);

                ob.left -= obSpeed;

                if (ob.left + obWidth< 0.0)
                {
                    ob.left = width;
                    ob.height = r.NextDouble();
                    ob.neg = (r.Next() % 2) * 2 - 1;
                    ob.hit = false;
                }

                //fügt zum GUI hinzu
                canvasBase.Children.Add(scores);
                canvasBase.Children.Add(topScoretext);
                canvasBase.Children.Add(playerName);

                if (counter > 30 || (counter< 30 && counter % 5 < 3))
                {
                    canvasBase.Children.Add(gameCharacter);
                }

                foreach (Obstacles obstacle in obstacleList)
                {
                    if (!obstacle.hit && collision(gameCharacter, obstacle.visual_rect_top)){
                        MessageBox.Show("Game Over");
                        obstacle.hit = true;
                        resetAll();
                        return;
                    }
                }
            }
        }
        void MainWindowLoaded(object sencer, RoutedEventArgs e)
{
    for (int i = 0; i < 30; i++)
    {
        xs[i] = r.Next() % Convert.ToInt16(canvasBase.ActualWidth);
        ys[i] = r.Next() % Convert.ToInt16(canvasBase.ActualHeight);
    }

    MouseDown += canvasBaseMouseDown;
    KeyDown += MainWindowKeyDown;
    KeyUp += MainWindowKeyUp;

    timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.02) };
    timer.Tick += timerTick;
    timer.Start();

    resetAll();
}

void MainWindowKeyUp(object sender, KeyEventArgs e)
{
    if (e.Key == Key.Space)
    {
        spaceDown = false;
    }
}

void MainWindowKeyDown(object sender, KeyEventArgs e)
{
    if (e.Key == Key.Space && counter > 100)
    {
        spaceDown = true;
        playerSpeed = 0;
        counter -= 100;
    }
}

void resetAll()
{
    counter = 0;

    obstacleList.Clear();
    for (int i = 0; i < partitions; i++)
    {
        obstacleList.Add(new Obstacles() { height = r.NextDouble(), left = 500 + (canvasBase.ActualHeight + obWidth) * (i / partitions), neg = (r.Next() % 2) * 2 - 1 });
    }

    playerLift = 200.0;
    playerSpeed = 0.0;
}


void canvasBaseMouseDown(object sender, MouseButtonEventArgs e)
{
    playerSpeed = -5;
    lastMouseCounter = counter;
}

bool collision(Rectangle r1, Rectangle r2)
{
    double r1L = Convert.ToDouble(r1.GetValue(Canvas.LeftProperty));
    double r1T = Convert.ToDouble(r1.GetValue(Canvas.LeftProperty));
    double r1R = r1L + r1.Width;
    double r1B = r1T + r1.Height;

    double r2L = Convert.ToDouble(r1.GetValue(Canvas.LeftProperty));
    double r2T = Convert.ToDouble(r1.GetValue(Canvas.LeftProperty));
    double r2R = r2L + r2.Width;
    double r2B = r2T + r2.Height;

    if (r1T < 0)
    {
        return true;
    }
    if (r1B > canvasBase.ActualHeight)
    {
        return true;
    }

    return r1R > r2L && r1L < r2R && r1B > r2T && r1T < r2B;
}
    }
 */

