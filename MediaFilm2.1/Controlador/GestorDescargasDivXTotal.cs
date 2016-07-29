using HtmlAgilityPack;
using MediaFilm2._1.Modelo;
using MediaFilm2._1.Modelo.Response;
using MediaFilm2._1.Vista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MediaFilm2._1.Controlador
{
    class GestorDescargasDivXTotal
    {
        public string website = "http://www.divxtotal.com";
        public List<Serie> seriesDivX = new List<Serie>();


        public async Task  ParsearListaSeries()
        {
            string url = website + "/series/";


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
                    seriesDivX.Add(new Serie { tituloDivXTotal = iteradorSerie.Attributes["title"].Value.ToString(), href_divX = iteradorSerie.Attributes["href"].Value.ToString() });
                }
            }
        }

    }
}
