using MediaFilm2._1.Modelo;
using MediaFilm2._1.Modelo.Logs;
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
        XMLSeries SeriesXML;
        List<Serie> series;

        private bool enEjecucion = false;

        public OrdenarPage()
        {
            InitializeComponent();
            UpdateUI.updateOrdenarPage(Codigos.ESTADO_INICIAL, this);

            config = (Configuracion)new XMLConfiguracion().leerXML();
            IOLogger = new XMLLogger(config.ficheroIOLog);
            ErrorLogger = new XMLLogger(config.ficheroErrorLog);
            TiemposXML = new XMLTiempos(config.ficheroTiempos);
            SeriesXML = new XMLSeries(config.ficheroSeries, config.ficheroSerieLogger, config.ficheroPatrones, config.ficheroPatronLog);

        }

        private void ImageRecogerVideos_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (!enEjecucion)
            {
                enEjecucion = true;
                this.Cursor = Cursors.Wait;

                UpdateUI.updateOrdenarPage(Codigos.RESULTADO_RECOGER_VIDEOS, this);

                //Limpieza de antiguos resultados
                PanelResultadoErroresBorrando.Children.Clear();
                PanelResultadoFicherosBorrados.Children.Clear();
                PanelResultadoVideosMovidos.Children.Clear();

                Stopwatch tiempo = Stopwatch.StartNew();
                recorrerTorrent();

                //Borra directorios y define el label con la cantidad
                int ficherosBorrados = borrarDirectoriosVacios(config.directorioTorrent);
                if (ficherosBorrados > 0) ficherosBorrados--;
                labelDirectoriosBorrados.Content = ficherosBorrados;
                int tiempoTranscurrido = (int)tiempo.ElapsedMilliseconds;

                //Define el contenido de los labels
                LabelCantidadVideosMovidos.Content = PanelResultadoVideosMovidos.Children.Count;
                LabelCantidadErroresBorrando.Content = PanelResultadoErroresBorrando.Children.Count;
                LabelCantidadFicherosBorrados.Content = PanelResultadoFicherosBorrados.Children.Count;

                //Define label y color del tiempo transcurrido
                if (tiempoTranscurrido > 100)
                    TiemposXML.insertar(Recursos.TIEMPO_RECORRER_TORRENT, tiempoTranscurrido);
                labelTiempoEjecucion.Content = tiempoTranscurrido + " ms";
                int media;
                try
                {
                    media = Convert.ToInt32((double)TiemposXML.obtenerMedia(Recursos.TIEMPO_RECORRER_TORRENT));
                }
                catch (OverflowException)
                {
                    media = 0;
                }
                labelTiempoEjecucion.Foreground = obtenerColorLabel(tiempoTranscurrido, media);
                labelTiempoEjecucion.ToolTip = new ToolTip { Content = "Media: " + media };


                //Crea el directorio borrado
                Directory.CreateDirectory(config.directorioTorrent);

                enEjecucion = false;
                this.Cursor = Cursors.Arrow;
            }
        }

        #region recorrer videos
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
                    IOLogger.insertar(new LogIO(Recursos.LOG_TIPO_BORRADO_OK, Mensajes.FICHERO_BORRADO_OK, fichero));
                }
                catch (Exception ex)
                {
                    PanelResultadoErroresBorrando.Children.Add(CrearVistas.LabelLista(Mensajes.errorBorrandoFichero(fichero.Name)));
                    ErrorLogger.insertar(new LogIO(Recursos.LOG_TIPO_ERROR_BORRANDO, Mensajes.errorBorrandoFichero(ex), fichero));
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
                    IOLogger.insertar(new LogIO(Recursos.LOG_TIPO_MOVIDO_OK, Mensajes.FICHERO_MOVIDO_OK, fichero));
                }
                catch (Exception ex)
                {
                    ErrorLogger.insertar(new LogIO(Recursos.LOG_TIPO_ERROR_MOVIENDO, Mensajes.ErrorMoviendoFichero(ex), fichero));
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
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(dir);
            }
            return retorno;
        }
        #endregion

        private void ImageRenombrarVideos_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateUI.updateOrdenarPage(Codigos.RESULTADO_RENOMBRAR_VIDEOS, this);

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

                series = SeriesXML.leerXML();
                foreach (Serie itSerie in series)
                {
                    if (itSerie.estado == "A")
                    {
                        itSerie.leerPatrones(config.ficheroPatrones, config.ficheroPatronLog);
                        foreach (Patron itPatron in itSerie.patrones)
                        {
                            for (int temp = itSerie.temporadaActual; temp <= itSerie.numeroTemporadas; temp++)
                            {
                                for (int cap = 0; cap <= itSerie.capitulosPorTemporada; cap++)
                                {
                                    FileInfo fi;
                                    string dirSerie = config.directorioSeries + @"\" + itSerie.titulo + @"\Temporada" + temp + @"\";
                                    string[] strPatrones = new string[]
                                    {
                                    //patrones para capitulos<10  y extension == mkv
                                    itPatron.textoPatron + "*" + temp.ToString() + "0" + cap.ToString() + "*.mkv" ,
                                    itPatron.textoPatron + "*" + temp.ToString() + "x0" + cap.ToString() + "*.mkv" ,
                                    temp.ToString()+"x0"+cap.ToString()+"*"+itPatron.textoPatron+"*.mkv",

                                    //patrones para capitulos<10  y extension == avi
                                    itPatron.textoPatron + "*" + temp.ToString() + "0" + cap.ToString() + "*.avi" ,
                                    itPatron.textoPatron + "*" + temp.ToString() + "x0" + cap.ToString() + "*.avi" ,
                                    temp.ToString()+"x0"+cap.ToString()+"*"+itPatron.textoPatron+"*.avi",

                                    //patrones para capitulos>10  y extension == mkv
                                    itPatron.textoPatron + "*" + temp.ToString() + cap.ToString() + "*.mkv",
                                    itPatron.textoPatron + "*" + temp.ToString() + "x" + cap.ToString() + "*.mkv",
                                    temp.ToString()+"x"+cap.ToString()+"*"+itPatron.textoPatron+"*.mkv",

                                    //patrones para capitulos>10  y extension == avi
                                    itPatron.textoPatron + "*" + temp.ToString() + cap.ToString() + "*.avi",
                                    itPatron.textoPatron + "*" + temp.ToString() + "x" + cap.ToString() + "*.avi",
                                    temp.ToString()+"x"+cap.ToString()+"*"+itPatron.textoPatron+"*.avi",

                                    //patrones especiales
                                    itPatron.textoPatron+"*"+temp + "(" + cap.ToString()+ ")" + ".avi",
                                    itPatron.textoPatron+"*"+temp + "(" + cap.ToString()+ ")" + ".mkv"
                                };

                                    /* 
                                    Los ultimos 2 patrones no dependen de si el capitulo es mayor o menor que 10
                                    luego se ejecuta 1 vez cada patron cuando i pasa por las posiciones 6 y 7
                                    como los patrones estan en la posicion 12 y 13 respectivamente se suma 6 a la i

                                    Los patrones de capitulos mayores que 10 estan en las posiciones del 7 al 12
                                    asi que si el numero del capitulo es >=  10 se coge la posicion 6+i
                                    */
                                    for (int i = 0; i < 8; i++)
                                    {
                                        if (cap >= 10 || i > 5)
                                        {
                                            fi = obtenerCoincidenciaBusqueda(strPatrones[i + 6]);
                                        }
                                        else
                                        {
                                            fi = obtenerCoincidenciaBusqueda(strPatrones[i]);
                                        }
                                        if (fi != null)
                                        {
                                            string nombreOriginal = fi.Name;
                                            //Crea todos los directorios y subdirectorios en la ruta de acceso especificada, a menos que ya existan.
                                            Directory.CreateDirectory(dirSerie);
                                            try
                                            {
                                                if (cap < 10)
                                                {
                                                    fi.MoveTo(dirSerie + @"\" + itSerie.titulo + " " + temp + "x0" + cap + fi.Extension);
                                                }
                                                else
                                                {
                                                    fi.MoveTo(dirSerie + @"\" + itSerie.titulo + " " + temp + "x" + cap + fi.Extension);
                                                }
                                                IOLogger.insertar(new LogRenombrado(Recursos.LOG_TIPO_RENOMBRADO_OK, Mensajes.FICHERO_RENOMBRADO_OK, fi, nombreOriginal));
                                                PanelResultadoVideosRenombrados.Children.Add(CrearVistas.LabelLista(nombreOriginal + " => " + fi.Name));
                                            }
                                            catch (IOException ex)
                                            {
                                                if (ex.Message == "No se puede crear un archivo que ya existe.\r\n")
                                                    try
                                                    {
                                                        File.SetAttributes(fi.FullName, FileAttributes.Normal);
                                                        fi.Delete();
                                                        IOLogger.insertar(new LogIO(Recursos.LOG_TIPO_BORRADO_OK, Mensajes.FICHERO_BORRADO_OK, fi));
                                                        PanelResultadoErroresRenombrando.Children.Add(CrearVistas.LabelLista("(Borrado) " + nombreOriginal));

                                                    }
                                                    catch (Exception)
                                                    {
                                                        ErrorLogger.insertar(new LogIO(Recursos.LOG_TIPO_ERROR_BORRANDO, Mensajes.errorBorrandoFichero(ex), fi));
                                                        PanelResultadoErroresRenombrando.Children.Add(CrearVistas.LabelLista("No se pudo borrar " + nombreOriginal));
                                                    }

                                            }
                                            catch (Exception ex)
                                            {
                                                PanelResultadoErroresRenombrando.Children.Add(CrearVistas.LabelLista("Error renombrando: " + nombreOriginal));
                                                ErrorLogger.insertar(new LogIO(Recursos.LOG_TIPO_ERROR_RENOMBRADO, Mensajes.errorRenombrandoFichero(ex), fi));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                enEjecucion = false;
                this.Cursor = Cursors.Arrow;
            }
        }

        private FileInfo obtenerCoincidenciaBusqueda(string pat)
        {
            DirectoryInfo iomegaInfo = new DirectoryInfo(config.directorioTrabajo);
            FileSystemInfo[] fsi;
            fsi = iomegaInfo.GetFileSystemInfos(pat);
            if (fsi.Length == 1 && fsi[0] is FileInfo)
            {
                return (FileInfo)fsi[0];
            }
            return null;
        }

        private void ImageRecogerVideos_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!enEjecucion)
                this.Cursor = Cursors.Hand;
        }

        private void ImageRecogerVideos_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!enEjecucion)
                this.Cursor = Cursors.Arrow;
        }



        private void ImageRenombrarVideos_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!enEjecucion)
                this.Cursor = Cursors.Hand;
        }

        private void ImageRenombrarVideos_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!enEjecucion)
                this.Cursor = Cursors.Arrow;
        }
    }
}
