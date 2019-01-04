
using System.Collections.Generic;

namespace PJW.Task
{
    /// <summary>
    /// 任务池
    /// </summary>
    /// <typeparam name="T">任务类型</typeparam>
    public sealed class TaskPool<T> where T : ITask
    {
        /// <summary>
        /// 空闲的任务代理
        /// </summary>
        private readonly Stack<ITaskAgent<T>> m_FreeAgents;
        /// <summary>
        /// 正在执行的任务
        /// </summary>
        private readonly LinkedList<ITaskAgent<T>> m_WorkingAgent;
        /// <summary>
        /// 正在等待执行的任务
        /// </summary>
        private readonly LinkedList<T> m_WaitingTasks;
        /// <summary>
        /// 初始化任务池的新实例
        /// </summary>
        public TaskPool()
        {
            m_FreeAgents = new Stack<ITaskAgent<T>>();
            m_WorkingAgent = new LinkedList<ITaskAgent<T>>();
            m_WaitingTasks = new LinkedList<T>();
        }
        /// <summary>
        /// 获取空闲代理数量
        /// </summary>
        public int GetFreeAgentCount
        {
            get
            {
                return m_FreeAgents.Count;
            }
        }
        /// <summary>
        /// 获取正在执行的任务代理数量
        /// </summary>
        public int GetWorkingAgentCount
        {
            get
            {
                return m_WorkingAgent.Count;
            }
        }
        /// <summary>
        /// 获取等待执行任务数量
        /// </summary>
        public int GetWaitingAgentCount
        {
            get
            {
                return m_WaitingTasks.Count;
            }
        }
        /// <summary>
        /// 获取总的任务代理数量
        /// </summary>
        public int GetTotalAgentCount
        {
            get
            {
                return GetFreeAgentCount + GetWorkingAgentCount + GetWaitingAgentCount;
            }
        }
        /// <summary>
        /// 任务池轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">实际流逝时间</param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            LinkedListNode<ITaskAgent<T>> current = m_WorkingAgent.First;
            while (current != null)
            {
                if (current.Value.GetTask.Done)
                {
                    LinkedListNode<ITaskAgent<T>> next = current.Next;
                    current.Value.Reset();
                    m_FreeAgents.Push(current.Value);
                    m_WorkingAgent.Remove(current.Value);
                    current = next;
                    continue;
                }
                current.Value.Update(elapseSeconds, realElapseSeconds);
                current = current.Next;
            }
            while (GetFreeAgentCount > 0 && GetWaitingAgentCount > 0)
            {
                ITaskAgent<T> agent = m_FreeAgents.Pop();
                LinkedListNode<ITaskAgent<T>> agentNode = m_WorkingAgent.AddLast(agent);
                T task = m_WaitingTasks.First.Value;
                m_WaitingTasks.RemoveFirst();
                agent.Start(task);
                if (task.Done)
                {
                    agent.Reset();
                    m_FreeAgents.Push(agent);
                    m_WorkingAgent.Remove(agentNode);
                }
            }
        }
        /// <summary>
        /// 关闭并清理任务池
        /// </summary>
        public void Shutdown()
        {
            while (GetFreeAgentCount > 0)
            {
                m_FreeAgents.Pop().Shutdown();
            }
            foreach (var workingAgent in m_WorkingAgent)
            {
                workingAgent.Shutdown();
            }
            m_WorkingAgent.Clear();
            m_WaitingTasks.Clear();
        }
        /// <summary>
        /// 添加任务代理
        /// </summary>
        /// <param name="agent">要添加的任务代理</param>
        public void AddAgent(ITaskAgent<T> agent)
        {
            if (agent == null)
            {
                throw new FrameworkException(" Task agent is invalid ");
            }
            agent.Init();
            m_FreeAgents.Push(agent);
        }
        /// <summary>
        /// 增加任务
        /// </summary>
        /// <param name="task">要添加的任务</param>
        public void AddTask(T task)
        {
            LinkedListNode<T> current = m_WaitingTasks.First;
            while (current != null)
            {
                if (task.GetPriority > current.Value.GetPriority)
                {
                    break;
                }
                current = current.Next;
            }
            if (current != null)
            {
                m_WaitingTasks.AddBefore(current, task);
            }
            else
            {
                m_WaitingTasks.AddLast(task);
            }
        }
        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="serialId">需要移除的任务的序列号</param>
        /// <returns>被移除的任务</returns>
        public T RemoveTask(float serialId)
        {
            foreach (var waitingTask in m_WaitingTasks)
            {
                if (waitingTask.GetSerialId == serialId)
                {
                    m_WaitingTasks.Remove(waitingTask);
                    return waitingTask;
                }
            }

            foreach (var workingAgent in m_WorkingAgent)
            {
                if (workingAgent.GetTask.GetSerialId == serialId)
                {
                    workingAgent.Reset();
                    m_FreeAgents.Push(workingAgent);
                    m_WorkingAgent.Remove(workingAgent);
                    return workingAgent.GetTask;
                }
            }

            return default(T);
        }
        /// <summary>
        /// 移除所有任务
        /// </summary>
        public void RemoveAllTasks()
        {
            m_WaitingTasks.Clear();
            foreach (var workingAgent in m_WorkingAgent)
            {
                workingAgent.Reset();
                m_FreeAgents.Push(workingAgent);
            }
            m_WorkingAgent.Clear();
        }
    }
}