using HidLibrary;

namespace MouseMovementLibraries.ArduinoSupport
{
    internal class ArduinoMouse
    {
        private const int MOUSE_LEFT = 0x01;
        private const int MOUSE_RIGHT = 0x02;
        private const int MOUSE_MIDDLE = 0x04;
        private const int MOUSE_ALL = MOUSE_LEFT | MOUSE_RIGHT | MOUSE_MIDDLE;
        
        private static int buttonsMask = 0;
        public static HidDevice? Arduino;

        private static void SetButtons(int buttons)
        {
            if (buttons != buttonsMask)
            {
                buttonsMask = buttons;
                Move(0, 0);
            }
        }

        public static void Click(int button = MOUSE_LEFT)
        {
            buttonsMask = button;
            Move(0, 0);
            buttonsMask = 0;
            Move(0, 0);
        }

        public static void Press(int button = MOUSE_LEFT)
        {
            SetButtons(buttonsMask | button);
        }

        public static void Release(int button = MOUSE_LEFT)
        {
            SetButtons(buttonsMask & ~button);
        }

        public static bool IsPressed(int button = MOUSE_LEFT)
        {
            return (button & buttonsMask) != 0;
        }

        public static void Move(int x, int y)
        {
            int limitedX = LimitXY(x);
            int limitedY = LimitXY(y);
            SendRawReport(MakeReport(limitedX, limitedY));
        }

        private static byte[] MakeReport(int x, int y)
        {
            return new byte[]
            {
                0x01,   // Report ID: 0
                (byte)buttonsMask,
                LowByte(x), HighByte(x),
                LowByte(y), HighByte(y)
            };
        }

        private static void SendRawReport(byte[] reportData)
        {
            Arduino?.Write(reportData);
        }

        private static int LimitXY(int xy)
        {
            if (xy < -32767) return -32767;
            if (xy > 32767) return 32767;
            return xy;
        }

        private static byte LowByte(int x)
        {
            return (byte)(x & 0xFF);
        }

        private static byte HighByte(int x)
        {
            return (byte)((x >> 8) & 0xFF);
        }
    }
}
