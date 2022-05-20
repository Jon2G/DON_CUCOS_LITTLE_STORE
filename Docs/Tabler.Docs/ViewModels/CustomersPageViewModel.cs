using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Interfaces;
using CucoStore.Docs.Models;

namespace CucoStore.Docs.ViewModels
{
   public class CustomersPageViewModel : IRefresh
    {
        public bool IsLoading { get; set; }
       public List<Customer> Customers { get; set; }
        public CustomersPageViewModel()
        {
            Customers = new List<Customer>();
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
                this.Customers.Clear();
                this.Customers.AddRange(await Customer.GetAll());
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
