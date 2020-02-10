using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace NissenCode
{
    public class ThreadTasker
    {
        List<Action> TaskList = new List<Action>();
        ThreadHandler[] ThreadHandlers;
        private bool isRunning;
        Thread myThread;
        public ThreadTasker(int maxThreads = 8)
        {
            ThreadHandlers = new ThreadHandler[maxThreads];
            myThread = new Thread(run);
        }
        public void start() {
            isRunning = true;
            myThread.Start();
        }
        public void stop()
        {
            isRunning = false;
            myThread.Abort();
        }
        private void run() {
            while (isRunning) {
                for (int i = ThreadHandlers.Length-1; i >= 0; i--)
                {
                    if (TaskList.Count == 0)
                        break;
                    ThreadHandler threadHandler = (ThreadHandlers[i] == null) ? new ThreadHandler() : ThreadHandlers[i];
                    if (!threadHandler.isRuning)
                    {
                        Action toDo = RemoveAndGet(TaskList.Count - 1);
                        Debug.Log("starting " + toDo.Target.GetType());
                        threadHandler.start(toDo);
                    }
                }
            }
        }
        public Action RemoveAndGet(int index)
        {
            if (index < 0 || index >= TaskList.Count)
                Debug.Log("asking for index:" + index + " of list with:" + TaskList.Count);
            Action value = TaskList[index];
            TaskList.RemoveAt(index);
            return value;
        }
        public void addTask(Action task) {
            TaskList.Insert(0, task);
        }
    }

    public class ThreadHandler
    {
        Action myTask;
        Thread myThread;
        public bool isRuning { get; internal set; }

        public ThreadHandler()
        {
            myThread = new Thread(run);
        }

        public void start(Action toDo)
        {
            isRuning = true;
            myTask = toDo;
            myThread.Start();
        }

        private void run() {
            if (myTask == null)
            {
                stop();
                return;
            }
            myTask.Invoke();
            stop();
        }
        private void stop() {
            myTask = null;
            isRuning = false;
            myThread.Abort();
        }
    }
}
