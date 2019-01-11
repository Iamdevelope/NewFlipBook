namespace PJW.DataNode
{
    /// <summary>
    /// 数据节点管理器接口
    /// </summary>
    public interface IDataNodeManager
    {
        /// <summary>
        /// 获取根节点
        /// </summary>
        /// <value></value>
         IDataNode GetRoot{get;}

         /// <summary>
         /// 获取数据节点数据
         /// </summary>
         /// <param name="path">相对于node的查找路径</param>
         /// <typeparam name="T">数据类型</typeparam>
         /// <returns>指定类型的数据</returns>
         T GetData<T>(string path)where T:Variable;

        /// <summary>
        /// 获取数据节点数据
        /// </summary>
        /// <param name="path">相对于node的查找路径</param>
        /// <returns>指定类型的数据</returns>
         Variable GetData(string path);
        
        /// <summary>
        /// 获取数据节点数据
        /// </summary>
        /// <param name="path">相对于node的查找路径</param>
        /// <param name="node">要查找的起始节点</param>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>指定类型的数据</returns>
         T GetData<T>(string path,IDataNode node)where T : Variable;

        /// <summary>
        /// 获取数据节点数据
        /// </summary>
        /// <param name="path">相对于node的查找路径</param>
        /// <param name="node">要查找的起始节点</param>
        /// <returns>指定类型的数据</returns>
         Variable GetData(string path,IDataNode node);
         
         /// <summary>
         /// 设置节点数据
         /// </summary>
         /// <param name="path">相对于node的查找路径</param>
         /// <param name="data">要设置的数据</param>
         /// <typeparam name="T">数据类型</typeparam>
         void SetData<T>(string path,T data) where T : Variable;
         
         /// <summary>
         /// 设置节点数据
         /// </summary>
         /// <param name="path">相对于node的查找路径</param>
         /// <param name="data">要设置的数据</param>
         void SetData(string path,Variable data);
         
         /// <summary>
         /// 设置节点数据
         /// </summary>
         /// <param name="path">相对于node的查找路径</param>
         /// <param name="node">要查找的起始节点</param>
         /// <param name="Data">要设置的数据</param>
         /// <typeparam name="T">数据类型</typeparam>
         void SetData<T>(string path,IDataNode node,T Data) where T : Variable;
         
         /// <summary>
         /// 设置节点数据
         /// </summary>
         /// <param name="path">相对于node的查找路径</param>
         /// <param name="node">要查找的起始节点</param>
         /// <param name="data">要设置的数据</param>
         void SetData(string path,IDataNode node,Variable data);

        /// <summary>
        /// 获取数据节点
        /// </summary>
        /// <param name="path">相对于node的查找路径</param>
        /// <returns>数据节点</returns>
         IDataNode GetNode(string path);
         
         /// <summary>
         /// 获取数据节点
         /// </summary>
         /// <param name="path">相对于node的查找路径</param>
         /// <param name="node">要查找的起始节点</param>
         /// <returns>数据节点</returns>
         IDataNode GetNode(string path,IDataNode node);
         
         /// <summary>
         /// 得到数据节点,如果没有则添加再获取
         /// </summary>
         /// <param name="path">相对于node的查找路径</param>
         /// <returns>数据节点</returns>
         IDataNode GetOrAddNode(string path);
         
         /// <summary>
         /// 得到数据节点,如果没有则添加再获取
         /// </summary>
         /// <param name="path">相对于node的查找路径</param>
         /// <param name="node">要查找的起始节点</param>
         /// <returns>数据节点</returns>
         IDataNode GetOrAddNode(string path,IDataNode node);
         
         /// <summary>
         /// 移除数据节点
         /// </summary>
         /// <param name="path">相对于node的查找路径</param>
         void RemoveNode(string path);
         
         /// <summary>
         /// 移除数据节点
         /// </summary>
         /// <param name="path">相对于node的查找路径</param>
         /// <param name="node">要查找的起始节点</param>
         void RemoveNode(string path,IDataNode node);
         
         /// <summary>
         /// 清空所有数据节点
         /// </summary>
         void Clear();
    }
}