﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class EEG : PhysioParameter, IPhysioFunctions
    {

        public EEG() : base() { }
        public EEG(double amplitude, double frequency, int harmonics) : base(amplitude, frequency, harmonics) { }


        public override double NextSample(double timeIndex)
        {
            timeIndex = timeIndex / 6000;

            double sample = 0.0;
            double signalLength = 1.0 / Frequency;
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
