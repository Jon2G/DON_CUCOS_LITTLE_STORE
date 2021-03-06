using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Data;
using CucoStore.Docs.Interfaces;
using CucoStore.Docs.Models;
using Customer = CucoStore.Docs.Models.Customer;

namespace CucoStore.Docs.ViewModels
{
    public class SalesPageViewModel 
    {
        public bool IsLoading { get; set; }
        public Sale Sale { get; set; }
        public List<GroupLine> Lines { get; set; }
        public GroupLine GroupLine { get; set; }
        public List<Customer> Customers { get; set; }

        public bool IsInCategory => (GroupLine?.Category?.Id ?? 0) > 0;


        public int PageSize
        {
            get
            {
                int size = GroupLine.Products.Count / 4;
                if (size < 4)
                {
                    size = 4;
                }

                return size;
            }
        }

        public SalesPageViewModel()
        {
            this.GroupLine = new GroupLine(new Category());
            this.Customers = new List<Customer>();
            this.Lines = new List<GroupLine>();
            this.Sale = new Sale()
            {
                User = AppData.Current.User
            };
        }


        public async Task LoadLine(GroupLine line)
        {
            IsLoading = true;
            await Task.Yield();
            await line.Refresh();
            IsLoading = false;
        }
    }
}
