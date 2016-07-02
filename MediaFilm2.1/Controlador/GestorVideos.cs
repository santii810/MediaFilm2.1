﻿using MediaFilm2._1.Modelo;
using MediaFilm2._1.Modelo.Logs;
using MediaFilm2._1.Modelo.Request;
using MediaFilm2._1.Res;
using MediaFilm2._1.Vista;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace MediaFilm2._1.Controlador
{
    class GestorVideos
    {

        public static RecorrerTorrentRequest recorrerTorrent()
        {
            RecorrerTorrentRequest recorrerTorrentRequest = new RecorrerTorrentRequest();

            Stopwatch tiempo = Stopwatch.StartNew();

            //mueve o borra archivos
            DirectoryInfo dir = new DirectoryInfo(MainWindow.config.directorioTorrent);
            if (dir.Exists)
            {
                foreach (FileInfo fichero in listarFicheros(dir.GetFileSystemInfos()))
                {
                    manejarFichero(recorrerTorrentRequest, fichero);
                }
            }

            //borra directorios
            recorrerTorrentRequest.directoriosBorrados = borrarDirectoriosVacios(MainWindow.config.directorioTorrent);
            recorrerTorrentRequest.tiempoTranscurrido = (int)tiempo.ElapsedMilliseconds;

            Thread.Sleep(100);
            Directory.CreateDirectory(MainWindow.config.directorioTorrent);

            return recorrerTorrentRequest;
        }

        public static RenombrarVideosRequest renombrarVideos()
        {
            RenombrarVideosRequest renombrarVideosRequest = new RenombrarVideosRequest();
            Stopwatch tiempo = Stopwatch.StartNew();

            List<Serie> series = MainWindow.SeriesXML.leerXML();
            renombrarVideosRequest.seriesTotales = series.Count;
            foreach (Serie itSerie in series)
            {
                if (itSerie.estado == "A")
                {
                    renombrarVideosRequest.seriesActivas++;
                    itSerie.leerPatrones();
                    foreach (Patron itPatron in itSerie.patrones)
                    {
                        for (int temp = itSerie.temporadaActual; temp <= itSerie.numeroTemporadas; temp++)
                        {
                            for (int cap = 0; cap <= itSerie.capitulosPorTemporada; cap++)
                            {
                                FileInfo fi;
                                string dirSerie = MainWindow.config.directorioSeries + @"\" + itSerie.titulo + @"\Temporada" + temp + @"\";
                                renombrarVideosRequest.patronesEjecutados += 8;
                                string[] strPatrones = new string[]
                                {
                                    //patrones para capitulos<10  y extension == mkv
                                    itPatron.textoPatron + "*" + temp.ToString() + "0" + cap.ToString() + "*.mkv" ,
                                    itPatron.textoPatron + "*" + temp.ToString() + "x0" + cap.ToString() + "*.mkv" ,
                                    temp.ToString()+"x0"+cap.ToString()+"*"+itPatron.textoPatron+"*.mkv",

                                    //patrones para capitulos<10  y extension == avi
                                    itPatron.textoPatron + "*" + temp.ToString() + "0" + cap.ToString() + "*.avi" ,
                                    itPatron.textoPatron + "*" + temp.ToString() + "x0" + cap.ToString() + "*.avi" ,
                                    temp.ToString()+"x0"+cap.ToString()+"*"+itPatron.textoPatron+"*.avi",

                                    //patrones para capitulos>10  y extension == mkv
                                    itPatron.textoPatron + "*" + temp.ToString() + cap.ToString() + "*.mkv",
                                    itPatron.textoPatron + "*" + temp.ToString() + "x" + cap.ToString() + "*.mkv",
                                    temp.ToString()+"x"+cap.ToString()+"*"+itPatron.textoPatron+"*.mkv",

                                    //patrones para capitulos>10  y extension == avi
                                    itPatron.textoPatron + "*" + temp.ToString() + cap.ToString() + "*.avi",
                                    itPatron.textoPatron + "*" + temp.ToString() + "x" + cap.ToString() + "*.avi",
                                    temp.ToString()+"x"+cap.ToString()+"*"+itPatron.textoPatron+"*.avi",

                                    //patrones especiales
                                    itPatron.textoPatron+"*"+temp + "(" + cap.ToString()+ ")" + ".avi",
                                    itPatron.textoPatron+"*"+temp + "(" + cap.ToString()+ ")" + ".mkv"
                            };

                                /* 
                                Los ultimos 2 patrones no dependen de si el capitulo es mayor o menor que 10
                                luego se ejecuta 1 vez cada patron cuando i pasa por las posiciones 6 y 7
                                como los patrones estan en la posicion 12 y 13 respectivamente se suma 6 a la i

                                Los patrones de capitulos mayores que 10 estan en las posiciones del 7 al 12
                                asi que si el numero del capitulo es >=  10 se coge la posicion 6+i
                                */
                                for (int i = 0; i < 8; i++)
                                {
                                    if (cap >= 10 || i > 5)
                                    {
                                        fi = obtenerCoincidenciaBusqueda(strPatrones[i + 6]);
                                    }
                                    else
                                    {
                                        fi = obtenerCoincidenciaBusqueda(strPatrones[i]);
                                    }
                                    if (fi != null)
                                    {
                                        string nombreOriginal = fi.Name;
                                        //Crea todos los directorios y subdirectorios en la ruta de acceso especificada, a menos que ya existan.
                                        Directory.CreateDirectory(dirSerie);
                                        try
                                        {
                                            if (cap < 10)
                                            {
                                                fi.MoveTo(dirSerie + @"\" + itSerie.titulo + " " + temp + "x0" + cap + fi.Extension);
                                            }
                                            else
                                            {
                                                fi.MoveTo(dirSerie + @"\" + itSerie.titulo + " " + temp + "x" + cap + fi.Extension);
                                            }
                                            MainWindow.IOLogger.insertar(new LogRenombrado(Recursos.LOG_TIPO_RENOMBRADO_OK, Mensajes.FICHERO_RENOMBRADO_OK, fi, nombreOriginal));
                                            renombrarVideosRequest.videosRenombrados.Add(new string[] { nombreOriginal, fi.Name });

                                        }
                                        catch (IOException ex)
                                        {
                                            if (ex.Message == "No se puede crear un archivo que ya existe.\r\n")
                                                try
                                                {
                                                    File.SetAttributes(fi.FullName, FileAttributes.Normal);
                                                    fi.Delete();
                                                    MainWindow.IOLogger.insertar(new LogIO(Recursos.LOG_TIPO_BORRADO_OK, Mensajes.FICHERO_BORRADO_OK, fi));
                                                    renombrarVideosRequest.erroresRenombrando.Add("(Borrado) " + nombreOriginal);

                                                }
                                                catch (Exception)
                                                {
                                                    MainWindow.ErrorLogger.insertar(new LogIO(Recursos.LOG_TIPO_ERROR_BORRANDO, Mensajes.errorBorrandoFichero(ex), fi));
                                                    renombrarVideosRequest.erroresRenombrando.Add("No se pudo borrar " + nombreOriginal);
                                                }

                                        }
                                        catch (Exception ex)
                                        {
                                            MainWindow.ErrorLogger.insertar(new LogIO(Recursos.LOG_TIPO_ERROR_RENOMBRADO, Mensajes.errorRenombrandoFichero(ex), fi));
                                            renombrarVideosRequest.erroresRenombrando.Add("Error renombrando: " + nombreOriginal);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            renombrarVideosRequest.tiempoTranscurrido = (int)tiempo.ElapsedMilliseconds;

            return renombrarVideosRequest;
        }

        public static UltimoFicheroRequest getUltimoFichero(Serie item)
        {
            string directorioSerie = MainWindow.config.directorioSeries + @"/" + item.titulo;
            try
            {
                //obtiene los directorios dentro de la carpeta de la serie
                FileInfo finfo = new DirectoryInfo(directorioSerie).GetDirectories().Last().GetFiles().Last();
                //string temporada = finfo.Name.Substring((finfo.Name.Length - 9), 2).Trim();
                int temporada = Convert.ToInt32(finfo.Name.Substring((finfo.Name.Length - 9), 2).Trim());
                int capitulo = Convert.ToInt32(finfo.Name.Substring((finfo.Name.Length - 6), 2).Trim());
                return new UltimoFicheroRequest { temporada = temporada, capitulo = capitulo };
            }
            catch
            {
                //series sin carpeta en A:/series
                return null;
            }

        }


        #region Private clases

        /// <summary>
        /// Funcion recursiva que retorna todos los ficheros del directorio parametrizado
        /// </summary>
        /// <param name="filesInfo">Directorio raiz</param>
        /// <returns> Lista de ficheros y subficheros</returns>
        static private List<FileInfo> listarFicheros(FileSystemInfo[] filesInfo)
        {
            List<FileInfo> retorno = new List<FileInfo>();

            foreach (FileSystemInfo item in filesInfo)
            {
                if (item is DirectoryInfo)
                {
                    DirectoryInfo dInfo = (DirectoryInfo)item;
                    retorno.AddRange(listarFicheros(dInfo.GetFileSystemInfos()));
                }
                else if (item is FileInfo)
                {
                    retorno.Add((FileInfo)item);
                }
            }
            return retorno;
        }

        /// <summary>
        /// Maneja el fichero parametrizado, borrandolo o moviendolo segun convenga
        /// </summary>
        /// <param name="fichero">Fichero a gestionar</param>
        private static void manejarFichero(RecorrerTorrentRequest recorrerTorrentRequest, FileInfo fichero)
        {
            if (fichero.Extension == ".txt" || fichero.Extension == ".!ut" || fichero.Extension == ".url" || fichero.Extension == ".jpg" || fichero.Extension == ".wmv")
            {
                try
                {
                    File.SetAttributes(fichero.FullName, FileAttributes.Normal);
                    fichero.Delete();
                    recorrerTorrentRequest.ficherosBorrados.Add(fichero.Name);
                    MainWindow.IOLogger.insertar(new LogIO(Recursos.LOG_TIPO_BORRADO_OK, Mensajes.FICHERO_BORRADO_OK, fichero));
                }
                catch (Exception ex)
                {
                    recorrerTorrentRequest.videosMovidos.Add(fichero.Name);
                    MainWindow.ErrorLogger.insertar(new LogIO(Recursos.LOG_TIPO_ERROR_BORRANDO, Mensajes.errorBorrandoFichero(ex), fichero));
                }
            }
            else if (fichero.Extension == ".mp4" || fichero.Extension == ".mkv" || fichero.Extension == ".avi")
            {
                string pathDestino = MainWindow.config.directorioTrabajo + @"\" + fichero.Name;
                try
                {
                    File.SetAttributes(fichero.FullName, FileAttributes.Normal);
                    fichero.MoveTo(pathDestino);
                    recorrerTorrentRequest.videosMovidos.Add(fichero.Name);
                    MainWindow.IOLogger.insertar(new LogIO(Recursos.LOG_TIPO_MOVIDO_OK, Mensajes.FICHERO_MOVIDO_OK, fichero));
                }
                catch (Exception ex)
                {
                    recorrerTorrentRequest.videosMovidos.Add(fichero.Name);
                    MainWindow.ErrorLogger.insertar(new LogIO(Recursos.LOG_TIPO_ERROR_MOVIENDO, Mensajes.ErrorMoviendoFichero(ex), fichero));
                }
            }
            else
            {
                MessageBox.Show(Mensajes.ExtensionNoRegistrada(fichero.Name));
            }
        }

        /// <summary>
        /// Funcion recursiva que borra directorios y subdirectorios vacios.
        /// </summary>
        /// <param name="dir">Directorio vacio</param>
        /// <returns>Numero de directirios borados</returns>
        static private int borrarDirectoriosVacios(string dir)
        {
            int retorno = 0;
            try
            {
                foreach (var d in Directory.EnumerateDirectories(dir))
                {
                    retorno += borrarDirectoriosVacios(d);
                }
                var entries = Directory.EnumerateFileSystemEntries(dir);

                if (!entries.Any())
                {
                    try
                    {
                        Directory.Delete(dir);
                        retorno++;
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (DirectoryNotFoundException) { }
                }
            }
            catch (UnauthorizedAccessException) { }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(dir);
            }
            return retorno;
        }

        private static FileInfo obtenerCoincidenciaBusqueda(string pat)
        {
            DirectoryInfo iomegaInfo = new DirectoryInfo(MainWindow.config.directorioTrabajo);
            FileSystemInfo[] fsi;
            fsi = iomegaInfo.GetFileSystemInfos(pat);
            if (fsi.Length == 1 && fsi[0] is FileInfo)
            {
                return (FileInfo)fsi[0];
            }
            return null;
        }
        #endregion
    }
}
