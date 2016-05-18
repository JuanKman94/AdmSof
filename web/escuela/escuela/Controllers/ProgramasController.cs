using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using escuela.Models;
using Microsoft.Win32;

namespace escuela.Controllers
{
    public class ProgramasController : Controller
    {
        private string UninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        public ViewResult Index()
        {
            List<Programa> programas = ObtenProgramas();

            return View(programas);
        }

        private List<Programa> ObtenProgramas()
        {
            int counter = 1;
            List<Programa> programas = new List<Programa>();

            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(UninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {
                            var displayName = sk.GetValue("DisplayName");

                            if (displayName != null)
                            {
                                Programa programa = new Programa();

                                programa.Nombre = displayName.ToString();
                                programa.Id = counter++;
                                programa.Tamanho = (Int32)sk.GetValue("EstimatedSize");

                                programas.Add(programa);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine("err: Exception =>", ex);
                        }
                    }
                }
            }

            return programas;
        }
    }
}
