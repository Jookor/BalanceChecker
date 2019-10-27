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
using System.Windows.Threading;

namespace BalanceChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string side;
        private string filename;
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            side = "Right";
            DateTime date = DateTime.UtcNow;
            filename = date.ToString().Replace(".", "").Replace(":", "").Replace(" ", "_");
            btnStart.IsEnabled = true;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            Record.RecordAudio();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Start();
            btnStart.IsEnabled = false;
            btnStart.Content = "Recording...";

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Record.StopRecording(side, filename);
            dispatcherTimer.Stop();
            if(side == "Left")
            {
                String result = Analyse.CheckBalance(filename);
                lblStatus.Content = result;
                if (result == "Pass")
                {
                    lblStatus.Background = Brushes.Green; 

                }
                else
                {
                    lblStatus.Background = Brushes.Red;
                }

            }
            btnStart.IsEnabled = true;
            side = "Left";
            btnStart.Content = "Record left channel";
        }
    }
}
