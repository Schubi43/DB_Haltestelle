using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace DB_Haltestelle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Haltestelle[] haltestellen;
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == true)
            {
                string dateiname = ofd.FileName;
                string [] zeilen = File.ReadAllLines(dateiname);
                haltestellen = new Haltestelle[zeilen.Length - 1]; // eins weniger, weil Spaltentitel als oberste Zeile
                for (int i = 1; i < zeilen.Length; i++)
                {
                   string[] teile = zeilen[i].Split(';');
                    double länge = double.Parse(teile[5]); //System.Globalization.CultureInfo.InvariantCulture);
                    double breite = double.Parse(teile[6]); // System.Globalization.CultureInfo.InvariantCulture);
                    haltestellen[i - 1] = new Haltestelle(teile[3], länge, breite);
                }
                  
                double minLänge = haltestellen.Min(x => x.Länge);
                double maxLänge = haltestellen.Max(x => x.Länge);
                double minBreite = haltestellen.Min(x => x.Breite);
                double maxBreite = haltestellen.Max(x => x.Breite);

                for (int i = 0; i < haltestellen.Length; i++)
                {
                    Haltestelle h = haltestellen[i];
                    if (!h.IstHbf)
                    {

                    Ellipse elli = new Ellipse();
                    elli.Width = 5.0;
                    elli.Height = 5.0;
                    elli.Fill = Brushes.DarkBlue;
                    elli.ToolTip = h.Ort;
                    zeichenfläche.Children.Add(elli);
                    Canvas.SetLeft(elli, zeichenfläche.ActualWidth / (maxLänge - minLänge) * (h.Länge - minLänge));
                    Canvas.SetBottom(elli, zeichenfläche.ActualHeight / (maxBreite - minBreite) * (h.Breite - minBreite));
                    }
                }

                for (int i = 0; i < haltestellen.Length; i++)
                {
                    Haltestelle h = haltestellen[i];
                    if (h.IstHbf)
                    {

                    Ellipse elli = new Ellipse();
                    elli.Width = 5.0;
                    elli.Height = 5.0;
                    elli.Fill = Brushes.Red;
                    elli.ToolTip = h.Ort;
                    zeichenfläche.Children.Add(elli);
                    Canvas.SetLeft(elli, zeichenfläche.ActualWidth / (maxLänge - minLänge) * (h.Länge - minLänge));
                    Canvas.SetBottom(elli, zeichenfläche.ActualHeight / (maxBreite - minBreite) * (h.Breite - minBreite));
                    }
                }
            }
        }
    }

    class Haltestelle
    {
      string ort;
      double länge;
      double breite;

        public Haltestelle(string ort, double länge, double breite)
        {
            this.ort = ort;
            this.länge = länge;
            this.breite = breite;

        }

        public double GetLänge()
        {
            return länge;
        }

        public double Länge
        {
            get { return länge;  }
        }

        public double Breite
        {
            get { return breite; }
        }

        public string Ort
        {
            get { return ort; }
        }
        public bool IstHbf
        {
            get { return ort.EndsWith("Hbf"); }
        }
    }
}
