using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFilm2._1.Modelo.Logs
{
    class LogSerie: Log
    {


        public Serie serie { get; set; }
       

        public LogSerie(string tipo, string mensaje, Serie serie) : base(tipo, mensaje)
        {
            this.serie = serie;
        }
    }
}
