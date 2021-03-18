using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WeatherService service;
        private CancellationTokenSource tokenSource;
        public MainWindow()
        {
            InitializeComponent();
            service = new WeatherService();
            tokenSource = new CancellationTokenSource();
            tb_villes.Text = "Paris;Montpellier;Toulouse;Lyon;Bordeaux;Nantes";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            btn_get.IsEnabled = false;
            btn_cancel.IsEnabled = true;
            gd_results.Items.Clear();
            progress.IsIndeterminate = true;
            progress.Visibility = Visibility.Visible;

            var towns = tb_villes.Text.Split(';');
            tokenSource = new CancellationTokenSource();

            try
            {
                var weathers = new ConcurrentDictionary<string, List<WeatherForecast>>();

                var getWeathersTask = Task.Factory.StartNew(() =>
                {
                    foreach (var town in towns)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            var result = service.GetWeatherSync(town);
                            weathers.AddOrUpdate(town, _ => result, (_, __) => result);
                        }, TaskCreationOptions.AttachedToParent);
                    }
                }, TaskCreationOptions.None);

                await getWeathersTask;

                foreach (var result in weathers)
                {
                    gd_results.Items.Add(new ResultRow
                    {
                        Town = result.Key,
                        JPlus1 = result.Value[0].TemperatureC.ToString(),
                        JPlus2 = result.Value[1].TemperatureC.ToString(),
                        JPlus3 = result.Value[2].TemperatureC.ToString(),
                        JPlus4 = result.Value[3].TemperatureC.ToString(),
                        JPlus5 = result.Value[4].TemperatureC.ToString()
                    });
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception)
            {
                // TODO log
            }
            finally
            {
                ResetGUI();
            }

        }

        private void ResetGUI()
        {
            progress.IsIndeterminate = false;
            progress.Visibility = Visibility.Hidden;
            btn_get.IsEnabled = true;
            btn_cancel.IsEnabled = false;
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            tokenSource.Cancel();
            ResetGUI();
        }
    }
}
