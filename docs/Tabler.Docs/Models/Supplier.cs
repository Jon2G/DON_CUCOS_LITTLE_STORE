using Kit.Model;
using Kit.Sql.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Data;

namespace Tabler.Docs.Models
{
   public class Supplier
    {
        public int Id { get; set; }
        public string  Picture { get; set; }
        public string Name { get; set; }
        public string Cellphone { get; set; }
        public string Notes { get; set; }
        public bool Enabled { get; set; }

        public async Task Save()
        {
            await Task.Yield();
            AppData.SQL.EXEC("SP_ABC_SUPPLIERS", System.Data.CommandType.StoredProcedure,
                new System.Data.SqlClient.SqlParameter("Id", Id),
                new System.Data.SqlClient.SqlParameter("PICTURE", Picture),
                new System.Data.SqlClient.SqlParameter("NAME", Name),
                new System.Data.SqlClient.SqlParameter("CELLPHONE", Cellphone),
                new System.Data.SqlClient.SqlParameter("NOTES", Notes),
                 new System.Data.SqlClient.SqlParameter("ENABLED", Enabled)
                );
        }
        public static Supplier GetById(int Id)
        {
            Supplier proveedor = null;
            using (IReader reader = AppData.SQL.Read("SP_GETIDSUPPLIERS", System.Data.CommandType.StoredProcedure,
                new System.Data.SqlClient.SqlParameter("ID", Id)))
            {
                while (reader.Read())
                {
                    proveedor = new Supplier()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        Name = reader[2].ToString(),
                        Cellphone = reader[3].ToString(),
                        Notes = reader[4].ToString(),
                        Picture = reader[1].ToString(),
                        Enabled = Convert.ToBoolean(reader[5])
                    };
                }
            }
            return proveedor;

        }
        public static async Task<List<Supplier>> GetAll()
        {
            await Task.Yield();
            List<Supplier> proveedor = new List<Supplier>();
            foreach (int Id in AppData.SQL.Lista<int>("SELECT *FROM VIEW_GETALLSUPPLIERS"))
            {
                proveedor.Add(GetById(Id));
            }
            return proveedor;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Supplier supplier)
            {
                return supplier.Id == this.Id;
            }
            return false;
        }
    }
}