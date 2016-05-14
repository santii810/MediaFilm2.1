using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFilm2._1.Res
{
    public static class Mensajes
    {
        public static string directorioNoEncontrado(string nombreDirectorio) {
            return "Directorio " + nombreDirectorio+" no encontrado";
        }

        internal static string ficheroBorradoOk(string name)
        {
            return "Fichero '" + name + "' borrado correctamente";
        }

        internal static string errorBorrandoFichero(string name, Exception e)
        {
            return "Error borrando fichero " + name + " /t " + e.ToString();
        }
    }
}
