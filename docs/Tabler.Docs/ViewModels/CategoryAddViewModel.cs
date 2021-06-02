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

        public static void Save(Category category)
        {
            AppData.SQL.EXEC("SP_ABC_CATEGORY", CommandType.StoredProcedure,
                new SqlParameter("ID",category.Id),
                new SqlParameter("DESCRIPTION", category.Descripcion));
        }

        public static Category GetById(int Id)
        {
            Category category = null;
            using (IReader reader = AppData.SQL.Read("SP_GET_CATEGORY_BY_ID", System.Data.CommandType.StoredProcedure,
                new System.Data.SqlClient.SqlParameter("ID", Id)))
            {
                while (reader.Read())
                {
                    category = new Category()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        Descripcion = reader[1].ToString()
                    };
                }
            }
            return category;

        }
    }
}
