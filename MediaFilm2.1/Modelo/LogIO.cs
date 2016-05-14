using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFilm2._1.Modelo
{
    public class LogIO : Log
    {
        public FileInfo fichero;

        public LogIO(string tipo, string mensaje, FileInfo fichero) : base(tipo, mensaje)
        {
            this.fichero = fichero;
        }
    }
}
