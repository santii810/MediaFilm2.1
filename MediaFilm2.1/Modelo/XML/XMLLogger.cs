using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MediaFilm2._1.Modelo.XML
{
    class XMLLogger : InterfaceXML
    {

        string nombreFichero;
        XmlNode raiz;
        public XmlDocument Documento { get; set; }

        private const string RAIZ = "Logs";
        private const string DOCUMENT_ELEMENT = "Log";
        private const string TIPO_LOG = "tipo";
        private const string NOMBRE_FICHERO = "fichero";
        private readonly string FECHA = "fecha";
        private readonly string MENSAJE = "mensaje";

        public XMLLogger(string nombreFichero)
        {
            this.nombreFichero = nombreFichero;
        }

        public bool cargarXML()
        {
            if (File.Exists(nombreFichero))
            {
                Documento = new XmlDocument();
                Documento.Load(nombreFichero);
                raiz = Documento.DocumentElement;
                return true;
            }
            else return false;
        }

        public object leerXML()
        {
            throw new NotImplementedException();
        }

        public void insertar(object entrada)
        {

            Documento = new XmlDocument();
            if (!File.Exists(nombreFichero))
            {
                XmlDeclaration declaracion = Documento.CreateXmlDeclaration("1.0", "ISO-8859-1", null);
                Documento.AppendChild(declaracion);
                raiz = Documento.CreateElement(RAIZ);
                Documento.AppendChild(raiz);
            }
            else
            {
                Documento.Load(nombreFichero);
                raiz = Documento.DocumentElement;
            }

            raiz.AppendChild(crearNodo(entrada));

            Documento.Save(nombreFichero);
        }

        public XmlNode crearNodo(object entrada)
        {
            Log log = (Log)entrada;
            XmlElement nodo = Documento.CreateElement(DOCUMENT_ELEMENT);
            nodo.SetAttribute(TIPO_LOG, log.tipo);
            nodo.SetAttribute(FECHA, log.fecha.ToString());
            nodo.SetAttribute(MENSAJE, log.mensaje);

            try
            {
                nodo.SetAttribute(NOMBRE_FICHERO, ((LogIO)log).fichero.FullName);
            }
            catch { }
            return nodo;
        }
    }
}
