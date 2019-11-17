using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

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
        /// Die ID des SerialDevice
        /// </summary>
        public string DeviceId { get; set; }

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
            var task = GetDeviceID();
            DeviceId = task.Result;
        }

        /// <summary>
        /// Ermittelt die DeviceID
        /// </summary>
        /// <returns>Die DeviceID</returns>
        public static async Task<string> GetDeviceID()
        {
            var aqs = SerialDevice.GetDeviceSelector();
            var dis = await DeviceInformation.FindAllAsync(aqs);
            var list = dis.ToList();
            if (list.Count > 0)
            {
                var deviceInfo = list.First();

                return deviceInfo.Id;
            }

            return string.Empty;
        }

        /// <summary>
        /// Erstelle ein SerialDevice
        /// </summary>
        /// <param name="deviceId">Die DeviceID</param>
        /// <returns>Das SerialDevice oder null</returns>
        private async Task<SerialDevice> CreateSerialPortAsync(string deviceId)
        {
            var serialPort = await SerialDevice.FromIdAsync(deviceId);
            if (serialPort == null)
            {
                return null;
            }

            // Konfiguriere serialPort
            serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
            serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
            serialPort.Parity = SerialParity.None;
            serialPort.StopBits = SerialStopBitCount.One;
            serialPort.DataBits = 8;
            serialPort.Handshake = SerialHandshake.None;

            return serialPort;
        }

        /// <summary>
        /// Schreibt ein Byte
        /// </summary>
        /// <param name="b"></param>
        /// <param name="writer">Der Wirter</param>
        /// <param name="reader">Der Reader</param>
        /// <returns></returns>
        private async Task WriteByte(byte b, DataWriter writer, DataReader reader)
        {
            for (byte i = 0; i < 8; i++, b = (byte)(b >> 1))
            {
                // Run through the bits in the byte, extracting the
                // LSB (bit 0) and sending it to the bus
                await Send((byte)(b & 0x01), writer, reader);
            }
        }

        /// <summary>
        /// Ließt ein Byte
        /// </summary>
        /// /// <param name="writer">Der Wirter</param>
        /// <param name="reader">Der Reader</param>
        /// <returns>Das zu emfangende Byte</returns>
        private async Task<byte> ReadByte(DataWriter writer, DataReader reader)
        {
            byte b = 0;
            for (byte i = 0; i < 8; i++)
            {
                // Build up byte bit by bit, LSB first
                b = (byte)((b >> 1) + 0x80 * await Send(1, writer, reader));
            }

            return b;
        }

        /// <summary>
        /// Daten Senden und Ergebnis empfangen
        /// </summary>
        /// <param name="b">Das zu sendende Byte</param>
        /// <param name="writer">Der Wirter</param>
        /// <param name="reader">Der Reader</param>
        /// <returns>Das Antwort-Byte</returns>
        private async Task<byte> Send(byte b, DataWriter writer, DataReader reader)
        {
            var bit = b > 0 ? 0xFF : 0x00;

            writer.WriteByte((byte)bit);
            await writer.StoreAsync();

            await reader.LoadAsync(1);
            var data = reader.ReadByte();

            return (byte)(data & 0xFF);
        }

        /// <summary>
        /// Ermittle die aktuelle Temperatur
        /// </summary>
        /// <param name="log">Das Logging</param>
        /// <returns>Die Temperatur in °C</returns>
        public async Task<double> GetTemperature(Action<string> log)
        {
            var tempCelsius = double.NaN;

            log("OneWire: GetTemperature >>>>>>>>>>>>>>>>>>");

            var serialPort = await CreateSerialPortAsync(DeviceId);
            if (serialPort == null)
            {
                log("OneWire: SerialPort wurde nicht erstellt");
                return double.NaN;
            }

            var writer = new DataWriter(serialPort.OutputStream);
            var reader = new DataReader(serialPort.InputStream);

            try
            {
                log("OneWire: Anfrage an DS18B20 um Temperatur zu ermitteln");
                log("OneWire: Sende RESET Signal");

                serialPort.BaudRate = 9600;
                writer.WriteByte(FunctionCommand_RESET);
                await writer.StoreAsync();

                log("OneWire: Empfange Presence Pulse");
                await reader.LoadAsync(1);
                var response = reader.ReadByte();

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
                await WriteByte(FunctionCommand_SKIP_ROM, writer, reader); 

                log("OneWire: Sende convert T command an DS18B20");
                await WriteByte(FunctionCommand_CONVERT_T, writer, reader); 

                // Warte für 750ms bis die Daten zusammen gesammelt sind
                await Task.Delay(750);

                log("OneWire: Auslesen der Temperatur vom DS18B20");
                log("OneWire: Sende RESET Signal");

                serialPort.BaudRate = 9600;
                writer.WriteByte(FunctionCommand_RESET);
                await writer.StoreAsync();

                log("OneWire: Empfange Presence Pulse");
                await reader.LoadAsync(1);
                response = reader.ReadByte();

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
                await WriteByte(FunctionCommand_SKIP_ROM, writer, reader); 

                log("OneWire: Sende read scratchpad command an DS18B20");
                await WriteByte(FunctionCommand_READ_SCRATCHPAD, writer, reader); 

                var tempLSB = await ReadByte(writer, reader); // Lese lsb
                var tempMSB = await ReadByte(writer, reader); // Lese msb
                var thRegister = await ReadByte(writer, reader); // Lese Th Register
                var tlRegister = await ReadByte(writer, reader); // Lese Tl Register
                var configRegister = await ReadByte(writer, reader); // Lese Configguration Register
                var reserved1 = await ReadByte(writer, reader); // 0xFF
                var reserved2 = await ReadByte(writer, reader); //
                var reserved3 = await ReadByte(writer, reader); // 0x10h
                var crc = await ReadByte(writer, reader); // CRC

                log("OneWire: Sende RESET Signal");

                serialPort.BaudRate = 9600;
                writer.WriteByte(FunctionCommand_RESET);
                await writer.StoreAsync();

                log("OneWire: Empfange Presence Pulse");
                await reader.LoadAsync(1);
                response = reader.ReadByte();

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
                writer.DetachStream();
                reader.DetachStream();
                writer.Dispose();
                reader.Dispose();

                serialPort.BreakSignalState = true;
                serialPort.Dispose();

                log("OneWire: GetTemperature <<<<<<<<<<<<<<<<<<");
            }

            return tempCelsius;
        }
    }
}
