using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class EEG
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
        public EEG(double amplitude, double frequency, int harmonics)
        {
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.harmonics = harmonics;
        }
        public EEG() { }

        public double NextSample(double timeIndex)
        {

            timeIndex = timeIndex / 6000;

            // Beispielhafte Frequenzen für verschiedene EEG-Bänder
            double deltaFrequency = 1.0; // 0.5 - 4 Hz
            double thetaFrequency = 6.0; // 4 - 8 Hz
            double alphaFrequency = 10.0; // 8 - 12 Hz
            double betaFrequency = 20.0; // 12 - 30 Hz
            double gammaFrequency = 40.0; // 30 - 100 Hz

            // Erzeugung des EEG-Signals durch Addition der verschiedenen Frequenzkomponenten
            double sample = Math.Sin(2 * Math.PI * deltaFrequency * timeIndex) +
                            Math.Sin(2 * Math.PI * thetaFrequency * timeIndex) +
                            Math.Sin(2 * Math.PI * alphaFrequency * timeIndex) +
                            Math.Sin(2 * Math.PI * betaFrequency * timeIndex) +
                            Math.Sin(2 * Math.PI * gammaFrequency * timeIndex);

            sample *= amplitude;
            return sample;
        }
    }
}
