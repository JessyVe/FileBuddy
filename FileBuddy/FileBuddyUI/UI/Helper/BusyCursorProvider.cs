using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace FileBuddyUI.UI.Helper
{
    /// <summary>
    /// Provides a global entry point for changing the 
    /// mouse cursor apperence.
    /// </summary>
    public static class BusyCursorProvider
    {
        /// <summary>
        /// A value indicating whether the cursor 
        /// should be shown as busy or not. 
        /// </summary>
        private static bool IsBusy;

        /// <summary>
        /// Sets the busystate to busy.
        /// </summary>
        public static void SetBusyState()
        {
            SetBusyState(true);
        }

        /// <summary>
        /// Handles the Tick event of the dispatcherTimer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (sender is DispatcherTimer dispatcherTimer)
            {
                SetBusyState(false);
                dispatcherTimer.Stop();
            }
        }

        /// <summary>
        /// Sets the busystate to busy or not busy.
        /// </summary>
        /// <param name="busy">if set to <c>true</c> the application is now busy.</param>
        private static void SetBusyState(bool busy)
        {
            if (busy != IsBusy)
            {
                IsBusy = busy;
                Mouse.OverrideCursor = busy ? Cursors.Wait : null;

                if (IsBusy)
                {
                    new DispatcherTimer(TimeSpan.FromSeconds(0), DispatcherPriority.ApplicationIdle, 
                        DispatcherTimer_Tick, System.Windows.Application.Current.Dispatcher);
                }
            }
        }
    }
}
