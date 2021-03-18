using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WeatherService service;
        public MainWindow()
        {
            InitializeComponent();
            service = new WeatherService();
        }

        private  async  void Button_Click(object sender, RoutedEventArgs e)
        {
            progress.IsIndeterminate = true;
            progress.Visibility = Visibility.Visible;
            //tb_result.Text = await service.getWeatherResult();
            tb_result.Text = await Task.Run(()=>  service.getWeatherResult()); //Passer une méthode sychnrone(getWeatherresult() ) en une méthode asychrone task.Run
            progress.IsIndeterminate = false;
            progress.Visibility = Visibility.Hidden;
        }
    }
}
