using PJW.MVC.Model;
using PJW.MVC.Patterns;

namespace PJW.MVC
{
    /// <summary>
    /// 所有Mediator的基类,用来获取所有的Proxy
    /// </summary>
    public class BaseMediator : Mediator
    {
        public new const string NAME = "BaseMediator";

        public BaseMediator()
        {
            this.MediatorName = NAME;
        }
        private static UserProxy userProxy;
        protected static UserProxy UserProxy
        {
            get
            {
                if (userProxy == null) userProxy = ApplicationFacade.Instance.GetProxy(UserProxy.NAME) as UserProxy;
                return userProxy;
            }
        }
        private static ResourcesProxy resourcesProxy;
        protected static ResourcesProxy ResourcesProxy
        {
            get
            {
                if (resourcesProxy == null) resourcesProxy = ApplicationFacade.Instance.GetProxy(ResourcesProxy.NAME) as ResourcesProxy;
                return resourcesProxy;
            }
        }
    }
}
