using MediaFilm2._1.Modelo.Logs;
using MediaFilm2._1.Res;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MediaFilm2._1.Modelo.XML
{
    class XMLPatron :InterfaceSeries
    {
        string nombreFichero;
        XmlNode raiz;
        public XmlDocument Documento { get; set; }

        XMLLogger PatronLogger;
        private const string SERIE_TAG_NAME = "serie";
        private const string TITULO_TAG_NAME = "titulo";


        public XMLPatron(string nombreFichero, string nombreFicheroPatronLogger)
        {
            this.nombreFichero = nombreFichero;
            PatronLogger = new XMLLogger(nombreFicheroPatronLogger);
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

        public XmlNode crearNodo(object entrada)
        {
            Patron patron = (Patron)entrada;
            XmlElement serie = Documento.CreateElement(SERIE_TAG_NAME);
            serie.SetAttribute(TITULO_TAG_NAME, patron.nombreSerie);
            serie.InnerText = patron.textoPatron;

            return serie;
        }

        public void insertar(object entrada)
        {
            Patron patron = (Patron)entrada;
            Documento = new XmlDocument();
            if (!File.Exists(this.nombreFichero))
            {
                XmlDeclaration declaracion = Documento.CreateXmlDeclaration("1.0", "ISO-8859-1", null);
                Documento.AppendChild(declaracion);
                raiz = Documento.CreateElement("raiz");
                Documento.AppendChild(raiz);
            }
            else
            {
                Documento.Load(this.nombreFichero);
                raiz = Documento.DocumentElement;
            }
            if (!existe(patron.textoPatron))
            {
                raiz.AppendChild(crearNodo(patron));
                Documento.Save(this.nombreFichero);
               PatronLogger.insertar(new LogPatrones(Recursos.LOG_TIPO_ADD_PATRON, Mensajes.ADD_PATRON_OK, patron.nombreSerie ,patron.textoPatron ));
            }
        }

        public List<object> leerXML(string serie)
        {
            List<Patron> patrones = new List<Patron>();
            if (cargarXML())
            {
                foreach (XmlNode item in Documento.GetElementsByTagName(SERIE_TAG_NAME))
                {
                    if (item.Attributes[TITULO_TAG_NAME].Value.Equals(serie))
                    {
                        patrones.Add(new Patron
                        {
                            nombreSerie = serie,
                            textoPatron = item.InnerText.ToString()
                        });
                    }
                }
            }
            return patrones;
        }

        public bool existe(string textoPatron)
        {
            foreach (XmlNode item in Documento.GetElementsByTagName("serie"))
            {
                if (item.InnerText.Equals(textoPatron))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
