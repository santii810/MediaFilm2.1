using MediaFilm2._1.Modelo.XML;
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
        public string href_divX { get; set; }
        public int temporadaActual { get; set; }
        public int numeroTemporadas { get; set; }
        public int capitulosPorTemporada { get; set; }
        public int estado { get; set; }
        public HashSet<string> patrones { get; set; }
        public HashSet<Capitulo> capitulos { get; set; }


        public Serie()
        {
            patrones = new HashSet<string>();
            capitulos = new HashSet<Capitulo>();
        }
        public int CompareTo(object obj)
        {
            Serie tmp = (Serie)obj;

            if (this.estado == tmp.estado)
            {
                return String.Compare(this.tituloLocal, tmp.tituloLocal);
            }
            else if (this.estado == 1 || (this.estado == 2 && tmp.estado == 3))
            {
                return -1;
            }
            return 1;


        }
    }
}
