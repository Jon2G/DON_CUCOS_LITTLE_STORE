using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kit.Sql.Readers;
using CucoStore.Docs.Data;

namespace CucoStore.Docs.Models
{
    public class Stock
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public float Quantity { get; set; }
        public float Minimum { get; set; }
        public float Maximum { get; set; }
        public bool OverStocked => Quantity >= Maximum;
        public bool UnderStocked => Quantity <= Minimum;

        public string Status
        {
            get
            {
                if (OverStocked)
                {
                    return "Sobre inventario";
                }
                else if (UnderStocked)
                {
                    return "Inventario mínimo";
                }
                return "Ok";
            }
        }
        public string CssClass
        {
            get
            {
                if (OverStocked)
                {
                    return "over_stocked";
                }
                else if (UnderStocked)
                {
                    return "under_stocked";
                }
                return String.Empty;
            }
        }

        internal static async Task<IEnumerable<Stock>> GetAll()
        {
            await Task.Yield();
            List<Stock> stocks = new List<Stock>();
            using (IReader reader = AppData.SQL.Read("select * FROM VIEW_GET_STOCK"))
            {
                while (reader.Read())
                {
                    stocks.Add(new Stock()
                    {
                        Code = Convert.ToString(reader[0]),
                        Name = Convert.ToString(reader[1]),
                        Quantity = (float)Convert.ToDouble(reader[2]),
                        Minimum = (float)Convert.ToDouble(reader[3]),
                        Maximum = (float)Convert.ToDouble(reader[4])
                    });
                }
            }
            return stocks;
        }
    }
}
