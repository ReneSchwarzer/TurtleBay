using DS18B201WireLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Windows.Devices.Gpio;
using Windows.Storage;
using Windows.System;

namespace TurtleBayNet.Plugin.Model
{
    public class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Die momentane Temperatur
        /// </summary>
        private double _temperature = 0;

        /// <summary>
        /// Minimale Nachttemperatur
        /// </summary>
        private int _nightMin;

        /// <summary>
        /// Minimale Tagestemperatur
        /// </summary>
        private int _dayMin;

        /// <summary>
        /// Maximale Temperatur
        /// </summary>
        private int _max;

        /// <summary>
        /// Startzeit der UVB-Lampe
        /// </summary>
        private int _from;

        /// <summary>
        /// Endzeit der UVB-Lampe
        /// </summary>
        private int _till;

        /// <summary>
        /// Startzeit der UVB-Lampe
        /// </summary>
        private int _from2;

        /// <summary>
        /// Endzeit der UVB-Lampe
        /// </summary>
        private int _till2;

        /// <summary>
        /// Endzeit des Tages
        /// </summary>
        private int _dayTill;

        /// <summary>
        /// Startzeit des Tages
        /// </summary>
        private int _dayFrom;

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

        private readonly DateTime _start = DateTime.Now;

        /// <summary>
        /// Zeitspanne beim Auftreten eines Fehlers
        /// </summary>
        private TimeSpan _diagnosticErrorTimeSpan = new TimeSpan();

        /// <summary>
        /// Der GPIO-Pin, welcher die StatusLED steuert
        /// </summary>
        private GpioPin _statusPin;

        /// <summary>
        /// Der GPIO-Pin, welcher die UVB-Lampe steuert
        /// </summary>
        private GpioPin _lightingPin;

        /// <summary>
        /// Der GPIO-Pin, welcher die Heizlampe steuert
        /// </summary>
        private GpioPin _heatingPin;

        /// <summary>
        /// Die 1Wire-Schnittstelle
        /// </summary>
        private OneWire _onewire;

        /// <summary>
        /// Die ID des Temperaturfühlers DS18B20
        /// </summary>
        private string _deviceId = string.Empty;

        /// <summary>
        /// Instanz des einzigen Modells
        /// </summary>
        private static ViewModel m_this = null;

        /// <summary>
        /// Lifert die einzige Instanz der Modell-Klasse
        /// </summary>
        public static ViewModel Instance
        {
            get
            {
                if (m_this == null)
                {
                    m_this = new ViewModel();
                }

                return m_this;
            }
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        private ViewModel()
        {
            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        private void Init()
        {
            // Init the Pins
            var gpio = GpioController.GetDefault();

            if (gpio != null)
            {
                _statusPin = gpio.OpenPin(5);
                _statusPin.SetDriveMode(GpioPinDriveMode.Output);

                _lightingPin = gpio.OpenPin(22);
                _lightingPin.SetDriveMode(GpioPinDriveMode.Output);

                _heatingPin = gpio.OpenPin(23);
                _heatingPin.SetDriveMode(GpioPinDriveMode.Output);

                Logging.Add(new LogItem(LogItem.LogLevel.Info, "GpioController gestartet"));
                Logging.Add(new LogItem(LogItem.LogLevel.Info, "StatusPin " + _statusPin.PinNumber));
                Logging.Add(new LogItem(LogItem.LogLevel.Info, "LightingPin " + _lightingPin.PinNumber));
                Logging.Add(new LogItem(LogItem.LogLevel.Info, "HeatingPin " + _heatingPin.PinNumber));
            }

            _onewire = new OneWire();

            NightMin = 10; // °C
            DayMin = 15; // °C
            Max = 19; // °C
            From = 9; // Uhr
            Till = 12; // Uhr
            From2 = 15; // Uhr
            Till2 = 17; // Uhr
            DayFrom = 7; // Uhr
            DayTill = 20; // Uhr

            ResetSettings();

            Chart24h = new ObservableCollection<ChartData>();

            _lastHour = DateTime.Now.Hour;
            _lastUpdate = DateTime.Now;

            try
            {
                var now = DateTime.Now;
                var time = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
                time = time.AddHours(-24);

                for (var i = 24; i >= 0; i--)
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
                Logging.Add(new LogItem(LogItem.LogLevel.Error, "Fehler bei der Initialisierung"));
                Logging.Add(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
            }
        }

        /// <summary>
        /// Initialfunktion
        /// </summary>
        public virtual async void InitAsync()
        {
            Status = true;
            Lighting = false;
            Heating = false;

            _deviceId = await _onewire.GetFirstSerialPort();
        }

        /// <summary>
        /// Updatefunktion
        /// </summary>
        public virtual async void UpdateAsync()
        {
            try
            {
                Temperature = await _onewire.GetTemperature(_deviceId);
            }
            catch (Exception ex)
            {
                Logging.Add(new LogItem(LogItem.LogLevel.Error, "Fehler bei Ermittlung der aktuellen Temperatur"));
                Logging.Add(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
            }

            var now = DateTime.Now;

            if (Logging.Count > 10000)
            {
                Logging.RemoveAt(0);
            }

            try
            {
                Lighting = LightTime && Temperature < Max;
                Heating = Temperature < Min;

                // Charting
                if (now.Hour != _lastHour)
                {
                    _lastHour = now.Hour;

                    ApplicationData.Current.RoamingSettings.Values["heatingCount0"] = (int)Math.Round(_heatingCount.TotalMinutes);
                    ApplicationData.Current.RoamingSettings.Values["lightingCount0"] = (int)Math.Round(_lightingCount.TotalMinutes);

                    for (var i = 24; i > 0; i--)
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
                Logging.Add(new LogItem(LogItem.LogLevel.Error, "Fehler beim Update"));
                Logging.Add(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
            }

            // Fehlerdiagnose und Selbstreperatur
            if (_diagnosticErrorTimeSpan.TotalMinutes > 10)
            {
                Reboot();
                _diagnosticErrorTimeSpan = new TimeSpan();
            }

            if (Logging.Where(x => x.Level == LogItem.LogLevel.Exception || x.Level == LogItem.LogLevel.Error).Count() > 0)
            {
                _diagnosticErrorTimeSpan += now - _lastUpdate;
            }
            else
            {
                _diagnosticErrorTimeSpan = new TimeSpan();
            }

            _lastUpdate = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Setzt oder liefert die Minimaltemperatur in der Nacht
        /// </summary>
        public int NightMin
        {
            get => _nightMin;
            set
            {
                if (_nightMin != value)
                {
                    _nightMin = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Setzt oder liefert die Minimaltemperatur am Tag
        /// </summary>
        public int DayMin
        {
            get => _dayMin;
            set
            {
                if (_dayMin != value)
                {
                    _dayMin = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Ermittelt ob es Zeit für den Scheinwerfer ist  
        /// </summary>
        public bool LightTime
        {
            get
            {
                var now = DateTime.Now;
                var f = new DateTime(now.Year, now.Month, now.Day, From, 0, 0);
                var t = new DateTime(now.Year, now.Month, now.Day, Till, 0, 0);
                var lightTime = f <= now && now <= t;

                f = new DateTime(now.Year, now.Month, now.Day, From2, 0, 0);
                t = new DateTime(now.Year, now.Month, now.Day, Till2, 0, 0);

                return lightTime || f <= now && now <= t;
            }
        }

        /// <summary>
        /// Setzt oder liefert die Minimaltemperatur
        /// </summary>
        public int Min
        {
            get
            {
                var now = DateTime.Now;
                var f = new DateTime(now.Year, now.Month, now.Day, DayFrom, 0, 0);
                var t = new DateTime(now.Year, now.Month, now.Day, DayTill, 0, 0);
                var day = f <= now && now <= t;

                return day ? DayMin : NightMin;
            }
        }

        /// <summary>
        /// Setzt oder liefert die Maximaltemperatur
        /// </summary>
        public int Max
        {
            get => _max;
            set
            {
                if (_max != value)
                {
                    _max = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Setzt oder liefert die Startzeit
        /// </summary>
        public int From
        {
            get => _from;
            set
            {
                if (_from != value)
                {
                    _from = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Setzt oder liefert die Endzeit
        /// </summary>
        public int Till
        {
            get => _till;
            set
            {
                if (_till != value)
                {
                    _till = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Setzt oder liefert die Startzeit
        /// </summary>
        public int From2
        {
            get => _from2;
            set
            {
                if (_from2 != value)
                {
                    _from2 = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Setzt oder liefert die Endzeit
        /// </summary>
        public int Till2
        {
            get => _till2;
            set
            {
                if (_till2 != value)
                {
                    _till2 = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Setzt oder liefert die Startzeit des Tages
        /// </summary>
        public int DayFrom
        {
            get => _dayFrom;
            set
            {
                if (_dayFrom != value)
                {
                    _dayFrom = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Setzt oder liefert die Tagesendzeit
        /// </summary>
        public int DayTill
        {
            get => _dayTill;
            set
            {
                if (_dayTill != value)
                {
                    _dayTill = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt die Temperatur
        /// </summary>
        public virtual double Temperature
        {
            get => _temperature;
            set
            {
                if (value != _temperature)
                {
                    _temperature = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt ob die Status-LED angeschaltet ist
        /// </summary>
        public virtual bool Status
        {
            get
            {
                try
                {
                    if (_statusPin == null)
                    {
                        Init();
                    }

                    return _statusPin.Read() == GpioPinValue.High ? true : false;
                }
                catch (Exception ex)
                {
                    Logging.Add(new LogItem(LogItem.LogLevel.Error, "Status konnte nicht ermittelt werden"));
                    Logging.Add(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));

                    return false;
                }
            }
            set
            {
                try
                {
                    if (value != Status)
                    {
                        if (value)
                        {
                            _statusPin.Write(GpioPinValue.High);
                        }
                        else
                        {
                            _statusPin.Write(GpioPinValue.Low);
                        }

                        NotifyPropertyChanged();
                    }
                }
                catch (Exception ex)
                {
                    Logging.Add(new LogItem(LogItem.LogLevel.Error, "Status konnte nicht ermittelt werden"));
                    Logging.Add(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt ob die UVB_Lampe angeschaltet ist
        /// </summary>
        public virtual bool Lighting
        {
            get
            {
                try
                {
                    if (_lightingPin == null)
                    {
                        Init();
                    }

                    return _lightingPin.Read() == GpioPinValue.Low ? true : false;
                }
                catch (Exception ex)
                {
                    Logging.Add(new LogItem(LogItem.LogLevel.Error, "Status des Scheinwerfers konnte nicht ermittelt werden"));
                    Logging.Add(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));

                    return false;
                }
            }
            set
            {
                try
                {
                    if (value != Lighting)
                    {
                        if (value)
                        {
                            _lightingPin.Write(GpioPinValue.Low);
                        }
                        else
                        {
                            _lightingPin.Write(GpioPinValue.High);
                        }

                        Logging.Add(new LogItem(LogItem.LogLevel.Info, Lighting ? "Scheinwerfer wurde eingeschaltet" : "Scheinwerfer wurde ausgeschaltet"));

                        NotifyPropertyChanged();
                    }
                }
                catch (Exception ex)
                {
                    Logging.Add(new LogItem(LogItem.LogLevel.Error, "Status des Scheinwerfers konnte nicht ermittelt werden"));
                    Logging.Add(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt ob die Heizlampe angeschaltet ist
        /// </summary>
        public virtual bool Heating
        {
            get
            {
                try
                {
                    if (_heatingPin == null)
                    {
                        Init();
                    }
                    return _heatingPin.Read() == GpioPinValue.Low ? true : false;
                }
                catch (Exception ex)
                {
                    Logging.Add(new LogItem(LogItem.LogLevel.Error, "Status des Infarotstrahlers konnte nicht ermittelt werden"));
                    Logging.Add(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));

                    return false;
                }
            }
            set
            {
                try
                {
                    if (value != Heating)
                    {
                        if (value)
                        {
                            _heatingPin.Write(GpioPinValue.Low);
                        }
                        else
                        {
                            _heatingPin.Write(GpioPinValue.High);
                        }

                        Logging.Add(new LogItem(LogItem.LogLevel.Info, Heating ? "Heizung wurde eingeschaltet" : "Heizung wurde ausgeschaltet"));

                        NotifyPropertyChanged();
                    }
                }
                catch (Exception ex)
                {
                    Logging.Add(new LogItem(LogItem.LogLevel.Error, "Status des Infarotstrahlers konnte nicht ermittelt werden"));
                    Logging.Add(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt den Staustext
        /// </summary>
        public virtual List<LogItem> Logging { get; private set; } = new List<LogItem>();

        /// <summary>
        /// Liefert die ID des Temperaturfühlers DS18B20
        /// </summary>
        public virtual string DeviceId => _deviceId;

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
        public string Now => DateTime.Now.ToString("dd.MM.yyyy<br>HH:mm:ss");

        /// <summary>
        /// Liefert die vergagene Zeit seit dem Programmstart
        /// </summary>
        public TimeSpan ProgramCounter => DateTime.Now - _start;

        /// <summary>
        /// Liefert oder setzt die Diagrammdaten
        /// </summary>
        public ObservableCollection<ChartData> Chart24h { get; set; }

        /// <summary>
        /// Liefert die Programmversion
        /// </summary>
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();


        /// <summary>
        /// Neustart des Raspberry PI
        /// </summary>
        public void Reboot()
        {
            Logging.Add(new LogItem(LogItem.LogLevel.Info, "Neustart des Raspberry Pi erfolgt..."));

            try
            {
                // siehe http://code-13.net/Content/Blog/1252
                ShutdownManager.BeginShutdown(ShutdownKind.Restart, TimeSpan.FromSeconds(30));
            }
            catch (Exception ex)
            {
                Logging.Add(new LogItem(LogItem.LogLevel.Error, "Neustart nicht erfolgreich"));
                Logging.Add(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn das Speichern der Einstellungen erfolgen soll
        /// </summary>
        public void SaveSettings()
        {
            Logging.Add(new LogItem(LogItem.LogLevel.Info, "Konfiguration wird gespeichert"));

            // Konfiguration speichern
            ApplicationData.Current.RoamingSettings.Values["nightmin"] = NightMin;
            ApplicationData.Current.RoamingSettings.Values["daymin"] = DayMin;
            ApplicationData.Current.RoamingSettings.Values["max"] = Max;
            ApplicationData.Current.RoamingSettings.Values["from"] = From;
            ApplicationData.Current.RoamingSettings.Values["till"] = Till;
            ApplicationData.Current.RoamingSettings.Values["from2"] = From2;
            ApplicationData.Current.RoamingSettings.Values["till2"] = Till2;
            ApplicationData.Current.RoamingSettings.Values["dayfrom"] = DayFrom;
            ApplicationData.Current.RoamingSettings.Values["daytill"] = DayTill;
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Einstellungen zurückgesetzt werden sollen
        /// </summary>
        public void ResetSettings()
        {
            Logging.Add(new LogItem(LogItem.LogLevel.Info, "Konfiguration wird geladen"));

            // Konfiguration laden
            NightMin = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["nightmin"]);
            DayMin = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["daymin"]);
            Max = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["max"]);
            From = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["from"]);
            Till = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["till"]);
            From2 = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["from2"]);
            Till2 = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["till2"]);
            DayFrom = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["dayfrom"]);
            DayTill = Convert.ToInt32(ApplicationData.Current.RoamingSettings.Values["daytill"]);
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Zähler zurückgesetzt werden sollen
        /// </summary>
        public void ResetCounter()
        {
            Logging.Add(new LogItem(LogItem.LogLevel.Info, "Zähler werden zurückgesetzt"));

            HeatingCounter = new TimeSpan();
            LightingCounter = new TimeSpan();
        }
    }
}
