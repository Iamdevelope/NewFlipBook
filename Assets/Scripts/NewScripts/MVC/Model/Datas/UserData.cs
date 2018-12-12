using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.Datas
{
    [SerializeField]
    public class UserData
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public override string ToString()
        {
            return string.Format("用户ID：{0},用户名：{1}", Id, Username);
        }
    }
}