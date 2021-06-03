using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
    public class CustomerViewModel:IRefresh
    {
        public Customer Customer { get; set; }
        public CustomerViewModel()
        {
            Customer = new Customer();
        }

        public bool IsLoading { get; set; }
        public event EventHandler Refreshed;
        public int CustomerId { get; set; }
        public async Task RequestLoad(int CustomerId)
        {
            this.CustomerId = CustomerId;
            await Refresh();
        }


        public async Task Refresh()
        {
            try
            {
                await Task.Yield();
                IsLoading = true;
                Customer =  Customer.GetById(CustomerId);
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
