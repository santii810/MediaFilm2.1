using MediaFilm2._1.Res;
using MediaFilm2._1.Vista;
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

namespace MediaFilm2._1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists(Recursos.configURL))
            {
            //    FrameHerramientaInicio.Source = new Uri(Recursos.InicioRapidoPageURL, UriKind.Relative);
            }
            frameConfiguracion.Source = new Uri(Recursos.ConfigPageURL, UriKind.Relative);
            frameDescargas.Source = new Uri(Recursos.DescargasPageURL, UriKind.Relative);
            frameGestionarDatos.Source = new Uri(Recursos.GestionarDatosPageURL, UriKind.Relative);
            frameMantenimiento.Source = new Uri(Recursos.MantenimientoPageURL, UriKind.Relative);
            frameOrdenar.Source = new Uri(Recursos.OrdenarPageURL, UriKind.Relative);

        }

        private void ImageOrdenar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateUI.updateMainWindow(Codigos.MOSTRAR_ORDENAR_PAGE, this);
        }

        private void ImageGestionarDatos_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateUI.updateMainWindow(Codigos.MOSTRAR_GESTIONAR_DATOS_PAGE, this);

        }

        private void ImageMantenimiento_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateUI.updateMainWindow(Codigos.MOSTRAR_MANTENIMIENTO_PAGE, this);

        }

        private void ImageDescargas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateUI.updateMainWindow(Codigos.MOSTRAR_DESCARGAS_PAGE, this);

        }

        private void ImageConfiguracion_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateUI.updateMainWindow(Codigos.MOSTRAR_CONFIG_PAGE, this);

        }
    }
}
