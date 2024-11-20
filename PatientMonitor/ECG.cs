using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class ECG : PhysioParameter, IPhysioFunctions
    {
        public ECG() : base() { }
        public ECG(double amplitude, double frequency, int harmonics) : base(amplitude, frequency, harmonics) { }

        

        public double NextSample(double timeIndex)
        {
            const double HzToBeatsPerMin = 6000.0;
            double sample;

            //sample = Math.Cos(2 * Math.PI * (frequency / HzToBeatsPerMin) * timeIndex);
            //sample *= amplitude;

           if(Harmonics == 0)
            {
                sample = Math.Cos(2 * Math.PI * (Frequency / HzToBeatsPerMin) * timeIndex);
                sample *= Amplitude;

                return(sample);
            }else if(Harmonics == 1)
            {
                sample =  Amplitude * Math.Cos(2 * Math.PI * (Frequency / HzToBeatsPerMin) * timeIndex);
                sample += Amplitude/2 * Math.Cos(2 * Math.PI * (2*Frequency / HzToBeatsPerMin) * timeIndex);
                return (sample);
            } else if(Harmonics == 2)
            {
                sample =   Amplitude * Math.Cos(2 * Math.PI * (Frequency / HzToBeatsPerMin) * timeIndex);
                sample +=  Amplitude/2 * Math.Cos(2 * Math.PI * (2 * Frequency / HzToBeatsPerMin) * timeIndex);
                sample +=  Amplitude/3 * Math.Cos(2 * Math.PI * (3 * Frequency / HzToBeatsPerMin) * timeIndex);
                return (sample);
            } else
            {
                sample = Math.Cos(2 * Math.PI * (Frequency / HzToBeatsPerMin) * timeIndex);
                sample *= Amplitude;
                return (sample);
            }
        }

        public new string LowAlarmString => base.LowAlarmString;
        public new string HighAlarmString => base.HighAlarmString;

        public new double LowAlarm
        {
            get => base.LowAlarm;
            set => base.LowAlarm = value;
        }
        public new double HighAlarm
        {
            get => base.HighAlarm;
            set => base.HighAlarm = value;
        }

    }
}

