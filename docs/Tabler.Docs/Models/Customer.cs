using Kit.Sql.Readers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management;
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
        public bool Enabled { get; set; }

        public void Save()
        {
            AppData.SQL.EXEC("SP_ABC_CUSTOMER", System.Data.CommandType.StoredProcedure,
                new System.Data.SqlClient.SqlParameter("ID", Id),
                new System.Data.SqlClient.SqlParameter("NAME",Name),
                new System.Data.SqlClient.SqlParameter("NOTES",Note),
                new System.Data.SqlClient.SqlParameter("PICTURE", Picture),
                new System.Data.SqlClient.SqlParameter("ENABLED", Enabled)
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
                        Picture = reader[3].ToString(),
                        Enabled = Convert.ToBoolean(reader[4])
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
        public override bool Equals(object? obj)
        {
            if (obj is Customer customer)
            {
                return customer.Id == this.Id;
            }
            return false;
        }

        public static Customer Default()
        {
            return Customer.GetById(1);
        }

      
    }
}
