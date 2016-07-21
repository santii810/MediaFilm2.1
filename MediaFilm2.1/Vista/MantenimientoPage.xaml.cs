using MediaFilm2._1.Controlador;
using MediaFilm2._1.Res;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para MantenimientoPage.xaml
    /// </summary>
    public partial class MantenimientoPage : Page
    {
        public MantenimientoPage()
        {
            InitializeComponent();
        }

        private void ButtonVerContinuidad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartMantenimiento_LeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MantenimientoResponse mantenimientoResponse = GestorVideos.realizarMantenimiento();



            #region Continuidad
            labelResultadoContinuidad.Content = mantenimientoResponse.ErroresContinuidad.Count + " errores.";
            if (mantenimientoResponse.ErroresContinuidad.Count == 0)
                circuloContinuidad.Source = CrearVistas.getPunto(Codigos.PUNTO_VERDE);
            else if (mantenimientoResponse.ErroresContinuidad.Count > 5)
                circuloContinuidad.Source = CrearVistas.getPunto(Codigos.PUNTO_ROJO);
            else
                circuloContinuidad.Source = CrearVistas.getPunto(Codigos.PUNTO_AMARILLO);
            #endregion

        }
    }
}
