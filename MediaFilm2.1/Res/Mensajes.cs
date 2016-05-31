using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFilm2._1.Res
{
    public static class Mensajes
    {
        public const string ADD_PATRON_OK = "Patron añadido correctamente";
        public const string ADD_SERIE_OK = "Serie añadida correctamente";
        public const string FICHERO_BORRADO_OK = "Fichero borrado correctamente";
        public const string FICHERO_RENOMBRADO_OK = "Fichero renombrado correctamente";
        public const string FICHERO_MOVIDO_OK = "Fichero movido correctamente";

        public static string directorioNoEncontrado(string nombreDirectorio) {
            return "Directorio " + nombreDirectorio+" no encontrado";
        }
         

        internal static string errorBorrandoFichero( Exception ex)
        {
            return "Error borrando fichero: /t " + ex.ToString();
        }
        internal static string errorBorrandoFichero(string name)
        {
            return "Error borrando " + name;
        }

        internal static string ErrorMoviendoFichero(Exception ex)
        {
            return "Error moviendo fichero: /t " + ex.ToString();
        }

        internal static string ErrorMoviendoFichero(string name)
        {
            return "Error moviendo " + name;
        }

        internal static string ExtensionNoRegistrada(string name)
        {
            return "Fichero con extension no registrada: " + name;
        }

        internal static string errorRenombrandoFichero(Exception ex)
        {
            return "Error renombrando fichero: /t  " + ex.ToString();
        }
    }
}
