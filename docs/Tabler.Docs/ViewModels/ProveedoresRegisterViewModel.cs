using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Interfaces;
using CucoStore.Docs.Models;

namespace CucoStore.Docs.ViewModels
{
  public class ProveedoresRegisterViewModel:IRefresh
    {

        public Supplier Supplier { get; set; }
        public ProveedoresRegisterViewModel()
        {
            Supplier = new Supplier();
        }
        public int SupplierId { get; set; }
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
