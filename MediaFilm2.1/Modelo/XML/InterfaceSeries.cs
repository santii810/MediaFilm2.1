using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MediaFilm2._1.Modelo.XML
{
    interface InterfaceSeries

    {

        bool cargarXML();
        List<object> leerXML(string filtro);
        void insertar(object entrada);
        XmlNode crearNodo(object entrada);
        bool existe(string campo);
    }
}
