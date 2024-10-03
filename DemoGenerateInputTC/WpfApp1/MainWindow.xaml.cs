using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using RecorderLib;
namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Step> users = new ObservableCollection<Step>();
        string url;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                dgUsers.ItemsSource = users;
                // [GT] https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.backgroundworker?view=netframework-4.7.2
                //BackgroundWorker bgw = new BackgroundWorker();
                //bgw.DoWork +=
                //new DoWorkEventHandler(backgroundWorker1_DoWork);
                //bgw.RunWorkerAsync();
            }
            catch (Exception ex)
            {

            }

        }




        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {            
            Recorder recorder = new Recorder();
            recorder.LaunchBrowser();
            recorder.GoToUrl(url);
            Application.Current.Dispatcher.Invoke(() =>
            {
                users.Add(new Step { Action = "Navigate", Value = url,ElementId = "url", ActionNumber = 1});
            });
            while (true)
            {
                try
                {
                    Step obj = recorder.AttachListener(users.Count);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        users.Add(obj);
                    });
                }
                catch
                {

                }

            }

        }

        private void Record_Click(object sender, RoutedEventArgs e)
        {
            url = Url.Text; 
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork +=
            new DoWorkEventHandler(backgroundWorker1_DoWork);
            bgw.RunWorkerAsync();
        }
    }
}
