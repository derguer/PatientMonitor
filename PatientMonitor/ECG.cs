using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class ECG
    {
        
        private double amplitude = 1.0;
        private double frequency = 5.0;
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
        public ECG() { }
        
        public double NextSample(double timeIndex)
        {
            const double HzToBeatsPerMin = 6000.0;
            double sample;

            //sample = Math.Cos(2 * Math.PI * (frequency / HzToBeatsPerMin) * timeIndex);
            //sample *= amplitude;

           if(harmonics == 0)
            {
                sample = Math.Cos(2 * Math.PI * (frequency / HzToBeatsPerMin) * timeIndex);
                sample *= amplitude;

                return(sample);
            }else if(harmonics == 1)
            {
                sample =  amplitude * Math.Cos(2 * Math.PI * (frequency / HzToBeatsPerMin) * timeIndex);
                sample += amplitude/2 * Math.Cos(2 * Math.PI * (2*frequency / HzToBeatsPerMin) * timeIndex);
                return (sample);
            } else if(harmonics == 2)
            {
                sample =   amplitude * Math.Cos(2 * Math.PI * (frequency / HzToBeatsPerMin) * timeIndex);
                sample +=  amplitude/2 * Math.Cos(2 * Math.PI * (2 * frequency / HzToBeatsPerMin) * timeIndex);
                sample +=  amplitude/3 * Math.Cos(2 * Math.PI * (3 * frequency / HzToBeatsPerMin) * timeIndex);
                return (sample);
            } else
            {
                sample = Math.Cos(2 * Math.PI * (frequency / HzToBeatsPerMin) * timeIndex);
                sample *= amplitude;
                return (sample);
            }
        }
    }
}

