using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Interfaces;
using CucoStore.Docs.Models;

namespace CucoStore.Docs.ViewModels
{
   public class SuppliersPageViewModel:IRefresh
    {
        public bool IsLoading { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public SuppliersPageViewModel()
        {
            Suppliers = new List<Supplier>();
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
                this.Suppliers.Clear();
                this.Suppliers.AddRange(await Supplier.GetAll());
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
