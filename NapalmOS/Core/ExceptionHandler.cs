using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Hardware;

namespace NapalmOS.Core
{
    public static class ExceptionHandler
    {
        // log messages
        public static List<string> LogMessages = new List<string>();

        // throw ok 
        public static void ThrowOK(string msg)
        {
            Terminal.Write("[ ");
            Terminal.Write("OK", ConsoleColor.Green);
            Terminal.Write(" ] ");
            Terminal.WriteLine(msg);
        }

        // throw warning
        public static void ThrowWarning(string msg)
        {
            Terminal.Write("[ ");
            Terminal.Write("??", ConsoleColor.DarkYellow);
            Terminal.Write(" ] ");
            Terminal.WriteLine(msg);
        }

        // throw error
        public static void ThrowError(string msg)
        {
            Terminal.Write("[ ");
            Terminal.Write("!!", ConsoleColor.Red);
            Terminal.Write(" ] ");
            Terminal.WriteLine(msg);
        }

        // throw fatal crash
        public static void ThrowFatal(string msg, string details)
        {
            // set video mode
            VGADriver.SetMode(VideoMode.Text80x25);

            // clear screen
            Terminal.BackColor = ConsoleColor.DarkRed;
            Terminal.TextColor = ConsoleColor.White;
            Terminal.Clear();

            // message
            Terminal.WriteLine("NapalmOS has crashed");
            Terminal.WriteLine("ERROR: " + msg);
            Terminal.WriteLine(details);

            // wait
            for (int i = 0; i < 5; i++) { Cosmos.HAL.Global.PIT.Wait(1000); }

            // reboot on key press
            Terminal.ReadLine();
            Cosmos.System.Power.Reboot();
        }

        // add message to log
        public static void Log(string msg) { LogMessages.Add(msg); }

        // print log values
        public static void PrintLog()
        {
            for (int i = 0; i < LogMessages.Count; i++)
            {
                Console.WriteLine(LogMessages[i]);
            }
        }
    }
}
