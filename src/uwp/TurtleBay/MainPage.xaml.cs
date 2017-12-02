using TurtleBay.Model;
using DS18B201WireLib;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using System.Xml.Linq;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml.Data;

namespace TurtleBay
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer _timer;
        private bool _inprog = false;
        DataAdvancedModel _data;
        LinearAxis _axis;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            _data = new DataAdvancedModel();
            this.DataContext = _data;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(15);
            _timer.Tick += OnTimerTick;           
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Seite geladen wurde
        /// </summary>
        /// <param name="sender">Der Sender des Events</param>
        /// <param name="e">Eventargumente</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _data.InitAsync();

            OnResetSettings(sender, e);
            _timer.Start();
        }

        /// <summary>
        /// Wird aufgerufen, wenn der Timer auslöst
        /// </summary>
        /// <param name="sender">Der Sender des Events</param>
        /// <param name="e">Eventargumente</param>
        private void OnTimerTick(object sender, object e)
        {
            if (!_inprog)
            {
                _inprog = true;
                _data.UpdateAsync();
                
                _inprog = false;
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn das ChartControl geladen wurde
        /// </summary>
        /// <param name="sender">Der Sender des Events</param>
        /// <param name="e">Eventargumente</param>
        private void OnChartLoaded(object sender, RoutedEventArgs e)
        {
            var chart = (Chart)sender;
            var tempSeries = chart.Series[0] as AreaSeries;
            var heatingSeries = chart.Series[1] as LineSeries;
            var lightingSeries = chart.Series[2] as LineSeries;

            tempSeries.ItemsSource = _data.Chart24h;
            heatingSeries.ItemsSource = _data.Chart24h;
            lightingSeries.ItemsSource = _data.Chart24h;

            _axis = new LinearAxis()
            {
                Maximum = 60,
                Minimum = 0,
                Orientation = AxisOrientation.Y,
                Interval = 10,
                ShowGridLines = false,
                Location = AxisLocation.Left
            };

            tempSeries.DependentRangeAxis = _axis;
            heatingSeries.DependentRangeAxis = _axis;
            lightingSeries.DependentRangeAxis = _axis;
        }

        /// <summary>
        /// Wird aufgerufen, wenn das Speichern der Einstellungen erfolgen soll
        /// </summary>
        /// <param name="sender">Der Auslöser</param>
        /// <param name="e">Eventparameter</param>
        private void OnSaveSettings(object sender, RoutedEventArgs e)
        {
            // Konfiguration speichern
            ApplicationData.Current.RoamingSettings.Values["nightmin"] = _data.NightMin;
            ApplicationData.Current.RoamingSettings.Values["daymin"] = _data.DayMin;
            ApplicationData.Current.RoamingSettings.Values["max"] = _data.Max;
            ApplicationData.Current.RoamingSettings.Values["from"] = _data.From;
            ApplicationData.Current.RoamingSettings.Values["till"] = _data.Till;
            ApplicationData.Current.RoamingSettings.Values["from2"] = _data.From2;
            ApplicationData.Current.RoamingSettings.Values["till2"] = _data.Till2;
            ApplicationData.Current.RoamingSettings.Values["dayfrom"] = _data.DayFrom;
            ApplicationData.Current.RoamingSettings.Values["daytill"] = _data.DayTill;
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Einstellungen zurückgesetzt werden sollen
        /// </summary>
        /// <param name="sender">Der Auslöser</param>
        /// <param name="e">Eventparameter</param>
        private void OnResetSettings(object sender, RoutedEventArgs e)
        {
            // Konfiguration laden
            _data.NightMin = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["nightmin"]);
            _data.DayMin = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["daymin"]);
            _data.Max = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["max"]);
            _data.From = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["from"]);
            _data.Till = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["till"]);
            _data.From2 = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["from2"]);
            _data.Till2 = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["till2"]);
            _data.DayFrom = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["dayfrom"]);
            _data.DayTill = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["daytill"]);
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Zähler zurückgesetzt werden sollen
        /// </summary>
        /// <param name="sender">Der Auslöser</param>
        /// <param name="e">Eventparameter</param>
        private void OnReset(object sender, RoutedEventArgs e)
        {
            _data.HeatingCounter = new TimeSpan();
            _data.LightingCounter = new TimeSpan();
        }

        /// <summary>
        /// Wird aufgerufen, wenn ein Neustart erfolgen soll
        /// </summary>
        /// <param name="sender">Der Auslöser</param>
        /// <param name="e">Eventparameter</param>
        private void OnReboot(object sender, RoutedEventArgs e)
        {
            _data.Reboot();
        }

        /// <summary>
        /// Wird aufgerufen sich die Maximaltemperatur ändert
        /// </summary>
        /// <param name="sender">Der Auslöser</param>
        /// <param name="e">Eventparameter</param>
        private void OnMaxChange(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            
        }
    }
}
