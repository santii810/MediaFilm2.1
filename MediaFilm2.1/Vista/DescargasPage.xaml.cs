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


                //obtengo todas las series de divx
                GestorDescargasDivXTotal gestorDescargas = new GestorDescargasDivXTotal();
                /*
                await gestorDescargas.ParsearListaSeries();
                */


                foreach (Serie serie in MainWindow.series)
                {
                    if ((serie.estado == 1 || serie.estado == 2) && serie.href_divX != null)
                    {
                        await gestorDescargas.ParsingSerie(serie);
                    }
                }


                
                enEjecucion = false;
                this.Cursor = Cursors.Arrow;
            }
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
