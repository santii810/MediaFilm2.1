using MediaFilm2._1.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediaFilm2._1.Vista
{
    class UpdateUI
    {
        internal static void updateMainWindow(int cod, MainWindow xaml)
        {
                    colapsarTodo(xaml);
            switch (cod)
            {
                case Codigos.MOSTRAR_ORDENAR_PAGE:
                    xaml.frameOrdenar.Visibility = Visibility.Visible;
                    break;
                case Codigos.MOSTRAR_GESTIONAR_DATOS_PAGE:
                    xaml.frameGestionarDatos.Visibility = Visibility.Visible;
                    break;
                case Codigos.MOSTRAR_MANTENIMIENTO_PAGE:
                    xaml.frameMantenimiento.Visibility = Visibility.Visible;
                    break;
                case Codigos.MOSTRAR_DESCARGAS_PAGE:
                    xaml.frameDescargas.Visibility = Visibility.Visible;
                    break;
                case Codigos.MOSTRAR_CONFIG_PAGE:
                    xaml.frameConfiguracion.Visibility = Visibility.Visible;
                    break;
            }
        }

        internal static void updateOrdenarPage(int cod, OrdenarPage xaml)
        {
                    colapsarTodo(xaml);
            switch (cod)
            {
                case Codigos.RESULTADO_RECOGER_VIDEOS:
                    xaml.PanelResultadoRecogerVideos.Visibility = Visibility.Visible;
                    xaml.PanelTiemposRecogerVideos.Visibility = Visibility.Visible;
                    xaml.labelDirectoriosBorrados.Visibility = Visibility.Visible;
                    xaml.labelTituloDirectoriosBorrados.Visibility = Visibility.Visible;
                    break;
            }
        }

        private static void colapsarTodo(OrdenarPage xaml)
        {
            xaml.PanelResultadoRecogerVideos.Visibility = Visibility.Collapsed;
            xaml.PanelTiemposRecogerVideos.Visibility = Visibility.Collapsed;
            xaml.labelDirectoriosBorrados.Visibility = Visibility.Collapsed;
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
