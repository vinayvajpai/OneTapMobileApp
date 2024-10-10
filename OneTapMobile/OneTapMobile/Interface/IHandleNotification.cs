namespace OneTapMobile.Interface
{
    public interface IHandleNotification
    {
        void EnablePush();
        void CancelPush(int id);
        bool registeredForNotifications();
    }
}
