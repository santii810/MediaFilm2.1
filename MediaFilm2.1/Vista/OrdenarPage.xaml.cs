using MediaFilm2._1.Modelo;
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
        Configuracion config;
        XMLLogger IOLogger;
        XMLLogger ErrorLogger;
        XMLTiempos TiemposXML;

        public OrdenarPage()
        {
            InitializeComponent();
            config = (Configuracion)new XMLConfiguracion().leerXML();
            IOLogger = new XMLLogger(config.ficheroIOLog);
            ErrorLogger = new XMLLogger(config.ficheroErrorLog);
            TiemposXML = new XMLTiempos(config.ficheroTiempos);

        }

        private void ImageRecogerVideos_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateUI.updateOrdenarPage(Codigos.RESULTADO_RECOGER_VIDEOS, this);
            Stopwatch tiempo = Stopwatch.StartNew();
            recorrerTorrent();

            //Borra directorios y define el label con la cantidad
            labelDirectoriosBorrados.Content = borrarDirectoriosVacios(config.directorioTorrent);
            //Crea el directorio borrado
            Directory.CreateDirectory(config.directorioTorrent);
            //Define el label con la cantidad de videos borrados
            LabelCantidadVideosMovidos.Content = PanelResultadoVideosMovidos.Children.Count;
            //Define el label con la cantidad de errores
            LabelCantidadErroresBorrando.Content = PanelResultadoErroresBorrando.Children.Count;
            //Define el label con la cantidad de ficheros borrados
            LabelCantidadFicherosBorrados.Content = PanelResultadoFicherosBorrados.Children.Count;

            //Define label y color del tiempo transcurrido
            int tiempoTranscurrido = (int)tiempo.ElapsedMilliseconds;
            TiemposXML.insertar(Recursos.TIEMPO_RECORRER_TORRENT, tiempoTranscurrido);
            labelTiempoEjecucion.Content = tiempoTranscurrido + " ms";
            labelTiempoEjecucion.Foreground = obtenerColorLabel(tiempoTranscurrido, TiemposXML.obtenerMedia(Recursos.TIEMPO_RECORRER_TORRENT));

        }

        private Brush obtenerColorLabel(int tiempoTranscurrido, object v)
        {
            int media = Convert.ToInt32((double)v);

            //tiempo 50% mayor a la media
            if (tiempoTranscurrido > (media * 1.5))
                return new SolidColorBrush(Colors.Red);
            //tiempo 20% mayor a la media
            else if (tiempoTranscurrido > (media * 1.2))
                return new SolidColorBrush(Colors.Yellow);
            else
                return new SolidColorBrush(Colors.Green);
        }

        private void recorrerTorrent()
        {
            DirectoryInfo dir = new DirectoryInfo(config.directorioTorrent);
            if (dir.Exists)
            {
                foreach (FileInfo fichero in listarFicheros(dir.GetFileSystemInfos()))
                {
                    manejarFichero(fichero);
                }
            }
            else
            {
                MessageBox.Show(Mensajes.directorioNoEncontrado(dir.Name));
            }


        }

        /// <summary>
        /// Funcion recursiva que retorna todos los ficheros del directorio parametrizado
        /// </summary>
        /// <param name="filesInfo">Directorio raiz</param>
        /// <returns> Lista de ficheros y subficheros</returns>
        static private List<FileInfo> listarFicheros(FileSystemInfo[] filesInfo)
        {
            List<FileInfo> retorno = new List<FileInfo>();

            foreach (FileSystemInfo item in filesInfo)
            {
                if (item is DirectoryInfo)
                {
                    DirectoryInfo dInfo = (DirectoryInfo)item;
                    retorno.AddRange(listarFicheros(dInfo.GetFileSystemInfos()));
                }
                else if (item is FileInfo)
                {
                    retorno.Add((FileInfo)item);
                }
            }
            return retorno;
        }

        /// <summary>
        /// Maneja el fichero parametrizado, borrandolo o moviendolo segun convenga
        /// </summary>
        /// <param name="fichero">Fichero a gestionar</param>
        private void manejarFichero(FileInfo fichero)
        {
            if (fichero.Extension == ".txt" || fichero.Extension == ".!ut" || fichero.Extension == ".url" || fichero.Extension == ".jpg" || fichero.Extension == ".wmv")
            {
                try
                {
                    File.SetAttributes(fichero.FullName, FileAttributes.Normal);
                    fichero.Delete();
                    PanelResultadoFicherosBorrados.Children.Add(CrearVistas.LabelLista((fichero.Name)));
                    IOLogger.insertar(new LogIO(Recursos.LOG_TIPO_BORRADO_OK, Mensajes.ficheroBorradoOk(fichero.Name), fichero));
                }
                catch (Exception ex)
                {
                    PanelResultadoErroresBorrando.Children.Add(CrearVistas.LabelLista(Mensajes.errorBorrandoFichero(fichero.Name)));
                    ErrorLogger.insertar(new LogIO(Recursos.LOG_TIPO_ERROR_BORRANDO, Mensajes.errorBorrandoFichero(fichero.Name, ex), fichero));
                }
            }
            else if (fichero.Extension == ".mp4" || fichero.Extension == ".mkv" || fichero.Extension == ".avi")
            {
                string pathDestino = config.directorioTrabajo + @"\" + fichero.Name;
                try
                {
                    File.SetAttributes(fichero.FullName, FileAttributes.Normal);
                    fichero.MoveTo(pathDestino);
                    PanelResultadoVideosMovidos.Children.Add(CrearVistas.LabelLista(fichero.Name));
                    IOLogger.insertar(new LogIO(Recursos.LOG_TIPO_MOVIDO_OK, Mensajes.FicheroMovidoOK(fichero.Name), fichero));
                }
                catch (Exception ex)
                {
                    ErrorLogger.insertar(new LogIO(Recursos.LOG_TIPO_ERROR_MOVIENDO, Mensajes.ErrorMoviendoFichero(fichero.Name, ex), fichero));
                    PanelResultadoErroresBorrando.Children.Add(CrearVistas.LabelLista(Mensajes.ErrorMoviendoFichero(fichero.Name)));
                }
            }
            else
            {
                MessageBox.Show(Mensajes.ExtensionNoRegistrada(fichero.Name));
            }
        }

        /// <summary>
        /// Funcion recursiva que borra directorios y subdirectorios vacios.
        /// </summary>
        /// <param name="dir">Directorio vacio</param>
        /// <returns>Numero de directirios borados</returns>
        static private int borrarDirectoriosVacios(string dir)
        {
            int retorno = 0;
            try
            {
                foreach (var d in Directory.EnumerateDirectories(dir))
                {
                    retorno += borrarDirectoriosVacios(d);
                }
                var entries = Directory.EnumerateFileSystemEntries(dir);

                if (!entries.Any())
                {
                    try
                    {
                        Directory.Delete(dir);
                        retorno++;
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (DirectoryNotFoundException) { }
                }
            }
            catch (UnauthorizedAccessException) { }
            return retorno;
        }

    }
}
