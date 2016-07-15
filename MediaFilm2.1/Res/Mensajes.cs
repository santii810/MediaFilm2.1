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
        //Ordenar
        public const string ADD_PATRON_OK = "Patron añadido correctamente";
        public const string ADD_SERIE_OK = "Serie añadida correctamente";
        public const string FICHERO_BORRADO_OK = "Fichero borrado correctamente";
        public const string FICHERO_RENOMBRADO_OK = "Fichero renombrado correctamente";
        public const string FICHERO_MOVIDO_OK = "Fichero movido correctamente";
        // GestorDatos
        public const string TITULO_SERIE_VACIO = "El titulo de la serie no puede estar vacio";
        public const string TITULO_SERIE_NOK = "El titulo de la serie debe ser de al menos 3 caracteres";
        public const string SERIE_ADDED_OK = "Serie añadida correctamente";
        public const string SERIE_ADD_ERROR = "Datos insertados incorrectos";
        internal static readonly string DIRECTORIO_TRABAJO_NO_ENCONTRADO = "Directorio de trabajo no encontrado";
        internal static string PATRON_INVALIDO = "Patron invalido";

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
