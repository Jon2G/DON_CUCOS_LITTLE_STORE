using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kit.WPF.Reportes;
using Tabler.Docs.Models;

namespace Tabler.Docs.Data
{
    public static class Reports
    {
        public static readonly Reporteador Reporteador;
        static Reports()
        {
            Reporteador = new Reporteador( $"{Kit.Tools.Instance.LibraryPath}\\Logo.png");
        }

        public static void PrintTicket(Sale sale)
        {
            
        }
    }
}
