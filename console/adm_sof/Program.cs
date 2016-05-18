using System;
using System.Collections.Generic;
using System.DirectoryServices;
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
            var myVar = Program.VisibleComputers();

            List<string> programs = p.PrintInstalledPrograms();

            foreach (var miniVar in myVar)
                Console.WriteLine(miniVar);

            Console.WriteLine("Fin de loop");
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

        public static IEnumerable<string> VisibleComputers(bool workgroupOnly = false)
        {
            Func<string, IEnumerable<DirectoryEntry>> immediateChildren = key => new DirectoryEntry("WinNT:" + key)
                    .Children
                    .Cast<DirectoryEntry>();
            Func<IEnumerable<DirectoryEntry>, IEnumerable<string>> qualifyAndSelect = entries => entries.Where(c => c.SchemaClassName == "Computer")
                    .Select(c => c.Name);
            return (
                !workgroupOnly ?
                    qualifyAndSelect(immediateChildren(String.Empty)
                        .SelectMany(d => d.Children.Cast<DirectoryEntry>()))
                    :
                    qualifyAndSelect(immediateChildren("//WORKGROUP"))
            ).ToArray();
        }
    }
}
