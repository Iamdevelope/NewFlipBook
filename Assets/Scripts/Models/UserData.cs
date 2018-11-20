using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.Book
{
    [SerializeField]
    public class UserData
    {
        private string id;
        private string username;
        private string password;

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Username
        {
            get
            {
                return username;
            }

            set
            {
                username = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public override string ToString()
        {
            return string.Format("用户名：{0},密码：{1}", username, password);
        }
    }
}