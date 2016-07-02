using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MediaFilm2._1.Modelo;

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

     
        internal static UIElement PanelSeleccionarSerie(Serie item, GestionarDatosPage xaml)
        {
            StackPanel tmpPanel = new StackPanel();
            tmpPanel.Orientation = Orientation.Horizontal;
            tmpPanel.HorizontalAlignment = HorizontalAlignment.Center;


            Label tmpLabelTitulo = new Label();
            tmpLabelTitulo.Content = item.titulo;
            tmpLabelTitulo.Style = (Style)Application.Current.Resources["LabelListas"];
            tmpLabelTitulo.Width = 250;
            tmpPanel.Children.Add(tmpLabelTitulo);


            Button tmpButton = new Button();
            tmpButton.Click += delegate
            {
                xaml.seleccionarSerie(item);
            };
            tmpButton.Style = (Style)Application.Current.Resources["Button"];
            tmpButton.Content = "Seleccionar";
            tmpButton.FontSize = 11;
            tmpPanel.Children.Add(tmpButton);

            return tmpPanel;
        }

        internal static UIElement LabelFicherosARenombrar(object name)
        {
            throw new NotImplementedException();
        }
    }
}
