using PJW.MVC;
using PJW.MVC.Patterns;

/// <summary>
/// 班级类别
/// </summary>
public class ClassTypeMediator : BaseMediator {

    public new const string NAME = "ClassTypeMediator";
    public ClassTypeMediator()
    {
        MediatorName = NAME;
    }

    public override string[] NotificationList()
    {
        return base.NotificationList();
    }

    public override void HandleNotification(Notification notification)
    {
        switch (notification.name)
        {
            
        }
    }
}