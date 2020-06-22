using System;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace FileBuddyUI.UI.Helper
{
    /// <summary>
    /// Provides a globally accessible implemention of 
    /// the Toast messenger for the UI.
    /// </summary>
    public class ToastMessenger
    {
        public Notifier Notifier { get; set; }

        private static ToastMessenger _instance;
        public static ToastMessenger NotifierInstance
        {
            get
            {
                if (_instance == null)
                    _instance = new ToastMessenger();

                    return _instance;
            }
        }
        private ToastMessenger()
        {
            InitializeNotifier();
        }

        private void InitializeNotifier()
        {
            Notifier = new Notifier(cfg =>
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
