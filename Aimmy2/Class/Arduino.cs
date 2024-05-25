using HidLibrary;

namespace Class
{
    internal class Arduino
    {
        public static HidDevice? arduino { get; set; }

        public static void Close()
        {
            if (arduino != null)
            {
                arduino.CloseDevice();
                arduino = null;
            }
        }
    }
}
