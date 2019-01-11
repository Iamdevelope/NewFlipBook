using System.Collections.Generic;

namespace PJW.DataNode
{
    /// <summary>
    /// 数据节点接口
    /// </summary>
    public interface IDataNode
    {
         /// <summary>
         /// 获取节点名
         /// </summary>
         /// <value></value>
         string GetName{get;}

         /// <summary>
         /// 获取节点全名
         /// </summary>
         /// <value></value>
         string GetFullName{get;}

         /// <summary>
         /// 获取父节点
         /// </summary>
         /// <value></value>
         IDataNode GetParent{get;}

         /// <summary>
         /// 获取子节点个数
         /// </summary>
         /// <value></value>
         int GetChildCount{get;}

         /// <summary>
         /// 获取节点数据，根据数据类型
         /// </summary>
         /// <typeparam name="T">数据类型</typeparam>
         /// <returns>指定类型的数据</returns>
         T GetData<T>() where T : Variable;

         /// <summary>
         /// 获取节点数据
         /// </summary>
         /// <returns>数据节点数据</returns>
        Variable GetData();

        /// <summary>
        /// 设置节点数据
        /// </summary>
        /// <param name="data"></param>
        void SetData(Variable data);
        
        /// <summary>
        /// 设置节点数据
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        void SetData<T>(T data) where T:Variable;
        
        /// <summary>
        /// 根据索引得到节点的子节点
        /// </summary>
        /// <param name="index">需要获取的节点的索引</param>
        /// <returns>得到的子节点，如果没有则返回空</returns>
        IDataNode GetChild(int index);
        
        /// <summary>
        /// 根据名称得到节点的子节点
        /// </summary>
        /// <param name="name">需要获取的节点的名称</param>
        /// <returns>得到的子节点，如果没有则返回空</returns>
        IDataNode GetChild(string name);
        
        /// <summary>
        /// 根据名称得到节点的子节点,如果没有则添加再获取
        /// </summary>
        /// <param name="name">需要获取的节点的名称</param>
        /// <returns></returns>
        IDataNode GetOrAddChild(string name);
        
        /// <summary>
        /// 获取所有子节点
        /// </summary>
        /// <returns></returns>
        IDataNode[] GetAllChild();
        
        /// <summary>
        /// 获取所有子节点
        /// </summary>
        /// <param name="childNodes"></param>
        void GetAllChild(List<IDataNode> childNodes);

        /// <summary>
        /// 移除指定索引的子节点
        /// </summary>
        /// <param name="index"></param>
        void RemoveChild(int index);
        
        /// <summary>
        /// 移除指定名称的子节点
        /// </summary>
        /// <param name="name"></param>
        void RemoveChild(string name);

        /// <summary>
        /// 移除当前节点的数据及其所有子节点
        /// </summary>
        void Clear();
        
        /// <summary>
        /// 获取数据节点字符串
        /// </summary>
        /// <returns></returns>
        string ToString();

        /// <summary>
        /// 获取数据字符串
        /// </summary>
        /// <returns></returns>
        string ToDataString();
    }
}