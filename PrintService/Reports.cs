using System.Diagnostics;
using System.Windows.Forms;
using Kit.WPF.Reportes;

namespace PrintService
{
    public static class Reports
    {
        public static readonly Reporteador Reporteador;
        static Reports()
        {
            Reporteador = new Reporteador($"{Kit.Tools.Instance.LibraryPath}\\Logo.png");
        }

        public static void PrintTicket(Variable[] variable)
        {
            string file = Reporteador.NuevoReporte("Ticket.mrt", false, true, FormatoReporte.PDF, variable);
            Process.Start(file);
        }
    }
}
