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
        XmlDocument Documento;
        XmlNode raiz;
        XMLLogger SerieLogger;
        XMLPatron PatronesXML;


        public const string RAIZ = "Series";
        public const string SERIE_TAG_NAME = "serie";
        public const string TITULO_TAG_NAME = "titulo";
        public const string TEMPORADA_ACTUAL_TAG_NAME = "temporadaActual";
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

        public List<Serie> leerXML()
        {
            List<Serie> series = new List<Serie>();
            if (cargarXML())
            {
                foreach (XmlNode item in Documento.GetElementsByTagName(SERIE_TAG_NAME))
                {
                    series.Add(leerNodo(item)
                    );
                }
            }
            return series;
        }

        private static Serie leerNodo(XmlNode item)
        {
            return new Serie
            {
                titulo = item[TITULO_TAG_NAME].InnerText.ToString(),
                temporadaActual = Convert.ToInt32(item[TEMPORADA_ACTUAL_TAG_NAME].InnerText.ToString()),
                numeroTemporadas = Convert.ToInt32(item[NUMERO_TEMPORADAS_TAG_NAME].InnerText.ToString()),
                capitulosPorTemporada = Convert.ToInt32(item[CAPITULOS_POR_TEMPORADA_TAG_NAME].InnerText.ToString()),
                estado = item[ESTADO_TAG_NAME].InnerText,
                tituloDescarga = item[TITULO_DESCARGA_TAG_NAME].InnerText
            };
        }

        public void insertar(object entrada)
        {
            Serie serie = (Serie)entrada;
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
            if (!existe(serie.titulo))
            {
                raiz.AppendChild(crearNodo(serie));
                Documento.Save(nombreFichero);

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

            XmlElement nodoSerie = Documento.CreateElement(SERIE_TAG_NAME);
            nodoSerie.SetAttribute(TITULO_TAG_NAME, serie.titulo);

            XmlElement titulo = Documento.CreateElement(TITULO_TAG_NAME);
            titulo.InnerText = serie.titulo;
            nodoSerie.AppendChild(titulo);

            XmlElement temporadaActual = Documento.CreateElement(TEMPORADA_ACTUAL_TAG_NAME);
            temporadaActual.InnerText = serie.temporadaActual.ToString();
            nodoSerie.AppendChild(temporadaActual);

            XmlElement numeroTemporadas = Documento.CreateElement(NUMERO_TEMPORADAS_TAG_NAME);
            numeroTemporadas.InnerText = serie.numeroTemporadas.ToString();
            nodoSerie.AppendChild(numeroTemporadas);

            XmlElement capitulosPorTemporada = Documento.CreateElement(CAPITULOS_POR_TEMPORADA_TAG_NAME);
            capitulosPorTemporada.InnerText = serie.capitulosPorTemporada.ToString();
            nodoSerie.AppendChild(capitulosPorTemporada);

            XmlElement descarga = Documento.CreateElement(TITULO_DESCARGA_TAG_NAME);
            descarga.InnerText = serie.tituloDescarga;
            nodoSerie.AppendChild(descarga);

            XmlElement estado = Documento.CreateElement(ESTADO_TAG_NAME);
            estado.InnerText = serie.estado;
            nodoSerie.AppendChild(estado);

            return nodoSerie;
        }

        public bool existe(string campo)
        {
            foreach (XmlNode item in Documento.GetElementsByTagName(SERIE_TAG_NAME))
                if (item.Attributes[TITULO_TAG_NAME].Value.Equals(campo))
                    return true;
            return false;
        }

        private XmlNode buscarNodo(string tituloSerie)
        {
            if (cargarXML())
                foreach (XmlNode item in Documento.GetElementsByTagName(SERIE_TAG_NAME))
                    if (item[TITULO_TAG_NAME].InnerText.ToString().Equals(tituloSerie)) return item;
            return null;
        }

        public void updateSerie(Serie serie)
        {
            XmlNode nodoViejo = buscarNodo(serie.titulo);
            if (nodoViejo != null)
            {
                raiz.ReplaceChild(crearNodo(serie), nodoViejo);
                Documento.Save(nombreFichero);
            }

        }

        public Serie buscarSerie(string nombreSerie)
        {
            Serie serie = new Serie();
            if (cargarXML())
            {
                foreach (XmlNode item in Documento.GetElementsByTagName(SERIE_TAG_NAME))
                {
                    if (item[TITULO_TAG_NAME].InnerText.ToString().Equals(nombreSerie))
                    {
                        serie = leerNodo(item);
                    }
                }
            }
            return serie;
        }
    }
}


