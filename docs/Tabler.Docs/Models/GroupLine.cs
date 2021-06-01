using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;

namespace Tabler.Docs.Models
{
    public class GroupLine:IRefresh
    {
        public bool IsLoading { get; set; }
        public event EventHandler Refreshed;

        public List<Product> Products { get; set; }
        public string Name { get; set; }
        public GroupLine(string Name)
        {
            this.Name = Name;
            this.Products = new List<Product>();
        }

        public async Task Refresh()
        {
            try
            {
                Products.Clear();
                IsLoading = true;
                await Task.Delay(1000);
                this.Products.Add(new Product("0001","HELADO"));
                this.Products.Add(new Product("0002", "PAPITAS"));
                this.Products.Add(new Product("0003", "REFRESQUITO"));
                this.Products.Add(new Product("0004", "BOTANAS"));
                this.Products.Add(new Product("0005", "LACTEOS"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                IsLoading = false;
                Refreshed?.Invoke(this,EventArgs.Empty);
            }
        }
    }
}
