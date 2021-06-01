using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
    public class ProductsPageViewModel:IRefresh
    {
        public bool IsLoading { get; set; }
        public List<GroupLine> Lines { get; set; }
        public event EventHandler Refreshed;

        public ProductsPageViewModel()
        {
            this.Lines = new List<GroupLine>();
        }

        public async Task Refresh()
        {
            try
            {
                this.Lines.Clear();
                IsLoading = true;
                await Task.Delay(1000);
                this.Lines.Add(new GroupLine("HELADO"));
                this.Lines.Add(new GroupLine("PAPITAS"));
                this.Lines.Add(new GroupLine("REFRESQUITO"));
                this.Lines.Add(new GroupLine("BOTANAS"));
                this.Lines.Add(new GroupLine("LACTEOS"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                IsLoading = false;
                Refreshed?.Invoke(this, EventArgs.Empty);
            }

        }


    }
}
