using System;

namespace PJW.DataNode
{
    /// <summary>
    /// 数据节点管理器
    /// </summary>
    internal partial class DataNodeManager : FrameworkModule, IDataNodeManager
    {
        private static readonly string[] EmptyStringArray=new string[]{};
        private static readonly string[] PathSplit=new string[]{",","/","\\"};
        private const string RootName="Root";
        private DataNode _Root;

        public DataNodeManager(){
            _Root=new DataNode(RootName,null);
        }

        public IDataNode GetRoot
        {
            get
            {
                return _Root;
            }
        }

        /// <summary>
        /// 清空所有数据节点
        /// </summary>
        public void Clear()
        {
            _Root.Clear();
        }

        /// <summary>
        /// 得到数据节点
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>数据节点</returns>
        public T GetData<T>(string path) where T : Variable
        {
            return GetData<T>(path,null);
        }

        /// <summary>
        /// 得到数据节点
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <returns>数据节点</returns>
        public Variable GetData(string path)
        {
            return GetData(path,null);
        }

        /// <summary>
        /// 得到数据节点
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <param name="node">需要开始的节点</param>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>数据节点</returns>
        public T GetData<T>(string path, IDataNode node) where T : Variable
        {
            IDataNode current=GetNode(path,node);
            if(current==null){
                throw new FrameworkException(Utility.Text.Format(" Data node is invalid "));
            }
            return current.GetData<T>();
        }

        /// <summary>
        /// 得到数据节点
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <param name="node">需要开始的节点</param>
        /// <returns>数据节点</returns>
        public Variable GetData(string path, IDataNode node)
        {
            IDataNode current=GetNode(path,node);
            if(current==null){
                throw new FrameworkException(Utility.Text.Format(" Data node is invalid "));
            }
            return current.GetData();
        }

        /// <summary>
        /// 得到数据节点
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <returns>数据节点</returns>
        public IDataNode GetNode(string path)
        {
            return GetNode(path,null);
        }

        /// <summary>
        /// 得到数据节点
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <param name="node">需要开始的节点</param>
        /// <returns>数据节点</returns>
        public IDataNode GetNode(string path, IDataNode node)
        {
            IDataNode current=node??_Root;
            string[] splitPath=GetSplitPath(path);
            foreach(string i in splitPath){
                current=current.GetChild(i);
                if(current==null){
                    return null;
                }
            }
            return current;
        }

        /// <summary>
        /// 获取或得到数据节点
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <returns>数据节点</returns>
        public IDataNode GetOrAddNode(string path)
        {
            return GetOrAddNode(path,null);
        }

        /// <summary>
        /// 获取或得到数据节点
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <param name="node">需要开始的节点</param>
        /// <returns>数据节点</returns>
        public IDataNode GetOrAddNode(string path, IDataNode node)
        {
            IDataNode current=node??_Root;
            string[] splitPath=GetSplitPath(path);
            foreach(string i in splitPath){
                current=current.GetOrAddChild(i);
            }
            return current;
        }

        /// <summary>
        /// 移除数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        public void RemoveNode(string path)
        {
            RemoveNode(path,null);
        }

        /// <summary>
        /// 移除数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="node">查找起始结点。</param>
        public void RemoveNode(string path, IDataNode node)
        {
            IDataNode current=node??_Root;
            IDataNode parent=current.GetParent;
            string[] splitPath=GetSplitPath(path);
            foreach(string i in splitPath){
                parent=current;
                current=current.GetChild(i);
                if(current==null){
                    return;
                }
            }
            if(parent!=null){
                parent.RemoveChild(current.GetName);
            }
        }

        /// <summary>
         /// 设置节点数据
         /// </summary>
         /// <param name="path">相对于node的查找路径</param>
         /// <param name="data">要设置的数据</param>
         /// <typeparam name="T">数据类型</typeparam>
        public void SetData<T>(string path, T data) where T : Variable
        {
            SetData<T>(path,null,data);
        }

        /// <summary>
         /// 设置节点数据
         /// </summary>
         /// <param name="path">相对于node的查找路径</param>
         /// <param name="data">要设置的数据</param>
        public void SetData(string path, Variable data)
        {
            SetData(path,null,data);
        }

        /// <summary>
         /// 设置节点数据
         /// </summary>
         /// <param name="path">相对于node的查找路径</param>
         /// <param name="node">查找起始结点。</param>
         /// <param name="data">要设置的数据</param>
         /// <typeparam name="T">数据类型</typeparam>
        public void SetData<T>(string path, IDataNode node, T data) where T : Variable
        {
            IDataNode current=GetOrAddNode(path,node);
            current.SetData<T>(data);
        }

        /// <summary>
         /// 设置节点数据
         /// </summary>
         /// <param name="path">相对于node的查找路径</param>
         /// <param name="node">查找起始结点。</param>
         /// <param name="data">要设置的数据</param>
        public void SetData(string path, IDataNode node, Variable data)
        {
            IDataNode current=GetOrAddNode(path,node);
            current.SetData(data);
        }

        public override void Shutdown()
        {
            Clear();
            _Root=null;
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 数据节点路径切分
        /// </summary>
        /// <param name="path">需要切分的数据节点路径</param>
        /// <returns>切分后的字符串数组</returns>
        private static string[] GetSplitPath(string path){
            if(string.IsNullOrEmpty(path)){
                return EmptyStringArray;
            }
            return path.Split(PathSplit,StringSplitOptions.RemoveEmptyEntries);
        }
    }
}