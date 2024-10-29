using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class ECG
    {
        
        private double amplitude = 0.0;
        private double frequency = 0;
        private int harmonics = 1;


        public double Amplitude { get {return amplitude; } set { amplitude = value; } }
        public double Frequency { get { return frequency; } set { frequency = value; } }

        public int Harmonics { get {return harmonics; } set {harmonics=value; } }

        public ECG(double amplitude, double frequency, int harmonics)
        {
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.harmonics = harmonics;
        }
        
        public double NextSample(double timeIndex)
        {
            const double HzToBeatsPerMin = 60.0;
            double sample;

            sample = Math.Cos(2 * Math.PI * (frequency / HzToBeatsPerMin) * timeIndex);
            sample *= amplitude;
            return (sample);
        }
    }
}

