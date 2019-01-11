using System.Collections.Generic;

namespace PJW.DataNode
{
    internal partial class DataNodeManager
    {
        /// <summary>
        /// 数据节点
        /// </summary>
        private sealed class DataNode : IDataNode
        {

            private static readonly DataNode[] EmptyArray=new DataNode[]{};
            private readonly string _Name;
            private Variable _Data;
            private readonly DataNode _Parent;
            private List<DataNode> _Childs;

            public DataNode(string name,DataNode parent){
                if(!IsValidName(name)){
                    throw new FrameworkException(" name of data node is invalid ");
                }
                _Name=name;
                _Parent=parent;
                _Childs=null;
                _Data=null;
            }
            public string GetName
            {
                get
                {
                    return _Name;
                }
            }

            public string GetFullName {
                get{
                    return _Parent==null?_Name:Utility.Text.Format("{0}{1}{2}",_Parent.GetFullName,PathSplit[0],_Name);
                }
            }

            public IDataNode GetParent {get{return _Parent;}}

            public int GetChildCount {get{return _Childs.Count;}}

            /// <summary>
            /// 移除当前数据结点的数据和所有子数据结点。
            /// </summary>
            public void Clear()
            {
                _Data=null;
                if(_Childs!=null){
                    foreach (var item in _Childs)
                    {
                        item.Clear();
                    }
                    _Childs.Clear();
                }
            }

            /// <summary>
            /// 获取所有子数据结点。
            /// </summary>
            /// <param name="results">所有子数据结点。</param>
            public IDataNode[] GetAllChild()
            {
                if(_Childs==null){
                    return EmptyArray;
                }
                return _Childs.ToArray();
            }

            /// <summary>
            /// 获取所有子数据结点。
            /// </summary>
            /// <returns>所有子数据结点。</returns>
            public void GetAllChild(List<IDataNode> childNodes)
            {
                if(childNodes==null){
                    throw new FrameworkException(" the list is invalid ");
                }
                childNodes.Clear();
                if(_Childs==null)
                {
                    return;
                }
                foreach (var item in _Childs)
                {
                    childNodes.Add(item);
                }
            }

            /// <summary>
            /// 根据索引获取子数据结点。
            /// </summary>
            /// <param name="index">子数据结点的索引。</param>
            /// <returns>指定索引的子数据结点，如果索引越界，则返回空。</returns>
            public IDataNode GetChild(int index)
            {
                return index>=GetChildCount?null:_Childs[index];
            }
            
            /// <summary>
            /// 根据名称获取子数据结点。
            /// </summary>
            /// <param name="name">子数据结点名称。</param>
            /// <returns>指定名称的子数据结点，如果没有找到，则返回空。</returns>
            public IDataNode GetChild(string name)
            {
                if(name==null){
                    throw new FrameworkException(" name is invalid ");
                }
                foreach (var item in _Childs)
                {
                    if(name.Contains(item.GetName)){
                        return item;
                    }
                }
                return null;
            }

            /// <summary>
            /// 根据类型获取数据结点的数据。
            /// </summary>
            /// <typeparam name="T">要获取的数据类型。</typeparam>
            /// <returns>指定类型的数据。</returns>
            public T GetData<T>() where T : Variable
            {
                return (T)_Data;
            }

            /// <summary>
            /// 获取数据结点的数据。
            /// </summary>
            /// <returns>数据结点数据。</returns>
            public Variable GetData()
            {
                return _Data;
            }

            /// <summary>
            /// 根据名称获取或增加子数据结点。
            /// </summary>
            /// <param name="name">子数据结点名称。</param>
            /// <returns>指定名称的子数据结点，如果对应名称的子数据结点已存在，则返回已存在的子数据结点，否则增加子数据结点。</returns>
            public IDataNode GetOrAddChild(string name)
            {
                DataNode node=(DataNode)GetChild(name);
                if(node!=null){
                    return node;
                }
                node=new DataNode(name,this);
                if(_Childs==null){
                    _Childs=new List<DataNode>();
                }
                _Childs.Add(node);
                return node;
            }

            /// <summary>
            /// 根据索引移除子数据结点。
            /// </summary>
            /// <param name="index">子数据结点的索引位置。</param>
            public void RemoveChild(int index)
            {
                DataNode node=(DataNode)GetChild(index);
                if(node==null){
                    return;
                }
                node.Clear();
                _Childs.RemoveAt(index);
            }

            /// <summary>
            /// 根据名称移除子数据结点。
            /// </summary>
            /// <param name="name">子数据结点名称。</param>
            public void RemoveChild(string name)
            {
                DataNode node=(DataNode)GetChild(name);
                if(node==null){
                    return;
                }
                node.Clear();
                _Childs.Remove(node);
            }

            /// <summary>
            /// 设置数据结点的数据。
            /// </summary>
            /// <param name="data">要设置的数据。</param>
            public void SetData(Variable data)
            {
                _Data=data;
            }

            /// <summary>
            /// 设置数据结点的数据。
            /// </summary>
            /// <typeparam name="T">要设置的数据类型。</typeparam>
            /// <param name="data">要设置的数据。</param>
            public void SetData<T>(T data) where T : Variable
            {
                _Data=data;
            }

            /// <summary>
            /// 获取数据结点字符串。
            /// </summary>
            /// <returns>数据结点字符串。</returns>
            public override string ToString(){
                return Utility.Text.Format("{0}:{1}",GetFullName,ToDataString());
            }

            /// <summary>
            /// 获取数据字符串。
            /// </summary>
            /// <returns>数据字符串。</returns>
            public string ToDataString()
            {
                if(_Data==null){
                    return "null";
                }
                return Utility.Text.Format("[{0}] {1}",_Data.GetType.Name,_Data.ToString());
            }
            /// <summary>
            /// 检测数据节点名称是否合法
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            private static bool IsValidName(string name){
                if(string.IsNullOrEmpty(name)){
                    return false;
                }
                foreach (string item in PathSplit)
                {
                    if(name.Contains(item)){
                        return false;
                    }
                }
                return true;
            }
        }
    }
}