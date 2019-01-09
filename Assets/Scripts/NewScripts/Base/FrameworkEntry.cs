
using System.Collections.Generic;

namespace PJW
{
    /// <summary>
    /// ¿ò¼ÜÈë¿Ú
    /// </summary>
    public static class FrameworkEntry
    {
        private static readonly LinkedList<FrameworkModule> _FrameworkModules = new LinkedList<FrameworkModule>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (FrameworkModule module in _FrameworkModules)
            {
                module.Update(elapseSeconds, realElapseSeconds);
            }
        }
        public static void ShutDown()
        {
            foreach (FrameworkModule module in _FrameworkModules)
            {
                module.Shutdown();
            }
            _FrameworkModules.Clear();
            ReferencePool.ClearAll();

        }
    }
}