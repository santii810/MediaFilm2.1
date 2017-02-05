using MediaFilm2._1.Controlador;
using MediaFilm2._1.Modelo;
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

        private const int CANTIDAD_ERROR_GRAVE = 5;


        MantenimientoResponse mantenimientoResponse;

        public MantenimientoPage()
        {
            InitializeComponent();
            UpdateUI.updateManteminientoPage(Codigos.ESTADO_INICIAL, this);
        }

        private void ButtonVerContinuidad_Click(object sender, RoutedEventArgs e)
        {
            UpdateUI.updateManteminientoPage(Codigos.MANTENIMIENTO_MOSTRAR_RESULTADO, this);
            this.panelResultadoContinuidad.Children.Clear();
            foreach (string error in mantenimientoResponse.ErroresContinuidad)
            {
                this.panelListaResultadoMantenimiento.Children.Add(CrearVistas.LabelLista(error));
            }
        }

        private void StartMantenimiento_LeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mantenimientoResponse = GestorVideos.realizarMantenimiento();

            #region Continuidad
            labelResultadoContinuidad.Content = mantenimientoResponse.ErroresContinuidad.Count + " errores.";
            if (mantenimientoResponse.ErroresContinuidad.Count == 0)
                circuloContinuidad.Source = CrearVistas.getPunto(Codigos.PUNTO_VERDE);
            else if (mantenimientoResponse.ErroresContinuidad.Count > CANTIDAD_ERROR_GRAVE)
                circuloContinuidad.Source = CrearVistas.getPunto(Codigos.PUNTO_ROJO);
            else
                circuloContinuidad.Source = CrearVistas.getPunto(Codigos.PUNTO_AMARILLO);
            #endregion


            UpdateUI.updateManteminientoPage(Codigos.MANTENIMIENTO_ANALISIS_EJECUTADO, this);
        }
    }
}
