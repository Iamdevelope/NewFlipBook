
using System;
using System.Collections.Generic;

namespace PJW.Download
{

    internal partial class DownloadManager
    {
        /// <summary>
        /// 下载计数器
        /// </summary>
        private sealed partial class DownloadCounter
        {
            private readonly Queue<DownloadCounterNode> downloadCounterNodes;
            private float updateInterval;
            private float recordInterval;
            private float currentSpeed;
            private float accumulator;
            private float timeLeft;
            public DownloadCounter(float updateInterval,float recordInterval)
            {
                if (updateInterval <= 0f)
                {
                    throw new FrameworkException(" update interval is invalid ");
                }
                if (recordInterval <= 0f)
                {
                    throw new FrameworkException(" record interval is invalid ");
                }
                downloadCounterNodes = new Queue<DownloadCounterNode>();
                this.updateInterval = updateInterval;
                this.recordInterval = recordInterval;
                Reset();
            }
            public float UpdateInterval
            {
                get { return updateInterval; }
                set
                {
                    if (value <= 0)
                    {
                        throw new FrameworkException(" update interval is invalid ");
                    }
                    updateInterval = value;
                    Reset();
                }
            }
            public float RecordInterval
            {
                get { return recordInterval; }
                set
                {
                    if (value <= 0)
                    {
                        throw new FrameworkException(" record interval is invalid ");
                    }
                    recordInterval = value;
                    Reset();
                }
            }
            public float GetCurrentSpeed
            {
                get { return currentSpeed; }
            }
            public void Shutdown()
            {
                Reset();
            }
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (downloadCounterNodes.Count <= 0)
                {
                    return;
                }
                accumulator += realElapseSeconds;
                if (accumulator > recordInterval)
                {
                    accumulator = recordInterval;
                }
                timeLeft -= realElapseSeconds;
                foreach (var downloadCounterNode in downloadCounterNodes)
                {
                    downloadCounterNode.Update(elapseSeconds, realElapseSeconds);
                }
                while (downloadCounterNodes.Count > 0 && downloadCounterNodes.Peek().ElapseSeconds >= recordInterval)
                {
                    downloadCounterNodes.Dequeue();
                }
                if (downloadCounterNodes.Count <= 0)
                {
                    Reset();
                    return;
                }
                if (timeLeft <= 0)
                {
                    int totalDownloadLength = 0;
                    foreach (var downloadCounterNode in downloadCounterNodes)
                    {
                        totalDownloadLength += downloadCounterNode.DownloadLength;
                    }
                    currentSpeed = accumulator > 0 ? totalDownloadLength / accumulator : 0;
                    timeLeft += updateInterval;
                }
            }
            public void RecordDownloadLength(int downloadLength)
            {
                if (downloadLength <= 0)
                {
                    return;
                }
                downloadCounterNodes.Enqueue(new DownloadCounterNode(downloadLength));
            }
            private void Reset()
            {
                downloadCounterNodes.Clear();
                currentSpeed = 0;
                accumulator = 0;
                timeLeft = 0;
            }
        }
    }
}