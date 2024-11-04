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
        private double amplitude = 1.0;
        private double frequency = 5;
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
            const double HzToBeatsPerMin = 6000.0;

            double sample = 0.0;
            double stepIndex = 0.0;
            double signalLength = 0.0;

            signalLength = (double)(1.0 * HzToBeatsPerMin /frequency);
            stepIndex = (double)(timeIndex % signalLength);
            if (stepIndex > (signalLength / 2.0))
            {
                sample = 1;
            }
            else
            {
                sample = -1;
            }
            sample *= this.Amplitude;
            return (sample);
        }
    }
}
