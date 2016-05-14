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
    class XMLConfiguracion : InterfaceXML
    {
        string nombreFichero = Recursos.XML_CONFIGURACION;
        XmlNode raiz;
        public XmlDocument Documento { get; set; }

        private const string DIRECTORIO_TORRENT_TAG_NAME = "DirectorioTorrent";
        private const string FICHERO_IO_LOGGER_TAG_NAME = "LogIO";
        private const string FICHERO_ERROR_LOGGER_TAG_NAME = "LogError";
        private const string DIRECTORIO_TRABAJO = "DirectorioTrabajo";

    
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
            Configuracion config = new Configuracion();

            if (cargarXML())
            {
                config.directorioTorrent = Documento.GetElementsByTagName(DIRECTORIO_TORRENT_TAG_NAME)[0].InnerText;
                config.directorioTrabajo = Documento.GetElementsByTagName(DIRECTORIO_TRABAJO)[0].InnerText;
                config.ficheroIOLog = Documento.GetElementsByTagName(FICHERO_IO_LOGGER_TAG_NAME)[0].InnerText;
                config.ficheroErrorLog = Documento.GetElementsByTagName(FICHERO_ERROR_LOGGER_TAG_NAME)[0].InnerText;
            }


            return config;
        }

        public void insertar(object entrada)
        {
            throw new NotImplementedException();
        }

        public XmlNode crearNodo(object entrada)
        {
            throw new NotImplementedException();
        }
    }
}
