using MediaFilm2._1.Modelo.Logs;
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
        private const string TIPO_LOG_TAG_NAME = "tipo";
        private const string NOMBRE_FICHERO_TAG_NAME = "fichero";
        private const string FECHA_TAG_NAME = "fecha";
        private const string MENSAJE_TAG_NAME = "mensaje";
        //LogPatrones
        private const string PATRON_TAG_NAME = "patron";
        private const string SERIE_TAG_NAME = "serie";
        //LogSerie
        public const string TITULO_TAG_NAME = "titulo";
        public const string TEMPORADA_ACTUAL_TAG_NAME = "temporadaActual";
        public const string NUMERO_TEMPORADAS_TAG_NAME = "numeroTemporadas";
        public const string CAPITULOS_POR_TEMPORADA_TAG_NAME = "capitulosPorTemporada";
        public const string TITULO_DESCARGA_TAG_NAME = "tituloDescarga";
        public const string ESTADO_TAG_NAME = "estado";
        public const string NOMBRE_FICHERO_ANTIGUO_TAG_NAME = "nombreAntiguo";


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
            nodo.SetAttribute(TIPO_LOG_TAG_NAME, log.tipo);
            nodo.SetAttribute(FECHA_TAG_NAME, log.fecha.ToString());
            nodo.SetAttribute(MENSAJE_TAG_NAME, log.mensaje);


            if (log.GetType() == typeof(LogIO) || log.GetType() == typeof(LogRenombrado))
            {
                nodo.SetAttribute(NOMBRE_FICHERO_TAG_NAME, ((LogIO)log).fichero.FullName);
                if (log.GetType() == typeof(LogRenombrado))
                {
                    nodo.SetAttribute(NOMBRE_FICHERO_ANTIGUO_TAG_NAME, ((LogRenombrado)log).nombreAntiguo);
                }
            }

            if (log.GetType() == typeof(LogPatrones))
            {
                nodo.SetAttribute(PATRON_TAG_NAME, ((LogPatrones)log).patron);
                nodo.SetAttribute(SERIE_TAG_NAME, ((LogPatrones)log).serie);
            }

            if (log.GetType() == typeof(LogSerie))
            {
                nodo.SetAttribute(TITULO_TAG_NAME, ((LogSerie)log).serie.titulo);
                nodo.SetAttribute(TEMPORADA_ACTUAL_TAG_NAME, ((LogSerie)log).serie.temporadaActual.ToString());
                nodo.SetAttribute(NUMERO_TEMPORADAS_TAG_NAME, ((LogSerie)log).serie.numeroTemporadas.ToString());
                nodo.SetAttribute(CAPITULOS_POR_TEMPORADA_TAG_NAME, ((LogSerie)log).serie.capitulosPorTemporada.ToString());
                nodo.SetAttribute(TITULO_DESCARGA_TAG_NAME, ((LogSerie)log).serie.tituloDescarga);
                nodo.SetAttribute(ESTADO_TAG_NAME, ((LogSerie)log).serie.estado);
            }
            return nodo;
        }
    }
}
