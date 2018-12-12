
using System.Collections.Generic;

public class DictionaryHelp<K,V> {
    /// <summary>
    /// 通过索引得到字典中对应位置的键
    /// </summary>
    /// <param name="dictionary">字典</param>
    /// <param name="index">索引</param>
    /// <returns>返回得到的键</returns>
    public static K GetKeyFromDictionary(Dictionary<K,V> dictionary,int index)
    {
        K k = default(K);
        int n = 0;
        foreach (var item in dictionary.Keys)
        {
            n++;
            if (n == index + 1)
                k = item;
        }
        return k;
    }
    /// <summary>
    /// 通过索引得到字典中对应位置的值
    /// </summary>
    /// <param name="dictionary">字典</param>
    /// <param name="index">索引</param>
    /// <returns>返回得到的值</returns>
    public static V GetValueFromDictionary(Dictionary<K, V> dictionary, int index)
    {
        V v = default(V);
        int n = 0;
        foreach (var item in dictionary.Values)
        {
            n++;
            if (n == index + 1 )
                v = item;
        }
        return v;
    }
}
