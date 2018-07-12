using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProfitChecker
{
    class Threads
    {
        public static Dictionary<string, Thread> threads = new Dictionary<string, Thread>();
        public static Dictionary<string, Thread> activeThreads = new Dictionary<string, Thread>();
        public static Dictionary<string, Thread> pausedThreads = new Dictionary<string, Thread>();

        public static void Add(string name, ThreadStart function)
        {
            if (threads.TryGetValue(name, out Thread temp)) return;
            Thread t = new Thread(function);
            t.IsBackground = true;
            threads.Add(name, t);
        }

        public static void ToggleThread(string name)
        {
            if (activeThreads.TryGetValue(name, out Thread temp))
            {
                #pragma warning disable CS0618
                temp.Suspend();
                #pragma warning restore CS0618

                activeThreads.Remove(name);

                pausedThreads.Add(name, temp);
            }
            else
            {
                if (!threads.TryGetValue(name, out temp))
                {
                    Console.WriteLine("Error starting Thread \"" + name + "\". Please consult the developer.");
                    return;
                }

                if (pausedThreads.TryGetValue(name, out Thread temp2))
                {
                    #pragma warning disable CS0618
                    temp2.Resume();
                    #pragma warning restore CS0618

                    pausedThreads.Remove(name);
                }
                else
                {
                    temp.Start();
                }

                activeThreads.Add(name, temp);
            }
        }
    }
}