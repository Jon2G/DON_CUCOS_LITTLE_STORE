using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kit.Sql.SqlServer;

namespace Tabler.Docs.Data
{
    public class AppData
    {
        public static AppData Current { get; private set; }
        public static SQLServerConnection SQL { get; private set; }

        private AppData()
        {
            
        }

        public static void Init()
        {
            Current = new AppData();
            SQL = new SQLServerConnection("DON_CUCO", ".\\SQLXEXPRESS", "1433", "sa", "12345678");
            DirectoryInfo directory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "\\Library"));
            if (!directory.Exists)
            {
                directory.Create();
            }
            Kit.WPF.Tools.Init(directory.FullName);
        }
    }
}
