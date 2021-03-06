﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFilm2._1.Res
{
    public static class Recursos
    {
        #region ficheros
        public const string CONFIG_URL = @"xml\config.xml";
        #endregion

        #region paginas
        public const string INICIO_RAPIDO_PAGE_URL = @"Vista\InicioRapidoPage.xaml";
        public const string ORDENAR_PAGE_URL = @"Vista\OrdenarPage.xaml";
        public const string GESTIONAR_DATOS_PAGE_URL = @"Vista\GestionarDatosPage.xaml";
        public const string MANTENIMIENTO_PAGE_URL = @"Vista\MantenimientoPage.xaml";
        public const string DESCARGAS_PAGE_URL = @"Vista\DescargasPage.xaml";
        public const string CONFIG_PAGE_URL = @"Vista\ConfigPage.xaml";
        #endregion

        #region nombre Ficheros
        public const string XML_CONFIGURACION = @"xml\configuracion.xml";
        #endregion

        #region tipoLog
        public const string LOG_TIPO_BORRADO_OK = "Borrado";
        public const string LOG_TIPO_ERROR_BORRANDO = "Error borrando";
        public const string LOG_TIPO_MOVIDO_OK = "Movido";
        public const string LOG_TIPO_ERROR_MOVIENDO = "Error moviendo";
        public const string LOG_TIPO_ADD_PATRON = "Añadir patron";
        public const string LOG_TIPO_ADD_SERIE = "Añadir serie";
        public const string LOG_TIPO_RENOMBRADO_OK = "Renombrar capitulo";
        public const string LOG_TIPO_ERROR_RENOMBRADO = "Error renombrando";

        #endregion


        #region tiempos
        public const string TIEMPO_RECORRER_TORRENT = "TiempoRecorrerTorrent";
        public const string TIEMPO_RENOMBRAR_VIDEOS = "TiempoRenombrarVideos";
        public const string NUMERO_SERIES_ACTIVAS = "NumeroSeriesActivas";
        public const string NUMERO_PATRONES_EJECUTADOS = "NumeroPatronesEjecutados";


        #endregion
    }
}
