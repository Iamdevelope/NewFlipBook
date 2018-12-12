using PJW.MyEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.Book.UI
{
    [SerializeField]
    /// <summary>
    /// 设置界面的数据
    /// </summary>
    public class SetData
    {
        public float Volume { get; set; }
        public LanguageData LanguageData { get; set; }
    }
    /// <summary>
    /// 设置界面
    /// </summary>
    public class SetPanel : BasePanel
    {
        public void SetSerializeField(float volume,LanguageData data)
        {

        }
        //public override void Reset(Vector3 scale, float t, string msg = "")
        //{
        //    base.Reset(scale, t, msg);
        //}
    }
}