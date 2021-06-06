using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Kit;
using Kit.WPF.Reportes;
using PrintService;
using Stimulsoft.Base.Json;
using Stimulsoft.Report.QuickButtons.Design;
using Tabler.Docs.Models;
using Formatting = Newtonsoft.Json.Formatting;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Tabler.Docs.Data
{
    public static class Reports
    {

        private static void Print(params string[] tables)
        {
            List<string> params_table = new List<string> { Tools.Instance.LibraryPath, "PrintTicket" };
            foreach (var s in tables)
            {
                string name = $"{Guid.NewGuid():N}.xml";
                string path = $"{Tools.Instance.LibraryPath}\\{name}";
                File.WriteAllText(path, s);
                params_table.Add(name);
            }
            Process.Start($"{Tools.Instance.LibraryPath}\\PrintService.exe", params_table);
        }
        public static void PrintTicket(Sale sale)
        {
            if (!sale.Parts.Any())
            {
                return;
            }
            Print(sale.ToTable().SerializeObject(),
                sale.User.ToTable().SerializeObject(),
                sale.Payment.Payments.ToTable().SerializeObject(),
                sale.Payment.ToTable().SerializeObject(),
                sale.Parts.ToTable().SerializeObject());
        }
    }
}
