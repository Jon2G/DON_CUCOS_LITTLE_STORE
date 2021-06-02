using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
   public class ProveedoresPageViewModel:IRefresh
    {
        public bool IsLoading { get; set; }
        public List<Supplier> Proveedores { get; set; }
        public ProveedoresPageViewModel()
        {
            Proveedores = new List<Supplier>();
        }
        public event EventHandler Refreshed;

        public async Task Refresh()
        {
            try
            {
                if (IsLoading)
                {
                    return;
                }
                IsLoading = true;
                this.Proveedores.Clear();
                this.Proveedores.AddRange(await Supplier.GetAll());
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
