using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFilm2._1.Modelo.Logs
{
    public class Log
    {
        public DateTime fecha { get; set; }
        public string tipo { get; set; }
        public string mensaje { get; set; }

        public Log(string tipo, string mensaje)
        {
            fecha = DateTime.Now;
            this.tipo = tipo;
            this.mensaje = mensaje;
        }


    }
    
}
