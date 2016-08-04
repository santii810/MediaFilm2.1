using MediaFilm2._1.Controlador;
using MediaFilm2._1.Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace MediaFilm2._1.Vista
{
    /// <summary>
    /// Lógica de interacción para DescargasPage.xaml
    /// </summary>
    public partial class DescargasPage : Page
    {


        private bool enEjecucion = false;


        public DescargasPage()
        {
            InitializeComponent();
        }

        private async void ImageIniciarDescarga_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!enEjecucion)
            {
                enEjecucion = true;
                this.Cursor = Cursors.Wait;

                MainWindow.updateSeries();


                //obtengo todas las series de divx
                /*
                await gestorDescargas.ParsearListaSeries();
                */

                this.panelListaSeriesDescarga.Children.Clear();
                MainWindow.updateSeries();
                foreach (Serie serie in MainWindow.series)
                {
                    if ((serie.estado == 1 || serie.estado == 2) && serie.href_divX != null)
                    {
                        await MainWindow.gestorDescargas.ParsingSerie(serie);

                        compararCapitulos(serie);

                        if (serie.capitulos.Count > 0)
                        {
                            panelListaSeriesDescarga.Children.Add(CrearVistas.panelDescargarSerie(serie, this));
                        }
                    }
                }

                enEjecucion = false;
                this.Cursor = Cursors.Arrow;
            }
        }

        private void compararCapitulos(Serie serie)
        {

            HashSet<Capitulo> tmpCapitulos = new HashSet<Capitulo>();
            //Rellena el capitulo y la temporada de cada capitulo de la serie
            foreach (Capitulo cap in serie.capitulos)
            {


                foreach (string splitTitulo in cap.titulo.Split(' '))
                {
                    string exp = @"^\d{1,2}x\d{1,3}$";
                    Regex rgx = new Regex(exp);
                    if (rgx.IsMatch(splitTitulo))
                    {
                        string[] tempCap = splitTitulo.Split('x');
                        cap.numTemporada = Convert.ToInt32(tempCap[0]);
                        cap.numCapitulo = Convert.ToInt32(tempCap[1]);
                    }
                }
                string rutaCapitulo;
                if (cap.numCapitulo < 10)
                    rutaCapitulo = "/" + serie.tituloLocal + "/Temporada" + cap.numTemporada + "/" + serie.tituloLocal + " " + cap.numTemporada + "x0" + cap.numCapitulo + ".mkv";
                else
                    rutaCapitulo = "/" + serie.tituloLocal + "/Temporada" + cap.numTemporada + "/" + serie.tituloLocal + " " + cap.numTemporada + "x" + cap.numCapitulo + ".mkv";

                if (!File.Exists(MainWindow.config.directorioSeries + rutaCapitulo) && serie.temporadaActual <= cap.numTemporada)
                {
                    tmpCapitulos.Add(cap);
                }

            }

            serie.capitulos = tmpCapitulos;


        }

        private void ImageIniciarDescarga_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!enEjecucion)
                this.Cursor = Cursors.Hand;
        }
        private void ImageIniciarDescarga_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!enEjecucion)
                this.Cursor = Cursors.Arrow;
        }
    }
}
