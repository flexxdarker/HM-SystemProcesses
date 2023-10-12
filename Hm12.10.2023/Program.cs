namespace Hm12._10._2023
{
    internal class Program
    {
        public static void CreateArray(object obj)
        {
            var semaphore = obj as Semaphore;

            bool stop = false;
            while (!stop)
            {
                if (semaphore.WaitOne())
                {
                    try
                    {
                        int[] arr = new int[new Random().Next(1, 10)];
                        for (int i = 0; i < arr.Length; i++) 
                        { 
                            arr[i] = new Random().Next(1, 50); 
                        }
                        string tmp = $"Thread {Thread.CurrentThread.ManagedThreadId} generated the list of number : ";
                        for (int i = 0; i < arr.Length; i++) 
                        { 
                            tmp += ($" {arr[i]}"); 
                        }
                        Console.WriteLine(tmp);
                        Thread.Sleep(2000);
                    }
                    finally
                    {
                        stop = true;
                        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} removed a lock");
                        semaphore.Release();
                    }
                }
                else
                    Console.WriteLine($"Timeout for thread {Thread.CurrentThread.ManagedThreadId} expired. Waiting...");
            }
        }
        static void Main(string[] args)
        {
            Semaphore s = new Semaphore(3, 3);
            for (int i = 0; i < 10; ++i)
            {
                ThreadPool.QueueUserWorkItem(CreateArray, s);
            }

            Console.ReadKey();
        }
    }
}