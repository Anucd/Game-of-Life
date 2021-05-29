﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Linq;

namespace Game_of_Life
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int tod = 100;
        int lebend = 0;
        bool init = true;
        int breiteViereck = 25;
        int hoeheViereck = 25;
        Rectangle[,] zellen = new Rectangle[25, 25];
        bool SBtnZustand = false;
        Stopwatch timer = new Stopwatch();
        Random rand = new Random();
        Brush[] brushes = new Brush[] {
            Brushes.Black,
            Brushes.White
        };
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
            for (int i = 0; i < breiteViereck; i++)
            {
                for(int j = 0; j < hoeheViereck; j++)
                {
                    Rectangle r = new Rectangle     // Erstellt ein Rechteck in der Variable r
                    {
                        Width = Spielfeld.ActualWidth / breiteViereck - 2.0,    // Gibt die Breite des Rechtecks an
                        Height = Spielfeld.ActualHeight / hoeheViereck - 2.0,   // Gibt die Höhe des Rechtecks an
                        Stroke = Brushes.DarkGray,                              // Verleiht Zellen einen grauen Rand
                        Fill = Brushes.White,                                   // Gibt die Farbe des Inhalts der Zellen an
                    };
                    Console.WriteLine(Spielfeld.ActualWidth);
                    Spielfeld.Children.Add(r);      // Fügt ein Rechteck als Kindelement zu Spielfeld hinzu

                    Canvas.SetLeft(r, j * Spielfeld.ActualWidth / breiteViereck);
                    Canvas.SetTop(r, i * Spielfeld.ActualHeight / hoeheViereck);

                    r.MouseEnter += R_MouseEnter;
                    r.MouseDown += R_MouseDown;

                    zellen[i, j] = r;
                }
            }
            if (init) { init = false; }
        }


        private void aenderSpielfeld(object sender, TextChangedEventArgs e)
        {
            if (!init)
            {

                try
                {
                    SpielfeldY.Text = SpielfeldX.Text;

                    if (SpielfeldX.Text != "" && SpielfeldY.Text != "")
                    {
                        hoeheViereck = int.Parse(SpielfeldX.Text);
                        breiteViereck = int.Parse(SpielfeldX.Text);
                        Spielfeld.Children.Clear();

                        zellen = new Rectangle[breiteViereck, hoeheViereck];
                        tod = hoeheViereck * breiteViereck;
                        erstelleSpielfeld();
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        int CursorIndex = SpielfeldX.SelectionStart - 1;
                        SpielfeldX.Text = SpielfeldX.Text.Remove(CursorIndex, 1);

                        //Align Cursor to same index
                        SpielfeldX.SelectionStart = CursorIndex;
                        SpielfeldX.SelectionLength = 0;

                        SpielfeldY.Text = SpielfeldX.Text;
                    }
                    catch (Exception) { }
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
        }
        /// <summary>
        /// Startet/Stoppt das Game of Life
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            string timerzeit = "";
            if (!SBtnZustand)
            {
                SBtnZustand = true;
                StartBtn.Content = "Generationswechsel Anhalten"; // Setzt den Btn Text auf Stoppen
                SpielfeldX.IsEnabled = false;
                TimerTxt.Visibility = Visibility.Visible;
                TimerTxt2.Visibility = Visibility.Visible;
                ZufallBtn.IsEnabled = false;
                NeuBtn.IsEnabled = false;
                SchrittBtn.IsEnabled = false;
                timer.Start();  // Startet Timer
                while (SBtnZustand) {
                    GenerationsWechsel();
                    timerzeit = timer.Elapsed.ToString();
                    weiss.Text = tod.ToString();
                    schwarz.Text = lebend.ToString();
                    TimerTxt2.Text = timerzeit.Substring(0, timerzeit.Length -8);
                    await Task.Delay(250);
                    
                }
                
            }
            else
            {
                SBtnZustand = false;
                timer.Stop();
                SpielfeldX.IsEnabled = true;
                ZufallBtn.IsEnabled = true;
                NeuBtn.IsEnabled = true;
                SchrittBtn.IsEnabled = true;
                StartBtn.Content = "Generationswechsel Fortsetzen"; // Setzt den Btn Text auf Starten
                return;
            }
        }


        private void GenerationsWechsel()
        {
            int[,] anzahlNachbarn = new int[breiteViereck, hoeheViereck];
            int[,] lebenzeichen = new int[breiteViereck, hoeheViereck];

            for (int i = 0; i < breiteViereck; i++)
            {
                for (int j = 0; j < hoeheViereck; j++)
                {
                    int zeichen = 0;
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
                    if (zellen[i, j].Fill == Brushes.Black)
                    { zeichen++; }


                    anzahlNachbarn[i, j] = nachbarn;

                    lebenzeichen[i, j] = zeichen;
                    Console.WriteLine("i: " + i + ", j: " + j + ", zeichen: " + zeichen);
                }
            }
            for (int i = 0; i < breiteViereck; i++)
            {
                for (int j = 0; j < hoeheViereck; j++)
                {
                    if(lebenzeichen[i, j] == 1)
                    {
                        lebend++;
                        tod--;
                    }
                    else if (lebenzeichen[i, j] == 0)
                    {
                        lebend--;
                        tod++;
                    }

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
            timer.Reset();
            TimerTxt2.Text = "---";
            StartBtn.Content = "Generationswechsel Starten";
        }

        //Erstellt zufällig belebte/tote Zellen
        private void ButtonZufall_Click(object sender, RoutedEventArgs e)
        {
            Spielfeld.Children.OfType<Rectangle>().ToList().ForEach(x => x.Fill = brushes[rand.Next(brushes.Length)]);
            timer.Reset();
            TimerTxt2.Text = "---";
            StartBtn.Content = "Generationswechsel Starten";
        }

        /// <summary>
        /// Schrittweiser Generationswechsel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSchritt_Click(object sender, RoutedEventArgs e)
        {
            GenerationsWechsel();
        }

        /// <summary>
        /// Öffnet eine Messagebox mit Anwendungshilfe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ButtonHilfe_Click(object sender, RoutedEventArgs eventArgs)
        {
            MessageBox.Show("Willkommen zum Game of Life.\r\n\r\nEin Spielfeld wird in Zeilen und Spalten unterteilt. Jedes Gitterquadrat ist eine Zelle, welche einen von zwei Zuständen besitzen kann: tot (weiß) oder lebendig (schwarz).\r\n\r\nZunächst wird eine Anfangsgeneration von lebenden Zellen auf dem Spielfeld platziert. (Maustaste betätigen)\r\n\r\nDer „Zufall“ -Button ermöglich eine Generation von zufällig belebten oder toten Zellen.\r\n\r\nMit einem Mausklick auf die „Starten“-Taste kann nun das Spiel beginnen.\r\n\r\nJede lebende oder tote Zelle hat auf diesem Spielfeld genau acht Nachbarzellen, die berücksichtigt werden. Die nächste Generation ergibt sich durch die Befolgung einfacher Regeln:\r\n\r\n•Eine tote Zelle mit genau drei lebenden Nachbarn wird in der Folgegeneration neu geboren.\r\n\r\n•Lebende Zellen mit weniger als zwei lebenden Nachbarn sterben in der Folgegeneration an Einsamkeit.\r\n\r\n•Lebende Zellen mit mehr als drei lebenden Nachbarn sterben in der Folgegeneration an Überbevölkerung");
        }

    }
}
