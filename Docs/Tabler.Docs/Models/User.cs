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
using Stimulsoft.Report.Dictionary;
using CucoStore.Docs.Data;

namespace CucoStore.Docs.Models
{
  public  class User 
    {
        public int Id { get; set; }
        public int Key_Id { get; set; }
        public string Nickname { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        private string _Picture;
        public string Picture
        {
            get => string.IsNullOrEmpty(_Picture) ? "_content/CucoStore.Docs/img/LogoWhite.png" : _Picture;
            set => _Picture = value;
        }
        public bool Enabled { get; set; }
        public Permissions Permissions { get; set; }

        public User()
        {
            Permissions = new Permissions();
        }
        internal async Task<User> Login()
        {
          if(AppData.SQL.Single<bool>("SP_LOGIN", CommandType.StoredProcedure,
                new SqlParameter("NICKNAME", Nickname),
                new SqlParameter("PASSWORD", Password)
                ))
            {
                return await User.FindByNickName(Nickname);
            }
            return this;
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
            User user = await User.FindByNickName(this.Nickname);
            Permissions permissions = Permissions.GetById(user.Id);
            this.Permissions.Id = permissions.Id;
            this.Permissions.Save();

        }

        public static async Task<User> GetById(int Id)
        {
            await Task.Yield();
            using (IReader reader = AppData.SQL.Read("GET_USER_BY_ID",
                CommandType.StoredProcedure, new SqlParameter("ID", Id)))
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
        public static async Task<User> FindByNickName(string nickname)//eSTE PIDE NICKNAME
        {
            int id = AppData.SQL.Single<int>("GET_USER_BY_NICKNAME",
                CommandType.StoredProcedure, new SqlParameter("NICKNAME", nickname));
            return await GetById(id);

        }
        public static async Task<List<User>> GetAll()
        {
            await Task.Yield();
            List<User> users = new List<User>();
            foreach (int id in AppData.SQL.Lista<int>("SELECT *FROM VIEW_GETALLUSERS",0))
            {
                users.Add(await GetById(id));
            }
            return users;
        }
    }
}
