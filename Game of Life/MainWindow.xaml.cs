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
using System.Diagnostics;

namespace Game_of_Life
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int squareZise = 20;
        bool SBtnZustand = true;
        Stopwatch timer = new Stopwatch();
        


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
            int zähler = 0;

            if (Mouse.LeftButton == MouseButtonState.Pressed)
                ((Rectangle)sender).Fill =  ((Rectangle)sender).Fill = (((Rectangle)sender).Fill == Brushes.White) ? Brushes.Black : Brushes.White;

            foreach (Rectangle child in Playfield.Children)
            {
                if (child.Fill == Brushes.White)
                {
                    Debug.WriteLine(zähler++);
                }
            }
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
            // Wenn Btn zum Starten bestätigt wurde
            if (SBtnZustand)
            {
                timer.Start();  // Startet Timer
                TimerTxt2.Text = timer.Elapsed.ToString(); // Zeigt die Vergangene Zeit seit dem Timer gestartet wurde
                StartBtn.Content = "Stopen"; // Setzt den Btn Text auf Stopen
                SBtnZustand = false; // Setzt die Variable SBtnZustand auf false damit der Button Zustand ermittelt werden kann
            }

            // Wenn Btn zum Stoppen betätigt wurde
            else
            {
                timer.Stop(); // Stopt den Timer
                timer.Reset(); // Setzt den Timer auf null
                TimerTxt2.Text = "Gestoppt!"; // Setzt den Timer Txt auf Gestoppt
                StartBtn.Content = "Starten"; // Setzt den Btn Text auf Starten
                SBtnZustand = true; // Setzt die Variable SBtnZustand auf true damit der Button Zustand ermittelt werden kann
            }

            // ToDO... Timer Text muss nach jedem Berechnen einmal aktualiesiert werden.

        }
    }
}
