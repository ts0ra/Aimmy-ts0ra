﻿using Accord.Math.Distances;
using HidLibrary;
using System.Diagnostics;
using System.Security.Policy;
using System.Windows;
using Visuality;

namespace MouseMovementLibraries.ArduinoSupport
{
    internal class ArduinoMain
    {
        public static void Close()
        {
            if (ArduinoMouse.Arduino != null)
            {
                ArduinoMouse.Arduino.CloseDevice();
                ArduinoMouse.Arduino = null;
            }
        }

        public static bool Load(int pingCode = 0xf9)
        {
            // Check if the Arduino is already loaded
            if (ArduinoMouse.Arduino != null)
            {
                new NoticeBar("Arduino device already loaded", 2000).Show();
                return true;
            }

            var devices = HidDevices.Enumerate();
            foreach (var dev in devices)
            {
                dev.OpenDevice();

                (short inputReport, short outputReport) = (dev.Capabilities.InputReportByteLength, dev.Capabilities.OutputReportByteLength);
                if (inputReport != 65 && outputReport != 65)
                {
                    dev.CloseDevice();
                    continue;
                }

                if (PING(dev, pingCode))
                {
                    new NoticeBar("Arduino loaded", 2000).Show();
                    ArduinoMouse.Arduino = dev;
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

        private static bool PING(HidDevice dev, int pingCode)
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
