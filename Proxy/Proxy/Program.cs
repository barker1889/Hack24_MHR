using System;
using System.Runtime.InteropServices;

namespace Proxy
{
    public class Program
    {
        private static readonly ProxyTestController Controller = new ProxyTestController();

        public static void Main(string[] args)
        {
            NativeMethods.Handler = ConsoleEventCallback;
            NativeMethods.SetConsoleCtrlHandler(NativeMethods.Handler, true);
            Controller.StartProxy();
            Console.WriteLine();
            Console.Read();

            Controller.Stop();
        }


        private static bool ConsoleEventCallback(int eventType)
        {
            if (eventType != 2) return false;
            try
            {
                Controller.Stop();
            }
            catch
            {
                // ignored
            }
            return false;
        }
    }

    internal static class NativeMethods
    {
        internal static ConsoleEventDelegate Handler;
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        internal delegate bool ConsoleEventDelegate(int eventType);
    }
}