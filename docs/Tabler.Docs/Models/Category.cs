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
        public string Picture { get; set; }

        public static async Task<List<Category>> GetAll()
        {
            await Task.Yield();
            List<Category> lineas = new List<Category>();
            foreach (int id in AppData.SQL.Lista<int>("SP_GET_CATEGORIES", CommandType.StoredProcedure))
            {
                lineas.Add(GetById(id));
            }
            return lineas;
        }
        public static async Task<IEnumerable<Category>> Search(string search)
        {
            await Task.Yield();
            List<Category> lineas = new List<Category>();
            foreach (int id in AppData.SQL.Lista<int>("SP_FIND_CATEGORIES", CommandType.StoredProcedure,0,
                new SqlParameter("SEARCH",search)))
            {
                lineas.Add(GetById(id));
            }
            return lineas;
        }

        public static Category GetByName(string categoryName)
        {
            int id = AppData.SQL.Single<int>("SP_GET_CATEGORY_BY_NAME", CommandType.StoredProcedure,
                new SqlParameter("NAME", categoryName));
            return GetById(id);
        }

        public void Save()
        {
            AppData.SQL.EXEC("SP_ABC_CATEGORY", CommandType.StoredProcedure,
                new SqlParameter("ID", Id),
                new SqlParameter("DESCRIPTION", Description),
                new SqlParameter("PICTURE", Picture)
                );
        }

        public static Category GetById(int Id)
        {
            Category category = new Category();
            if (Id <= 0)
            {
                return category;
            }
      
            using (IReader reader = AppData.SQL.Read("SP_GET_CATEGORY_BY_ID", System.Data.CommandType.StoredProcedure,
                new System.Data.SqlClient.SqlParameter("ID", Id)))
            {
                while (reader.Read())
                {
                    category = new Category()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        Description = reader[1].ToString(),
                        Picture =Convert.ToString(reader[2])
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
