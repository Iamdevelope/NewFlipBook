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

        public T GetData<T>(string path) where T : Variable
        {
            throw new System.NotImplementedException();
        }

        public Variable GetData(string path)
        {
            throw new System.NotImplementedException();
        }

        public T GetData<T>(string path, IDataNode node) where T : Variable
        {
            throw new System.NotImplementedException();
        }

        public Variable GetData(string path, IDataNode node)
        {
            throw new System.NotImplementedException();
        }

        public IDataNode GetNode(string path)
        {
            throw new System.NotImplementedException();
        }

        public IDataNode GetNode(string path, IDataNode node)
        {
            throw new System.NotImplementedException();
        }

        public IDataNode GetOrAddNode(string path)
        {
            throw new System.NotImplementedException();
        }

        public IDataNode GetOrAddNode(string path, IDataNode node)
        {
            throw new System.NotImplementedException();
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

        public void SetData<T>(string path, T data) where T : Variable
        {
            throw new System.NotImplementedException();
        }

        public void SetData(string path, Variable data)
        {
            throw new System.NotImplementedException();
        }

        public void SetData<T>(string path, IDataNode node, T Data) where T : Variable
        {
            throw new System.NotImplementedException();
        }

        public void SetData(string path, IDataNode node, Variable data)
        {
            throw new System.NotImplementedException();
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