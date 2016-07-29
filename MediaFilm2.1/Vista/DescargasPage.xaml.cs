using MediaFilm2._1.Controlador;
using MediaFilm2._1.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                


                GestorDescargasDivXTotal gestorDescargas = new GestorDescargasDivXTotal();

                await gestorDescargas.ParsearListaSeries();


                HashSet<Serie> seriesDefinitivo = new HashSet<Serie>();
                foreach (Serie item in MainWindow.SeriesXML.obtenerSeries())
                {
                    seriesDefinitivo.Add(item);
                }
                
                List<Serie> seriesLocales = MainWindow.SeriesXML.obtenerSeries();


                foreach (Serie serieDescargada in gestorDescargas.seriesDivX)
                {
                    Console.WriteLine(serieDescargada.tituloDivXTotal);
                    foreach (Serie serieLocal in seriesLocales)
                    {

                        if (!comprobarSiExiste(serieDescargada.tituloDivXTotal, serieLocal))
                        {
                            serieDescargada.estado = 4;
                            serieDescargada.tituloLocal = serieDescargada.tituloDivXTotal;
                            seriesDefinitivo.Add(serieDescargada);

                            MainWindow.SeriesXML.insertarSerie(serieDescargada);
                        }
                    }
                }








                enEjecucion = false;
                this.Cursor = Cursors.Arrow;



            }
        }


        private bool comprobarSiExiste(string tituloDivXTotal, Serie serieLocal)
        {
            if (tituloDivXTotal.ToUpper().Contains(serieLocal.tituloDivXTotal.ToUpper()) || tituloDivXTotal.ToUpper().Contains(serieLocal.tituloLocal.ToUpper()))
                return true;
            
            return false;
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
