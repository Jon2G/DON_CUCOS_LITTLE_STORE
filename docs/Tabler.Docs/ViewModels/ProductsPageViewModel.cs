using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
    public class ProductsPageViewModel : IRefresh
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
                if (IsLoading)
                {
                    return;
                }
                IsLoading = true;
                this.Lines.Clear();
                foreach (Category linea in await Category.GetAll())
                {
                    var gropup= new GroupLine(linea);
                    await gropup.Refresh();
                    this.Lines.Add(gropup);
                }
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
