using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

/// <summary>
/// Siehe https://www.hackster.io/selom/1-wire-ds18b20-sensor-on-windows-10-iot-core-raspberry-pi-2-7d9b67
/// </summary>
namespace DS18B201WireLib
{
    class WireSearchResult
    {
        public byte[] id = new byte[8];
        public int lastForkPoint = 0;
    }

    public class OneWire
    {
        private SerialDevice serialPort = null;
        DataWriter dataWriteObject = null;
        DataReader dataReaderObject = null;

        public async Task<string> GetFirstSerialPort()
        {
            try
            {
                string aqs = SerialDevice.GetDeviceSelector();
                var dis = await DeviceInformation.FindAllAsync(aqs);
                if (dis.Count > 0)
                {
                    var deviceInfo = dis.First();
                    return deviceInfo.Id;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
            }

            return null;
        }

        public void Shutdown()
        {
            if (serialPort != null)
            {
                serialPort.Dispose();
                serialPort = null;
            }
        }

        async Task<bool> OnewireReset(string deviceId)
        {
            try
            {
                if (serialPort != null)
                    serialPort.Dispose();

                serialPort = await SerialDevice.FromIdAsync(deviceId);

                // Configure serial settings
                serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.BaudRate = 9600;
                serialPort.Parity = SerialParity.None;
                serialPort.StopBits = SerialStopBitCount.One;
                serialPort.DataBits = 8;
                serialPort.Handshake = SerialHandshake.None;

                dataWriteObject = new DataWriter(serialPort.OutputStream);
                dataWriteObject.WriteByte(0xF0);
                await dataWriteObject.StoreAsync();

                dataReaderObject = new DataReader(serialPort.InputStream);
                await dataReaderObject.LoadAsync(1);
                byte resp = dataReaderObject.ReadByte();
                if (resp == 0xFF)
                {
                    System.Diagnostics.Debug.WriteLine("Nothing connected to UART");
                    return false;
                }
                else if (resp == 0xF0)
                {
                    System.Diagnostics.Debug.WriteLine("No 1-wire devices are present");
                    return false;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Response: " + resp);
                    serialPort.Dispose();
                    serialPort = await SerialDevice.FromIdAsync(deviceId);

                    // Configure serial settings
                    serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                    serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                    serialPort.BaudRate = 115200;
                    serialPort.Parity = SerialParity.None;
                    serialPort.StopBits = SerialStopBitCount.One;
                    serialPort.DataBits = 8;
                    serialPort.Handshake = SerialHandshake.None;
                    dataWriteObject = new DataWriter(serialPort.OutputStream);
                    dataReaderObject = new DataReader(serialPort.InputStream);
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        public async Task OnewireWriteByte(byte b)
        {
            for (byte i = 0; i < 8; i++, b = (byte)(b >> 1))
            {
                // Run through the bits in the byte, extracting the
                // LSB (bit 0) and sending it to the bus
                await OnewireBit((byte)(b & 0x01));
            }
        }

        async Task<byte> OnewireBit(byte b)
        {
            var bit = b > 0 ? 0xFF : 0x00;
            dataWriteObject.WriteByte((byte)bit);
            await dataWriteObject.StoreAsync();
            await dataReaderObject.LoadAsync(1);
            var data = dataReaderObject.ReadByte();
            return (byte)(data & 0xFF);
        }

        async Task<byte> OnewireReadByte()
        {
            byte b = 0;
            for (byte i = 0; i < 8; i++)
            {
                // Build up byte bit by bit, LSB first
                b = (byte)((b >> 1) + 0x80 * await OnewireBit(1));
            }
            System.Diagnostics.Debug.WriteLine("onewireReadByte result: " + b);
            return b;
        }

        public async Task<double> GetTemperature(string deviceId)
        {
            double tempCelsius = 0;

            if (await OnewireReset(deviceId))
            {
                await OnewireWriteByte(0xCC); //1-Wire SKIP ROM command (ignore device id)
                await OnewireWriteByte(0x44); //DS18B20 convert T command 
                                              // (initiate single temperature conversion)
                                              // thermal data is stored in 2-byte temperature 
                                              // register in scratchpad memory

                // Wait for at least 750ms for data to be collated
                await Task.Delay(750);

                // Get the data
                await OnewireReset(deviceId);
                await OnewireWriteByte(0xCC); //1-Wire Skip ROM command (ignore device id)
                await OnewireWriteByte(0xBE); //DS18B20 read scratchpad command
                                              // DS18B20 will transmit 9 bytes to master (us)
                                              // starting with the LSB

                byte tempLSB = await OnewireReadByte(); //read lsb
                byte tempMSB = await OnewireReadByte(); //read msb

                // Reset bus to stop sensor sending unwanted data
                await OnewireReset(deviceId);

                // Log the Celsius temperature
                tempCelsius = ((tempMSB * 256) + tempLSB) / 16.0;
                var temp2 = ((tempMSB << 8) + tempLSB) * 0.0625; //just another way of calculating it

                System.Diagnostics.Debug.WriteLine("Temperature: " + tempCelsius + " degrees C " + temp2);
            }
            return tempCelsius;
        }
    }
}
