using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFilm2._1.Modelo.Logs
{
    class LogPatrones : Log
    {
        public string serie { get; set; }
        public string patron { get; set; }

        public LogPatrones(string tipo, string mensaje, string serie, string patron) : base(tipo, mensaje)
        {
            this.serie = serie;
            this.patron = patron;
        }
    }
}
