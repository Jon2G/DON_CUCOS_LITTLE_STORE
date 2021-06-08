using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kit;
using Kit.Extensions;
using Kit.Sql.SqlServer;
using Kit.WPF.Extensions;
using Microsoft.AspNetCore.Components.Forms;
using CucoStore.Docs.Models;
using System.ComponentModel;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace CucoStore.Docs.Data
{
    public class AppData
    {
        public static AppData Current { get; private set; }
        public static SQLServerConnection SQL { get; private set; }
        public User User { get; set; }
        private AppData()
        {

        }

        public static void Init()
        {
            Current = new AppData();
            SQL = new SQLServerConnection("DON_CUCO", ".\\SQLXEXPRESS",
                "1433", "sa", "12345678");
            DirectoryInfo directory = new DirectoryInfo($"{Environment.CurrentDirectory}\\Library");
            if (!directory.Exists)
            {
                directory.Create();
            }
            Kit.WPF.Tools.Init(directory.FullName);
            Current.User = new User();
            //{
            //    Id = 1,
            //    Nickname = "CUCO",
            //    Permissions = new Permissions()
            //    {
            //        UserManager = true,
            //        ReadOnly = false,
            //        ReadReports = true,
            //        Sales = true,
            //        StockIn = true,
            //        StockOut = true
            //    }
            //};
        }

        public static async Task<string> Compress(IBrowserFile file)
        {
            using (Stream stream = file.OpenReadStream())
            {
                try
                {
                    string path = Path.Combine(Kit.Tools.Instance.LibraryPath, $"{Guid.NewGuid()}.png");
                    using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        await stream.CopyToAsync(fileStream);
                    }

                    System.Drawing.Image myImage = Image.FromFile(path, true);
                    string lowbase64 = await myImage.CompressAsJpeg(10).ToImageString();
                    return lowbase64;
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "Compress");
                    return await stream.ToImageString();
                }
            }

        }
    }
}
