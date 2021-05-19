using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Linq;

namespace Game_of_Life
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int breiteViereck = 25;
        const int hoeheViereck = 25;
        Rectangle[,] zellen = new Rectangle[breiteViereck, hoeheViereck];
        //int naechstesY = 0, naechstesX = 0;
        bool SBtnZustand = false;
        Stopwatch timer = new Stopwatch();
        int maxX = 0; // Maximale Breite des Spielfelds
        int maxY = 0; // Maximale Höhe des Spielfelds

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            erstelleSpielfeld();
        }

        /// <summary>
        /// Erstellt das Spielfeld
        /// </summary>
        private void erstelleSpielfeld()
        {
            for(int i = 0; i < breiteViereck; i++)
            {
                for(int j = 0; j < hoeheViereck; j++)
                {
                    Rectangle r = new Rectangle     // Erstellt ein Rechteck in der Variable r
                    {
                        Width = Spielfeld.ActualWidth / breiteViereck - 1.0, // Gibt die Höhe des Rechtecks an
                        Height = Spielfeld.ActualWidth / hoeheViereck - 3.0, // Gibt die Breite des Rechtecks an
                        Stroke = Brushes.DarkGray,                           // Verleiht Zellen einen grauen Rand
                        Fill = Brushes.White,                                // Gibt die Farbe des Inhalts der Zellen an
                        //Margin = new Thickness(naechstesX, naechstesY, 0, 0) // Fügt eine Margin für die Positionierung hinzu
                    };
                    Spielfeld.Children.Add(r);   // Fügt ein Rechteck als Kindelement zu Spielfeld hinzu

                    Canvas.SetLeft(r, j * Spielfeld.ActualWidth / breiteViereck);
                    Canvas.SetTop(r, i * Spielfeld.ActualHeight / hoeheViereck);
                    /*naechstesX += breiteViereck;            // Rutscht um eine X Koordinate nach Rechts
                    if (naechstesX >= Spielfeld.ActualWidth) // Prüft ob die aktuelle Position den rechten Rand des Elternelements erreicht hat
                    {
                        maxX = naechstesX;
                        x.Text = "X: " + maxX.ToString();
                        naechstesX = 0;             // Setzt X wieder auf 0
                        naechstesY += hoeheViereck; // erhöht Y um hoeheViereck und rutscht somit eins tiefer
                    }

                    

                    if (naechstesY >= Spielfeld.ActualHeight) // Prüft ob die aktuelle Position den unteren Rand des Elternelements erreicht hat
                    {
                        maxY = naechstesY;
                        y.Text = "Y: " + maxY.ToString();
                    }*/
                    r.MouseEnter += R_MouseEnter;
                    r.MouseDown += R_MouseDown;

                    zellen[i, j] = r;
                }
            }
        }
        
        /// <summary>
        /// Ändert die Farbe einer Zelle nur wenn der Benutzer die linke Maustaste gedrückt hält und eine Zelle betritt
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
        /// Ändert die Farbe einer Zelle wenn der Benutzer die linke Maustaste drückt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void R_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Rectangle)sender).Fill = (((Rectangle)sender).Fill == Brushes.White) ? Brushes.Black : Brushes.White;
            ax.Text = "X: "+((Rectangle)sender).Margin.Left.ToString();
            ay.Text = "Y: "+((Rectangle)sender).Margin.Top.ToString();

        }

        private async void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (SBtnZustand == false)
            {
                SBtnZustand = true;
                StartBtn.Content = "Stoppen"; // Setzt den Btn Text auf Stoppen
                while (SBtnZustand) {
                    await GenerationsWechsel();
                    await Task.Delay(250);
                }
            }
            else
            {
                SBtnZustand = false;
                StartBtn.Content = "Starten"; // Setzt den Btn Text auf Starten
                return;
            }

        }
        /*if (SBtnZustand)
         {
             timer.Start();  // Startet Timer
             TimerTxt2.Text = timer.Elapsed.ToString(); // Zeigt die Vergangene Zeit seit dem Timer gestartet wurde
             StartBtn.Content = "Stoppen"; // Setzt den Btn Text auf Stoppen
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
             */

        private async Task GenerationsWechsel()
        {
            int[,] anzahlNachbarn = new int[breiteViereck, hoeheViereck];

            for (int i = 0; i < breiteViereck; i++)
            {
                for (int j = 0; j < hoeheViereck; j++)
                {
                    int nachbarn = 0;
                    int obenTorus = i - 1;
                    int untenTorus = i + 1;
                    int rechtsTorus = j + 1;
                    int linksTorus = j - 1;
                    if (obenTorus < 0)
                    { obenTorus = hoeheViereck - 1; }
                    if (untenTorus == hoeheViereck)
                    { untenTorus = 0; }
                    if (rechtsTorus == breiteViereck)
                    { rechtsTorus = 0; }
                    if (linksTorus < 0)
                    { linksTorus = breiteViereck - 1; }

                    if (zellen[obenTorus, linksTorus].Fill == Brushes.Black)
                    { nachbarn++; }
                    if (zellen[obenTorus, j].Fill == Brushes.Black)
                    { nachbarn++; }
                    if (zellen[obenTorus, rechtsTorus].Fill == Brushes.Black)
                    { nachbarn++; }
                    if (zellen[i, linksTorus].Fill == Brushes.Black)
                    { nachbarn++; }
                    if (zellen[i, rechtsTorus].Fill == Brushes.Black)
                    { nachbarn++; }
                    if (zellen[untenTorus, linksTorus].Fill == Brushes.Black)
                    { nachbarn++; }
                    if (zellen[untenTorus, j].Fill == Brushes.Black)
                    { nachbarn++; }
                    if (zellen[untenTorus, rechtsTorus].Fill == Brushes.Black)
                    { nachbarn++; }

                    anzahlNachbarn[i, j] = nachbarn;

                }
            }
            for (int i = 0; i < breiteViereck; i++)
            {
                for (int j = 0; j < hoeheViereck; j++)
                {
                    if (anzahlNachbarn[i, j] < 2 || anzahlNachbarn[i, j] > 3)
                    {
                        zellen[i, j].Fill = Brushes.White;
                    }
                    else if (anzahlNachbarn[i, j] == 3)
                    {
                        zellen[i, j].Fill = Brushes.Black;
                    }
                }
            }
        }

        /// <summary>
        /// Setzt das Spielfeld zurück
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNeu_Click(object sender, RoutedEventArgs e)
        {
            Spielfeld.Children.OfType<Rectangle>().ToList().ForEach(x => x.Fill = Brushes.White);
            SBtnZustand = false;
            StartBtn.Content = "Starten";
        }

            /*private void positionErmittlung()
            {
                int ix = 0;
                bool ex = true;
                int iy = 0;

                while (true) // Wird beendet sobald Methode ein Return abgegeben wird
                {
                    if (ex)
                    {
                        ix += viereckGroeße;
                    }
                    else
                    {
                        iy += viereckGroeße;

                        if (iy >= Spielfeld.ActualHeight)
                        {
                            x.Text = ix.ToString();
                            y.Text = iy.ToString();
                            // return ix.ToString() + "," + iy.ToString();
                        }
                    }
                    if (ix >= Spielfeld.ActualWidth)
                    {
                        ex = false;
                    }
                }
            }*/
        }
}
