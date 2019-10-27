using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BalanceChecker
{
    public class Record
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        public static void RecordAudio()
        {
            mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
            mciSendString("record recsound", "", 0, 0);
            //System.Threading.Thread.Sleep(10000);
        }

        public static void StopRecording(string side, string filename)
        {
            mciSendString("save recsound d:\\result\\" + filename + side + ".wav", "", 0, 0);
            mciSendString("close recsound ", "", 0, 0);
        }

    }
}
