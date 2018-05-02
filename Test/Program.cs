using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {

        static string File_Activity;
        static void Main(string[] args)
        {

            try
            {
                File_Activity = Path.Combine(@"C:\Users\eabasir\Desktop", "contacts.vcf");
                
                if (!File.Exists(File_Activity))
                {
                    using (var fa = File.Create(File_Activity))
                    {
                        fa.Close();
                 
                    }
                    
                }

                File.WriteAllText(File_Activity, String.Empty);

                using (StreamWriter sw = File.AppendText(File_Activity))
                {

                    for (int i = 0; i < 2000; i++)
                    {
                        string name = "name " + i;
                        string family = "family " + i;
                        string email = "number " + i;
                        string number = 989121111111 + i+ "";

                        sw.WriteLine("BEGIN:VCARD");
                        sw.WriteLine(string.Format("N:{0};{1};;;", family, name));
                        sw.WriteLine(string.Format("EMAIL;INTERNET:{0}", email));
                        sw.WriteLine(string.Format("TEL;CELL:{0}", number));
                        sw.WriteLine(string.Format("END:VCARD"));
                    }

                    sw.Close();

                }


            }
            catch (Exception e)
            {

            }
        }
    }
}
