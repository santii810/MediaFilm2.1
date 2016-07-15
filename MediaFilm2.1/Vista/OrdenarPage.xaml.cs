using MediaFilm2._1.Controlador;
using MediaFilm2._1.Modelo;
using MediaFilm2._1.Modelo.Logs;
using MediaFilm2._1.Modelo.Request;
using MediaFilm2._1.Modelo.XML;
using MediaFilm2._1.Res;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MediaFilm2._1.Vista
{
    /// <summary>
    /// Lógica de interacción para OrdenarPage.xaml
    /// </summary>
    public partial class OrdenarPage : Page
    {
        private bool enEjecucion = false;

        public OrdenarPage()
        {
            InitializeComponent();
            UpdateUI.updateOrdenarPage(Codigos.ESTADO_INICIAL, this);



        }


        private Brush obtenerColorLabel(int tiempoTranscurrido, int media)
        {
            //tiempo 50% mayor a la media
            if (tiempoTranscurrido > (media * 1.5))
                return new SolidColorBrush(Colors.Red);
            //tiempo 20% mayor a la media
            else if (tiempoTranscurrido > (media * 1.2))
                return new SolidColorBrush(Colors.Orange);
            else
                return new SolidColorBrush(Colors.Green);
        }

        private void ImageRecogerVideos_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (!enEjecucion)
            {
                enEjecucion = true;
                this.Cursor = Cursors.Wait;

                UpdateUI.updateOrdenarPage(Codigos.ORDENAR_RESULTADO_RECOGER_VIDEOS, this);

                //Limpieza de antiguos resultados
                PanelResultadoErroresBorrando.Children.Clear();
                PanelResultadoFicherosBorrados.Children.Clear();
                PanelResultadoVideosMovidos.Children.Clear();


                RecorrerTorrentRequest recorrerTorrentRequest = GestorVideos.recorrerTorrent();

                
                foreach (string item in recorrerTorrentRequest.ficherosBorrados)
                {
                    PanelResultadoFicherosBorrados.Children.Add(CrearVistas.LabelLista((item)));
                }
                foreach (string item in recorrerTorrentRequest.erroresBorrando)
                {
                    PanelResultadoErroresBorrando.Children.Add(CrearVistas.LabelLista(item));

                }
                foreach (string item in recorrerTorrentRequest.videosMovidos)
                {
                    PanelResultadoVideosMovidos.Children.Add(CrearVistas.LabelLista(item));
                }

                //Borra directorios y define el label con la cantidad
                if (recorrerTorrentRequest.directoriosBorrados > 0) recorrerTorrentRequest.directoriosBorrados--;
                labelDirectoriosBorrados.Content = recorrerTorrentRequest.directoriosBorrados;

                //Define el contenido de los labels
                LabelCantidadVideosMovidos.Content = recorrerTorrentRequest.ficherosBorrados.Count;
                LabelCantidadErroresBorrando.Content = recorrerTorrentRequest.erroresBorrando.Count;
                LabelCantidadFicherosBorrados.Content = recorrerTorrentRequest.ficherosBorrados.Count;

                //Define label y color del tiempo transcurrido
                if (recorrerTorrentRequest.tiempoTranscurrido > 100)
                    MainWindow.EstadisticasXML.insertar(Recursos.TIEMPO_RECORRER_TORRENT, recorrerTorrentRequest.tiempoTranscurrido);
                labelTiempoEjecucion.Content = recorrerTorrentRequest.tiempoTranscurrido + " ms";
                int media;
                try
                {
                    media = Convert.ToInt32((double)MainWindow.EstadisticasXML.obtenerMedia(Recursos.TIEMPO_RECORRER_TORRENT));
                }
                catch (OverflowException)
                {
                    media = 0;
                }
                labelTiempoEjecucion.Foreground = obtenerColorLabel(recorrerTorrentRequest.tiempoTranscurrido, media);
                labelTiempoEjecucion.ToolTip = new ToolTip { Content = "Media: " + media };

                enEjecucion = false;
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Images_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!enEjecucion)
                this.Cursor = Cursors.Hand;
        }
        private void Images_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!enEjecucion)
                this.Cursor = Cursors.Arrow;
        }

        private void ImageRenombrarVideos_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateUI.updateOrdenarPage(Codigos.ORDENAR_RESULTADO_RENOMBRAR_VIDEOS, this);

            //Limpieza de antiguos resultados
            PanelResultadoErroresRenombrando.Children.Clear();
            PanelResultadoVideosRenombrados.Children.Clear();
            labelNumeroSeriesActivas.Content = "";
            labelNumeroPatronesEjecutados.Content = "";
            labelTiempoEjecucion.Content = "";


            if (!enEjecucion)
            {
                enEjecucion = true;
                this.Cursor = Cursors.Wait;


                RenombrarVideosRequest renombrarVideosRequest = GestorVideos.renombrarVideos();


                //Muestra resultados
                foreach (string[] item in renombrarVideosRequest.videosRenombrados)
                {
                    PanelResultadoVideosRenombrados.Children.Add(CrearVistas.LabelLista(item[0] + " => " + item[1]));
                }

                foreach (string item in renombrarVideosRequest.erroresRenombrando)
                {
                    PanelResultadoErroresRenombrando.Children.Add(CrearVistas.LabelLista(item));
                }



                //Define el contenido de los labels
                LabelCantidadVideosRenombrados.Content = renombrarVideosRequest.videosRenombrados.Count;
                LabelCantidadErroresRenombrando.Content = renombrarVideosRequest.erroresRenombrando.Count;





                //Guardo datos estadisticos
                if (renombrarVideosRequest.tiempoTranscurrido > 100)
                    MainWindow.EstadisticasXML.insertar(Recursos.TIEMPO_RENOMBRAR_VIDEOS, renombrarVideosRequest.tiempoTranscurrido);
                if (renombrarVideosRequest.seriesActivas > 0)
                    MainWindow.EstadisticasXML.insertar(Recursos.NUMERO_SERIES_ACTIVAS, renombrarVideosRequest.seriesActivas);
                if (renombrarVideosRequest.patronesEjecutados > 0)
                    MainWindow.EstadisticasXML.insertar(Recursos.NUMERO_PATRONES_EJECUTADOS, renombrarVideosRequest.patronesEjecutados);

                // Asignar content a los label
                labelTiempoEjecucion.Content = renombrarVideosRequest.tiempoTranscurrido + " ms";
                labelNumeroSeriesActivas.Content = renombrarVideosRequest.seriesActivas + "/" + renombrarVideosRequest.seriesTotales;
                labelNumeroPatronesEjecutados.Content = renombrarVideosRequest.patronesEjecutados;


                //definir colores y tooltips
                int media;
                try
                {
                    media = Convert.ToInt32((double)MainWindow.EstadisticasXML.obtenerMedia(Recursos.TIEMPO_RENOMBRAR_VIDEOS));
                }
                catch (OverflowException)
                {
                    media = 0;
                }
                labelTiempoEjecucion.Foreground = obtenerColorLabel(renombrarVideosRequest.tiempoTranscurrido, media);
                labelTiempoEjecucion.ToolTip = new ToolTip { Content = "Media: " + media };

                try
                {
                    media = Convert.ToInt32((double)MainWindow.EstadisticasXML.obtenerMedia(Recursos.NUMERO_SERIES_ACTIVAS));
                }
                catch (OverflowException)
                {
                    media = 0;
                }
                labelNumeroSeriesActivas.Foreground = obtenerColorLabel(renombrarVideosRequest.seriesActivas, media);
                labelNumeroSeriesActivas.ToolTip = new ToolTip { Content = "Media: " + media };

                try
                {
                    media = Convert.ToInt32((double)MainWindow.EstadisticasXML.obtenerMedia(Recursos.NUMERO_PATRONES_EJECUTADOS));
                }
                catch (OverflowException)
                {
                    media = 0;
                }
                labelNumeroPatronesEjecutados.Foreground = obtenerColorLabel(renombrarVideosRequest.patronesEjecutados, media);
                labelNumeroPatronesEjecutados.ToolTip = new ToolTip { Content = "Media: " + media };


                enEjecucion = false;
                this.Cursor = Cursors.Arrow;
            }
        }
       
    }
}
