using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace ClimaYahoo_Actualizado
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*private localhost.WSMeteorologico wsm;*/
        //private conversor.CurrencyConvertor conv = new conversor.CurrencyConvertor();
        //private conversor.Currency fromDolar = conversor.Currency.USD;
       // private conversor.Currency fromEuro = conversor.Currency.EUR;
        //private conversor.Currency to = conversor.Currency.CLP;

        public MainWindow()
        {
            InitializeComponent();
            this.Left = SystemParameters.PrimaryScreenWidth - this.Width;
            this.Top = 0;
            Load_Clima();

        }
        //METODO QUE CARGARA CLIMA
        public void Load_Clima()
        {
            try
            {
                // double dolar = this.conv.ConversionRate(fromDolar, to);
                //double euro = this.conv.ConversionRate(fromEuro, to);

                var woeid = "349871";
                string results = "";

                using (WebClient wc = new WebClient())
                {
                    String url = String.Format("https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%3D{0}&format=json",woeid);
                    results = wc.DownloadString(url);
                }

                dynamic jo = Newtonsoft.Json.Linq.JObject.Parse(results);

                var items       = jo.query.results.channel.item.condition;
                var citys       = jo.query.results.channel.location;
                var images      = jo.query.results.channel.item.description;
                var atmos       = jo.query.results.channel.atmosphere;
                var astro       = jo.query.results.channel.astronomy;
                var temperatura = items.temp;
                var cuidad      = citys.city;
                var pais        = citys.country;

                int temp = (int)((double)temperatura - 32) * 5 / 9;

                this.lCity.Content = string.Format("{0}, {1}", cuidad, pais);
                this.Label1.Content += string.Format("\nHumedad  {0} g/m3", atmos.humidity);
                this.Label1.Content += string.Format("\nPresion Atmos.  {0} ", atmos.pressure);
                this.Label2.Content += string.Format("\nAmanacer.  {0} ", astro.sunrise);
                this.Label2.Content += string.Format("\nOcaso  {0} ", astro.sunset);
                //this.Label2.Content = string.Format("Dolar: {0} CLP\nEuro: {1}", dolar, euro);
                this.lTemperature.Content = string.Format("{0}º {1}", temp, "C");             
                                                                               
                int inicio = images.ToString().IndexOf("http");
                String aux = images.ToString().Substring(inicio, 200);
                int fin = aux.IndexOf('"');
                String img = images.ToString().Substring(inicio,fin);
                //Console.WriteLine(img);
                this.lImageTiempo.Source = new BitmapImage(new Uri(img));
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Verifique que esta conectado a internet", "Alerta", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();//SE PUEDE MOVER LA PANTALLA
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error move()", ex.Message);
            }
        }

        private void lbl_cerrar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
