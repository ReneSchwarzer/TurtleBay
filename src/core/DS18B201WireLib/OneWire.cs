using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Siehe https://www.hackster.io/selom/1-wire-ds18b20-sensor-on-windows-10-iot-core-raspberry-pi-2-7d9b67
/// </summary>
namespace DS18B201WireLib
{
    public class OneWire
    {
        /// <summary>
        /// Definition der Funktionskommandos
        /// </summary>
        private readonly byte FunctionCommand_RESET = 0xF0;
        private readonly byte FunctionCommand_SKIP_ROM = 0xCC;
        private readonly byte FunctionCommand_CONVERT_T = 0x44;
        private readonly byte FunctionCommand_READ_SCRATCHPAD = 0xBE;

        /// <summary>
        /// Instanz des einzigen Modells
        /// </summary>
        private static OneWire _this = null;

        /// <summary>
        /// Liefert oder setzt den Portnamen
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// Lifert die einzige Instanz der Modell-Klasse
        /// </summary>
        public static OneWire Instance
        {
            get
            {
                if (_this == null)
                {
                    _this = new OneWire();
                }

                return _this;
            }
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        private OneWire()
        {
            PortName = GetPortName();
        }

        /// <summary>
        /// Ermittelt den PortNamen
        /// </summary>
        /// <returns>Der Portname</returns>
        public static string GetPortName()
        {
            var ports = SerialPort.GetPortNames();

            if (ports.Count() > 0)
            {
                var portName = ports.FirstOrDefault();

                return portName;
            }

            return string.Empty;
        }

        /// <summary>
        /// Erstelle ein SerialDevice
        /// </summary>
        /// <param name="portName">Die DeviceID</param>
        /// <returns>Das SerialDevice oder null</returns>
        private SerialPort CreateSerialPort(string portName)
        {
            var serialPort = new SerialPort(PortName, 9600);
            if (serialPort == null)
            {
                return null;
            }

            // Konfiguriere serialPort
            serialPort.WriteTimeout = 1500;
            serialPort.ReadTimeout = 1500;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.DataBits = 8;
            serialPort.Handshake = Handshake.None;

            return serialPort;
        }

        /// <summary>
        /// Schreibt ein Byte
        /// </summary>
        /// <param name="b">Das zu sendende Byte</param>
        /// <param name="serialPort">Der serielle Port</param>
        private void WriteByte(byte b, SerialPort serialPort)
        {
            for (byte i = 0; i < 8; i++, b = (byte)(b >> 1))
            {
                // Run through the bits in the byte, extracting the
                // LSB (bit 0) and sending it to the bus
                var buf = new byte[] { (byte)(b & 0x01) };

                serialPort.Write(buf, 0, 1);
            }
        }

        /// <summary>
        /// Ließt ein Byte
        /// </summary>
        /// <param name="serialPort">Der serielle Port</param>
        /// <returns>Das zu emfangende Byte</returns>
        private byte ReadByte(SerialPort serialPort)
        {
            byte b = 0;
            for (byte i = 0; i < 8; i++)
            {
                // Build up byte bit by bit, LSB first
                b = (byte)((b >> 1) + 0x80 * Send(1, serialPort));
            }

            return b;
        }

        /// <summary>
        /// Daten Senden und Ergebnis empfangen
        /// </summary>
        /// <param name="b">Das zu sendende Byte</param>
        /// <param name="serialPort">Der serielle Port</param>
        /// <returns>Das Antwort-Byte</returns>
        private byte Send(byte b, SerialPort serialPort)
        {
            var bit = b > 0 ? 0xFF : 0x00;

            var buf = new byte[] { (byte)bit };
            serialPort.Write(buf, 0, 1);

            var data = serialPort.ReadByte();

            return (byte)(data & 0xFF);
        }

        /// <summary>
        /// Sendet ein Funktionskommando
        /// </summary>
        /// <param name="command">Das zu sendende Kommando</param>
        /// <param name="serialPort">Der serielle Port</param>
        private void SendCommand(int command, SerialPort serialPort)
        {
            var buf = new byte[] { (byte)command };
            serialPort.Write(buf, 0, 1);
        }

        /// <summary>
        /// empfängt den Presence Pulse
        /// </summary>
        /// <param name="serialPort">Der serielle Port</param>
        /// <returns>Der Presence Pulse</returns>
        private byte ReadresencePulse(SerialPort serialPort)
        {
            var response = serialPort.ReadByte();

            return (byte)response;
        }

        /// <summary>
        /// Ermittle die aktuelle Temperatur
        /// </summary>
        /// <param name="log">Das Logging</param>
        /// <returns>Die Temperatur in °C</returns>
        public double GetTemperature(Action<string> log)
        {
            var tempCelsius = double.NaN;

            log("OneWire: GetTemperature >>>>>>>>>>>>>>>>>>");

            var serialPort = CreateSerialPort(PortName);
            if (serialPort == null)
            {
                log("OneWire: SerialPort wurde nicht erstellt");
                return double.NaN;
            }

            serialPort.Open();

            try
            {
                log("OneWire: Anfrage an DS18B20 um Temperatur zu ermitteln");
                log("OneWire: Sende RESET Signal");

                serialPort.BaudRate = 9600;
                SendCommand(FunctionCommand_RESET, serialPort);

                log("OneWire: Empfange Presence Pulse");
                var response = ReadresencePulse(serialPort);

                if (response == 0xFF)
                {
                    log("OneWire: Keine Verbindung zum UART!");

                    return double.NaN;
                }
                else if (response == 0xF0)
                {
                    log("OneWire: Es sind keine OnwWire-Geräte präsent!");

                    return double.NaN;
                }

                serialPort.BaudRate = 115200;

                log("OneWire: Sende SKIP ROM command an DS18B20");
                SendCommand(FunctionCommand_SKIP_ROM, serialPort);

                log("OneWire: Sende convert T command an DS18B20");
                SendCommand(FunctionCommand_CONVERT_T, serialPort);
                
                // Warte für 750ms bis die Daten zusammen gesammelt sind
                Thread.Sleep(750);

                log("OneWire: Auslesen der Temperatur vom DS18B20");
                log("OneWire: Sende RESET Signal");

                serialPort.BaudRate = 9600;
                SendCommand(FunctionCommand_RESET, serialPort);

                log("OneWire: Empfange Presence Pulse");
                response = ReadresencePulse(serialPort);

                if (response == 0xFF)
                {
                    log("OneWire: Keine Verbindung zum UART!");

                    return double.NaN;
                }
                else if (response == 0xF0)
                {
                    log("OneWire: Es sind keine OnwWire-Geräte präsent!");

                    return double.NaN;
                }

                serialPort.BaudRate = 115200;

                log("OneWire: Sende skip ROM command an DS18B20");
                SendCommand(FunctionCommand_SKIP_ROM, serialPort);

                log("OneWire: Sende read scratchpad command an DS18B20");
                SendCommand(FunctionCommand_READ_SCRATCHPAD, serialPort);
                
                var tempLSB = ReadByte(serialPort);         // Lese lsb
                var tempMSB = ReadByte(serialPort);         // Lese msb
                var thRegister = ReadByte(serialPort);      // Lese Th Register
                var tlRegister = ReadByte(serialPort);      // Lese Tl Register
                var configRegister = ReadByte(serialPort);  // Lese Configguration Register
                var reserved1 = ReadByte(serialPort);       // 0xFF
                var reserved2 = ReadByte(serialPort);       //
                var reserved3 = ReadByte(serialPort);       // 0x10h
                var crc = ReadByte(serialPort);             // CRC

                log("OneWire: Sende RESET Signal");

                serialPort.BaudRate = 9600;
                SendCommand(FunctionCommand_RESET, serialPort);

                log("OneWire: Empfange Presence Pulse");
                response = ReadresencePulse(serialPort);

                log("OneWire: TempLSB = " + BitConverter.ToString(new byte[] { tempLSB }));
                log("OneWire: TempMSB = " + BitConverter.ToString(new byte[] { tempMSB }));
                log("OneWire: Th Register = " + BitConverter.ToString(new byte[] { thRegister }));
                log("OneWire: Tl Register = " + BitConverter.ToString(new byte[] { tlRegister }));
                log("OneWire: Config Register = " + BitConverter.ToString(new byte[] { configRegister }));
                log("OneWire: Reserved1 (FFh) = " + BitConverter.ToString(new byte[] { reserved1 }));
                log("OneWire: Reserved2 = " + BitConverter.ToString(new byte[] { reserved2 }));
                log("OneWire: Reserved3 (10h)= " + BitConverter.ToString(new byte[] { reserved3 }));
                log("OneWire: CRC = " + BitConverter.ToString(new byte[] { tempLSB }));

                // Berechne die Temperatur
                tempCelsius = ((tempMSB * 256) + tempLSB) / 16.0;
                log("OneWire: Die Temperatur beträgt " + tempCelsius + " °C");
            }
            catch (Exception ex)
            {
                log("OneWire: " + ex.Message);
            }
            finally
            {
                serialPort.Close();
                serialPort.Dispose();

                log("OneWire: GetTemperature <<<<<<<<<<<<<<<<<<");
            }

            return tempCelsius;
        }
    }
}
