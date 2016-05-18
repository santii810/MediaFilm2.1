using MediaFilm2._1.Modelo.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFilm2._1.Modelo
{
    class Serie : IComparable
    {
        public string titulo { get; set; }
        public string tituloDescarga { get; set; }
        public int temporadaActual { get; set; }
        public int numeroTemporadas { get; set; }
        public int capitulosPorTemporada { get; set; }
        public string estado { get; set; }
        public List<Patron> patrones { get; set; }



        public void leerPatrones(string ficheroPatrones, string ficheroPatronLog)
        {
            XMLPatron xmlPat = new XMLPatron(ficheroPatrones, ficheroPatronLog);
            patrones = (List<Patron>) xmlPat.leerXML(titulo);
        }

        public int CompareTo(object obj)
        {
            Serie tmp = (Serie)obj;
            return String.Compare(this.titulo, tmp.titulo);
        }
    }
}
