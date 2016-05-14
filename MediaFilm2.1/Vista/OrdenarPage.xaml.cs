using MediaFilm2._1.Modelo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaFilm2._1.Vista
{
    /// <summary>
    /// Lógica de interacción para OrdenarPage.xaml
    /// </summary>
    public partial class OrdenarPage : Page
    {
        Configuracion config;

        public OrdenarPage()
        {
            InitializeComponent();


        }

        private void ImageRecogerVideos_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Stopwatch tiempo = Stopwatch.StartNew();

            DirectoryInfo dir = new DirectoryInfo(config.directorioTorrent);





        }
    }
}
