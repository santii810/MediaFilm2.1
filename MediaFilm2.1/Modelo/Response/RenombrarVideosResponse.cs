using System.Collections.Generic;

namespace MediaFilm2._1.Modelo.Response
{
    public class RenombrarVideosResponse
    {

        public int tiempoTranscurrido { get; set; }
        public int seriesActivas { get; set; }
        public int seriesTotales { get; set; }
        public int patronesEjecutados { get; set; }
        public List<string[]> videosRenombrados { get; set; }
        public List<string> erroresRenombrando { get; set; }


        public RenombrarVideosResponse()
        {
            seriesActivas = 0;
            patronesEjecutados = 0;
            videosRenombrados = new List<string[]>();
            erroresRenombrando = new List<string>();
        }

    }
}