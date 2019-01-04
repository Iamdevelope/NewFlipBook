
namespace PJW.Download
{
    internal partial class DownloadManager
    {
        private partial class DownloadCounter
        {

            private sealed class DownloadCounterNode
            {
                private readonly int downloadLength;
                private float elapseSeconds;
                public DownloadCounterNode(int downloadLength )
                {
                    this.downloadLength = downloadLength;
                    this.elapseSeconds = 0;
                }
                public int DownloadLength
                {
                    get { return downloadLength; }
                }
                public float ElapseSeconds
                {
                    get { return elapseSeconds; }
                }
                public void Update(float elapseSeconds, float realElapseSeconds)
                {
                    elapseSeconds += realElapseSeconds;
                }
            }
        }
    }
}