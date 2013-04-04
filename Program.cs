using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace exceptiondemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var crash = Task.Factory.StartNew(lockedWork, 1);
            Thread.Sleep(1000);
            var nocrash = Task.Factory.StartNew(lockedWork, null);

            try
            {
                Task.WaitAll(new[] { crash, nocrash });
            }
            catch (Exception)
            {
            }
        }

        static object lockRef = new object();
        static void lockedWork(object shouldCrash)
        {
            Console.WriteLine("thread {0} entering lock...", Thread.CurrentThread.ManagedThreadId);
            try
            {
                lock (lockRef)
                {
                    Console.WriteLine("thread {0} entered lock successfully.", Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(5000);
                    if (shouldCrash != null)
                        throwException();
                }
                Console.WriteLine("thread {0} exited lock normally.", Thread.CurrentThread.ManagedThreadId);
            }
            catch (Exception)
            {
                Console.WriteLine("thread {0} exited lock abnormally.", Thread.CurrentThread.ManagedThreadId);
            }
            finally
            {
                Console.WriteLine("thread {0} completed.", Thread.CurrentThread.ManagedThreadId);
            }
        }

        static void throwException()
        {
            object[] xs = new object[int.MaxValue];
        }
    }
}
