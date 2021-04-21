﻿using System;
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
        int maxX = 0; // Maximale Breite des Spielfelds
        int maxY = 0; // Maximale Höhe des Spielfelds


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

            while (doneDrawingPlayfield == false) // Wird solange ausgeführt bis Spielfeld generiert wurde
            {
                Rectangle r = new Rectangle // Erstellt ein Rechteck in der Variable r
                {
                    Width = squareZise - 2.0, // Gibt die Höhe des Rechtecks an
                    Height = squareZise - 2.0, // Gibt die Breite des Rechtecks an
                    Stroke = Brushes.DarkGray, // Wenn Stroke dann soll das Rechteck DarkGrey sein
                    Fill = Brushes.White, // Wenn Fill dann soll das Rechteck White sein
                    Margin = new Thickness(nextX, nextY, 0, 0) // Fügt eine Margin für die Positionierung hinzu
                };
                Playfield.Children.Add(r); // Fügt einen Rechteck als Kindelement zu Playfield hinzu

                nextX += squareZise; // Rutscht um eine X Coordinate nach Rechts
                if(nextX >= Playfield.ActualWidth) // Prüft ob die aktuelle Position den rechten Rand des Elternelements erreicht hat
                {
                    maxX = nextX;
                    x.Text = "X: " + maxX.ToString();
                    nextX = 0; // Setzt X wieder auf 0
                    nextY += squareZise; // erhöht Y um 1 und rutscht somit eins tiefer
                }

                r.MouseEnter += R_MouseEnter;
                r.MouseDown += R_MouseDown;

                if (nextY >= Playfield.ActualHeight) // Prüft ob die aktuelle Position den unteren Rand des Elternelements erreicht hat
                {
                    maxY = nextY;
                    y.Text = "Y: " + maxY.ToString();
                    doneDrawingPlayfield = true; // Speichert True in doneDrawingPlayfield um Spielfeld Generierung abzuschließen
                }
                    
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
            {
                ((Rectangle)sender).Fill = ((Rectangle)sender).Fill = (((Rectangle)sender).Fill == Brushes.White) ? Brushes.Black : Brushes.White;
                ax.Text = "X: " + ((Rectangle)sender).Margin.Left.ToString();
                ay.Text = "Y: " + ((Rectangle)sender).Margin.Top.ToString();
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
            ax.Text = "X: "+((Rectangle)sender).Margin.Left.ToString();
            ay.Text = "Y: "+((Rectangle)sender).Margin.Top.ToString();

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

        private void positionErmittelung()
        {

            int ix = 0;
            bool ex = true;
            int iy = 0;

            while (true) // Wird beendet sobald Methode ein Return abgegeben wird
            {
                if (ex)
                {
                    ix += squareZise;
                }
                else
                {
                    iy += squareZise;

                    if (iy >= Playfield.ActualHeight)
                    {
                        x.Text = ix.ToString();
                        y.Text = iy.ToString();
                        // return ix.ToString() + "," + iy.ToString();
                    }
                }
                if (ix >= Playfield.ActualWidth)
                {
                    ex = false;
                }
            }
        }
    }
}
