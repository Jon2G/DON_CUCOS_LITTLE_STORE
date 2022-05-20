using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kit.Sql.Readers;
using CucoStore.Docs.Data;
using CucoStore.Docs.Interfaces;
using CucoStore.Docs.Models;

namespace CucoStore.Docs.ViewModels
{
    public class CategoryAddViewModel : IRefresh
    {
        public Category Category { get; set; }
        public CategoryAddViewModel()
        {
            Category = new();
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
                Category = Category.GetById(SupplierId);
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
