namespace PJW.Config
{
    internal partial class ConfigManager{
        /// <summary>
        /// 配置文件数据
        /// </summary>
        public struct ConfigData
        {
            private readonly bool _Bool;
            private readonly int _Int;
            private readonly float _Float;
            private readonly string _String;

            /// <summary>
            /// 配置文件数据
            /// </summary>
            /// <param name="boolValue">布尔值</param>
            /// <param name="intValue">整数值</param>
            /// <param name="floatValue">浮点值</param>
            /// <param name="stringValue">字符值</param>
            public ConfigData(bool boolValue,int intValue,float floatValue,string stringValue){
                _Bool=boolValue;
                _Int=intValue;
                _Float=floatValue;
                _String=stringValue;
            }

            public bool GetBool{
                get{return _Bool;}
            }
            public int GetInt{
                get{return _Int;}
            }
            public float GetFloat{
                get{return _Float;}
            }
            public string GetString{
                get{return _String;}
            }
        }
    }
}