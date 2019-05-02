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
    class Obstacles
    {
        public int width = 50;
        public int position;
        public int speed = 3;

        public void CreateObstacles(bool startgame)
        {
            if(startgame == true)
            {
                Random r = new Random();
                Rectangle rect1 = new Rectangle() { Fill = Brushes.Blue, Height = 50, Width = 20 };

            }           

        }

        public void CreateObstacles(bool startgame,int length, int width)
        {

        }

        public void Move()
        {
            position -= speed;
            if(position < -width)
            {
                Respawn();
            }

        }

        public void Respawn()
        {
            CreateObstacles(true);

        }
        


    }
}
