using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Common;

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
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            btn_get.IsEnabled = false;
            btn_cancel.IsEnabled = true;
            gd_results.Items.Clear();
            var towns = tb_villes.Text.Split(';');
            progress.IsIndeterminate = true;
            progress.Visibility = Visibility.Visible;

            try
            {
                tokenSource = new CancellationTokenSource();
                tokenSource.CancelAfter(TimeSpan.FromSeconds(5));
                await Task.Run(() =>
                {
                    Parallel.ForEach(towns, new ParallelOptions { CancellationToken = tokenSource.Token }, town =>
                        {
                            var item = service.GetWeatherSync(town);
                            Dispatcher.Invoke(() =>
                            {
                                gd_results.Items.Add(new ResultRow
                                {
                                    Town = town,
                                    JPlus1 = item[0].TemperatureC.ToString(),
                                    JPlus2 = item[1].TemperatureC.ToString(),
                                    JPlus3 = item[2].TemperatureC.ToString(),
                                    JPlus4 = item[3].TemperatureC.ToString(),
                                    JPlus5 = item[4].TemperatureC.ToString()
                                });
                            });
                        });
                });
           
            }
            catch (OperationCanceledException exception)
            {
                Console.WriteLine(exception);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
            finally
            {
                resetGui();
            }

            
        }

        private void resetGui()
        {
            progress.IsIndeterminate = false;
            progress.Visibility = Visibility.Hidden;
            btn_get.IsEnabled = true;
            btn_cancel.IsEnabled = false;
        }

        private void Btn_cancel_OnClick(object sender, RoutedEventArgs e)
        {
            tokenSource.Cancel();
            resetGui();
        }
    }
}
