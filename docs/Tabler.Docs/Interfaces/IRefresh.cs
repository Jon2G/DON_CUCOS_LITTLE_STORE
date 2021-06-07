using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CucoStore.Docs.Interfaces
{
    public interface IRefresh
    {
        public bool IsLoading { get; set; }
        public event EventHandler Refreshed;
        public Task Refresh();


    }
}
