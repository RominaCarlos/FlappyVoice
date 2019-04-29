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
using System.Drawing;

namespace FlappyBirdAttempt
{
    class PlayerMovement
    {
        public Ellipse playerObj { get; set; }

        internal void MovePlayerObj()
        {
            //switch ( )
            //{
            //    case 
            //}
        }

    }

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public int speed_fall = 4;

        public MainWindow()
        {
            InitializeComponent();

        }


        private int increment = 0;

        private void dtTicker(object sender, EventArgs e)
        {
            //ball += speed_fall;
            increment++;
            timer.Content = increment.ToString();
            
        }

        private void Timer_Start(object sender, KeyEventArgs e)
        {
            //Spiel wird gestartet wenn space gedrückt wird
            if (e.Key == Key.Space)
            {
                DispatcherTimer dt = new DispatcherTimer();
                dt.Interval = TimeSpan.FromSeconds(1);
                dt.Tick += dtTicker;
                dt.Start();




            }

            //beenden des spiels
            if(e.Key == Key.Escape)
            {
                MessageBoxResult question = MessageBox.Show("Are you sure you want to quit?","lol", MessageBoxButton.YesNo);
                if(question == MessageBoxResult.Yes)
                {
                    this.Close();
                }
                
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                MessageBox.Show("Du hast die Taste 0 betätigt");
            }
        }
    }
}
