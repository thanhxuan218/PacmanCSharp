using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace PacmanWindowForms.Scripts
{

    class Logger
    {
        public static void Log(string message)
        {
            StackFrame frame = new StackFrame(1);
            string currentTime = DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff");
            var methodName = frame.GetMethod().Name;
            var codeLine = frame.GetFileLineNumber();
            Console.WriteLine(currentTime + ": " + methodName + ":" + codeLine + "\t:" + message);
        }
    }


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

        public static string GetRealFilePath(string fileName)
        {
            return Path.Combine(GetProjectDirectory(), fileName);
        }

        public static string GetProjectDirectory()
        {
            // Project dir is Path of
            return Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
        }

        public static string GetFullPathResource(string fileName)
        {
            return Path.Combine(GetProjectDirectory(), "Resource", fileName);
        }

        public static string GetFullPathMap(string fileName)
        {
            return Path.Combine(GetProjectDirectory(), "Resource", "Maps", fileName);
        }
    }
}
