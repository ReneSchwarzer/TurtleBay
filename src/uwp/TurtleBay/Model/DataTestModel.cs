using DS18B201WireLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace TurtleBay.Model
{
    internal class DataTestModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Minimale Temperatur
        /// </summary>
        private int _nightMin;

        /// <summary>
        /// Minimale Temperatur
        /// </summary>
        private int _dayMin;

        /// <summary>
        /// Maximale Temperatur
        /// </summary>
        private int _max;

        /// <summary>
        /// Startzeit der UVB-Lampe
        /// </summary>
        private int _dayFrom;

        /// <summary>
        /// Endzeit der UVB-Lampe
        /// </summary>
        private int _dayTill;

        /// <summary>
        /// Startzeit der UVB-Lampe
        /// </summary>
        private int _from;

        /// <summary>
        /// Endzeit der UVB-Lampe
        /// </summary>
        private int _till;

        /// <summary>
        /// Die momentane Temperatur
        /// </summary>
        double _temperature = 0;
        
        /// <summary>
        /// Der GPIO-Pin, welcher die StatusLED steuert
        /// </summary>
        private bool _statusPin;

        /// <summary>
        /// Der GPIO-Pin, welcher die UVB-Lampe steuert
        /// </summary>
        private bool _lightingPin;

        /// <summary>
        /// Der GPIO-Pin, welcher die Heizlampe steuert
        /// </summary>
        private bool _heatingPin;

        /// <summary>
        /// Statusinformationen
        /// </summary>
        private string _info = string.Empty;

        /// <summary>
        /// Statusinformationen zum Fehler
        /// </summary>
        private string _error = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public DataTestModel()
        {
        }

        /// <summary>
        /// Initialfunktion
        /// </summary>
        public virtual async void InitAsync()
        {
            Status = true;
            Lighting = false;
            Heating = false;

            ApplicationData.Current.RoamingSettings.Values["h24"] = 19;
            ApplicationData.Current.RoamingSettings.Values["h23"] = 19;
            ApplicationData.Current.RoamingSettings.Values["h22"] = 19;
            ApplicationData.Current.RoamingSettings.Values["h21"] = 20;
            ApplicationData.Current.RoamingSettings.Values["h20"] = 20;
            ApplicationData.Current.RoamingSettings.Values["h19"] = 21;
            ApplicationData.Current.RoamingSettings.Values["h18"] = 22;
            ApplicationData.Current.RoamingSettings.Values["h17"] = 24;
            ApplicationData.Current.RoamingSettings.Values["h16"] = 25;
            ApplicationData.Current.RoamingSettings.Values["h15"] = 25;
            ApplicationData.Current.RoamingSettings.Values["h14"] = 27;
            ApplicationData.Current.RoamingSettings.Values["h13"] = 27;
            ApplicationData.Current.RoamingSettings.Values["h12"] = 28;
            ApplicationData.Current.RoamingSettings.Values["h11"] = 28;
            ApplicationData.Current.RoamingSettings.Values["h10"] = 28;
            ApplicationData.Current.RoamingSettings.Values["h9"] = 27;
            ApplicationData.Current.RoamingSettings.Values["h8"] = 25;
            ApplicationData.Current.RoamingSettings.Values["h7"] = 24;
            ApplicationData.Current.RoamingSettings.Values["h6"] = 23;
            ApplicationData.Current.RoamingSettings.Values["h5"] = 22;
            ApplicationData.Current.RoamingSettings.Values["h4"] = 21;
            ApplicationData.Current.RoamingSettings.Values["h3"] = 20;
            ApplicationData.Current.RoamingSettings.Values["h2"] = 20;
            ApplicationData.Current.RoamingSettings.Values["h1"] = 19;
            ApplicationData.Current.RoamingSettings.Values["h0"] = 18;

            ApplicationData.Current.RoamingSettings.Values["heatingCount24"] = 40;
            ApplicationData.Current.RoamingSettings.Values["heatingCount23"] = 35;
            ApplicationData.Current.RoamingSettings.Values["heatingCount22"] = 19;
            ApplicationData.Current.RoamingSettings.Values["heatingCount21"] = 0;
            ApplicationData.Current.RoamingSettings.Values["heatingCount20"] = 0;
            ApplicationData.Current.RoamingSettings.Values["heatingCount19"] = 0;
            ApplicationData.Current.RoamingSettings.Values["heatingCount18"] = 5;
            ApplicationData.Current.RoamingSettings.Values["heatingCount17"] = 0;
            ApplicationData.Current.RoamingSettings.Values["heatingCount16"] = 0;
            ApplicationData.Current.RoamingSettings.Values["heatingCount15"] = 0;
            ApplicationData.Current.RoamingSettings.Values["heatingCount14"] = 1;
            ApplicationData.Current.RoamingSettings.Values["heatingCount13"] = 0;
            ApplicationData.Current.RoamingSettings.Values["heatingCount12"] = 0;
            ApplicationData.Current.RoamingSettings.Values["heatingCount11"] = 0;
            ApplicationData.Current.RoamingSettings.Values["heatingCount10"] = 0;
            ApplicationData.Current.RoamingSettings.Values["heatingCount9"] = 5;
            ApplicationData.Current.RoamingSettings.Values["heatingCount8"] = 15;
            ApplicationData.Current.RoamingSettings.Values["heatingCount7"] = 20;
            ApplicationData.Current.RoamingSettings.Values["heatingCount6"] = 30;
            ApplicationData.Current.RoamingSettings.Values["heatingCount5"] = 40;
            ApplicationData.Current.RoamingSettings.Values["heatingCount4"] = 50;
            ApplicationData.Current.RoamingSettings.Values["heatingCount3"] = 60;
            ApplicationData.Current.RoamingSettings.Values["heatingCount2"] = 50;
            ApplicationData.Current.RoamingSettings.Values["heatingCount1"] = 40;
            ApplicationData.Current.RoamingSettings.Values["heatingCount0"] = 18;

            ApplicationData.Current.RoamingSettings.Values["lightingCount24"] = 0;
            ApplicationData.Current.RoamingSettings.Values["lightingCount23"] = 0;
            ApplicationData.Current.RoamingSettings.Values["lightingCount22"] = 0;
            ApplicationData.Current.RoamingSettings.Values["lightingCount21"] = 0;
            ApplicationData.Current.RoamingSettings.Values["lightingCount20"] = 0;
            ApplicationData.Current.RoamingSettings.Values["lightingCount19"] = 0;
            ApplicationData.Current.RoamingSettings.Values["lightingCount18"] = 5;
            ApplicationData.Current.RoamingSettings.Values["lightingCount17"] = 60;
            ApplicationData.Current.RoamingSettings.Values["lightingCount16"] = 60;
            ApplicationData.Current.RoamingSettings.Values["lightingCount15"] = 60;
            ApplicationData.Current.RoamingSettings.Values["lightingCount14"] = 60;
            ApplicationData.Current.RoamingSettings.Values["lightingCount13"] = 60;
            ApplicationData.Current.RoamingSettings.Values["lightingCount12"] = 50;
            ApplicationData.Current.RoamingSettings.Values["lightingCount11"] = 60;
            ApplicationData.Current.RoamingSettings.Values["lightingCount10"] = 61;
            ApplicationData.Current.RoamingSettings.Values["lightingCount9"] = 58;
            ApplicationData.Current.RoamingSettings.Values["lightingCount8"] = 60;
            ApplicationData.Current.RoamingSettings.Values["lightingCount7"] = 60;
            ApplicationData.Current.RoamingSettings.Values["lightingCount6"] = 0;
            ApplicationData.Current.RoamingSettings.Values["lightingCount5"] = 0;
            ApplicationData.Current.RoamingSettings.Values["lightingCount4"] = 30;
            ApplicationData.Current.RoamingSettings.Values["lightingCount3"] = 0;
            ApplicationData.Current.RoamingSettings.Values["lightingCount2"] = 0;
            ApplicationData.Current.RoamingSettings.Values["lightingCount1"] = 0;
            ApplicationData.Current.RoamingSettings.Values["lightingCount0"] = 0;
        }

        /// <summary>
        /// Updatefunktion
        /// </summary>
        public virtual async void UpdateAsync()
        {
            Temperature = new Random().NextDouble() * 10;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Setzt oder liefert die Minimaltemperatur
        /// </summary>
        public int NightMin
        {
            get
            {
                return _nightMin;
            }
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
        /// Setzt oder liefert die Minimaltemperatur
        /// </summary>
        public int DayMin
        {
            get
            {
                return _dayMin;
            }
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
        /// Setzt oder liefert die Maximaltemperatur
        /// </summary>
        public int Max
        {
            get
            {
                return _max;
            }
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
            get
            {
                return _from;
            }
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
            get
            {
                return _till;
            }
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
        public int DayFrom
        {
            get
            {
                return _dayFrom;
            }
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
        /// Setzt oder liefert die Endzeit
        /// </summary>
        public int DayTill
        {
            get
            {
                return _dayTill;
            }
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
            get { return _temperature; }
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
                return _statusPin;
            }
            set
            {
                if (value != Status)
                {
                    _statusPin = value;

                    NotifyPropertyChanged();
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
                return _lightingPin;
            }
            set
            {
                if (value != Lighting)
                {
                    _lightingPin = value;

                    NotifyPropertyChanged();
                    NotifyPropertyChanged("LightingStr");
                    NotifyPropertyChanged("LightingBrush");
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
                return _heatingPin;
            }
            set
            {
                if (value != Heating)
                {
                    _heatingPin = value;

                    NotifyPropertyChanged();
                    NotifyPropertyChanged("HeatingStr");
                    NotifyPropertyChanged("HeatingBrush");
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt den Staustext
        /// </summary>
        public virtual string Info
        {
            get
            {
                return _info;
            }
            set
            {
                if (_info != value)
                {
                    _info = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt den Staustext
        /// </summary>
        public virtual string Error
        {
            get
            {
                return _error;
            }
            set
            {
                if (_error != value)
                {
                    _error = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
