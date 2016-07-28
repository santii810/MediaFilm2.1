﻿using MediaFilm2._1.Modelo.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFilm2._1.Modelo
{
    public class Serie : IComparable
    {
        public string tituloLocal { get; set; }
        public string tituloDivXTotal { get; set; }
        public int temporadaActual { get; set; }
        public int numeroTemporadas { get; set; }
        public int capitulosPorTemporada { get; set; }
        public int estado { get; set; }
        public List<Patron> patrones { get; set; }



        public void leerPatrones()
        {

            patrones = MainWindow.PatronesXML.leerXML(tituloLocal);
        }

        public int CompareTo(object obj)
        {
            Serie tmp = (Serie)obj;

            if (this.estado == tmp.estado)
            {
                return String.Compare(this.tituloLocal, tmp.tituloLocal);
            }
            else if(this.estado == 1 || (this.estado == 2 && tmp.estado == 3) )
            {
                return -1;
            }
            return 1;
         
            
        }
    }
}
