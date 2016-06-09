using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MediaFilm2._1.Modelo.XML
{
  public  class XMLEstadisticas
    {
        string nombreFichero;
        XmlNode raiz;
        public XmlDocument Documento { get; set; }

        private const string RAIZ = "Estadisticas";
        private const string FECHA = "fecha";
        private const string VALOR = "valor";
        private const string TIPO = "Tipo";



        public XMLEstadisticas(string nombreFichero)
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

        public XmlNode crearNodo(string tipo, int valor)
        {
            XmlElement nodo = Documento.CreateElement(tipo);
            nodo.SetAttribute(FECHA, DateTime.Now.ToString());
            nodo.SetAttribute(VALOR, valor.ToString());
            return nodo;
        }

        public void insertar(string tipo, int valor)
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

            raiz.AppendChild(crearNodo(tipo, valor));

            Documento.Save(nombreFichero);
        }

        public object obtenerMedia(string tipo)
        {
            const int NUMERO_DE_MUESTRAS = 20;
            double media = 0;
            int cont = 0;

            if (cargarXML())
            {
                XmlNodeList listaNodos = Documento.GetElementsByTagName(tipo);

                for (int i = listaNodos.Count - NUMERO_DE_MUESTRAS; i < listaNodos.Count; i++)
                {
                    if (i >= 0)
                    {
                        cont++;
                        media += Convert.ToInt32(listaNodos[i].Attributes[VALOR].Value);
                    }
                }
            }
            if (cont == NUMERO_DE_MUESTRAS)
                media /= NUMERO_DE_MUESTRAS;
            else
                media /= cont;
            return media;
        }
    }
}
