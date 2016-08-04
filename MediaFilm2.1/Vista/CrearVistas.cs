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
using MediaFilm2._1.Controlador;

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
            tmpLabelTitulo.Content = serie.tituloLocal;
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

        internal static UIElement PanelEstadoSerie(Serie serie, GestionarDatosPage xaml, int i)
        {
            StackPanel tmpPanel = new StackPanel();
            tmpPanel.Orientation = Orientation.Horizontal;
            tmpPanel.HorizontalAlignment = HorizontalAlignment.Center;
            tmpPanel.Width = 350;
            if (i % 2 == 1)
                tmpPanel.Background = new SolidColorBrush(Color.FromRgb(213, 215, 220));



            Label tmpLabelTitulo = new Label();
            tmpLabelTitulo.Content = serie.tituloLocal;
            tmpLabelTitulo.Style = (Style)Application.Current.Resources["LabelListas"];
            tmpLabelTitulo.Width = 250;
            tmpLabelTitulo.HorizontalAlignment = HorizontalAlignment.Center;
            tmpPanel.Children.Add(tmpLabelTitulo);


            Image circuloEstado = new Image();
            circuloEstado.HorizontalAlignment = HorizontalAlignment.Center;
            if (serie.estado == 1)
                circuloEstado.Source = getPunto(Codigos.PUNTO_VERDE);
            else if (serie.estado == 2)
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

        internal static UIElement panelDescargarSerie(Serie serie, DescargasPage xaml)
        {
            StackPanel tmpPanel = new StackPanel();
            tmpPanel.Orientation = Orientation.Horizontal;
            tmpPanel.HorizontalAlignment = HorizontalAlignment.Center;


            Label tmpLabelTitulo = new Label();
            tmpLabelTitulo.Content = serie.tituloLocal;
            tmpLabelTitulo.Style = (Style)Application.Current.Resources["LabelListas"];
            tmpLabelTitulo.Width = 230;
            tmpLabelTitulo.HorizontalContentAlignment = HorizontalAlignment.Center;
            tmpPanel.Children.Add(tmpLabelTitulo);


            Button tmpButton = new Button();
            tmpButton.Click += delegate
            {
                xaml.panelListaCapitulosDescarga.Children.Clear();
                foreach (Capitulo capi in serie.capitulos)
                {
                    xaml.panelListaCapitulosDescarga.Children.Add(CrearVistas.PanelDescargarCapitulo(capi, xaml));
                }
                
            };
            tmpButton.Style = (Style)Application.Current.Resources["Button"];
            tmpButton.Content = serie.capitulos.Count + " cap.";
            tmpButton.Width = 75;
            tmpButton.FontSize = 11;
            tmpPanel.Children.Add(tmpButton);

            return tmpPanel;
        }

        private static UIElement PanelDescargarCapitulo(Capitulo capi, DescargasPage xaml)
        {
            StackPanel tmpPanel = new StackPanel();
            tmpPanel.Orientation = Orientation.Horizontal;
            tmpPanel.HorizontalAlignment = HorizontalAlignment.Center;


            Label tmpLabelTitulo = new Label();
            tmpLabelTitulo.Content = capi.titulo;
            tmpLabelTitulo.Style = (Style)Application.Current.Resources["LabelListas"];
            tmpLabelTitulo.Width = 280;
      
                       tmpPanel.Children.Add(tmpLabelTitulo);


            Button tmpButton = new Button();
            tmpButton.Click += delegate
            {
                MainWindow.gestorDescargas.DescargarCapitulo(capi);
                
            };
            tmpButton.Style = (Style)Application.Current.Resources["Button"];
            tmpButton.Content = "Descargar";
            tmpButton.Width = 75;
            tmpButton.FontSize = 11;
            tmpPanel.Children.Add(tmpButton);

            return tmpPanel;
        }

        public static ImageSource getPunto(int cod)
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

        internal static UIElement PanelActualizarTemporadas(Serie serie, GestionarDatosPage xaml, int i)
        {
            StackPanel tmpPanel = new StackPanel();
            tmpPanel.Orientation = Orientation.Horizontal;
            tmpPanel.HorizontalAlignment = HorizontalAlignment.Center;
            tmpPanel.Width = 490;
            if (i % 2 == 1)
                tmpPanel.Background = new SolidColorBrush(Color.FromRgb(213, 215, 220));




            //bitmap
            BitmapImage srcSub = new BitmapImage();
            srcSub.BeginInit();
            srcSub.UriSource = new Uri("../Res/Iconos/sub.png", UriKind.Relative);
            srcSub.EndInit();


            //bitmap
            BitmapImage srcAdd = new BitmapImage();
            srcAdd.BeginInit();
            srcAdd.UriSource = new Uri("../Res/Iconos/suma.png", UriKind.Relative);
            srcAdd.EndInit();

            Image tmpImagenReducirMinimo = new Image();
            tmpImagenReducirMinimo.Source = srcSub;
            tmpImagenReducirMinimo.HorizontalAlignment = HorizontalAlignment.Center;
            tmpImagenReducirMinimo.Width = 20;
            tmpImagenReducirMinimo.MouseLeftButtonUp += delegate
            {
                serie.temporadaActual--;
                MainWindow.SeriesXML.updateSerie(serie);
                xaml.updatePanelTemporadas();
            };
            tmpPanel.Children.Add(tmpImagenReducirMinimo);


            Label tmpLabelTempMin = new Label();
            tmpLabelTempMin.Content = serie.temporadaActual;
            tmpLabelTempMin.Style = (Style)Application.Current.Resources["LabelListas"];
            tmpLabelTempMin.Width = 30;
            tmpLabelTempMin.HorizontalContentAlignment = HorizontalAlignment.Center;
            tmpLabelTempMin.HorizontalAlignment = HorizontalAlignment.Center;
            tmpPanel.Children.Add(tmpLabelTempMin);


            Image tmpImagenAumentarMinimo = new Image();
            tmpImagenAumentarMinimo.Source = srcAdd;
            tmpImagenAumentarMinimo.HorizontalAlignment = HorizontalAlignment.Center;
            tmpImagenAumentarMinimo.Width = 20;
            tmpImagenAumentarMinimo.MouseLeftButtonUp += delegate
            {
                serie.temporadaActual++;
                MainWindow.SeriesXML.updateSerie(serie);
                xaml.updatePanelTemporadas();

            };
            tmpPanel.Children.Add(tmpImagenAumentarMinimo);




            Label tmpLabelTitulo = new Label();
            tmpLabelTitulo.Content = serie.tituloLocal;
            tmpLabelTitulo.Style = (Style)Application.Current.Resources["LabelListas"];
            tmpLabelTitulo.Width = 250;
            tmpLabelTitulo.HorizontalAlignment = HorizontalAlignment.Center;
            tmpLabelTitulo.Margin = new Thickness(50, 0, 0, 0);
            tmpPanel.Children.Add(tmpLabelTitulo);




            Image tmpImagenReducirMaximo = new Image();
            tmpImagenReducirMaximo.Source = srcSub;
            tmpImagenReducirMaximo.HorizontalAlignment = HorizontalAlignment.Center;
            tmpImagenReducirMaximo.Width = 20;
            tmpImagenReducirMaximo.MouseLeftButtonUp += delegate
            {
                serie.numeroTemporadas--;
                MainWindow.SeriesXML.updateSerie(serie);
                xaml.updatePanelTemporadas();
            };
            tmpPanel.Children.Add(tmpImagenReducirMaximo);


            Label tmpLabelTempMax = new Label();
            tmpLabelTempMax.Content = serie.numeroTemporadas;
            tmpLabelTempMax.Style = (Style)Application.Current.Resources["LabelListas"];
            tmpLabelTempMax.Width = 30;
            tmpLabelTempMax.HorizontalContentAlignment = HorizontalAlignment.Center;
            tmpLabelTempMax.HorizontalAlignment = HorizontalAlignment.Center;
            tmpPanel.Children.Add(tmpLabelTempMax);


            Image tmpImagenAumentarMaximo = new Image();
            tmpImagenAumentarMaximo.Source = srcAdd;
            tmpImagenAumentarMaximo.HorizontalAlignment = HorizontalAlignment.Center;
            tmpImagenAumentarMaximo.Width = 20;
            tmpImagenAumentarMaximo.MouseLeftButtonUp += delegate
            {
                serie.numeroTemporadas++;
                MainWindow.SeriesXML.updateSerie(serie);
                xaml.updatePanelTemporadas();

            };
            tmpPanel.Children.Add(tmpImagenAumentarMaximo);


            return tmpPanel;
        }
    }
}
