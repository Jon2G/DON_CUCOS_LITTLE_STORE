using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Data;

namespace CucoStore.Docs.Models
{
    public class Payment
    {
        public string PayWay1 => Pay.GetName(Payments[0].PayWay);
        public string PayWay2 => Pay.GetName(Payments[1].PayWay);
        public string PayWay3 => Pay.GetName(Payments[2].PayWay);

        public float PayAmount1 => Payments[0].Amount;
        public float PayAmount2 => Payments[1].Amount;
        public float PayAmount3 => Payments[2].Amount;

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
