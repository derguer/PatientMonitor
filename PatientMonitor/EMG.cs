using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class EMG
    {
        //Parameters
        const double HzToBeatsPerMin = 60.0;
        private double amplitude = 0.0;
        private double frequency = 0.0;
        private int harmonics = 1;
        public double Amplitude
        {
            set { amplitude = value; }
            get { return amplitude; }
        }
        public double Frequency
        {
            set { frequency = value; }
            get { return frequency; }
        }
        public int Harmonics
        {
            set { harmonics = value; }
            get { return harmonics; }
        }
        public EMG(double amplitude, double frequency, int harmonics)
        {
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.harmonics = harmonics;
        }
        public EMG() { }

        public double NextSample(double timeIndex)
        {


            double sample = 0.0;
            double stepIndex = 0.0;
            double signalLength = 1.0;
            timeIndex=timeIndex/6000;

            signalLength = (double)(1.0 /frequency);
            stepIndex = (double)(timeIndex % signalLength);
            if (stepIndex > (signalLength / 2.0))
            {
                sample = 1;
                Console.Write("sample=1");
            }
            else
            {
                sample = -1;
                Console.Write("sample=-1");
            }
            sample *= amplitude;
            return (sample);
        }
    }
}
