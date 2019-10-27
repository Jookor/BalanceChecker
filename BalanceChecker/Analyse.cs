using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceChecker
{
    public class Analyse
    {
        public static string CheckBalance(string filename)
        {
            string result;

            //Path to files
            string rightPath = "d:\\result\\" + filename + "right.wav";
            string leftPath = "d:\\result\\" + filename + "left.wav";

            //Open files
            AudioFileReader right = new AudioFileReader(rightPath);
            AudioFileReader left = new AudioFileReader(leftPath);

            //Now we need the max value off the right side sample
            float rightMax = 0;

            float[] rightBuffer = new float[right.WaveFormat.SampleRate];
            int rightRead;
            do
            {
                rightRead = right.Read(rightBuffer, 0, rightBuffer.Length);
                for (int n = 0; n < rightRead; n++)
                {
                    var abs = Math.Abs(rightBuffer[n]);
                    if (abs > rightMax) rightMax = abs;
                }
            } while (rightRead > 0);

            //And same thing for the left side sample
            float leftMax = 0;
            float[] buffer = new float[left.WaveFormat.SampleRate];
            int leftRead;
            do
            {
                leftRead = left.Read(buffer, 0, buffer.Length);
                for (int n = 0; n < leftRead; n++)
                {
                    var abs = Math.Abs(buffer[n]);
                    if (abs > leftMax) leftMax = abs;
                }
            } while (leftRead > 0);

            //Let's compaire the both max values
            if (rightMax == leftMax)
            {
                result = "Pass";
            }
            else
            {
                result = "Fail";
            }

            return result;
        }
    }
}
