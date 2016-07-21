using MediaFilm2._1.Modelo;
using System.Collections.Generic;

namespace MediaFilm2._1.Vista
{
    internal class MantenimientoResponse
    {

        public List<ErrorDuplicidad> ErroresDuplicidad { get; set; }
        public List<string> ErroresContinuidad { get; set; }

        public MantenimientoResponse()
        {
            ErroresDuplicidad = new List<ErrorDuplicidad>();
            ErroresContinuidad = new List<string>();
        }

    }
}