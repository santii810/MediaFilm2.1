using MediaFilm2._1.Modelo;
using MediaFilm2._1.Modelo.Response;
using MediaFilm2._1.Vista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediaFilm2._1.Controlador
{
    class GestorDescargas
    {
        public void buscarDescargas()
        {
            List<Serie> series = MainWindow.SeriesXML.leerXML();
            foreach (Serie serie in series)
            {
                UltimoFicheroResponse ultimoFicheroRequest = GestorVideos.getUltimoFichero(serie);
                if (ultimoFicheroRequest != null)
                {
                    List<string> pruebas = new List<string>();
                    //prueba con capitulo++
                    if (ultimoFicheroRequest.capitulo < 9)
                    {
                        pruebas.Add("http://www.mejortorrent.com/uploads/torrents/series/" + serie.tituloDescarga + "_" + ultimoFicheroRequest.temporada + "_0" + (ultimoFicheroRequest.capitulo + 1) + ".torrent");
                        pruebas.Add("http://www.mejortorrent.com/uploads/torrents/series/" + serie.tituloDescarga + "_" + ultimoFicheroRequest.temporada + "_720p_0" + (ultimoFicheroRequest.capitulo + 1) + ".torrent");
                        pruebas.Add("http://www.mejortorrent.com/uploads/torrents/series/" + serie.tituloDescarga + "_" + ultimoFicheroRequest.temporada + "_1080p_0" + (ultimoFicheroRequest.capitulo + 1) + ".torrent");

                    }
                    else
                    {
                        pruebas.Add("http://www.mejortorrent.com/uploads/torrents/series/" + serie.tituloDescarga + "_" + ultimoFicheroRequest.temporada + "_" + (ultimoFicheroRequest.capitulo + 1) + ".torrent");
                        pruebas.Add("http://www.mejortorrent.com/uploads/torrents/series/" + serie.tituloDescarga + "_" + ultimoFicheroRequest.temporada + "_720p_" + (ultimoFicheroRequest.capitulo + 1) + ".torrent");
                        pruebas.Add("http://www.mejortorrent.com/uploads/torrents/series/" + serie.tituloDescarga + "_" + ultimoFicheroRequest.temporada + "_1080p_" + (ultimoFicheroRequest.capitulo + 1) + ".torrent");

                    }

                    foreach (string url in pruebas)
                    {
                        if (RemoteFileExists(url))
                        {
                            //  mainWindow.listaFicherosDescargar.Children.Add(CrearVistas.getFicheroDescargar((serie.titulo + " " + temp + "x" + cap), url));
                        }
                    }

                    //        ejemplos
                    // http://www.mejortorrent.com/uploads/torrents/series/Los_100_3_09.torrent
                    //  http://www.mejortorrent.com/uploads/torrents/series/Angie_Tribeca_1_720p_01.torrent
                    //http://www.mejortorrent.com/uploads/torrents/series/Gotham_2_01.torrent


                }
            }
        }


        private static bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }
    }
}
