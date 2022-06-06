using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebExpress.WebPlugin;

namespace TurtleBay.Model
{
    public class ViewModel
    {
        /// <summary>
        /// Der GPIO-Controller
        /// </summary>
        private GpioController GPIO { get; set; }

        /// <summary>
        /// Die momentane Temperatur
        /// </summary>
        private Dictionary<string, double> _temperature = new();

        /// <summary>
        /// Liefert oder setzt die letzte Stunde
        /// </summary>
        private int _lastHour;

        /// <summary>
        /// Liefert oder setzt die Zeit des letzen Aufruf der Updatefunktion
        /// </summary>
        private DateTime _lastUpdate;

        /// <summary>
        /// Liefert oder setzt die Zeit des letzen auslesen der Temperatur
        /// </summary>
        private DateTime _lastMetering;

        /// <summary>
        /// Liefert oder setzt die Startzeit
        /// </summary>
        private readonly DateTime _start = DateTime.Now;

        /// <summary>
        /// Der GPIO-Pin, welcher die StatusLED steuert
        /// </summary>
        private const int _statusPin = 5;

        /// <summary>
        /// Der GPIO-Pin, welcher die UVB-Lampe steuert
        /// </summary>
        private const int _lightingPin = 22;

        /// <summary>
        /// Der GPIO-Pin, welcher die Heizlampe steuert
        /// </summary>
        private const int _heatingPin = 23;

        /// <summary>
        /// Der GPIO-Pin, welcher die erste Steckdose steuert
        /// </summary>
        private const int _socket1Pin = 20;

        /// <summary>
        /// Der GPIO-Pin, welcher die zweite Steckdose steuert
        /// </summary>
        private const int _socket2Pin = 21;

        /// <summary>                       
        /// Der Zustand des GPIO-Pins, welcher die StatusLED steuert
        /// </summary>
        private bool _status;

        /// <summary>
        /// Der Zustand des GPIO-Pins, welcher die UVB-Lampe steuert
        /// </summary>
        private bool _lighting;

        /// <summary>
        /// Der Zustand des GPIO-Pins, welcher die Heizlampe steuert
        /// </summary>
        private bool _heating;

        /// <summary>
        /// Der Zustand des GPIO-Pins, welcher die etrste Steckdose steuert
        /// </summary>
        private bool _socket1;

        /// <summary>
        /// Der Zustand des GPIO-Pins, welcher die zweite Steckdose steuert
        /// </summary>
        private bool _socket2;

        /// <summary>
        /// Manueller Status der ersten Steckdose
        /// </summary>
        public bool Socket1Switch { get; set; }

        /// <summary>
        /// Manueller Status der zweiten Steckdose
        /// </summary>
        public bool Socket2Switch { get; set; }

        /// <summary>
        /// Liefert oder setzt die Settings
        /// </summary>
        public Settings Settings { get; private set; } = new Settings();

        /// <summary>
        /// Liefert oder setzt die Statistikdaten
        /// </summary>
        public Statistic Statistic { get; private set; } = new Statistic();

        /// <summary>
        /// Instanz des einzigen Modells
        /// </summary>
        private static ViewModel _this = null;

        /// <summary>
        /// Liefert oder setzt den Kontext
        /// </summary>
        public IPluginContext Context { get; set; }

        /// <summary>
        /// Liefert oder setzt den Solarkalender
        /// </summary>
        public Dictionary<int, SolarcalendarItem> Solarcalendar { get; private set; } = new Dictionary<int, SolarcalendarItem>();

        /// <summary>
        /// Lifert die einzige Instanz der Modell-Klasse
        /// </summary>
        public static ViewModel Instance
        {
            get
            {
                if (_this == null)
                {
                    _this = new ViewModel();
                }

                return _this;
            }
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        private ViewModel()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public void Init()
        {
            try
            {
                // Initialisierung des Controllers
                GPIO = new GpioController(PinNumberingScheme.Logical);
                GPIO.OpenPin(_statusPin, PinMode.Output);
                GPIO.OpenPin(_lightingPin, PinMode.Output);
                GPIO.OpenPin(_heatingPin, PinMode.Output);
                GPIO.OpenPin(_socket1Pin, PinMode.Output);
                GPIO.OpenPin(_socket2Pin, PinMode.Output);

                GPIO.Write(_statusPin, PinValue.High);
                GPIO.Write(_lightingPin, PinValue.High);
                GPIO.Write(_heatingPin, PinValue.High);
                GPIO.Write(_socket1Pin, PinValue.High);
                GPIO.Write(_socket2Pin, PinValue.High);

                Log(new LogItem(LogItem.LogLevel.Info, "GpioController gestartet"));
                Log(new LogItem(LogItem.LogLevel.Debug, "StatusPin " + _statusPin));
                Log(new LogItem(LogItem.LogLevel.Debug, "LightingPin " + _lightingPin));
                Log(new LogItem(LogItem.LogLevel.Debug, "HeatingPin " + _heatingPin));
                Log(new LogItem(LogItem.LogLevel.Debug, "Socket1Pin " + _socket1Pin));
                Log(new LogItem(LogItem.LogLevel.Debug, "Socket2Pin " + _socket2Pin));
            }
            catch
            {

            }

            Status = true;
            Lighting = false;
            Heating = false;

            Settings.NightMin = 10; // °C
            Settings.DayMin = 15; // °C
            Settings.Max = 19; // °C
            Settings.Lighting.From = 9; // Uhr
            Settings.Lighting.Till = 12; // Uhr
            Settings.Lighting.From2 = 15; // Uhr
            Settings.Lighting.Till2 = 17; // Uhr
            Settings.DayFrom = 7; // Uhr
            Settings.DayTill = 20; // Uhr

            ResetSettings();
            ResetStatistic();
            ResetSolarcalendar();

            _lastHour = DateTime.Now.Hour;
            _lastUpdate = DateTime.Now;
        }

        /// <summary>
        /// Updatefunktion
        /// </summary>
        public virtual void Update()
        {
            try
            {
                if ((DateTime.Now - _lastMetering).TotalSeconds > 60)
                {
                    _lastMetering = DateTime.Now;
                    var hash = Guid.NewGuid();
                    var temperature = new Dictionary<string, double>();

                    foreach (var v in Directory.EnumerateDirectories("/sys/bus/w1/devices"))
                    {
                        var regex = new Regex("[0-9]{2}-[0-9A-Fa-f]{12}");
                        if (regex.Match(Path.GetFileName(v)).Success)
                        {
                            var id = Path.GetFileName(v);
                            var file = File.ReadAllText(Path.Combine(v, "w1_slave"));

                            var tempRegex = new Regex("(t)=([0-9]{3,5})");
                            var match = tempRegex.Match(file);

                            if (match.Success)
                            {
                                var raw = match?.Groups[2]?.Value?.Trim();
                                var temp = Convert.ToDouble(raw) / 1000;
                                temperature.Add(id, temp);

                                Log(new LogItem(LogItem.LogLevel.Debug, string.Format("Temperatursensor '{0}': {1} °C", id, temp)));
                            }
                        }
                    }

                    Temperature = temperature;
                }
            }
            catch (Exception ex)
            {
                Log(new LogItem(LogItem.LogLevel.Error, "Fehler bei Ermittlung der aktuellen Temperatur"));
                Log(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
            }

            var now = DateTime.Now;

            if (Logging.Count > 10000)
            {
                Logging.RemoveRange(0, 100);
            }

            try
            {
                var temp = PrimaryTemperature;

                Lighting = LightTime && temp < Settings.Max;
                Heating = temp < Min;
                Socket1 = Socket1Time || Socket1Switch;
                Socket2 = Socket2Time || Socket2Switch;

                // Charting
                if (now.Hour != _lastHour)
                {
                    _lastHour = now.Hour;

                    if (_lastHour == 3 && now.DayOfWeek == DayOfWeek.Friday)
                    {
                        Reboot();
                    }

                    var last = Statistic.Chart24h.LastOrDefault();
                    if (last != null)
                    {
                        last.HeatingCount += Heating ? (int)(now - _lastUpdate).TotalMilliseconds : 0;
                        last.LightingCount += Lighting ? (int)(now - _lastUpdate).TotalMilliseconds : 0;
                    }

                    if (Statistic.Chart24h.Count > 24)
                    {
                        Statistic.Chart24h.RemoveAt(0);
                    }

                    Statistic.Chart24h.Add(new ChartData()
                    {
                        Time = now,
                        Temperature = double.IsNaN(temp) ? 0 : Convert.ToInt32(temp),
                        HeatingCount = 0,
                        LightingCount = 0
                    });

                    SaveStatistic();
                }
                else
                {
                    var last = Statistic.Chart24h.LastOrDefault();
                    if (last != null)
                    {
                        last.HeatingCount += Heating ? (int)(now - _lastUpdate).TotalMilliseconds : 0;
                        last.LightingCount += Lighting ? (int)(now - _lastUpdate).TotalMilliseconds : 0;
                    }
                }

                // Counting
                Statistic.HeatingCounter += Heating ? now - _lastUpdate : new TimeSpan();
                Statistic.LightingCounter += Lighting ? now - _lastUpdate : new TimeSpan();

                Status = !Status;
            }
            catch (Exception ex)
            {
                Log(new LogItem(LogItem.LogLevel.Error, "Fehler beim Update"));
                Log(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
            }

            // Fehlerdiagnose und Selbstreperatur
            if (Logging.Where(x => x.Level == LogItem.LogLevel.Exception || x.Level == LogItem.LogLevel.Error).Count() > 500)
            {
                Reboot();
            }

            _lastUpdate = DateTime.Now;
        }

        /// <summary>
        /// Ermittelt ob es Zeit für den Scheinwerfer ist  
        /// </summary>
        [XmlIgnore]
        private bool LightTime
        {
            get
            {
                var now = DateTime.Now;
                DateTime f;
                DateTime t;

                // Erste Zeit
                if (Settings.Lighting.From == -1)
                {
                    f = CurrentDayFrom;
                }
                else if (Settings.Lighting.From == -2)
                {
                    f = CurrentDayTill;
                }
                else
                {
                    f = new DateTime(now.Year, now.Month, now.Day, Settings.Lighting.From, 0, 0);
                }

                if (Settings.Lighting.Till == -1)
                {
                    t = CurrentDayFrom;
                }
                else if (Settings.Lighting.Till == -2)
                {
                    t = CurrentDayTill;
                }
                else
                {
                    t = new DateTime(now.Year, now.Month, now.Day, Settings.Lighting.Till, 0, 0);
                }

                var lightTime = f <= now && now <= t;

                // Zweite Zeit
                if (Settings.Lighting.From2 == -1)
                {
                    f = CurrentDayFrom;
                }
                else if (Settings.Lighting.From2 == -2)
                {
                    f = CurrentDayTill;
                }
                else
                {
                    f = new DateTime(now.Year, now.Month, now.Day, Settings.Lighting.From2, 0, 0);
                }

                if (Settings.Lighting.Till2 == -1)
                {
                    t = CurrentDayFrom;
                }
                else if (Settings.Lighting.Till2 == -2)
                {
                    t = CurrentDayTill;
                }
                else
                {
                    t = new DateTime(now.Year, now.Month, now.Day, Settings.Lighting.Till2, 0, 0);
                }

                return lightTime || f <= now && now <= t;
            }
        }

        /// <summary>
        /// Ermittelt ob es Zeit für die erste Steckdose ist  
        /// </summary>
        [XmlIgnore]
        private bool Socket1Time
        {
            get
            {
                var now = DateTime.Now;
                DateTime f;
                DateTime t;

                // Erste Zeit
                if (Settings.Socket1.From == -1)
                {
                    f = CurrentDayFrom;
                }
                else if (Settings.Socket1.From == -2)
                {
                    f = CurrentDayTill;
                }
                else
                {
                    f = new DateTime(now.Year, now.Month, now.Day, Settings.Socket1.From, 0, 0);
                }

                if (Settings.Socket1.Till == -1)
                {
                    t = CurrentDayFrom;
                }
                else if (Settings.Socket1.Till == -2)
                {
                    t = CurrentDayTill;
                }
                else
                {
                    t = new DateTime(now.Year, now.Month, now.Day, Settings.Socket1.Till, 0, 0);
                }

                var lightTime = f <= now && now <= t;

                // Zweite Zeit
                if (Settings.Socket1.From2 == -1)
                {
                    f = CurrentDayFrom;
                }
                else if (Settings.Socket1.From2 == -2)
                {
                    f = CurrentDayTill;
                }
                else
                {
                    f = new DateTime(now.Year, now.Month, now.Day, Settings.Socket1.From2, 0, 0);
                }

                if (Settings.Socket1.Till2 == -1)
                {
                    t = CurrentDayFrom;
                }
                else if (Settings.Socket1.Till2 == -2)
                {
                    t = CurrentDayTill;
                }
                else
                {
                    t = new DateTime(now.Year, now.Month, now.Day, Settings.Socket1.Till2, 0, 0);
                }

                return lightTime || f <= now && now <= t;
            }
        }

        /// <summary>
        /// Ermittelt ob es Zeit für die zweite Steckdose ist  
        /// </summary>
        [XmlIgnore]
        private bool Socket2Time
        {
            get
            {
                var now = DateTime.Now;
                DateTime f;
                DateTime t;

                // Erste Zeit
                if (Settings.Socket2.From == -1)
                {
                    f = CurrentDayFrom;
                }
                else if (Settings.Socket2.From == -2)
                {
                    f = CurrentDayTill;
                }
                else
                {
                    f = new DateTime(now.Year, now.Month, now.Day, Settings.Socket2.From, 0, 0);
                }

                if (Settings.Socket2.Till == -1)
                {
                    t = CurrentDayFrom;
                }
                else if (Settings.Socket2.Till == -2)
                {
                    t = CurrentDayTill;
                }
                else
                {
                    t = new DateTime(now.Year, now.Month, now.Day, Settings.Socket2.Till, 0, 0);
                }

                var lightTime = f <= now && now <= t;

                // Zweite Zeit
                if (Settings.Socket2.From2 == -1)
                {
                    f = CurrentDayFrom;
                }
                else if (Settings.Socket2.From2 == -2)
                {
                    f = CurrentDayTill;
                }
                else
                {
                    f = new DateTime(now.Year, now.Month, now.Day, Settings.Socket2.From2, 0, 0);
                }

                if (Settings.Socket2.Till2 == -1)
                {
                    t = CurrentDayFrom;
                }
                else if (Settings.Socket2.Till2 == -2)
                {
                    t = CurrentDayTill;
                }
                else
                {
                    t = new DateTime(now.Year, now.Month, now.Day, Settings.Socket2.Till2, 0, 0);
                }

                return lightTime || f <= now && now <= t;
            }
        }

        /// <summary>
        /// Setzt oder liefert die Minimaltemperatur
        /// </summary>
        [XmlIgnore]
        public int Min
        {
            get
            {
                var now = DateTime.Now;

                var f = CurrentDayFrom;
                var t = CurrentDayTill;
                var day = f <= now && now <= t;

                return day ? Settings.DayMin : Settings.NightMin;
            }
        }

        /// <summary>
        /// Liefert oder setzt die Temperatur
        /// </summary>
        [XmlIgnore]
        public virtual Dictionary<string, double> Temperature
        {
            get => _temperature;
            set
            {
                if (value != _temperature)
                {
                    _temperature = value;
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt ob die Status-LED angeschaltet ist
        /// </summary>
        public virtual bool Status
        {
            get => _status;
            set
            {
                try
                {
                    if (value != Status)
                    {
                        if (value)
                        {
                            GPIO.Write(_statusPin, PinValue.High);
                        }
                        else
                        {
                            GPIO.Write(_statusPin, PinValue.Low);
                        }

                        _status = value;
                    }
                }
                catch (Exception ex)
                {
                    Log(new LogItem(LogItem.LogLevel.Error, "Status konnte nicht ermittelt werden"));
                    Log(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt ob die UVB_Lampe angeschaltet ist
        /// </summary>
        [XmlIgnore]
        public virtual bool Lighting
        {
            get => _lighting;
            set
            {
                try
                {
                    if (value != Lighting)
                    {
                        if (value)
                        {
                            GPIO.Write(_lightingPin, PinValue.Low);
                        }
                        else
                        {
                            GPIO.Write(_lightingPin, PinValue.High);
                        }

                        _lighting = value;

                        Log(new LogItem(LogItem.LogLevel.Info, Lighting ? "Scheinwerfer wurde eingeschaltet" : "Scheinwerfer wurde ausgeschaltet"));
                    }
                }
                catch (Exception ex)
                {
                    Log(new LogItem(LogItem.LogLevel.Error, "Status des Scheinwerfers konnte nicht ermittelt werden"));
                    Log(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt ob die Heizlampe angeschaltet ist
        /// </summary>
        [XmlIgnore]
        public virtual bool Heating
        {
            get => _heating;
            set
            {
                try
                {
                    if (value != Heating)
                    {
                        if (value)
                        {
                            GPIO.Write(_heatingPin, PinValue.Low);
                        }
                        else
                        {
                            GPIO.Write(_heatingPin, PinValue.High);
                        }

                        _heating = value;

                        Log(new LogItem(LogItem.LogLevel.Info, Heating ? "Heizung wurde eingeschaltet" : "Heizung wurde ausgeschaltet"));
                    }
                }
                catch (Exception ex)
                {
                    Log(new LogItem(LogItem.LogLevel.Error, "Status des Infarotstrahlers konnte nicht ermittelt werden"));
                    Log(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt ob die erste Steckdose angeschaltet ist
        /// </summary>
        [XmlIgnore]
        public virtual bool Socket1
        {
            get => _socket1;
            set
            {
                try
                {
                    if (value != Socket1)
                    {
                        if (value)
                        {
                            GPIO.Write(_socket1Pin, PinValue.Low);
                        }
                        else
                        {
                            GPIO.Write(_socket1Pin, PinValue.High);
                        }

                        _socket1 = value;

                        Log(new LogItem(LogItem.LogLevel.Info, Socket1 ? "Steckdose 1 wurde eingeschaltet" : "Steckkdose 1 wurde ausgeschaltet"));
                    }
                }
                catch (Exception ex)
                {
                    Log(new LogItem(LogItem.LogLevel.Error, "Status der Steckdose 1 konnte nicht ermittelt werden"));
                    Log(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt ob die zweite Steckdose angeschaltet ist
        /// </summary>
        [XmlIgnore]
        public virtual bool Socket2
        {
            get => _socket2;
            set
            {
                try
                {
                    if (value != Socket2)
                    {
                        if (value)
                        {
                            GPIO.Write(_socket2Pin, PinValue.Low);
                        }
                        else
                        {
                            GPIO.Write(_socket2Pin, PinValue.High);
                        }

                        _socket2 = value;

                        Log(new LogItem(LogItem.LogLevel.Info, Socket2 ? "Steckdose 2 wurde eingeschaltet" : "Steckdose 2 wurde ausgeschaltet"));
                    }
                }
                catch (Exception ex)
                {
                    Log(new LogItem(LogItem.LogLevel.Error, "Status der Steckdose 2 konnte nicht ermittelt werden"));
                    Log(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt den Staustext
        /// </summary>
        [XmlIgnore]
        public List<LogItem> Logging { get; set; } = new List<LogItem>();

        /// <summary>
        /// Liefert die aktuelle Zeit
        /// </summary>
        [XmlIgnore]
        public static string Now => DateTime.Now.ToString("dd.MM.yyyy<br>HH:mm:ss");

        /// <summary>
        /// Liefert die vergagene Zeit seit dem Programmstart
        /// </summary>
        [XmlIgnore]
        public TimeSpan ProgramCounter => DateTime.Now - _start;

        /// <summary>
        /// Liefert die Programmversion
        /// </summary>
        [XmlIgnore]
        public static string Version
        {
            get
            {
                var assembly = typeof(ViewModel).Assembly;

                return assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Neustart des Raspberry PI
        /// </summary>
        public void Reboot()
        {
            Log(new LogItem(LogItem.LogLevel.Info, "Neustart des Raspberry Pi erfolgt..."));

            SaveStatistic();

            try
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \" sudo shutdown -r now \"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };

                Task.Run(() =>
                {
                    Thread.Sleep(10000);
                    process.Start();
                    var result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                });
            }
            catch (Exception ex)
            {
                Log(new LogItem(LogItem.LogLevel.Error, "Neustart nicht erfolgreich"));
                Log(new LogItem(LogItem.LogLevel.Exception, ex.ToString()));
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn das Speichern der Einstellungen erfolgen soll
        /// </summary>
        public void SaveSettings()
        {
            Log(new LogItem(LogItem.LogLevel.Info, "Einstellungen werden gespeichert"));

            // Konfiguration speichern
            var serializer = new XmlSerializer(typeof(Settings));

            using var memoryStream = new MemoryStream();
            serializer.Serialize(memoryStream, Settings);

            var utf = new UTF8Encoding();

            File.WriteAllText
            (
                Path.Combine(Context.Host.ConfigPath, "turtlebay.settings.xml"),
                utf.GetString(memoryStream.ToArray())
            );
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Einstellungen zurückgesetzt werden sollen
        /// </summary>
        public void ResetSettings()
        {
            Log(new LogItem(LogItem.LogLevel.Info, "Einstellungen werden geladen"));

            // Konfiguration laden
            var serializer = new XmlSerializer(typeof(Settings));

            try
            {
                using var reader = File.OpenText(Path.Combine(Context.Host.ConfigPath, "turtlebay.settings.xml"));
                Settings = serializer.Deserialize(reader) as Settings;
            }
            catch
            {
                Log(new LogItem(LogItem.LogLevel.Warning, "Datei mit den Einstellungen wurde nicht gefunden!"));
            }

            Log(new LogItem(LogItem.LogLevel.Debug, "NightMin = " + Settings.NightMin + " °C"));
            Log(new LogItem(LogItem.LogLevel.Debug, "DayMin = " + Settings.DayMin + " °C"));
            Log(new LogItem(LogItem.LogLevel.Debug, "Max = " + Settings.Max + " °C"));
            Log(new LogItem(LogItem.LogLevel.Debug, "From = " + Settings.Lighting.From + " Uhr"));
            Log(new LogItem(LogItem.LogLevel.Debug, "Till = " + Settings.Lighting.Till + " Uhr"));
            Log(new LogItem(LogItem.LogLevel.Debug, "From2 = " + Settings.Lighting.From2 + " Uhr"));
            Log(new LogItem(LogItem.LogLevel.Debug, "Till2 = " + Settings.Lighting.Till2 + " Uhr"));
            Log(new LogItem(LogItem.LogLevel.Debug, "DayFrom = " + Settings.DayFrom + " Uhr"));
            Log(new LogItem(LogItem.LogLevel.Debug, "DayTill = " + Settings.DayTill + " Uhr"));
        }

        /// <summary>
        /// Wird aufgerufen, wenn das Speichern der Einstellungen erfolgen soll
        /// </summary>
        public void SaveStatistic()
        {
            //Log(new LogItem(LogItem.LogLevel.Debug, "Statistik wird gespeichert"));

            // Konfiguration speichern
            var serializer = new XmlSerializer(typeof(Statistic));

            using var memoryStream = new MemoryStream();
            serializer.Serialize(memoryStream, Statistic);

            var utf = new UTF8Encoding();

            File.WriteAllText
            (
                Path.Combine(Context.Host.ConfigPath, "statistic.xml"),
                utf.GetString(memoryStream.ToArray())
            );
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Einstellungen zurückgesetzt werden sollen
        /// </summary>
        public void ResetStatistic()
        {
            Log(new LogItem(LogItem.LogLevel.Info, "Statistik wird geladen"));

            // Statistik laden
            var serializer = new XmlSerializer(typeof(Statistic));

            try
            {
                using var reader = File.OpenText(Path.Combine(Context.Host.ConfigPath, "statistic.xml"));
                Statistic = serializer.Deserialize(reader) as Statistic;
            }
            catch
            {
                Log(new LogItem(LogItem.LogLevel.Warning, "Datei mit den Statistikdaten wurde nicht gefunden!"));
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Zähler zurückgesetzt werden sollen
        /// </summary>
        public void ResetCounter()
        {
            Log(new LogItem(LogItem.LogLevel.Info, "Zähler werden zurückgesetzt"));

            Statistic.HeatingCounter = new TimeSpan();
            Statistic.LightingCounter = new TimeSpan();

            SaveStatistic();
        }

        /// <summary>
        /// Wird aufgerufen, wenn der Solarkalender geladen werden soll
        /// </summary>
        public void ResetSolarcalendar()
        {
            Log(new LogItem(LogItem.LogLevel.Info, "Solarkalender wird geladen"));

            // Konfiguration laden
            var serializer = new XmlSerializer(typeof(Solarcalendar));

            try
            {
                using var reader = File.OpenText(Path.Combine(Context.Host.ConfigPath, "solarcalendar.xml"));
                var solarcalendar = serializer.Deserialize(reader) as Solarcalendar;

                Solarcalendar.Clear();
                foreach (var v in solarcalendar.Items)
                {
                    Solarcalendar.Add(v.Day, new SolarcalendarItem()
                    {
                        Day = v.Day,
                        Sunrise = v.Sunrise,
                        Sunset = v.Sunset
                    });
                }
            }
            catch
            {
                Log(new LogItem(LogItem.LogLevel.Warning, "Datei mit den Solarkalender wurde nicht gefunden!"));
            }
        }

        /// <summary>
        /// Loggt ein Event
        /// </summary>
        /// <param name="logItem">Der Logeintrag</param>
        public void Log(LogItem logItem)
        {
            Logging.Add(logItem);

            switch (logItem.Level)
            {
                case LogItem.LogLevel.Info:
                    //Context.Log.Info(logItem.Instance, logItem.Massage);
                    break;
                case LogItem.LogLevel.Debug:
                    //Context.Log.Debug(logItem.Instance, logItem.Massage);
                    break;
                case LogItem.LogLevel.Warning:
                    Context.Log.Warning(logItem.Instance, logItem.Massage);
                    break;
                case LogItem.LogLevel.Error:
                    Context.Log.Error(logItem.Instance, logItem.Massage);
                    break;
                case LogItem.LogLevel.Exception:
                    Context.Log.Error(logItem.Instance, logItem.Massage);
                    break;
            }
        }

        /// <summary>
        /// Liefert die primäre Temperatur
        /// </summary>
        public double PrimaryTemperature
        {
            get
            {
                if
                (
                    !string.IsNullOrWhiteSpace(Settings?.PrimaryID) &&
                    Temperature.ContainsKey(Settings.PrimaryID)
                )
                {
                    return Temperature[Settings.PrimaryID];
                }
                else if (Temperature.Count > 0)
                {
                    return Temperature.FirstOrDefault().Value;
                }

                return double.NaN;
            }
        }

        /// <summary>
        /// Ermittelt den Tagesbeginn
        /// </summary>
        private DateTime CurrentDayFrom
        {
            get
            {
                var now = DateTime.Now;

                if (Settings.DayFrom == -1)
                {
                    var c = DateTime.Parse(Solarcalendar[now.DayOfYear].Sunrise);

                    return new DateTime(now.Year, now.Month, now.Day, c.Hour, c.Minute, 0);
                }

                return new DateTime(now.Year, now.Month, now.Day, Settings.DayFrom, 0, 0);
            }
        }

        /// <summary>
        /// Ermittelt das Tagesende
        /// </summary>
        private DateTime CurrentDayTill
        {
            get
            {
                var now = DateTime.Now;

                if (Settings.DayTill == -1)
                {
                    var c = DateTime.Parse(Solarcalendar[now.DayOfYear].Sunset);

                    return new DateTime(now.Year, now.Month, now.Day, c.Hour, c.Minute, 0);
                }

                return new DateTime(now.Year, now.Month, now.Day, Settings.DayTill, 0, 0);
            }
        }
    }
}
