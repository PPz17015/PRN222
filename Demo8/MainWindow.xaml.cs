using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient client = new HttpClient
        {
            MaxResponseContentBufferSize = 1_000_000
        };
        private readonly IEnumerable<string> urls = new List<string>
        {
            "https://www.google.com",
            "https://www.bing.com",           
            "https://zenless.hoyoverse.com/en-us/main",
            "https://www.microsoft.com/en-us",
            "https://www.github.com"

        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            btnStartButton.IsEnabled = false;
            txtResult.Clear();
            await sumPageSizeAsync();
            txtResult.Text+=$"\nControl returned to {nameof(OnStartButtonClick)}\n";
            btnStartButton.IsEnabled = true;

        }
        private async Task sumPageSizeAsync()
        {
            var stopwatch =Stopwatch.StartNew();    
            int total = 0;
            foreach (var url in urls)
            {
                int size=await ProcessUrlAsync(url,client);
                total += size;
            }
            stopwatch.Stop();
            txtResult.Text += $"Total size: {total} bytes\n";
            txtResult.Text += $"Elapsed time: {stopwatch.ElapsedMilliseconds} ms\n";
        }

        private async Task<int> ProcessUrlAsync(string url, HttpClient client)
        {
            byte[] content= await client.GetByteArrayAsync(url);
            DisplayResult(url, content.Length);
            return content.Length;
        }

        private void DisplayResult(string url, int size)
        {
            txtResult.Text += $"URL: {url}, Size: {size} bytes\n";
            txtResult.ScrollToEnd();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            client.Dispose();
            base.OnClosing(e);
        }

    }
}