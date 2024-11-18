using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class Resp : PhysioParameter, IPhysioFunctions
    {
        public Resp() : base() { }
        public Resp(double amplitude, double frequency, int harmonics) : base(amplitude, frequency, harmonics) { }

        public double NextSample(double timeIndex)
        {

            timeIndex = timeIndex / 6000;

            double sample = 0.0;
            double signalLength = 1.0 / Frequency; // Dauer eines Signals
            double stepIndex = timeIndex % signalLength; // Zeit innerhalb eines Signalzyklus

            // Normiere auf den Bereich von -1 bis 1
            sample = (2.0 * (stepIndex / signalLength)) - 1.0;

            // Skaliere mit der Amplitude
            sample *= Amplitude;

            return sample;
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
