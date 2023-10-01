using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hm_06._09._2023
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int from;
        public static int to;
        int lastInT1;
        int olderInT2 = 1;
        int lastInT2 = 1;
        Thread Task1;
        Thread Task2;
        CancellationTokenSource cts1 = new CancellationTokenSource();
        CancellationTokenSource cts2 = new CancellationTokenSource();
        CancellationToken token1;
        CancellationToken token2;

        public MainWindow()
        {
            InitializeComponent();
            Task1 = new Thread(AddTask1);
            Task2 = new Thread(AddTask2);
            Task1.IsBackground = true;
            Task2.IsBackground = true;
            token1 = cts1.Token;
            token2 = cts2.Token;
        }

        private void StartButton(object sender, RoutedEventArgs e)
        {
            cts1.Cancel();
            cts2.Cancel();
            cts1 = new CancellationTokenSource();
            cts2 = new CancellationTokenSource();
            token1 = cts1.Token;
            token2 = cts2.Token;
            Task1 = new Thread(AddTask1);
            Task2 = new Thread(AddTask2);
            from = Convert.ToInt32(fromTbx.Text);
            to = Convert.ToInt32(toTbx.Text);
            if (from > to)
            {
                int tmp = from;
                from = to;
                to = tmp;
            }
            lastInT1 = from;
            olderInT2 = 1;
            lastInT2 = 1;
            task1.Items.Clear();
            task2.Items.Clear();
            Task1.Start(new KeyValuePair<KeyValuePair<int, int>, CancellationToken>(new KeyValuePair<int, int>(from, to), token1));
            Task2.Start(new KeyValuePair<int, CancellationToken>(lastInT2, token2));
        }
        private void AddTask1(object obj)
        {
            var tmp = (KeyValuePair<KeyValuePair<int, int>, CancellationToken>)obj;
            while (tmp.Value.IsCancellationRequested == false)
            {
                for (int i = lastInT1; i <= to; i++)
                {
                    if (tmp.Value.IsCancellationRequested == true) break;
                    if (IsSimpleNum(i) == true)
                    {
                        lastInT1 = i;
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            lastInT1 = i;
                            task1.Items.Add(i);
                        }));
                        Thread.Sleep(1000);
                    }
                }
                return;
            }
        }
        private void AddTask2(object obj)
        {
            var tmp = (KeyValuePair<int, CancellationToken>)obj;
            lastInT2 = tmp.Key;
            while (tmp.Value.IsCancellationRequested == false)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    task2.Items.Add(lastInT2);
                    lastInT2 = GetFibonachi();
                }));
                Thread.Sleep(1000);
            }
        }
        private int GetFibonachi()
        {
            int sum = olderInT2 + lastInT2;
            olderInT2 = lastInT2;
            return sum;
        }
        private bool IsSimpleNum(int num)
        {
            for (int i = 2; i <= Math.Sqrt(num); i++)
                if (num % i == 0) return false;

            return true;
        }
        private int GetRandom(int a, int b) => new Random().Next(a, b);

        private void StopButton1(object sender, RoutedEventArgs e)
        {
            cts1.Cancel();
        }
        private void PauseButton1(object sender, RoutedEventArgs e)
        {
            lastInT1 = (int)task1.Items[task1.Items.Count - 1];
            cts1.Cancel();
        }
        private void ResumeButton1(object sender, RoutedEventArgs e)
        {
            cts1 = new CancellationTokenSource();
            token1 = cts1.Token;

            ++lastInT1;
            Task1 = new Thread(AddTask1);
            Task1.Start(new KeyValuePair<KeyValuePair<int, int>, CancellationToken>(new KeyValuePair<int, int>(lastInT1, to), token1));
        }

        private void StopButton2(object sender, RoutedEventArgs e)
        {
            cts2.Cancel();
        }

        private void PauseButton2(object sender, RoutedEventArgs e)
        {
            lastInT2 = (int)task2.Items[task2.Items.Count - 1];
            cts2.Cancel();
        }

        private void ResumeButton2(object sender, RoutedEventArgs e)
        {
            cts2 = new CancellationTokenSource();
            token2 = cts2.Token;
            Task2 = new Thread(AddTask2);
            Task2.Start(new KeyValuePair<int, CancellationToken>(lastInT2, token2));
        }
    }
}
