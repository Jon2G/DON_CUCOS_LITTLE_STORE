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
        public string Description { get; set; }

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
                        Description = reader["DESCRIPTION"]?.ToString()
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
                        Description = reader["DESCRIPTION"]?.ToString()
                    });
                }
            }

            return category;
        }

        public void Save()
        {
            AppData.SQL.EXEC("SP_ABC_CATEGORY", CommandType.StoredProcedure,
                new SqlParameter("ID", Id),
                new SqlParameter("DESCRIPTION", Description));
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
                        Description = reader[1].ToString()
                    };
                }
            }
            return category;

        }

        public override string ToString() => $"{Id} - {Description}";
        public override bool Equals(object? obj)
        {
            if (obj is Category category)
            {
                return category.Id == this.Id;
            }
            return false;
        }

    }
}
