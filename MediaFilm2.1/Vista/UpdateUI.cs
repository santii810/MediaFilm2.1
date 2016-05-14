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
            switch (cod)
            {
                case Codigos.MOSTRAR_ORDENAR_PAGE:
                    colapsarTodo(xaml);
                    xaml.frameOrdenar.Visibility = Visibility.Visible;
                    break;
                case Codigos.MOSTRAR_GESTIONAR_DATOS_PAGE:
                    colapsarTodo(xaml);
                    xaml.frameGestionarDatos.Visibility = Visibility.Visible;
                    break;
                case Codigos.MOSTRAR_MANTENIMIENTO_PAGE:
                    colapsarTodo(xaml);
                    xaml.frameMantenimiento.Visibility = Visibility.Visible;
                    break;
                case Codigos.MOSTRAR_DESCARGAS_PAGE:
                    colapsarTodo(xaml);
                    xaml.frameDescargas.Visibility = Visibility.Visible;
                    break;
                case Codigos.MOSTRAR_CONFIG_PAGE:
                    colapsarTodo(xaml);
                    xaml.frameConfiguracion.Visibility = Visibility.Visible;
                    break;
            }
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
