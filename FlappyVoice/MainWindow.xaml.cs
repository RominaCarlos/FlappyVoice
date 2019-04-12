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


namespace FlappyVoice
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
<<<<<<< HEAD
           



            Rectangle rec = new Rectangle()
            {
                Fill = Brushes.Red
                
            };

            Grid.SetRow(rec, 2);
            Grid.SetColumn(rec, 1);
            Keyboard.IsKeyDown();

            if (e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("Enter Key Pressed ");
            }







=======
            InitializeComponent();
>>>>>>> 7d3db8c80d6cb981410f191a4f5b62a0723230ae

        }
    }
}
