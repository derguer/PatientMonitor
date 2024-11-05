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

            double sample = 0.0;
            double signalLength = 1.0 / frequency;
            double halfSignalLength = signalLength / 2;
            double stepIndex = timeIndex % signalLength;

            // Konstanten für die exponentielle Funktion
            double alpha = 5.0; // Steilheit der exponentiellen Kurve, anpassbar

            if (stepIndex <= halfSignalLength)
            {
                // Exponentieller Anstieg von -Amplitude bis +Amplitude in der ersten Hälfte der Periode
                sample = -this.Amplitude + (2 * this.Amplitude * (1 - Math.Exp(-alpha * (stepIndex / halfSignalLength))));
            }
            else
            {
                // Exponentieller Abfall von +Amplitude bis -Amplitude in der zweiten Hälfte der Periode
                sample = this.Amplitude - (2 * this.Amplitude * (1 - Math.Exp(-alpha * ((stepIndex - halfSignalLength) / halfSignalLength))));
            }

            return sample;
        }
    }
}
