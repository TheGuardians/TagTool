using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TagTool.Tools
{
    static class AsyncJobManager
    {
        private static ConcurrentDictionary<Task, Task> Tasks = new ConcurrentDictionary<Task, Task>();
        //private static WeakList<Task> Tasks = new WeakList<Task>();

        private static void CleanupTask(Task task)
        {
            Tasks.TryRemove(task, out Task value);
        }

        public static void Schedule(Action action)
        {
            Task task = new Task(action);
            task.ContinueWith(CleanupTask);
            Tasks[task] = task;
            task.Start();
        }

        public static void Schedule(Action<object> action, object input)
        {
            Task task = new Task(action, input);
            task.ContinueWith(CleanupTask);
            Tasks[task] = task;
            task.Start();
        }

        public static void CleanupFile(string filename, int timeoutMs)
        {
            Schedule((_timeoutMs) => {

                Exception previous_error = null;
                bool success = false;
                var time = Stopwatch.StartNew();
                while (time.ElapsedMilliseconds < (int)_timeoutMs)
                {
                    try
                    {
                        if (File.Exists(filename as string))
                            File.Delete(filename as string);
                        success = true;
                        break;
                    }
                    catch (IOException e)
                    {
                        previous_error = e;
                        // access error
                        if (e.HResult != -2147024864)
                        {
                            throw;
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                    }
                }
                if(!success)
                {
                    throw previous_error;
                }

            }, timeoutMs);
        }

        public static void Schedule(Task task)
        {
            Tasks[task] = task;
            task.ContinueWith(CleanupTask);
        }

        public static void WaitForAll()
        {
            Task.WaitAll(Tasks.Values.ToArray());
        }
    }
}
