using Kit.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Kit.Sql.Readers;
using Tabler.Docs.Data;

namespace Tabler.Docs.Models
{
  public  class User 
    {
        public int Id { get; set; }
        public int Key_Id { get; set; }
        public string Nickname { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Picture { get; set; }
        public bool Enabled { get; set; }
        public Permissions Permissions { get; set; }

        public User()
        {
            Permissions = new Permissions();
        }

        public async  Task Save()
        {
            await Task.Yield();
            AppData.SQL.EXEC("SP_REGISTER", CommandType.StoredProcedure, 
                new SqlParameter("ID",this.Id),
                new SqlParameter("NICKNAME",Nickname),
                new SqlParameter("NAME",Name),
                new SqlParameter("PASSWORD",Password),
                new SqlParameter("PICTURE",Picture),
                new SqlParameter("ENABLED",Enabled)
            );
            User user = User.FindByNickName(this.Nickname);
            Permissions permissions = Permissions.GetById(user.Id);
            this.Permissions.Id = permissions.Id;
            this.Permissions.Save();

        }

        private static User FindByNickName(string nickname)
        {
            using (IReader reader = AppData.SQL.Read("GET_USER_BY_NICKNAME",
                CommandType.StoredProcedure, new SqlParameter("NICKNAME", nickname)))
            {
                if (reader.Read())
                {
                    return new User()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        Permissions = Permissions.GetById(Convert.ToInt32(reader[1])),
                        Key_Id = Convert.ToInt32(reader[2]),
                        Nickname = Convert.ToString(reader[3]),
                        Name = Convert.ToString(reader[4]),
                        Password = Convert.ToString(reader[5]),
                        Picture = Convert.ToString(reader[6]),
                        Enabled = Convert.ToBoolean(reader[7])
                    };
                }
            }
            return new User();
        }
    }
}
