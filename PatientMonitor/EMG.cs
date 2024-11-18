using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class EMG : PhysioParameter, IPhysioFunctions
    {
        public EMG() : base() { }
        public EMG(double amplitude, double frequency, int harmonics) : base(amplitude, frequency, harmonics) { }

        public double NextSample(double timeIndex)
        {
            double sample       = 0.0;
            double stepIndex    = 0.0;
            double signalLength = 1.0;

            timeIndex=timeIndex/6000;

            signalLength = (double)(1.0 /Frequency);
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
            sample *= Amplitude;
            return (sample);
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
