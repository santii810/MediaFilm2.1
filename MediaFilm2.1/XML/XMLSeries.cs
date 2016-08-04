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
    public class XMLSeries
    {

        string nombreFichero;
        XmlDocument Documento;
        XmlNode raiz;
        XMLLogger SerieLogger;


        private const string RAIZ = "Series";
        private const string SERIE_TAG_NAME = "serie";
        private const string TITULO_LOCAL_TAG_NAME = "tituloLocal";
        private const string TEMPORADA_ACTUAL_TAG_NAME = "temporadaActual";
        private const string NUMERO_TEMPORADAS_TAG_NAME = "numeroTemporadas";
        private const string CAPITULOS_POR_TEMPORADA_TAG_NAME = "capitulosPorTemporada";
        private const string HREF_DIVX_LOCAL_TAG_NAME = "href_divX";
        private const string ESTADO_TAG_NAME = "estado";
        private const string PATRONES_TAG_NAME = "patrones";
        private const string PATRON_TAG_NAME = "patron";

        public XMLSeries(string nombreFichero, string nombreFicheroSeriesLogger, string nombreFicheroPatrones, string nombreFicheroPatronLogger)
        {
            this.nombreFichero = nombreFichero;
            SerieLogger = new XMLLogger(nombreFicheroSeriesLogger);
            
        }
        public List<Serie> obtenerSeries()
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
        public void insertarSerie(object entrada)
        {
            Serie serie = (Serie)entrada;

            if(serie.estado != 4)
            {
            serie.patrones.Add(serie.tituloLocal);
            serie.patrones.Add(serie.tituloLocal.Replace(' ', '.'));
            serie.patrones.Add(serie.tituloLocal.Replace(' ', '_'));

            string[] splitSerie = serie.tituloLocal.Trim().Split(' ');
            if (splitSerie.Length > 2)
            {
                string tituloPatron = "";
                foreach (string item in splitSerie)
                {
                    tituloPatron += item[0];
                }
                serie.patrones.Add(tituloPatron);
            }
            }



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
            if (!existe(serie.tituloLocal))
            {
                raiz.AppendChild(crearNodo(serie));
                Documento.Save(nombreFichero);

                SerieLogger.insertar(new LogSerie(Recursos.LOG_TIPO_ADD_SERIE, Mensajes.ADD_SERIE_OK, serie));

               
            }
        }
        public void updateSerie(Serie serie)
        {
            XmlNode nodoViejo = buscarNodo(serie.tituloLocal);
            if (nodoViejo != null)
            {
                raiz.ReplaceChild(crearNodo(serie), nodoViejo);
                Documento.Save(nombreFichero);
            }

        }
        
        private XmlNode crearNodo(object entrada)
        {
            Serie serie = (Serie)entrada;

            XmlElement nodoSerie = Documento.CreateElement(SERIE_TAG_NAME);
            nodoSerie.SetAttribute(TITULO_LOCAL_TAG_NAME, serie.tituloLocal);

            XmlElement titulo = Documento.CreateElement(TITULO_LOCAL_TAG_NAME);
            titulo.InnerText = serie.tituloLocal;
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

            XmlElement descarga = Documento.CreateElement(HREF_DIVX_LOCAL_TAG_NAME);
            descarga.InnerText = serie.href_divX;
            nodoSerie.AppendChild(descarga);

            XmlElement estado = Documento.CreateElement(ESTADO_TAG_NAME);
            estado.InnerText = serie.estado.ToString();
            nodoSerie.AppendChild(estado);

            XmlElement patrones = Documento.CreateElement(PATRONES_TAG_NAME);



            foreach (string item in serie.patrones)
            {
                XmlElement patron = Documento.CreateElement(PATRON_TAG_NAME);
                patron.InnerText = item;
                patrones.AppendChild(patron);
            }

            nodoSerie.AppendChild(patrones);

            return nodoSerie;
        }
        private bool existe(string campo)
        {
            foreach (XmlNode item in Documento.GetElementsByTagName(SERIE_TAG_NAME))
                if (item.Attributes[TITULO_LOCAL_TAG_NAME].Value.Equals(campo))
                    return true;
            return false;
        }
        private Serie buscarSerie(string nombreSerie)
        {
            Serie serie = new Serie();
            if (cargarXML())
            {
                foreach (XmlNode item in Documento.GetElementsByTagName(SERIE_TAG_NAME))
                {
                    if (item[TITULO_LOCAL_TAG_NAME].InnerText.ToString().Equals(nombreSerie))
                    {
                        serie = leerNodo(item);
                    }
                }
            }
            return serie;
        }
        private bool cargarXML()
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
        private static Serie leerNodo(XmlNode item)
        {
            HashSet<string> patrones = new HashSet<string>();
            foreach (XmlNode itPatron in item[PATRONES_TAG_NAME].ChildNodes)
            {
                patrones.Add(itPatron.InnerText);
            }

            return new Serie
            {
                tituloLocal = item[TITULO_LOCAL_TAG_NAME].InnerText.ToString(),
                temporadaActual = Convert.ToInt32(item[TEMPORADA_ACTUAL_TAG_NAME].InnerText.ToString()),
                numeroTemporadas = Convert.ToInt32(item[NUMERO_TEMPORADAS_TAG_NAME].InnerText.ToString()),
                capitulosPorTemporada = Convert.ToInt32(item[CAPITULOS_POR_TEMPORADA_TAG_NAME].InnerText.ToString()),
                estado = Convert.ToInt32(item[ESTADO_TAG_NAME].InnerText),
                href_divX = item[HREF_DIVX_LOCAL_TAG_NAME].InnerText,
                patrones = patrones

            };
        }
        private XmlNode buscarNodo(string tituloSerie)
        {
            if (cargarXML())
                foreach (XmlNode item in Documento.GetElementsByTagName(SERIE_TAG_NAME))
                    if (item[TITULO_LOCAL_TAG_NAME].InnerText.ToString().Equals(tituloSerie)) return item;
            return null;
        }
    }
}


