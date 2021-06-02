using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kit.Sql.Readers;
using Tabler.Docs.Data;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
    public class CategoryAddViewModel
    {
        public Category Category { get; set; }
        public CategoryAddViewModel()
        {
            Category = new ();
        }

    }
}
