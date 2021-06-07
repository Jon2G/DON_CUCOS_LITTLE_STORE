using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Kit;
using Kit.WPF.Controls.RangoFechas;
using Kit.WPF.Reportes;
using PrintService;
using Stimulsoft.Base.Json;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.QuickButtons.Design;
using CucoStore.Docs.Models;
using DateTime = System.DateTime;
using Formatting = Newtonsoft.Json.Formatting;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace CucoStore.Docs.Data
{
    public static class Reports
    {

        private static void Print(string report, params string[] tables)
        {
            List<string> params_table = new List<string> { Tools.Instance.LibraryPath, report };
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
            Print("PrintTicket", sale.ToTable().SerializeObject(),
                sale.User.ToTable().SerializeObject(),
                sale.Payment.Payments.ToTable().SerializeObject(),
                sale.Payment.ToTable().SerializeObject(),
                sale.Parts.ToTable().SerializeObject());
        }

        public static void MovementsRegister(Rango rango)
        {
            DateTime inicio = (DateTime)rango.Inicio;
            DateTime fin = (DateTime)rango.Fin;
            rango.Inicio = new DateTime(inicio.Year, inicio.Month, inicio.Day);
            rango.Fin = new DateTime(fin.Year, fin.Month, fin.Day);

            DataTable register = AppData.SQL.DataTable("SP_MOVENTS_REGISTER", CommandType.StoredProcedure, "REGISTER",
                new SqlParameter("BEGIN", rango.Inicio),
                new SqlParameter("END", rango.Fin),
                new SqlParameter("ALL", rango.TodasLasFechas));
            DataTable data = new DataTable("Data")
            {
                Columns =
                {
                    new DataColumn("Today", typeof(DateTime)),
                    new DataColumn("Fechas", typeof(string))
                }
            };
            data.Rows.Add(DateTime.Now, $"{inicio:dd/MM/yyyy} al {fin:dd/MM/yyyy}");

            Print("MovementsRegister", register.SerializeObject(),data.SerializeObject());

        }
        public static void CorteZ()
        {
            DataTable sales = AppData.SQL.DataTable("SELECT *FROM VIEW_GETCORTEZ_SALES", CommandType.Text, "SALES");
            if (sales.Rows.Count < 0) { return; }
            float total = 0;
            int inicial = 0;
            int final = 0;
            if (sales.Rows.Count <= 0)
            {
                return;
            }
            foreach (DataRow row in sales.Rows)
            {
                total += Convert.ToSingle(row[2]);
                final = Convert.ToInt32(row[0]);
            }
            inicial =Convert.ToInt32(sales.Rows[0][0]);
            DataTable data = new DataTable("Data")
            {
                Columns =
                {
                    new DataColumn("Today", typeof(DateTime)),
                    new DataColumn("Total", typeof(float)),
                    new DataColumn("Promedio", typeof(float)),
                    new DataColumn("Id", typeof(int)),
                    new DataColumn("Inicial", typeof(int)),
                    new DataColumn("Final", typeof(int))
                }
            };
            DataTable products = AppData.SQL.DataTable("SELECT *FROM VIEW_GETCORTEZ_PRODUCTS", CommandType.Text, "PRODUCTS");
            DataTable payments = AppData.SQL.DataTable("SELECT *FROM VIEW_PAYMENTS_CORTE_Z", CommandType.Text, "PAYMENTS");
            int id = AppData.SQL.Single<int>("SP_CORTE_Z", CommandType.StoredProcedure,
                 new SqlParameter("USER_ID", AppData.Current.User.Id));
            data.Rows.Add(DateTime.Now, total, total / sales.Rows.Count, id,inicial,final);
            Print("CorteZ", sales.SerializeObject(), data.SerializeObject(), products.SerializeObject(), payments.SerializeObject());
        }
        public static void SalesReport(Rango rango)
        {
            DateTime inicio = (DateTime)rango.Inicio;
            DateTime fin = (DateTime)rango.Fin;
            rango.Inicio = new DateTime(inicio.Year, inicio.Month, inicio.Day);
            rango.Fin = new DateTime(fin.Year, fin.Month, fin.Day);

            DataTable sales = AppData.SQL.DataTable("SALES_REPORT", CommandType.StoredProcedure, "SALES",
                new SqlParameter("BEGIN", rango.Inicio),
                new SqlParameter("END", rango.Fin),
                new SqlParameter("ALL", rango.TodasLasFechas));
            if (sales.Rows.Count < 0) { return; }
            float total = 0;
            foreach (DataRow row in sales.Rows)
            {
                total += Convert.ToSingle(row[2]);
            }
            DataTable data = new DataTable("Data")
            {
                Columns =
                {
                    new DataColumn("Today", typeof(DateTime)),
                    new DataColumn("Fechas", typeof(string)),
                    new DataColumn("Total", typeof(float)),
                    new DataColumn("Promedio", typeof(float))
                }
            };
            data.Rows.Add(DateTime.Now, $"{inicio:dd/MM/yyyy} al {fin:dd/MM/yyyy}", total, total / sales.Rows.Count);

            Print("SalesReport", data.SerializeObject(), sales.SerializeObject());
        }
    }
}
