using HidLibrary;
using System.Windows;
using Class;

namespace MouseMovementLibraries.ArduinoSupport
{
    internal class ArduinoMain
    {
        public static bool Load(int pingCode = 0xf9)
        {
            var devices = HidDevices.Enumerate();
            foreach (var dev in devices)
            {
                dev.OpenDevice();
                if (CheckPing(dev, pingCode))
                {
                    Arduino.arduino = dev;
                    return true;
                }
                else
                {
                    dev.CloseDevice();
                }
            }
            MessageBox.Show("No Arduino device found", "Aimmy");
            return false;
        }

        private static bool CheckPing(HidDevice dev, int pingCode)
        {
            dev.Write(new byte[] { 0, (byte)pingCode });
            try
            {
                var response = dev.Read(10);
                return response.Status == HidDeviceData.ReadStatus.Success && response.Data[1] == (byte)pingCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception during ping: {ex.Message}", "Aimmy");
                return false;
            }
        }
    }
}
