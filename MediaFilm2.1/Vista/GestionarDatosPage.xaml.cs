using MediaFilm2._1.Modelo;
using MediaFilm2._1.Res;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para GestionarDatosPage.xaml
    /// </summary>
    public partial class GestionarDatosPage : Page
    {
       

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

        }

        private void ImageIOSerie_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void ImageIncTemp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void ButtonAñadirSerie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validarAddSerie(this))
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



        private static bool validarAddSerie(GestionarDatosPage xaml)
        {
            if (xaml.textBoxTitulo.Text.Trim() == "")
            {
                MessageBox.Show(Mensajes.TITULO_SERIE_VACIO);
                return false;
            }
            if (xaml.textBoxTitulo.Text.Trim().Length < 3)
            {
                MessageBox.Show(Mensajes.TITULO_SERIE_NOK);
                return false;
            }
            if (xaml.textBoxCapitulosTemporada.Text.Trim() == "")
            {
                xaml.textBoxCapitulosTemporada.Text = "25";
            }
            if (xaml.textBoxNumeroTemporadas.Text.Trim() == "")
            {
                xaml.textBoxNumeroTemporadas.Text = "1";
            }
            return true;
        }

        private static bool validarAddPatron(GestionarDatosPage xaml)
        {
            //if (xaml.textBoxNuevoPatron.Text.Trim() == "")
            //    return false;
            //if (xaml.textBoxNuevoPatron.Text.Trim().Length < 2)
            //    return false;
            return true;
        }
    }
}
