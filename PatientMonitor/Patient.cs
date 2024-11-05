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
        EEG eeg;
        Resp resp;

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
            eeg = new EEG(amplitude, frequency, harmonics);
            resp = new Resp(amplitude, frequency, harmonics);
        }
        public Patient(string patientName, int age, DateTime dateOfStudy)
        {
            this.patientName = patientName; this.age = age; this.dateOfStudy = dateOfStudy;
            ecg = new ECG();
            emg = new EMG();
            eeg = new EEG();
            resp = new Resp();
        }


        //ECG ----
        public double ECGAmplitude { set {ecg.Amplitude=value; } get { return ecg.Amplitude; } }
        public double ECGFrequency { set {ecg.Frequency=value; } get { return ecg.Frequency; } }
        public int ECGHarmonics { set {ecg.Harmonics=value; } get { return ecg.Harmonics; } }

        //EMG ----
        public double EMGAmplitude { set { emg.Amplitude = value; } get { return emg.Amplitude; } }
        public double EMGFrequency { set { emg.Frequency = value; } get { return emg.Frequency; } }
        public int EMGHarmonics { set { emg.Harmonics = value; } get { return emg.Harmonics; } }

        //EEG ----
        public double EEGAmplitude { set { eeg.Amplitude = value; } get { return eeg.Amplitude; } }
        public double EEGFrequency { set { eeg.Frequency = value; } get { return eeg.Frequency; } }
        public int EEGHarmonics { set { eeg.Harmonics = value; } get { return eeg.Harmonics; } }

        //Raspi ----
        public double RespAmplitude { set { resp.Amplitude = value; } get { return resp.Amplitude; } }
        public double RespFrequency { set { resp.Frequency = value; } get { return resp.Frequency; } }
        public int RespHarmonics { set { resp.Harmonics = value; } get { return resp.Harmonics; } }


        public double NextSample(double timeIndex, MonitorConstants.Parameter parameter)
        {
            double nextSample = 0.0;
            //nextSample = this.ecg.NextSample(timeIndex)

            switch (parameter)
            {
                case MonitorConstants.Parameter.ECG:
                    nextSample = ecg.NextSample(timeIndex);
                    break;
                case MonitorConstants.Parameter.EMG:
                    nextSample = emg.NextSample(timeIndex);
                    break;
                case MonitorConstants.Parameter.EEG:
                        nextSample = eeg.NextSample(timeIndex);
                    break;
                case MonitorConstants.Parameter.Resp:
                    nextSample = resp.NextSample(timeIndex);
                    break;
                default:
                    break;
            }
            return (nextSample);
        }

    }
}
