using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kit.Sql.Readers;
using Tabler.Docs.Data;

namespace Tabler.Docs.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }


        public static void ABC(Category category)
        {
            AppData.SQL.EXEC("SP_ABC_CATEGORY", CommandType.Text, 
                new SqlParameter("ID",category.Id),
                new SqlParameter("DESCRIPTION", category.Descripcion));
        }
        public static async Task<List<Category>> GetAll()
        {
            List<Category> lineas = new List<Category>();
            using (IReader reader = AppData.SQL.Read("SP_GET_CATEGORIES", CommandType.StoredProcedure))
            {
                while (reader.Read())
                {
                    lineas.Add(new Category()
                    {
                        Id =Convert.ToInt32(reader["ID"]),
                        Descripcion = reader["DESCRIPTION"]?.ToString()
                    });
                }
            }
            return lineas;
        }

        public static Category GetByName(string categoryName)
        {
            Category category = null;
            using (IReader reader = AppData.SQL.Read("SP_GET_CATEGORY_BY_NAME", CommandType.StoredProcedure,
                new SqlParameter("NAME",categoryName)))
            {
                while (reader.Read())
                {
                    category=(new Category()
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Descripcion = reader["DESCRIPTION"]?.ToString()
                    });
                }
            }

            return category;
        }
    }
}
