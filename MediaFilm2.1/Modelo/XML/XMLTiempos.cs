﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MediaFilm2._1.Modelo.XML
{
    class XMLTiempos
    {
        string nombreFichero;
        XmlNode raiz;
        public XmlDocument Documento { get; set; }

        private const string RAIZ = "Tiempos";
        private const string FECHA = "fecha";
        private const string TIEMPO = "tiempo";



        public XMLTiempos(string nombreFichero)
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

        public XmlNode crearNodo(string tipo, int tiempo)
        {
            XmlElement nodo = Documento.CreateElement(tipo);
            nodo.SetAttribute(FECHA, DateTime.Now.ToString());
            nodo.SetAttribute(TIEMPO, tiempo.ToString());
            return nodo;
        }

        public void insertar(string tipo, int tiempo)
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

            raiz.AppendChild(crearNodo(tipo, tiempo));

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

                for (int i = listaNodos.Count - NUMERO_DE_MUESTRAS - 1; i < listaNodos.Count - 1; i++)
                {
                    if (i > 0)
                    {
                        cont++;
                        media += Convert.ToInt32(listaNodos[i].Attributes[TIEMPO].Value);
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