﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFilm2._1.Modelo
{
    public class Configuracion
    {

        public string directorioTorrent { get; set; }
        public string directorioTrabajo { get; set; }
        public string directorioSeries { get; set; }

        public string ficheroIOLog { get; set; }
        public string ficheroErrorLog { get; set; }
        internal string ficheroPatronLog { get; set; }
        public string ficheroSerieLogger { get; set; }

        public string ficheroTiempos { get; set; }
        internal string ficheroPatrones { get; set; }
        public string ficheroSeries { get; set; }


    }
}
