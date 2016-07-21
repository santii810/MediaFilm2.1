using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFilm2._1.Modelo.Response
{
    class RecorrerTorrentResponse
    {

        public List<string> ficherosBorrados { get; set; }
        public List<string> erroresBorrando { get; set; }
        public List<string> videosMovidos { get; set; }
        public int directoriosBorrados { get; set; }
        public int tiempoTranscurrido { get; set; }


        public RecorrerTorrentResponse()
        {
            ficherosBorrados = new List<string>();
            erroresBorrando = new List<string>();
            videosMovidos = new List<string>();
            directoriosBorrados = 0;
            tiempoTranscurrido = 0;
        }
    }
}
