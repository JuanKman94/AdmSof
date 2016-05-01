using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace adm_sof
{
    class Program
    {
        string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        static void Main(string[] args)
        {
            Program p = new Program();
            List<string> programs = p.PrintInstalledPrograms();

            Console.ReadLine();
        }

        public List<string> PrintInstalledPrograms()
        {
            List<string> programs = new List<string>();
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {

                            var displayName = sk.GetValue("DisplayName");
                            var size = sk.GetValue("EstimatedSize");

                            string item;
                            if (displayName != null)
                            {
                                item = displayName.ToString();
                                programs.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine("err: Exception =>", ex);
                        }
                    }
                }

                Int32 i = 1;

                foreach (var program in programs)
                    System.Console.WriteLine(i++ + " - " + program);

                return programs;
            }
        }
    }
}
