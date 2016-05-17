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

        public static string directorioNoEncontrado(string nombreDirectorio) {
            return "Directorio " + nombreDirectorio+" no encontrado";
        }

        internal static string ficheroBorradoOk()
        {
            return "Fichero borrado correctamente";
        }

        internal static string errorBorrandoFichero( Exception ex)
        {
            return "Error borrando fichero: /t " + ex.ToString();
        }
        internal static string errorBorrandoFichero(string name)
        {
            return "Error borrando " + name;
        }

        internal static string FicheroMovidoOK(string name)
        {
            return "Fichero " + name + " movido correctamente";
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
    }
}
