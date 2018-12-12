
using PJW.Datas;
using PJW.MVC.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.MVC.Model
{
    /// <summary>
    /// 用户数据处理
    /// </summary>
    public class UserProxy : BaseProxy
    {
        public new const string NAME = "UserProxy";
        public UserProxy()
        {
            ProxyName = NAME;
        }
        public void Login(UserData ud)
        {
            OpenDB();
            reader = db.SelectWhere("user",
                new string[] { "Username" },
                new string[] { "Username", "Password" },
                new string[] { "=", "=" },
                new string[] { ud.Password, ud.Password }
                );
            //如果该条数据存在，则登录成功
            if (reader.HasRows)
            {

            }
            else
            {

            }
            CloseDB();
        }
        public void Register(UserData ud)
        {
            OpenDB();
            reader = db.Select("user", "Username", ud.Username);
            //如果数据库中存在相同用户名，则注册失败
            if (reader.HasRows)
            {

            }
            else
            {
                db.InsertInto("user", new string[] { ud.Username, ud.Password });
            }
            CloseDB();
        }
    }
}
