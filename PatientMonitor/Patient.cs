using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PatientMonitor
{
    class Patient
    {
        ECG ecg;
        EMG emg;
        EEG eeg;
        Resp resp;
        MRImages mRImages;

        private string patientName;
        private DateTime dateOfStudy;
        private int age;

        // New Variables for delivery 6
        const int maxSamples = 1024;
        List<double> sampleList = new List<double>(maxSamples);


        public int Age { get { return age; } set { age = value; } }
        public string PatientName { get {return patientName; } set {patientName=value; } }
        public DateTime DateOfStudy { get {return dateOfStudy; } set {dateOfStudy=value; } }

        public Patient(string patientName, int age, DateTime dateOfStudy, double amplitude, double frequency, int harmonics)
        {
            this.patientName = patientName; this.age = age; this.dateOfStudy = dateOfStudy;
            ecg = new  ECG(amplitude, frequency, harmonics);
            emg = new  EMG(amplitude, frequency, harmonics);
            eeg = new  EEG(amplitude, frequency, harmonics);
            resp = new Resp(amplitude, frequency, harmonics);
            mRImages = new MRImages(); mRImages.loadImages("Z:/Softwaretechnik/Labore/PatientMonitor/MRT_Bilder/CT+MR+contrast+montage-1574688855.jpg");
        }
        public Patient(string patientName, int age, DateTime dateOfStudy)
        {
            this.patientName = patientName; this.age = age; this.dateOfStudy = dateOfStudy;
            ecg  = new ECG();
            emg  = new EMG();
            eeg  = new EEG();
            resp = new Resp();
        }


        //ECG ----
        public double ECGAmplitude { set {ecg.Amplitude=value; } get { return ecg.Amplitude; } }
        public double ECGFrequency { set {ecg.Frequency=value; } get { return ecg.Frequency; } }
        public int ECGHarmonics { set {ecg.Harmonics=value; } get { return ecg.Harmonics; } }
        public double ECGHighAlarm { get { return ecg.HighAlarm; } set { ecg.HighAlarm = value; } }
        public double ECGLowAlarm { get { return ecg.LowAlarm; } set { ecg.LowAlarm = value; } }
        public string ECGLowAlarmString { get { return ecg.LowAlarmString; } }
        public string ECGHighAlarmString { get { return ecg.HighAlarmString; } }


        //EMG ----
        public double EMGAmplitude { set { emg.Amplitude = value; } get { return emg.Amplitude; } }
        public double EMGFrequency { set { emg.Frequency = value; } get { return emg.Frequency; } }
        public int EMGHarmonics { set { emg.Harmonics = value; } get { return emg.Harmonics; } }
        public double EMGHighAlarm { get { return emg.HighAlarm; } set { emg.HighAlarm = value; } }
        public double EMGLowAlarm { get { return emg.LowAlarm; } set { emg.LowAlarm = value; } }
        public string EMGLowAlarmString { get { return emg.LowAlarmString; }}
        public string EMGHighAlarmString { get { return emg.HighAlarmString; } }


        //EEG ----
        public double EEGAmplitude { set { eeg.Amplitude = value; } get { return eeg.Amplitude; } }
        public double EEGFrequency { set { eeg.Frequency = value; } get { return eeg.Frequency; } }
        public int EEGHarmonics { set { eeg.Harmonics = value; } get { return eeg.Harmonics; } }
        public double EEGHighAlarm { get { return eeg.HighAlarm; } set { eeg.HighAlarm = value; } }
        public double EEGLowAlarm { get { return eeg.LowAlarm; } set { eeg.LowAlarm = value; } }
        public string EEGLowAlarmString { get { return eeg.LowAlarmString; } }
        public string EEGHighAlarmString { get { return eeg.HighAlarmString; } }


        //Resp ----
        public double RespAmplitude { set { resp.Amplitude = value; } get { return resp.Amplitude; } }
        public double RespFrequency { set { resp.Frequency = value; } get { return resp.Frequency; } }
        public int RespHarmonics { set { resp.Harmonics = value; } get { return resp.Harmonics; } }
        public double RespHighAlarm { get { return resp.HighAlarm; } set { resp.HighAlarm = value; } }
        public double RespLowAlarm { get { return resp.LowAlarm; } set { resp.LowAlarm = value; } }
        public string RespiLowAlarmString { get { return resp.LowAlarmString; } }
        public string RespiHighAlarmString { get { return resp.HighAlarmString; } }

        public List<double> SampleList
        {
            get { return sampleList; }
        }



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

            if (sampleList.Count >= maxSamples)
            {
                sampleList.RemoveAt(0); // Remove the oldest sample
            }
            sampleList.Add(nextSample);

            return (nextSample);
        }

    }
}
