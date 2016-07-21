using MediaFilm2._1.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MediaFilm2._1.Vista
{
    class UpdateUI
    {
        internal static void updateMainWindow(int cod, MainWindow xaml)
        {
            colapsarTodo(xaml);
            switch (cod)
            {
                case Codigos.ESTADO_INICIAL:
                    break;
                case Codigos.MAIN_MOSTRAR_ORDENAR_PAGE:
                    xaml.frameOrdenar.Visibility = Visibility.Visible;
                    break;
                case Codigos.MAIN_MOSTRAR_GESTIONAR_DATOS_PAGE:
                    xaml.frameGestionarDatos.Visibility = Visibility.Visible;
                    break;
                case Codigos.MAIN_MOSTRAR_MANTENIMIENTO_PAGE:
                    xaml.frameMantenimiento.Visibility = Visibility.Visible;
                    break;
                case Codigos.MAIN_MOSTRAR_DESCARGAS_PAGE:
                    xaml.frameDescargas.Visibility = Visibility.Visible;
                    break;
                case Codigos.MAIN_MOSTRAR_CONFIG_PAGE:
                    xaml.frameConfiguracion.Visibility = Visibility.Visible;
                    break;
            }
        }

        internal static void updateOrdenarPage(int cod, OrdenarPage xaml)
        {
            colapsarTodo(xaml);
            switch (cod)
            {
                case Codigos.ESTADO_INICIAL:
                    break;
                case Codigos.ORDENAR_RESULTADO_RECOGER_VIDEOS:
                    xaml.PanelResultadoRecogerVideos.Visibility = Visibility.Visible;
                    xaml.PanelTiemposRecogerVideos.Visibility = Visibility.Visible;
                    xaml.labelDirectoriosBorrados.Visibility = Visibility.Visible;
                    xaml.labelTituloDirectoriosBorrados.Visibility = Visibility.Visible;
                    break;
                case Codigos.ORDENAR_RESULTADO_RENOMBRAR_VIDEOS:
                    xaml.PanelResultadoRenombrarVideos.Visibility = Visibility.Visible;
                    xaml.PanelTiemposRecogerVideos.Visibility = Visibility.Visible;
                    break;
            }
        }

  
        internal static void updateGestionarDatos(int cod, GestionarDatosPage xaml)
        {
            colapsarTodo(xaml);
            switch (cod)
            {
                case Codigos.GESTIONAR_DATOS_ADD_SERIE:
                    xaml.panelAddSerie.Visibility = Visibility.Visible;
                    break;
                case Codigos.GESTIONAR_DATOS_ADD_SERIE_OK:
                    xaml.panelAddSerie.Visibility = Visibility.Visible;
                    xaml.textBoxCapitulosTemporada.Text = "";
                    xaml.textBoxNumeroTemporadas.Text = "";
                    xaml.textBoxTitulo.Text = "";
                    break;
                case Codigos.GESTIONAR_DATOS_ADD_PATRON:
                    xaml.panelAddPatron.Visibility = Visibility.Visible;
                    xaml.panelFicherosARenombrar.Visibility = Visibility.Visible;
                    break;
                case Codigos.GESTIONAR_DATOS_ADD_PATRON_SERIE_SELECCIONADA:
                    xaml.panelAddPatron.Visibility = Visibility.Visible;
                    xaml.panelFicherosARenombrar.Visibility = Visibility.Visible;
                    xaml.panelPatronesActuales.Visibility = Visibility.Visible;
                    xaml.panelInsertarPatron.Visibility = Visibility.Visible;
                    break;
                case Codigos.GESTIONAR_DATOS_ADD_PATRON_OK:
                    xaml.textBoxNuevoPatron.Text = "";
                    //Reseteo el panel a como si acabaramos de pulsar sobre el
                    updateGestionarDatos(Codigos.GESTIONAR_DATOS_ADD_PATRON_SERIE_SELECCIONADA, xaml);
                    break;
                case Codigos.GESTIONAR_DATOS_IO_SERIE:
                    xaml.panelIOSerie.Visibility = Visibility.Visible;
                    break;
                case Codigos.GESTIONAR_DATOS_TEMPORADAS:
                    xaml.panelTemporadas.Visibility = Visibility.Visible;
                    break;

            }
        }

        internal static void updateManteminientoPage(int cod, MantenimientoPage xaml)
        {
            colapsarTodo(xaml);
            switch (cod)
            {
                case Codigos.MANTENIMIENTO_ANALISIS_EJECUTADO:
                    xaml.panelResultadosMantenimiento.Visibility = Visibility.Visible;
                    break;
                

            }
        }

        private static void colapsarTodo(MantenimientoPage xaml)
        {
            xaml.panelDetallesMantenimiento.Visibility = Visibility.Collapsed;
            xaml.panelResultadosMantenimiento.Visibility = Visibility.Collapsed;

        }

        private static void colapsarTodo(GestionarDatosPage xaml)
        {
            xaml.panelAddSerie.Visibility = Visibility.Collapsed;
            xaml.panelAddPatron.Visibility = Visibility.Collapsed;
            xaml.panelInsertarPatron.Visibility = Visibility.Collapsed;
            xaml.panelPatronesActuales.Visibility = Visibility.Collapsed;
            xaml.panelFicherosARenombrar.Visibility = Visibility.Collapsed;
            xaml.panelIOSerie.Visibility = Visibility.Collapsed;
            xaml.panelTemporadas.Visibility = Visibility.Collapsed;



        }

        private static void colapsarTodo(OrdenarPage xaml)
        {
            //ambos
            xaml.PanelTiemposRecogerVideos.Visibility = Visibility.Collapsed;

            //recoger videos
            xaml.PanelResultadoRecogerVideos.Visibility = Visibility.Collapsed;
            xaml.labelDirectoriosBorrados.Visibility = Visibility.Collapsed;

            //renombrar videos
            xaml.PanelResultadoRenombrarVideos.Visibility = Visibility.Collapsed;
            xaml.labelTituloDirectoriosBorrados.Visibility = Visibility.Collapsed;
        }

        private static void colapsarTodo(MainWindow xaml)
        {
            xaml.frameOrdenar.Visibility = Visibility.Collapsed;
            xaml.frameGestionarDatos.Visibility = Visibility.Collapsed;
            xaml.frameMantenimiento.Visibility = Visibility.Collapsed;
            xaml.frameDescargas.Visibility = Visibility.Collapsed;
            xaml.frameConfiguracion.Visibility = Visibility.Collapsed;
        }
    }
}
