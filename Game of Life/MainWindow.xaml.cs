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

namespace Game_of_Life
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int squareZise = 20;
        int numberBlack = 0, numberWhite = 0;
        int count = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawPlayfield();
        }

        /// <summary>
        /// Creates the Playfield
        /// </summary>
        private void DrawPlayfield()
        {
            bool doneDrawingPlayfield = false;
            int nextY = 0, nextX = 0;

            while (doneDrawingPlayfield == false)
            {
                Rectangle r = new Rectangle
                {
                    Width = squareZise - 2.0,
                    Height = squareZise - 2.0,
                    Stroke = Brushes.DarkGray,
                    Fill = Brushes.White
                };
                Playfield.Children.Add(r);
                Canvas.SetTop(r, nextY);
                Canvas.SetLeft(r, nextX);

                nextX += squareZise;
                if(nextX >= Playfield.ActualWidth)
                {
                    nextX = 0;
                    nextY += squareZise;
                }

                r.MouseEnter += R_MouseEnter;
                r.MouseDown += R_MouseDown;

                if (nextY >= Playfield.ActualHeight)
                    doneDrawingPlayfield = true;
            }
        }

        /// <summary>
        /// Changes colors of cells but only if the user holds down the left mouse button and enters a cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void R_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                ((Rectangle)sender).Fill = (((Rectangle)sender).Fill == Brushes.White) ? Brushes.Black : Brushes.White;
        }

        /// <summary>
        /// Changes color of cells when user clicks on a cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void R_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Rectangle)sender).Fill = (((Rectangle)sender).Fill == Brushes.White) ? Brushes.Black : Brushes.White;
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
