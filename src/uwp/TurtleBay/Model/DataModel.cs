using DS18B201WireLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace TurtleBay.Model
{
    internal class DataModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Die momentane Temperatur
        /// </summary>
        double _temperature = 0;

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
        public DataModel()
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

            }

            _onewire = new OneWire();
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
            Temperature = await _onewire.GetTemperature(_deviceId);
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
        /// Setzt oder liefert die Minimaltemperatur in der Nacht
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
        /// Setzt oder liefert die Minimaltemperatur am Tag
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
        public int From2
        {
            get
            {
                return _from2;
            }
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
            get
            {
                return _till2;
            }
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
        /// Setzt oder liefert die Tagesendzeit
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
                    Error = "Der Status konnte nicht ermittelt werden" + Environment.NewLine + ex.Message;

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
                    Error = "Der Status konnte nicht ermittelt werden" + Environment.NewLine + ex.Message;
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
                    Error = "Status des Scheinwerfers konnte nicht ermittelt werden" + Environment.NewLine + ex.Message;

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

                        NotifyPropertyChanged();
                        NotifyPropertyChanged("LightingStr");
                        NotifyPropertyChanged("LightingBrush");
                    }
                }
                catch (Exception ex)
                {
                    Error = "Status des Scheinwerfers konnte nicht ermittelt werden" + Environment.NewLine + ex.Message;
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
                    Error = "Status des Infarotstrahlers konnte nicht ermittelt werden" + Environment.NewLine + ex.Message;

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

                        NotifyPropertyChanged();
                    }
                }
                catch (Exception ex)
                {
                    Error = "Status des Infarotstrahlers konnte nicht ermittelt werden" + Environment.NewLine + ex.Message;
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
