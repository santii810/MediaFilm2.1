using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
    }
}
