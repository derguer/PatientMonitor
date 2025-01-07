using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    abstract class PhysioParameter
    {
        private double amplitude = 0.0;
        private double frequency = 0.0;
        private int harmonics = 1;

        // new Variables
        private double lowAlarm = 0;
        private double highAlarm = 0;
        private string lowAlarmString = " ";
        private string highAlarmString = " ";

        public double Amplitude { get { return amplitude; } set { amplitude = value; } }
        public double Frequency { get { return frequency; } set
            {
                frequency = value;
                CheckAlarms(); // Alarme prüfen, wenn die Frequenz geändert wird
            }
        }

        public int Harmonics { get { return harmonics; } set { harmonics = value; } }

        public PhysioParameter(double amplitude, double frequency, int harmonics)
        {
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.harmonics = harmonics;
        }
        public string LowAlarmString
        {
            get { return lowAlarmString; }
        }
        public string HighAlarmString
        {
            get { return highAlarmString; }
        }
        public double LowAlarm
        {
            get { return lowAlarm; }
            set
            {
                lowAlarm = value;
                CheckAlarms();
            }
        }

        public double HighAlarm
        {
            get { return highAlarm; }
            set
            {
                highAlarm = value;
                CheckAlarms();
            }
        }

        private void CheckAlarms()
        {
            if (frequency >= highAlarm)
            {
                highAlarmString = "HIGH ALARM: " + frequency;
                lowAlarmString = " "; // Vorrang für High Alarm
            }
            else if (frequency <= lowAlarm)
            {
                lowAlarmString = "LOW ALARM: " + frequency;
                highAlarmString = " ";
            }
            else
            {
                // Bestehende Alarme beibehalten, falls sie aktuell aktiv sind
                highAlarmString = highAlarmString.Contains("HIGH ALARM") ? highAlarmString : " ";
                lowAlarmString = lowAlarmString.Contains("LOW ALARM") ? lowAlarmString : " ";
            }
        }


        public PhysioParameter() { }
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

        public abstract double NextSample(double timeIndex);

    }
}