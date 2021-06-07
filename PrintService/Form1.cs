using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kit;
using Kit.WPF.Reportes;
using Newtonsoft.Json;

namespace PrintService
{
    public partial class Form1 : Form
    {
        private readonly string[] args;
        public Form1(string[] args)
        {
            this.args = args;
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (args?.Any() ?? false)
                {
                    List<string> xml_tables = new List<string>(args);
                    List<Variable> tables = new List<Variable>();
                    xml_tables.RemoveAt(0);
                    xml_tables.RemoveAt(0);
                    foreach (string file in xml_tables)
                    {
                        string path = $"{Tools.Instance.LibraryPath}\\{file}";
                        string xml = File.ReadAllText(path);
                        File.Delete(path);
                        tables.Add(new Variable(xml.XmlDeserializeFromString<DataTable>()));
                    }
                    switch (args[1])
                    {
                        case "PrintTicket":
                            Reports.PrintTicket(tables.ToArray());
                            break;
                        case "SalesReport":
                            Reports.SalesReport(tables.ToArray());
                            break;
                        case "CorteZ":
                            Reports.CorteZ(tables.ToArray());
                            break;
                        case "MovementsRegister":
                            Reports.MovementsRegister(tables.ToArray());
                            break;
                    }
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex?.ToString()??"What?");
                throw;
            }
        }

 
    }
}
