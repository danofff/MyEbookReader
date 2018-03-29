using System;
using System.Collections.Generic;
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
using System.Net;

namespace MyEbookReader
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string eBook;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnDownLoad_Click(object sender, RoutedEventArgs e)
        {
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += (s, eArgs) =>
            {
                eBook = eArgs.Result;
                txtBox.Text = eBook;
            };
            wc.DownloadStringAsync(new Uri(@"http://www.gutenberg.org/files/98/98-8.txt"));
        }

        private void btnGetStats_Click(object sender, RoutedEventArgs e)
        {
            string[] words = eBook.Split(new char[] { ' ', ',', '.', ';', ':', '-', '?', '!', '/' ,'\u000A'}, StringSplitOptions.RemoveEmptyEntries);
            string[] tenMostCommon = null;
            string longestWord = String.Empty;

            Parallel.Invoke(() =>
            {
                tenMostCommon = FindTenMostCommon(words);
            },
            () =>
            {
                longestWord = FindLongestWord(words);
            });
            
            
            StringBuilder bookStats = new StringBuilder("Ten Most Common Words are:\n");
            foreach (var item in tenMostCommon)
            {
                bookStats.AppendLine(item);
            }
            bookStats.Append("Longest word is " + longestWord);
            bookStats.AppendLine();
            MessageBox.Show(bookStats.ToString(), "Book info");

        }

        private string[]FindTenMostCommon(string[] words)
        {
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into g
                                 orderby g.Count() descending
                                 select g.Key;
            string[] commonWords = (frequencyOrder.Take(10)).ToArray();
            return commonWords;

        }
        private string FindLongestWord(string [] words)
        {
            return (from w in words orderby w.Length descending select w).FirstOrDefault();
        }
    }
}
