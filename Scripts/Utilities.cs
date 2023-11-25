using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PacmanWindowForms.Scripts
{
    public class Utilities
    {
        // Start a timer with specified interval, and execute the specified action when the timer expires
        // The timer is stopped when the action is executed, unless it's a recurring timer
        public static void StartTimer(int intervalMilliseconds, Action action, bool isRecurring = false)
        {
            Thread timerThread = new Thread(() =>
            {
                System.Threading.Timer timer = null;
                timer = new System.Threading.Timer(_ =>
                {
                    // Stop the timer if it's not recurring
                    if (!isRecurring)
                    {
                        timer.Dispose();
                    }
                    // Execute the action after the timer expires
                    action.Invoke();
                }, null, 0, isRecurring ? intervalMilliseconds : Timeout.Infinite);

                // Keep the thread alive until the action is executed (for single-shot timer)
                if (!isRecurring)
                {
                    Thread.Sleep(intervalMilliseconds);
                }
            });
            timerThread.Start();
        }
    }
}
