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

        public OrdenarPage()
        {
            InitializeComponent();
            config = (Configuracion)new XMLConfiguracion().leerXML();
            IOLogger = new XMLLogger(config.ficheroIOLog);
            ErrorLogger = new XMLLogger(config.ficheroErrorLog);

        }

        private void ImageRecogerVideos_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Stopwatch tiempo = Stopwatch.StartNew();

            DirectoryInfo dir = new DirectoryInfo(config.directorioTorrent);
            if (dir.Exists)
            {
                foreach (FileInfo fichero in listarFicheros(dir.GetFileSystemInfos()))
                {
                    if (fichero.Extension == ".txt" || fichero.Extension == ".!ut" || fichero.Extension == ".url" || fichero.Extension == ".jpg")
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
            }
            else
            {
                MessageBox.Show(Mensajes.directorioNoEncontrado(dir.Name));
            }
        }


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
    }
}
