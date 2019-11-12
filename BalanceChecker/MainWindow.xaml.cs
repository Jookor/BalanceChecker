using Microsoft.Win32;
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
        private bool allDone;
        private string filename;
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private MediaPlayer mediaPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            side = "Right";
            DateTime date = DateTime.UtcNow;
            filename = date.ToString().Replace(".", "").Replace(":", "").Replace(" ", "_");
            btnStart.IsEnabled = false;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {

            if (side=="Right")
            {
                allDone = false;
                lblStatus.Content = "";
                lblStatus.Visibility = Visibility.Hidden;
                
            }

            if (mediaPlayer.Source != null)
            {
                Record.RecordAudio();
                mediaPlayer.Play();
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
                dispatcherTimer.Start();
                btnStart.IsEnabled = false;
                btnStart.Content = "Recording...";

                if (side == "Left")
                {
                    allDone = true;
                } 
            }
            else
            {
                MessageBox.Show("Please select audio file!");
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            mediaPlayer.Stop();
            Record.StopRecording(side, filename);
            dispatcherTimer.Stop();
            if(allDone)
            {
                String result = Analyse.CheckBalance(filename);
                lblStatus.Content = result;
                lblStatus.Visibility = Visibility.Visible;
                if (result == "Pass")
                {
                    lblStatus.Background = Brushes.Green;
                }
                else
                {
                    lblStatus.Background = Brushes.Red;
                }
                btnStart.IsEnabled = true;
                btnStart.Content = "Check another one";
                side = "Right";
            }
            else
            {
                btnStart.IsEnabled = true;
                side = "Left";
                btnStart.Content = "Record left channel";
            }
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                mediaPlayer.Open(new Uri(openFileDialog.FileName));

            if (mediaPlayer.Source != null)
            {
                btnStart.IsEnabled = true;
                lblFile.Content = "Selected file: "+mediaPlayer.Source.LocalPath;
            }
        }
    }
}
