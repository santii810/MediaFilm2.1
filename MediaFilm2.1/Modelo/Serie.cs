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
        public string titulo { get; set; }
        public string tituloDescarga { get; set; }
        public int temporadaActual { get; set; }
        public int numeroTemporadas { get; set; }
        public int capitulosPorTemporada { get; set; }
        public string estado { get; set; }
        public List<Patron> patrones { get; set; }



        public void leerPatrones()
        {

            patrones = MainWindow.PatronesXML.leerXML(titulo);
        }

        public int CompareTo(object obj)
        {
            Serie tmp = (Serie)obj;

            if (this.estado == tmp.estado)
            {
                return String.Compare(this.titulo, tmp.titulo);
            }
            else if(this.estado == "A" || (this.estado == "D" && tmp.estado == "F") )
            {
                return -1;
            }
            return 1;
         
            
        }
    }
}
