using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kit.Sql.Readers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CucoStore.Docs.Data;

namespace CucoStore.Docs.Models
{
    public class Permissions
    {
        public int Id { get; set; }
        private bool _StockIn;
        public bool StockIn
        {
            get => _StockIn;
            set
            {
                _StockIn = value;
                if (_StockIn && _ReadOnly)
                {
                    ReadOnly = false;
                }
            }
        }
        private bool _StockOut;
        public bool StockOut
        {
            get => _StockOut;
            set
            {
                _StockOut = value;
                if (_StockOut && _ReadOnly)
                {
                    ReadOnly = false;
                }
            }
        }
        public bool ReadReports { get; set; }

        private bool _ReadOnly;
        public bool ReadOnly
        {
            get => _ReadOnly;
            set
            {
                _ReadOnly = value;
                if (_ReadOnly)
                {
                    StockIn = false;
                    StockOut = false;
                    UserManager = false;
                }
            }
        }

        public bool _UserManager;
        private bool _Sales;

        public bool UserManager
        {
            get => _UserManager;
            set
            {
                _UserManager = value;
                if (value && ReadOnly)
                {
                    ReadOnly = false;
                }
            }
        }
        public bool Sales
        {
            get => _Sales;
            set
            {
                _Sales = value;
            }
        }
        public static Permissions GetById(int id)
        {
            using (IReader reader=AppData.SQL.Read("GET_PERMISSIONS_BY_ID", CommandType.StoredProcedure,new SqlParameter("ID",id)))
            {
                if (reader.Read())
                {
                    return new Permissions()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        StockOut = Convert.ToBoolean(reader[1]),
                        StockIn = Convert.ToBoolean(reader[2]),
                        ReadReports = Convert.ToBoolean(reader[3]),
                        UserManager = Convert.ToBoolean(reader[4]),
                        ReadOnly = Convert.ToBoolean(reader[5]),
                        Sales = Convert.ToBoolean(reader[6])
                    };
                }
            }
            return new Permissions();
        }
        public void Save()
        {
            AppData.SQL.EXEC("SP_ABC_PERMISSIONS", CommandType.StoredProcedure,
                new SqlParameter("ID", this.Id),
                new SqlParameter("STOCK_OUT", StockOut),
                new SqlParameter("STOCK_IN", StockIn),
                new SqlParameter("REPORTS_READ", ReadReports),
                new SqlParameter("USER_MANAGER", UserManager),
                new SqlParameter("READ_ONLY", ReadOnly),
                new SqlParameter("SALES", Sales)
            );
        }
    }
}
