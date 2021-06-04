using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
    public class CategoriesPageViewModel : IRefresh
    {
        public bool IsLoading { get; set; }
        public List<Category> Categories { get; set; }
        public CategoriesPageViewModel()
        {
            Categories = new List<Category>();
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
                this.Categories.Clear();
                this.Categories.AddRange(await Category.GetAll());
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
