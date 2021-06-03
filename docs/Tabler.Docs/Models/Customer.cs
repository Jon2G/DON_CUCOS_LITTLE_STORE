using Kit.Sql.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Data;

namespace Tabler.Docs.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Picture { get; set; }

        public void Save()
        {
            AppData.SQL.EXEC("SP_ABC_CUSTOMER", System.Data.CommandType.StoredProcedure,
                new System.Data.SqlClient.SqlParameter("ID", Id),
                new System.Data.SqlClient.SqlParameter("NAME",Name),
                new System.Data.SqlClient.SqlParameter("NOTES",Note),
                new System.Data.SqlClient.SqlParameter("PICTURE", Picture)
                );
        }

        public static Customer GetById(int Id)
        {
            Customer customer = new Customer();
            using(IReader reader=AppData.SQL.Read("SP_SEARCHCUSTOMER", System.Data.CommandType.StoredProcedure,
                new System.Data.SqlClient.SqlParameter("ID", Id)))
            {
                while (reader.Read())
                {
                    customer = new Customer()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        Name = reader[1].ToString(),
                        Note = reader[2].ToString(),
                        Picture = reader[3].ToString()
                    };
                }
            }
            return customer;
          
        }

        public static async Task<List<Customer>> GetAll()
        {
            await Task.Yield();
            List<Customer> customers = new List<Customer>();
            foreach (int Id in AppData.SQL.Lista<int>("SELECT *FROM VIEW_GETALL"))
            {
                customers.Add(GetById(Id));
            }
            return customers;
        }

    }
}
