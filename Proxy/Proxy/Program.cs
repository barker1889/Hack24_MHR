using System.Runtime.InteropServices;
using Topshelf;

namespace Proxy
{
    public class Program
    {
        private static readonly GmailProxy Controller = new GmailProxy();

        public static void Main(string[] args)
        {
            NativeMethods.Handler = ConsoleEventCallback;
            NativeMethods.SetConsoleCtrlHandler(NativeMethods.Handler, true);
            HostFactory.Run(x =>
            {
                x.Service<GmailProxy>(p =>
                {
                    p.ConstructUsing(name => new GmailProxy());
                    p.WhenStarted(ptc => ptc.StartProxy());
                    p.WhenStopped(ptc => ptc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Analyser");
                x.SetDisplayName("Analyser");
                x.SetServiceName("Analyser");
            });
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