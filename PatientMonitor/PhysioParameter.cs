using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class PhysioParameter
    {
        private double amplitude = 0.0;
        private double frequency = 0.0;
        private int    harmonics =   1;


        public double Amplitude { get { return amplitude; } set { amplitude = value; } }
        public double Frequency { get { return frequency; } set { frequency = value; } }

        public int    Harmonics { get { return harmonics; } set { harmonics = value; } }

        public PhysioParameter(double amplitude, double frequency, int harmonics)
        {
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.harmonics = harmonics;
        }

        public PhysioParameter() {}

    }
}
