using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MediaFilm2._1.Modelo.XML
{
    interface InterfaceXML
    {

        bool cargarXML();
        object leerXML();
        void insertar(object entrada);
        XmlNode crearNodo(object entrada);


    }
}
