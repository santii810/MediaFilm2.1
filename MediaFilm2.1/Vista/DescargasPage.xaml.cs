﻿using System;
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
    /// Lógica de interacción para DescargasPage.xaml
    /// </summary>
    public partial class DescargasPage : Page
    {


        private bool enEjecucion = false;


        public DescargasPage()
        {
            InitializeComponent();
        }

        private void ImageIniciarDescarga_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!enEjecucion)
            {
                enEjecucion = true;
                this.Cursor = Cursors.Wait;






                enEjecucion = false;
                this.Cursor = Cursors.Arrow;
            }
        }
        private void ImageIniciarDescarga_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!enEjecucion)
                this.Cursor = Cursors.Hand;
        }
        private void ImageIniciarDescarga_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!enEjecucion)
                this.Cursor = Cursors.Arrow;
        }
    }
}
