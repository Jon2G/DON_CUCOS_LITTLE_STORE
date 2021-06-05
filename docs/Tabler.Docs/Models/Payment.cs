using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Data;

namespace Tabler.Docs.Models
{
    public class Payment
    {
        public float Total => Payments.Sum(x => x.Amount);
        public List<Pay> Payments { get; set; }
        public Payment()
        {
            Payments = new List<Pay>(new[]
            {
                new Pay(),
                new Pay(),
                new Pay()
            });
        }

        public Pay GetPay(int p0)
        {
            return Payments[p0];
        }

        public void Save(Sale sale)
        {
            foreach (Pay pay in this.Payments.Where(x=>x.Amount>0))
            {
                pay.Save();
                AppData.SQL.EXEC("SP_SAVE_SALE_PAYS", CommandType.StoredProcedure,
                    new SqlParameter("SALE_ID", sale.Id),
                    new SqlParameter("PAYMENT_ID", pay.Id));
            }
        }
    }
}
