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
    class XMLSeries  
    {

        string nombreFichero;
        XmlDocument documento;
        XmlDocument Documento;
        XmlNode raiz;
        XMLLogger SerieLogger;
        XMLPatron PatronesXML;


        public const string RAIZ = "Series";
        public const string SERIE_TAG_NAME = "serie";
        public const string TITULO_TAG_NAME ="titulo";
        public const string TEMPORADA_ACTUAL_TAG_NAME ="temporadaActual";
        public const string NUMERO_TEMPORADAS_TAG_NAME = "numeroTemporadas";
        public const string CAPITULOS_POR_TEMPORADA_TAG_NAME = "capitulosPorTemporada";
        public const string TITULO_DESCARGA_TAG_NAME = "tituloDescarga";
        public const string ESTADO_TAG_NAME = "estado";


        public XMLSeries(string nombreFichero, string nombreFicheroSeriesLogger, string nombreFicheroPatrones, string nombreFicheroPatronLogger)
        {
            this.nombreFichero = nombreFichero;
            SerieLogger = new XMLLogger(nombreFicheroSeriesLogger);
            PatronesXML = new XMLPatron(nombreFicheroPatrones, nombreFicheroPatronLogger);
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

        public object leerXML(string serie)
        {
            throw new NotImplementedException();
        }

        public void insertar(object entrada)
        {
            Serie serie = (Serie)entrada;
            documento = new XmlDocument();

            if (!File.Exists(nombreFichero))
            {
                XmlDeclaration declaracion = documento.CreateXmlDeclaration("1.0", "ISO-8859-1", null);
                documento.AppendChild(declaracion);
                raiz = documento.CreateElement(RAIZ);
                documento.AppendChild(raiz);
            }
            else
            {
                documento.Load(nombreFichero);
                raiz = documento.DocumentElement;
            }
            if (!existe(serie.titulo))
            {
                raiz.AppendChild(crearNodo(serie));
                documento.Save(nombreFichero);

                SerieLogger.insertar(new LogSerie(Recursos.LOG_TIPO_ADD_SERIE, Mensajes.ADD_SERIE_OK, serie));

                //Añado 3 patrones por defecto a todas las series nada mas ser añadidas
                PatronesXML.insertar(new Patron { nombreSerie = serie.titulo, textoPatron = serie.titulo });
                PatronesXML.insertar(new Patron { nombreSerie = serie.titulo, textoPatron = serie.titulo.Replace(' ', '.') });
                PatronesXML.insertar(new Patron { nombreSerie = serie.titulo, textoPatron = serie.titulo.Replace(' ', '_') });
            }
        }

        public XmlNode crearNodo(object entrada)
        {
            Serie serie = (Serie)entrada;

            XmlElement nodoSerie = documento.CreateElement(SERIE_TAG_NAME);
            nodoSerie.SetAttribute(TITULO_TAG_NAME, serie.titulo);

            XmlElement titulo = documento.CreateElement(TITULO_TAG_NAME);
            titulo.InnerText = serie.titulo;
            nodoSerie.AppendChild(titulo);

            XmlElement temporadaActual = documento.CreateElement(TEMPORADA_ACTUAL_TAG_NAME);
            temporadaActual.InnerText = serie.temporadaActual.ToString();
            nodoSerie.AppendChild(temporadaActual);

            XmlElement numeroTemporadas = documento.CreateElement(NUMERO_TEMPORADAS_TAG_NAME);
            numeroTemporadas.InnerText = serie.numeroTemporadas.ToString();
            nodoSerie.AppendChild(numeroTemporadas);

            XmlElement capitulosPorTemporada = documento.CreateElement(CAPITULOS_POR_TEMPORADA_TAG_NAME);
            capitulosPorTemporada.InnerText = serie.capitulosPorTemporada.ToString();
            nodoSerie.AppendChild(capitulosPorTemporada);

            XmlElement descarga = documento.CreateElement(TITULO_DESCARGA_TAG_NAME);
            descarga.InnerText = serie.tituloDescarga;
            nodoSerie.AppendChild(descarga);

            XmlElement estado = documento.CreateElement(ESTADO_TAG_NAME);
            estado.InnerText = serie.estado;
            nodoSerie.AppendChild(estado);

            return nodoSerie;
        }

        public bool existe(string campo)
        {
            throw new NotImplementedException();
        }
    }
    }


