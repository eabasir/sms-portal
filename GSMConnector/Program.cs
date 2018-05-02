
using System;
using System.Runtime.InteropServices;

namespace GSMConnector
{
    class Programا
    {

        static QueueMaker queueMaker;

        static void Main(string[] args)
        {


            Console.SetWindowSize(
                Math.Min(160, Console.LargestWindowWidth),
                Math.Min(40, Console.LargestWindowHeight));

            Guid Sim_Id = Guid.Parse(args[0].Substring(1));

            new Logger(Sim_Id.ToString());

            queueMaker = new QueueMaker(Sim_Id);

            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);

            while (true)
                Console.ReadLine();


        }


        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                queueMaker.shutDown();
            }
            return false;
        }
        static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
                                               // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);


    }
}
