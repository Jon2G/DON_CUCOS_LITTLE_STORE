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
    public enum PayWays
    {
        Cash=1,Vale=2,Card=3
    }
    public class Pay
    {
        public int Id { get; set; }
        public PayWays PayWay { get; set; }
        private float _Amount;

        public float Amount
        {
            get => _Amount;
            set
            {
                if (value < 0)
                {
                    _Amount = 0;
                }
                else
                {
                    _Amount = value;
                }
            }
        }

        public Pay(PayWays PayWay=PayWays.Cash)
        {
            this.PayWay = PayWay;
        }

        public static string GetName(PayWays payWay)
        {
            switch (payWay)
            {
                case PayWays.Card:
                    return "Tarjeta";
                case PayWays.Cash:
                    return "Efectivo";
                case PayWays.Vale:
                    return "Vale";
            }
            return String.Empty;
        }

        public void Save()
        {
            this.Id = AppData.SQL.Single<int>("SP_SAVE_PAYMENT", CommandType.StoredProcedure,
                new SqlParameter("PAY_WAY_ID", (int)this.PayWay),
                new SqlParameter("AMOUNT", this.Amount));
        }
    }
}
