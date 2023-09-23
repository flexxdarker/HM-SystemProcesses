using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace Hm23._09._2023
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static class Solving
        {
            
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowInWpf_Click(object sender, RoutedEventArgs e)
        {
            Parallel.Invoke(() => Solve(false));
        }

        private void ShowInFile_Click(object sender, RoutedEventArgs e)
        {
            Parallel.Invoke(()=> Solve(true));
        }
        private void Solve(bool save)
        {
            int sentenceCount = 0;
            int symbolCount = 0;
            int wordCount = 0;
            int questionCount = 0;
            int exclamatorySentence = 0;
            string text = userTextTb.Text;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '.')
                {
                    sentenceCount++; 
                }
                if (text[i] != ' ')
                {
                    symbolCount++;
                }
                if(text[i] == ' ')
                {
                    wordCount++;
                }
                if (text[i] == '?')
                {
                    questionCount++;
                }
                else if (text[i] == '!')
                {
                    exclamatorySentence++;
                }
            }
            string results = $"Senteces: {sentenceCount}, Symbols: {symbolCount}, Words: {wordCount}, Question Sentences: {questionCount}, Exclamentory Senteces: {exclamatorySentence}";
            if(save == true)
            {
                File.WriteAllText("C:\\Users\\ksenz\\source\\repos\\HM-SystemProcesses\\Hm23.09.2023\\save.txt", results);
            }
            else
            {
                showLB.Items.Add(sentenceCount);
                showLB.Items.Add(symbolCount);
                showLB.Items.Add(wordCount);
                showLB.Items.Add(questionCount);
                showLB.Items.Add(exclamatorySentence);
            }
        }

        private void Stop_Restart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            string text = userTextTb.Text;
            int symbolTimes = Convert.ToInt32(timeOfSymbol.Text);
            char symbol = Convert.ToChar(choseSymbol.Text);
            int times = 0;
            string texts = null;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == symbol) 
                {
                    times++;
                    texts += Convert.ToString(text[i]);
                    if (times == symbolTimes) 
                    {
                        showText.Content = texts;
                        
                    }
                }
            }
        }
    }
}
