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

        // new Variables
        private double lowAlarm        = 0;
        private double highAlarm       = 0;
        private string lowAlarmString  = " ";
        private string highAlarmString = " ";

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
        public void displayHighAlarm(double frequency, double alarmHigh)
        {
            if (frequency >= alarmHigh)
            {
                highAlarmString = "HIGH ALARM:" + frequency;
            }
            else
                highAlarmString = " ";
        }
        public void displayLowAlarm(double frequency, double alarmLow)
        {
            if (frequency <= alarmLow)
            {
                lowAlarmString = "LOW ALARM:" + frequency;
            }
            else
                lowAlarmString = " ";
        }

    }
}
