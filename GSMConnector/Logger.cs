using SMSPortalCross;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMConnector
{
    class Logger
    {
        string Dir_Log;
        static string File_Activity;


        public Logger(string simId)
        {
            try
            {
                Dir_Log = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
                File_Activity = Path.Combine(Dir_Log, "sim-" + simId + ".txt");

                if (!Directory.Exists(Dir_Log))
                    Directory.CreateDirectory(Dir_Log);


                if (!File.Exists(File_Activity))
                {
                    using (var fa = File.Create(File_Activity))
                    {
                        fa.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Log(Messages.LOGGER_FILE_CREATE, e);
            }



        }

        public static void Log(int errorCode, Exception exp)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(File_Activity))
                {
                    sw.Write("\r\n Error Code: {0} {1} {2} :", errorCode, DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
                    sw.WriteLine("\r\n Message:\r\n {0}", exp.Message);
                    if (exp.InnerException != null)
                        sw.WriteLine("\r\n Inner Message:\r\n {0}", exp.InnerException.Message);
                    sw.WriteLine("\r\n----------------------------------------------");
                    sw.Close();

                }
            }
            catch { }

        }


        public static void Show(string message, ConsoleColor color)
        {

            Console.ResetColor();
            Console.ForegroundColor = color;
            Console.WriteLine(message);


        }

        internal static void ShowError(string error)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ResetColor();
        }
    }
}
