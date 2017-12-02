using DS18B201WireLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Async;

namespace TurtleBay.Model
{
    internal class DataAdvancedModel :
#if ARM
        DataModel
#else
        DataTestModel
#endif
    {
        private static readonly AsyncLock m_lock = new AsyncLock();

        /// <summary>
        /// Liefert oder setzt die letzte Stunde
        /// </summary>
        private int _lastHour;

        /// <summary>
        /// Liefert oder setzt die Zeit des letzen Aufruf der Updatefunktion
        /// </summary>
        private DateTime _lastUpdate;

        private TimeSpan _heatingCount;
        private TimeSpan _lightingCount;

        private DispatcherTimer _timer;

        private DateTime _start = DateTime.Now;

        /// <summary>
        /// Zeitspanne beim Auftreten eines Fehlers
        /// </summary>
        private TimeSpan _diagnosticErrorTimeSpan = new TimeSpan();
        
        /// <summary>
        /// Konstruktor
        /// </summary>
        public DataAdvancedModel()
        {
            NightMin = 10;
            DayMin = 15;
            Max = 19;
            From = 9; // Uhr
            Till = 16; // Uhr
            DayFrom = 7; // Uhr
            DayTill = 20; // Uhr
  
            Chart24h = new ObservableCollection<ChartData>();

            _lastHour = DateTime.Now.Hour;
            _lastUpdate = DateTime.Now;
        }

        /// <summary>
        /// Initialfunktion
        /// </summary>
        public override async void InitAsync()
        {
            using (await m_lock.LockAsync())
            {
                try
                {
                    base.InitAsync();
                    var now = DateTime.Now;
                    var time = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
                    time = time.AddHours(-24);

                    for (int i = 24; i >= 0; i--)
                    {
                        Chart24h.Add(new ChartData()
                        {
                            Time = time,
                            Temperature = ApplicationData.Current.RoamingSettings.Values["h" + i] != null ? Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["h" + i]) : 0,
                            HeatingCount = ApplicationData.Current.RoamingSettings.Values["heatingCount" + i] != null ? Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["heatingCount" + i]) : 0,
                            LightingCount = ApplicationData.Current.RoamingSettings.Values["lightingCount" + i] != null ? Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["lightingCount" + i]) : 0
                        });

                        time = time.AddHours(1);
                    }

                    UpdateAsync();
                }
                catch (Exception ex)
                {
                    Error = "Fehler bei der Initialisierung" + Environment.NewLine + ex.Message;
                }
            }

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => 
            {
                NotifyPropertyChanged("Now");
                NotifyPropertyChanged("ProgramCounter");
                if (!string.IsNullOrWhiteSpace(Error))
                {
                    Status = !Status;
                }
            };
            _timer.Start();
        }

        /// <summary>
        /// Updatefunktion
        /// </summary>
        public override async void UpdateAsync()
        {
            var now = DateTime.Now;
            Error = string.Empty;

            using (await m_lock.LockAsync())
            {
                try
                {
                    base.UpdateAsync();

                    var f = new DateTime(now.Year, now.Month, now.Day, From, 0, 0);
                    var t = new DateTime(now.Year, now.Month, now.Day, Till, 0, 0);
                    var lightTime = f <= now && now <= t;

                    f = new DateTime(now.Year, now.Month, now.Day, From2, 0, 0);
                    t = new DateTime(now.Year, now.Month, now.Day, Till2, 0, 0);
                    lightTime = lightTime || f <= now && now <= t;

                    f = new DateTime(now.Year, now.Month, now.Day, DayFrom, 0, 0);
                    t = new DateTime(now.Year, now.Month, now.Day, DayTill, 0, 0);
                    var day = f <= now && now <= t;

                    var min = day ? DayMin : NightMin;

                    Lighting = lightTime && Temperature < Max;
                    Heating = Temperature < min;

                    // Charting
                    if (now.Hour != _lastHour)
                    {
                        _lastHour = now.Hour;

                        ApplicationData.Current.RoamingSettings.Values["heatingCount0"] = (int)Math.Round(_heatingCount.TotalMinutes);
                        ApplicationData.Current.RoamingSettings.Values["lightingCount0"] = (int)Math.Round(_lightingCount.TotalMinutes);

                        for (int i = 24; i > 0; i--)
                        {
                            ApplicationData.Current.RoamingSettings.Values["h" + i] = ApplicationData.Current.RoamingSettings.Values["h" + (i - 1)];
                            ApplicationData.Current.RoamingSettings.Values["heatingCount" + i] = ApplicationData.Current.RoamingSettings.Values["heatingCount" + (i - 1)];
                            ApplicationData.Current.RoamingSettings.Values["lightingCount" + i] = ApplicationData.Current.RoamingSettings.Values["lightingCount" + (i - 1)];
                        }

                        ApplicationData.Current.RoamingSettings.Values["h0"] = Temperature;
                        ApplicationData.Current.RoamingSettings.Values["heatingCount0"] = 0;
                        ApplicationData.Current.RoamingSettings.Values["lightingCount0"] = 0;

                        if (_lastHour == 3 && now.DayOfWeek == DayOfWeek.Friday)
                        {
                            Reboot();
                        }

                        if (Chart24h.Count > 24)
                        {
                            Chart24h.RemoveAt(0);
                        }

                        var temp = Convert.ToInt32(Temperature);
                        Chart24h.Add(new ChartData()
                        {
                            Time = now,
                            Temperature = temp,
                            HeatingCount = (int)Math.Round(_heatingCount.TotalMinutes),
                            LightingCount = (int)Math.Round(_lightingCount.TotalMinutes)
                        });

                        _heatingCount = new TimeSpan();
                        _lightingCount = new TimeSpan();
                    }

                    // Counting
                    _heatingCount += Heating ? now - _lastUpdate : new TimeSpan();
                    _lightingCount += Lighting ? now - _lastUpdate : new TimeSpan();

                    HeatingCounter += Heating ? now - _lastUpdate : new TimeSpan();
                    LightingCounter += Lighting ? now - _lastUpdate : new TimeSpan();

                    Status = !Status;  
                }
                catch (Exception ex)
                {
                    Error = "Fehler beim Update" + Environment.NewLine + ex.Message;
                }

                // Fehlerdiagnose und Selbstreperatur
                if (_diagnosticErrorTimeSpan.TotalMinutes > 10)
                {
                    Reboot();
                    _diagnosticErrorTimeSpan = new TimeSpan();
                }

                if (!string.IsNullOrEmpty(Error))
                {
                    _diagnosticErrorTimeSpan += now - _lastUpdate;
                }
                else
                {
                    _diagnosticErrorTimeSpan = new TimeSpan();
                }
                
                _lastUpdate = DateTime.Now;
            }
        }

        /// <summary>
        /// Neustart des Raspberry PI
        /// </summary>
        public void Reboot()
        {
            Info = "Neustart des Raspberry PI erfolgt...";
            try
            {
                // siehe http://code-13.net/Content/Blog/1252
                ShutdownManager.BeginShutdown(ShutdownKind.Restart, TimeSpan.FromSeconds(30));
            }
            catch
            {
                Error = "Neustart nicht erfolgreich";
            }
        }

        /// <summary>
        /// Liefert oder setzt die Temperatur
        /// </summary>
        public override double Temperature
        {
            get { return base.Temperature; }
            set
            {
                base.Temperature = value;

                NotifyPropertyChanged("TemperatureStr");
                NotifyPropertyChanged("TemperatureBrush");
            }
        }

        /// <summary>
        /// Liefert die Temperatur als String
        /// </summary>
        public string TemperatureStr
        {
            get
            {
                return string.Format
                (
                    "{0} °C",
                    Math.Round(Temperature, 1).ToString()
                );
            }
        }

        /// <summary>
        /// Liefert die Farbe in Abhängigkeit der Temperatur
        /// </summary>
        public Brush TemperatureBrush
        {
            get
            {
                if (Temperature < NightMin || Temperature > Max)
                {
                    return new SolidColorBrush(Colors.SaddleBrown);
                }
                else
                {
                    return new SolidColorBrush(Colors.CornflowerBlue);
                }
            }
        }
        
        /// <summary>
        /// Liefert den Status der UVB-Lampe als String
        /// </summary>
        public string LightingStr
        {
            get
            {
                try
                {
                    return Lighting ? "An" : "Aus";
                }
                catch (Exception ex)
                {
                    Error = "Status des Scheinwerfers konnte nicht ermittelt werden" + Environment.NewLine + 
                                ex.Message;

                    return "?";
                }
            }
        }

        /// <summary>
        /// Liefert die Farbe in Abhängigkeit ob die UVB-Lampe eingeschaltet ist
        /// </summary>
        public Brush LightingBrush
        {
            get
            {
                try
                {
                    return new SolidColorBrush
                    (
                        Lighting ? Colors.Goldenrod : Colors.Gray
                    );
                }
                catch (Exception ex)
                {
                    Error = "Status des Scheinwerfers konnte nicht ermittelt werden" + Environment.NewLine + ex.Message;

                    return new SolidColorBrush(Colors.Red);
                }
            }
        }

        /// <summary>
        /// Liefert den Status der Heizlampe als String
        /// </summary>
        public string HeatingStr
        {
            get
            {
                try
                {
                    return Heating ? "An" : "Aus";
                }
                catch (Exception ex)
                {
                    Error = "Status des Infarotstrahlers konnte nicht ermittelt werden" + Environment.NewLine + 
                                ex.Message;

                    return "?";
                }
            }
        }

        /// <summary>
        /// Liefert die Farbe in Abhängigkeit ob die Heizlampe eingeschaltet ist
        /// </summary>
        public Brush HeatingBrush
        {
            get
            {
                try
                {
                    return new SolidColorBrush
                    (
                        Heating ? Colors.Red : Colors.Gray
                    );
                }
                catch (Exception ex)
                {
                    Error = "Status des Infarotstrahlers konnte nicht ermittelt werden" + Environment.NewLine + ex.Message;

                    return new SolidColorBrush(Colors.Red);
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt den Zähler für die Heizung
        /// </summary>
        public TimeSpan HeatingCounter
        {
            get
            {
                try
                {
                    var ts = TimeSpan.Parse(ApplicationData.Current.RoamingSettings.Values["heatingcounter"].ToString());
                    return ts;
                }
                catch
                {
                    return new TimeSpan();
                }
            }
            set
            {
                if (HeatingCounter != value)
                {
                    ApplicationData.Current.RoamingSettings.Values["heatingcounter"] = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt den Zähler für die UVB-Lampe
        /// </summary>
        public TimeSpan LightingCounter
        {
            get
            {
                try
                {
                    var ts = TimeSpan.Parse(ApplicationData.Current.RoamingSettings.Values["lightingcounter"].ToString());
                    return ts;
                }
                catch
                {
                    return new TimeSpan();
                }
            }
            set
            {
                if (LightingCounter != value)
                {
                    ApplicationData.Current.RoamingSettings.Values["lightingcounter"] = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Liefert die aktuelle Zeit
        /// </summary>
        public string Now
        {
            get { return DateTime.Now.ToString("dd.MM.yyyy" + Environment.NewLine + "HH:mm:ss"); }
        }

        /// <summary>
        /// Liefert oder setzt ob die Status-LED angeschaltet ist
        /// </summary>
        public override bool Status
        {
            get
            {
                return base.Status;
            }
            set
            {
                base.Status = value;
            }
        }

        /// <summary>
        /// Liefert oder setzt ob die UVB_Lampe angeschaltet ist
        /// </summary>
        public override bool Lighting
        {
            get
            {
                return base.Lighting;
            }
            set
            {
                base.Lighting = value;
                NotifyPropertyChanged("LightingStr");
                NotifyPropertyChanged("LightingBrush");
            }
        }

        /// <summary>
        /// Liefert oder setzt ob die Heizlampe angeschaltet ist
        /// </summary>
        public override bool Heating
        {
            get
            {
                return base.Heating;
            }
            set
            {
                base.Heating = value;
                NotifyPropertyChanged("HeatingStr");
                NotifyPropertyChanged("HeatingBrush");
            }
        }

        /// <summary>
        /// Liefert die vergagene Zeit seit dem Programmstart
        /// </summary>
        public TimeSpan ProgramCounter
        {
            get
            {
                return DateTime.Now - _start;
            }
        }

        /// <summary>
        /// Liefert oder setzt die Diagrammdaten
        /// </summary>
        public ObservableCollection<ChartData> Chart24h { get; set; }
    }
}
