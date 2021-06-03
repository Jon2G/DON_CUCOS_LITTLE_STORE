using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
  public class ProveedoresViewModel:IRefresh
    {
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public ProveedoresViewModel()
        {
            Supplier = new Supplier();
        }

        public bool IsLoading { get; set; }
        public event EventHandler Refreshed;
        public async Task RequestLoad(int SupplierId)
        {
            this.SupplierId = SupplierId;
            await Refresh();
        }

        public async Task Refresh()
        {
            try
            {
                await Task.Yield();
                IsLoading = true;
                Supplier = Supplier.GetById(SupplierId);
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
