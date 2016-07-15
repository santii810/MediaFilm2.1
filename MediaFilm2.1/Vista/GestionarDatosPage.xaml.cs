using MediaFilm2._1.Controlador;
using MediaFilm2._1.Modelo;
using MediaFilm2._1.Res;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Lógica de interacción para GestionarDatosPage.xaml
    /// </summary>
    public partial class GestionarDatosPage : Page
    {
        private List<Serie> series = new List<Serie>();
        internal Serie serieSeleccionada;

        private string filtro = "";

        public GestionarDatosPage()
        {
            InitializeComponent();
            UpdateUI.updateGestionarDatos(Codigos.ESTADO_INICIAL, this);

        }

        private void textBoxNumeroTemporadas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        /// <summary>
        /// Funcion que se lanza como manejador del boton de "seleccionar" adjunto a cada lista de la serie
        /// </summary>
        /// <param name="item">The item.</param>
        internal void seleccionarSerie(Serie serieSeleccionada)
        {
            this.serieSeleccionada = serieSeleccionada;
            this.serieSeleccionada.leerPatrones();
            this.panelListaPatronesActuales.Children.Clear();
            foreach (Patron itPatrones in this.serieSeleccionada.patrones)
            {
                this.panelListaPatronesActuales.Children.Add(CrearVistas.LabelLista(itPatrones.textoPatron));
            }
            UpdateUI.updateGestionarDatos(Codigos.GESTIONAR_DATOS_ADD_PATRON_SERIE_SELECCIONADA, this);
        }



        private void textBoxCapitulosTemporada_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void ImageAddSerie_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateUI.updateGestionarDatos(Codigos.GESTIONAR_DATOS_ADD_SERIE, this);
        }

        private void ImageAddPatron_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateUI.updateGestionarDatos(Codigos.GESTIONAR_DATOS_ADD_PATRON, this);
            this.panelSeleccionarSeriePatron.Children.Clear();
            series = MainWindow.SeriesXML.leerXML();
            series.Sort();
            foreach (Serie itSerie in series)
            {
                if (itSerie.estado == "A")
                    this.panelSeleccionarSeriePatron.Children.Add(CrearVistas.PanelSeleccionarSerie(itSerie, this));
            }

            this.panelFicherosARenombrar.Children.Clear();
            FileSystemInfo[] fsi = GestorVideos.getFicherosARenombrar();
            this.labelCantidadFicherosARenombrar.Content = (fsi.Length -1).ToString();
            foreach (FileInfo itFichero in fsi)
                if (itFichero.Extension.Equals(".mkv") || itFichero.Extension.Equals(".avi") || itFichero.Extension.Equals(".mp4"))
                    panelFicherosARenombrar.Children.Add(CrearVistas.LabelLista(itFichero.Name));

        }

        internal void circuloEstado_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Thread t1 = new Thread(imagenIO_CambiarEstado1_Handler);
            Thread t2 = new Thread(imagenIO_CambiarEstado2_Handler);

            //doble click
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
                t2.Start();
            else
                t1.Start();

            updatePanelIOSeries();
        }

        internal void imagenIO_CambiarEstado1_Handler()
        {

            Thread.Sleep(100);
            if (serieSeleccionada.estado == "A")
                serieSeleccionada.estado = "D";
            else if (serieSeleccionada.estado == "D")
                serieSeleccionada.estado = "A";
            MainWindow.SeriesXML.updateSerie(serieSeleccionada);


        }
        internal void imagenIO_CambiarEstado2_Handler()
        {
            if (serieSeleccionada.estado == "F")
                serieSeleccionada.estado = "A";
            else
                serieSeleccionada.estado = "F";
            MainWindow.SeriesXML.updateSerie(serieSeleccionada);
        }



        private void updatePanelIOSeries()
        {
            Thread.Sleep(200);
            series = MainWindow.SeriesXML.leerXML();
            series.Sort();

            this.panelListaIOSerie.Children.Clear();
            for (int i = 0; i < series.Count; i++)
            {
                if (series[i].titulo.ToLower().Contains(filtro.ToLower())) {
                this.panelListaIOSerie.Children.Add(CrearVistas.PanelEstadoSerie(series[i], this, i));
                }
            }
        }

        private void ImageIOSerie_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateUI.updateGestionarDatos(Codigos.GESTIONAR_DATOS_IO_SERIE, this);
            updatePanelIOSeries();

        }



        private void ImageIncTemp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateUI.updateGestionarDatos(Codigos.GESTIONAR_DATOS_TEMPORADAS, this);
            updatePanelTemporadas();

        }

        public void updatePanelTemporadas()
        {
            series = MainWindow.SeriesXML.leerXML();
            series.Sort();

            this.panelListaTemporadas.Children.Clear();
            for (int i = 0; i < series.Count; i++)
            {
                this.panelListaTemporadas.Children.Add(CrearVistas.PanelActualizarTemporadas(series[i], this, i));
            }
        }

        private void ButtonAñadirSerie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validarAddSerie())
                {
                    MainWindow.SeriesXML.insertar(new Serie
                    {
                        titulo = textBoxTitulo.Text.Trim(),
                        capitulosPorTemporada = Convert.ToInt32(textBoxCapitulosTemporada.Text.Trim()),
                        estado = "A",
                        numeroTemporadas = Convert.ToInt32(textBoxNumeroTemporadas.Text.Trim()),
                        temporadaActual = 1
                    });

                    MessageBox.Show(Mensajes.SERIE_ADDED_OK);
                    UpdateUI.updateGestionarDatos(Codigos.GESTIONAR_DATOS_ADD_SERIE_OK, this);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(Mensajes.SERIE_ADD_ERROR);
            }
        }



        private bool validarAddSerie()
        {
            if (textBoxTitulo.Text.Trim() == "")
            {
                MessageBox.Show(Mensajes.TITULO_SERIE_VACIO);
                return false;
            }
            if (textBoxTitulo.Text.Trim().Length < 3)
            {
                MessageBox.Show(Mensajes.TITULO_SERIE_NOK);
                return false;
            }
            if (this.textBoxCapitulosTemporada.Text.Trim() == "")
            {
                textBoxCapitulosTemporada.Text = "25";
            }
            if (textBoxNumeroTemporadas.Text.Trim() == "")
            {
                textBoxNumeroTemporadas.Text = "1";
            }
            return true;
        }

        private bool validarAddPatron()
        {
            if (textBoxNuevoPatron.Text.Trim() == "")
                return false;
            if (textBoxNuevoPatron.Text.Trim().Length < 2)
                return false;
            return true;
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void textBoxNuevoPatron_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonAddPatron_Click(new object(), new RoutedEventArgs());
            }
        }

        private void ButtonAddPatron_Click(object sender, RoutedEventArgs e)
        {
            if (validarAddPatron())
            {
                MainWindow.PatronesXML.insertar(new Patron { nombreSerie = serieSeleccionada.titulo, textoPatron = textBoxNuevoPatron.Text.Trim() });
                serieSeleccionada.leerPatrones();
                UpdateUI.updateGestionarDatos(Codigos.GESTIONAR_DATOS_ADD_PATRON_OK, this);
                seleccionarSerie(this.serieSeleccionada);
            }
            else
            {
                MessageBox.Show(Mensajes.PATRON_INVALIDO);
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.textBoxFiltrarSeries.Text = "";
        }

        private void textBoxFiltrarSeries_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxFiltrarSeries.Text))
            {
                textBoxFiltrarSeries.Text = "Filtrar...";
            }
        }
          

        private void textBoxFiltrarSeries_KeyUp(object sender, KeyEventArgs e)
        {
            filtro = textBoxFiltrarSeries.Text.ToString().Trim();
            updatePanelIOSeries();
        }
    }
}
