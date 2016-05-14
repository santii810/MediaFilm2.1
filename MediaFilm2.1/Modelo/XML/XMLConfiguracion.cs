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
        string nombreFichero = Recursos.xmlConfiguracion;
        XmlNode raiz;
        public XmlDocument Documento { get; set; }

        private string directorioTorrentTagName = "directorioTorrent";



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
                config.directorioTorrent = Documento.GetElementsByTagName(directorioTorrentTagName)[0].InnerText;
            }


            return config;
        }


    }
}
