using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData {

    public string ID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public override string ToString()
    {
        return string.Format("用户名：{0},密码：{1}", Username, Password);
    }
}
