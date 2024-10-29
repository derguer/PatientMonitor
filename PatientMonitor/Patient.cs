using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class Patient
    {
        private string patientName;
        private DateTime dateOfStudy;
        private int age;
        private ECG ecg;

        public int Age { get { return age; } set { age = value; } }
        public string PatientName { get {return patientName; } set {patientName=value; } }
        public DateTime DateOfStudy { get {return dateOfStudy; } set {dateOfStudy=value; } }

        public Patient(string patientName, int age, DateTime dateOfStudy, double amplitude, double frequency, int harmonics)
        {
            this.patientName = patientName; this.age = age; this.dateOfStudy = dateOfStudy;
            ecg = new ECG(amplitude, frequency, harmonics);
        }


        public double ECGAmplitude { set {ecg.Amplitude=value; } }
        public double ECGFrequency { set {ecg.Frequency=value; } }
        public int ECGHarmonics { set {ecg.Harmonics=value; } }

        public double NextSample(double timeIndex)
        {
            //double nextSample = 0.0;
            //nextSample = this.ecg.NextSample(timeIndex)

            return this.ecg.NextSample(timeIndex);
            //return (nextSample);
        }

    }
}
