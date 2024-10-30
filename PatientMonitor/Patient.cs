using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class Patient
    {
        ECG ecg;
        EMG emg;

        private string patientName;
        private DateTime dateOfStudy;
        private int age;
        

        public int Age { get { return age; } set { age = value; } }
        public string PatientName { get {return patientName; } set {patientName=value; } }
        public DateTime DateOfStudy { get {return dateOfStudy; } set {dateOfStudy=value; } }

        public Patient(string patientName, int age, DateTime dateOfStudy, double amplitude, double frequency, int harmonics)
        {
            this.patientName = patientName; this.age = age; this.dateOfStudy = dateOfStudy;
            ecg = new ECG(amplitude, frequency, harmonics);
            emg = new EMG(amplitude, frequency, harmonics);
        }
        public Patient(string patientName, int age, DateTime dateOfStudy)
        {
            this.patientName = patientName; this.age = age; this.dateOfStudy = dateOfStudy;
            ecg = new ECG();
        }


        //ECG ----
        public double ECGAmplitude { set {ecg.Amplitude=value; } get { return ecg.Amplitude; } }
        public double ECGFrequency { set {ecg.Frequency=value; } get { return ecg.Frequency; } }
        public int ECGHarmonics { set {ecg.Harmonics=value; } get { return ecg.Harmonics; } }

        //EMG ----
        public double EMGAmplitude { set { emg.Amplitude = value; } get { return emg.Amplitude; } }
        public double EMGFrequency { set { emg.Frequency = value; } get { return emg.Frequency; } }
        public int EMGHarmonics { set { emg.Harmonics = value; } get { return emg.Harmonics; } }

        public double NextSample(double timeIndex, Constants.Parameter parameter)
        {
            double nextSample = 0.0;
            //nextSample = this.ecg.NextSample(timeIndex)

            switch (parameter)
            {
                case Constants.Parameter.ECG:
                    nextSample = ecg.NextSample(timeIndex);
                    break;
                case Constants.Parameter.EMG:
                    nextSample = emg.NextSample(timeIndex);
                    break;
                default:
                    break;
            }
            return (nextSample);
        }

    }
}
