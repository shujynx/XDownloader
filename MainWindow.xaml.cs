using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;

namespace XDownloader
{
    /*
     
     ytdlp can download from YouTube, Twitter, Reddit, Instagram?, TikTok
     
     */
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("XDownloader\nVersion 1.0");
        }

        private void DependencyInstall_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("./Dependencies/yt-dlp.exe")) 
            {
                MessageBox.Show("All dependencies needed are already installed.");
            } 
            else if (MessageBox.Show("Do you want to install all dependencies?", "Install all dependencies", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                if (!Directory.Exists("./Dependencies")) Directory.CreateDirectory("./Dependencies");
                WebClient wc = new WebClient();
                wc.DownloadFileAsync(new Uri("https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe"), "./Dependencies/yt-dlp.exe");
                MessageBox.Show("Finished downloading all dependencies needed!");
            }
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            string args = "yt-dlp -f";
            string url = URLTextbox.Text;
            string downloadPath = DownloadDirectoryTextbox.Text;

            if (url == "" || url == null) { MessageBox.Show("You need to provide a URL"); return; }

            args += " best";

            if(downloadPath != "") 
            {
                if (!Directory.Exists(downloadPath)) Directory.CreateDirectory(downloadPath);
                args += $@" {url} -o {downloadPath}/%(title)s-%(id)s.mp4";
            } else
            {
                args += $@" {url} -o ../Downloads/%(title)s-%(id)s.mp4";
            }

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine("cd Dependencies");
            cmd.StandardInput.WriteLine(args);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            cmd.Close();
            MessageBox.Show("Download Completed.", "", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
