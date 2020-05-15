using System;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace FileBuddyUI.UI.Helper
{
    public class ToastMessenger
    {
        private static Notifier _notifier;
        public static Notifier NotifierInstance
        {
            get
            {
                if (_notifier == null)
                    InitializeNotifier();

                return _notifier;
            }
        }

        private static void InitializeNotifier()
        {
            _notifier = new Notifier(cfg =>
           {
               cfg.PositionProvider = new WindowPositionProvider(
                   parentWindow: Application.Current.MainWindow,
                   corner: Corner.BottomCenter,
                   offsetX: 10,
                   offsetY: 10);

               cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                   notificationLifetime: TimeSpan.FromSeconds(3),
                   maximumNotificationCount: MaximumNotificationCount.FromCount(5));

               cfg.Dispatcher = Application.Current.Dispatcher;
           });
        }
    }
}
