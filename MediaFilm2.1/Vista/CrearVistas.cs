using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MediaFilm2._1.Modelo;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MediaFilm2._1.Res;

namespace MediaFilm2._1.Vista
{
    public class CrearVistas
    {
        internal static Label LabelLista(string v)
        {
            Label retorno = new Label();
            retorno.Style = (Style)Application.Current.Resources["LabelListas"];
            retorno.Content = v;
            return retorno;
        }


        internal static UIElement PanelSeleccionarSerie(Serie serie, GestionarDatosPage xaml)
        {
            StackPanel tmpPanel = new StackPanel();
            tmpPanel.Orientation = Orientation.Horizontal;
            tmpPanel.HorizontalAlignment = HorizontalAlignment.Center;


            Label tmpLabelTitulo = new Label();
            tmpLabelTitulo.Content = serie.titulo;
            tmpLabelTitulo.Style = (Style)Application.Current.Resources["LabelListas"];
            tmpLabelTitulo.Width = 250;
            tmpPanel.Children.Add(tmpLabelTitulo);


            Button tmpButton = new Button();
            tmpButton.Click += delegate
            {
                xaml.seleccionarSerie(serie);
            };
            tmpButton.Style = (Style)Application.Current.Resources["Button"];
            tmpButton.Content = "Seleccionar";
            tmpButton.FontSize = 11;
            tmpPanel.Children.Add(tmpButton);

            return tmpPanel;
        }

        internal static UIElement PanelEstadoSerie(Serie serie, GestionarDatosPage xaml)
        {
            StackPanel tmpPanel = new StackPanel();
            tmpPanel.Orientation = Orientation.Horizontal;
            tmpPanel.HorizontalAlignment = HorizontalAlignment.Center;


            Label tmpLabelTitulo = new Label();
            tmpLabelTitulo.Content = serie.titulo;
            tmpLabelTitulo.Style = (Style)Application.Current.Resources["LabelListas"];
            tmpLabelTitulo.Width = 250;
            tmpPanel.Children.Add(tmpLabelTitulo);


            Image circuloEstado = new Image();
            if (serie.estado == "A")
                circuloEstado.Source = getPunto(Codigos.PUNTO_VERDE);
            else if (serie.estado == "D")
                circuloEstado.Source = getPunto(Codigos.PUNTO_AMARILLO);
            else
            {
                circuloEstado.Source = getPunto(Codigos.PUNTO_ROJO);
            }
            circuloEstado.Width = 25;
            circuloEstado.Margin = new Thickness(0, 4, 10, 10);
            circuloEstado.MouseDown += delegate { xaml.serieSeleccionada = serie; };
            circuloEstado.MouseDown += new System.Windows.Input.MouseButtonEventHandler(xaml.circuloEstado_MouseUp);
            tmpPanel.Children.Add(circuloEstado);

         

            return tmpPanel;
        }



        private static ImageSource getPunto(int cod)
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            switch (cod)
            {
                case Codigos.PUNTO_VERDE:
                    src.UriSource = new Uri("../Res/Iconos/greenPoint.png", UriKind.Relative);
                    break;
                case Codigos.PUNTO_ROJO:
                    src.UriSource = new Uri("../Res/Iconos/redPoint.png", UriKind.Relative);
                    break;
                case Codigos.PUNTO_AMARILLO:
                    src.UriSource = new Uri("../Res/Iconos/yellowPoint.png", UriKind.Relative);
                    break;
                case Codigos.PUNTO_AZUL:
                    src.UriSource = new Uri("../Res/Iconos/bluePoint.png", UriKind.Relative);
                    break;

            }
            src.EndInit();

            return src;
        }
    }
}
