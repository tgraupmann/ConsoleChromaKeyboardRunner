using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleChromaKeyboardRunner
{
    class Program
    {
        private static ConsoleEventDelegate _mHandler;   // Keeps it from getting garbage collected
        
        // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                Console.WriteLine("Console window closing, death imminent");
                KeyboardRunner.Stop();
            }
            return false;
        }

        static void Main(string[] args)
        {
            _mHandler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(_mHandler, true);

            KeyboardRunner.Start();
            ThreadStart ts = new ThreadStart(KeyboardRunner.Update);
            Thread thread = new Thread(ts);
            thread.Start();
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
            KeyboardRunner.Stop();
        }
    }
}
