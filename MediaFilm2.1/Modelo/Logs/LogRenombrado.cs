using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFilm2._1.Modelo.Logs
{
    public class LogRenombrado : LogIO
    {
        public string nombreAntiguo;

        public LogRenombrado(string tipo, string mensaje, FileInfo fichero, string nombreAntiguo) : base(tipo, mensaje, fichero)
        {
            this.nombreAntiguo = nombreAntiguo;
        }
    }
}
