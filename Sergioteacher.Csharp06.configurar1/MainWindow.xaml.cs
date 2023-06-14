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
// Para poder usar config y PropertyInfo
using System.Configuration;
using System.Reflection;
// Para la lectura-escritura de .config
using System.IO;

namespace Sergioteacher.Csharp06.configurar1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string contenido_config;
        /// <summary>
        /// Inicializando cada uno de los componentes.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // Carga .config de forma manual
            contenido_config = LeerArchivo(@"config.txt");
            adorno3.Text = contenido_config;
            if (contenido_config == "")
            {
                contenido_config = "Red";
            }

            Color color_config = (Color)ColorConverter.ConvertFromString(contenido_config);
            adorno3.Background = new SolidColorBrush(color_config);
            if (contenido_config == "Blue")
            {
                BConf1.Content = "Desactiva Azul";
            }
            else
            {
                BConf1.Content = "Activa Azul";
            }


            // Usa los valores de App.config
            string backgroundSrt = ConfigurationManager.AppSettings["BackgroundTono"];
            Color backgroundCColor = (Color)ColorConverter.ConvertFromString(backgroundSrt);
            MiVentana.Background = new SolidColorBrush(backgroundCColor);

            string Adorno1srt = ConfigurationManager.AppSettings["Adorno1Tono"];
            string Adorno2srt = ConfigurationManager.AppSettings["Adorno2Tono"];
            Color Adorno1color = (Color)ColorConverter.ConvertFromString(Adorno1srt);
            adorno1.Background = new SolidColorBrush(Adorno1color);
            Color Adorno2color = (Color)ColorConverter.ConvertFromString(Adorno2srt);
            adorno2.Background = new SolidColorBrush(Adorno2color);

            // Carga datos al ComboBox de los posibles colores del sistema.
            Ccolor.ItemsSource = typeof(Colors).GetProperties();
        }

        /// <summary>
        /// Método para controlar los cambios de selección de COLOR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ccolor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Lee el valor seleccionado
            string colorselecionadoSrt = (Ccolor.SelectedItem as PropertyInfo).GetValue(Ccolor, null).ToString();
            adorno2.Text = colorselecionadoSrt;
            // actualiza el color de fondo, guardando la selección
            Color colorselecionado = (Color)(Ccolor.SelectedItem as PropertyInfo).GetValue(Ccolor, null);
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["BackgroundTono"].Value = colorselecionado + "";
            config.Save();
            // Carga ese valor, de lo contrario para ver los cambios tiene que ejecutarse otra vez.
            MiVentana.Background = new SolidColorBrush(colorselecionado); 
        }

        /// <summary>
        /// Controla de forma manual la activación-desactivación de 1 valor
        /// de una froma extremadamente simple
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BConf1_Click(object sender, RoutedEventArgs e)
        {
            //string contenido = LeerArchivo(@"config.txt");
            if ( contenido_config == "Blue")
            {
                BConf1.Content = "Activa Azul";
                contenido_config = "Red";
                Color color_config = (Color)ColorConverter.ConvertFromString(contenido_config);
                adorno3.Background = new SolidColorBrush(color_config);
            }
            else
            {
                BConf1.Content = "Desactiva Azul";
                contenido_config = "Blue";
                Color color_config = (Color)ColorConverter.ConvertFromString(contenido_config);
                adorno3.Background = new SolidColorBrush(color_config);
            }
            // Actualizando el ficheor .config
            EscribirEnArchivo(contenido_config, @"config.txt");
        }



        /// <summary>
        /// Usado para crear-actualizar el fichero de configuración
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="rutaArchivo"></param>
        public void EscribirEnArchivo(string texto, string rutaArchivo)
        {
            // Comprobamos si el archivo ya existe
            if (!File.Exists(rutaArchivo))
            {
                // Si no existe, lo creamos
                using (FileStream fs = File.Create(rutaArchivo)) { }
            }

            // Sobreescribimos el contenido del archivo con el nuevo texto
            File.WriteAllText(rutaArchivo, texto);
        }

        /// <summary>
        /// Usado para lerr el fichero de configuración
        /// </summary>
        /// <param name="rutaArchivo"></param>
        /// <returns></returns>
        public string LeerArchivo(string rutaArchivo)
        {
            // Comprobamos si el archivo existe
            if (!File.Exists(rutaArchivo))
            {
                // Creamos el archivo si no existe
                File.Create(rutaArchivo).Close();
            }

            // Leemos el contenido del archivo y lo devolvemos como un string
            return File.ReadAllText(rutaArchivo);
        }
    }
}
