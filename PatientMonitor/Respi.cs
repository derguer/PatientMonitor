using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class Resp
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
        public Resp(double amplitude, double frequency, int harmonics)
        {
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.harmonics = harmonics;
        }
        public Resp() { }

        public double NextSample(double timeIndex)
        {

            timeIndex = timeIndex / 6000;

            double sample = 0.0;
            double signalLength = 1.0 / frequency; // Dauer eines Signals
            double stepIndex = timeIndex % signalLength; // Zeit innerhalb eines Signalzyklus

            // Normiere auf den Bereich von -1 bis 1
            sample = (2.0 * (stepIndex / signalLength)) - 1.0;

            // Skaliere mit der Amplitude
            sample *= amplitude;

            return sample;
        }
    }
}
