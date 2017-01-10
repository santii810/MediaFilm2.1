using HtmlAgilityPack;
using MediaFilm2._1.Modelo;
using MediaFilm2._1.Modelo.Response;
using MediaFilm2._1.Vista;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediaFilm2._1.Controlador
{
    public class GestorDescargasDivXTotal
    {
        public string website = "http://www.divxtotal.com/";
        public List<Serie> seriesDivX = new List<Serie>();


        public async Task ParsearListaSeries()
        {
            string url = website + "series/";


            HttpClient http = new HttpClient();
            var response = await http.GetByteArrayAsync(url);
            String source = Encoding.GetEncoding("iso-8859-1").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            HtmlDocument resultat = new HtmlDocument();
            resultat.LoadHtml(source);

            List<HtmlNode> toftitle = resultat.DocumentNode.Descendants()
                .Where(x => x.Name == "ul"
                && x.Attributes["class"] != null
                && x.Attributes["class"].Value.Contains("ul_listadoseries")).ToList();

            // Obtengo todos los "li" que corresponden a cada uno de los cuadrados con la lista de las series
            foreach (HtmlNode iteradorCuadrado in toftitle[0].Descendants().ToList())
            {
                //obtengo cada uno de los href pertenecientes a cada una de las series
                foreach (HtmlNode iteradorSerie in iteradorCuadrado.Descendants("a").ToList())
                {
                    seriesDivX.Add(new Serie {/* tituloDivXTotal = iteradorSerie.Attributes["title"].Value.ToString(),*/ href_divX = iteradorSerie.Attributes["href"].Value.ToString() });
                }
            }
        }

        public async Task ParsingSerie(Serie serie)
        {
            string website = this.website + serie.href_divX;

            try
            {

                HttpClient http = new HttpClient();
                var response = await http.GetByteArrayAsync(website);
                String source = Encoding.GetEncoding("iso-8859-1").GetString(response, 0, response.Length - 1);
                source = WebUtility.HtmlDecode(source);
                HtmlDocument resultat = new HtmlDocument();
                resultat.LoadHtml(source);

                List<HtmlNode> toftitle = resultat.DocumentNode.Descendants()
                    .Where(x => x.Name == "td"
                    && x.Attributes["class"] != null
                    && x.Attributes["class"].Value.Contains("capitulonombre")).ToList();


                // Obtengo todos los "li" que corresponden a cada uno de los cuadrados con la lista de las series
                foreach (HtmlNode iteradorCuadrado in toftitle)
                {
                    //obtengo cada uno de los href pertenecientes a cada una de las series
                    foreach (HtmlNode iteradorSerie in iteradorCuadrado.Descendants("a").ToList())
                    {
                        // inserto el href de los capitulos en una lista de seriues en mainWindow
                        MainWindow.series.Where(i => serie.tituloLocal == i.tituloLocal).First().capitulos.Add(
                            new Capitulo { titulo = iteradorSerie.InnerText, href_divX = iteradorSerie.Attributes["href"].Value.ToString() });
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("HttpResponseException en serie  " + serie.tituloLocal);

            }
        }
        
        public void DescargarCapitulo(Capitulo capi)
        {
            //  anterior : string remoteUri = website + capi.href_divX;
            string remoteUri = capi.href_divX;
            string fileName = capi.titulo + ".torrent";
            // Create a new WebClient instance.
            WebClient myWebClient = new WebClient();
            // Concatenate the domain with the Web resource filename.
            myWebClient.DownloadFile(new Uri(remoteUri), fileName);

            File.Move(fileName, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Downloads/" + fileName);

        }
    }
}
