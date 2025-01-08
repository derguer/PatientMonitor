/// <summary>
/// Die Klasse Patient repräsentiert einen Patienten und speichert seine grundlegenden Informationen
/// wie Name, Alter, Untersuchungsdatum und Klinikzugehörigkeit. Sie enthält zudem Eigenschaften und
/// Methoden zur Verwaltung physiologischer Parameter wie ECG, EMG, EEG und Resp.
/// 
/// Funktionen der Klasse:
/// - Speichern und Abrufen von Patientendaten
/// - Initialisierung und Verwaltung physiologischer Parameter
/// - Berechnung und Speicherung von Samples für jeden Parameter
/// - Bereitstellung von Alarmgrenzen und Harmonischen für die Parameter
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PatientMonitor
{
    // Klasse, die Patientendaten und zugehörige Parameter verwaltet
    class Patient
    {
        // Objekte zur Erfassung physiologischer Parameter
        ECG ecg;
        EMG emg;
        EEG eeg;
        Resp resp;
        MRImages mRImages; // Bilder des Patienten

        // Private Felder für Patientendaten
        private string patientName;
        private DateTime dateOfStudy;
        private MonitorConstants.clinic clinic;
        private int age;
        // Liste zur Speicherung von Samples mit einer maximalen Größe
        List<double> sampleList = new List<double>(maxSamples);

        
        const int maxSamples = 1024;  // Maximale Anzahl an Samples




        // Öffentliche Eigenschaften für Patiententyp und Raum
        public string Type { get; set; } // Typ des Patienten (Ambulatory/Stationary)
        public string Room { get; set; } = "No Room"; // Standardwert für Raum
                                                     
        public int Age { get { return age; } set { age = value; } }  // Eigenschaft für das Alter des Patienten
        public string PatientName { get {return patientName; } set {patientName=value; } } // Eigenschaft für den Namen des Patienten
        public DateTime DateOfStudy { get {return dateOfStudy; } set {dateOfStudy=value; } }  // Eigenschaft für das Untersuchungsdatum

        // Eigenschaften zur Kennzeichnung, ob der Patient ambulant oder stationär ist
        public bool Ambulatory { get; set; }
        public bool Stationary { get; set; }
        // Konstruktor mit vollständigen Parameterangaben
        public Patient(string patientName, int age, DateTime dateOfStudy, double amplitude, double frequency, int harmonics, MonitorConstants.clinic clinic)
        {
            this.patientName = patientName;
            this.age = age;
            this.dateOfStudy = dateOfStudy;
            this.clinic = clinic;
            // Initialisierung der physiologischen Parameter
            ecg = new  ECG(amplitude, frequency, harmonics);
            emg = new  EMG(amplitude, frequency, harmonics);
            eeg = new  EEG(amplitude, frequency, harmonics);
            resp = new Resp(amplitude, frequency, harmonics);
           

            mRImages = new MRImages();
            
        }
        // Konstruktor ohne Angabe von Amplitude, Frequenz und Harmonischen
        public Patient(string patientName, int age, DateTime dateOfStudy)
        {
            this.patientName = patientName;
            this.age = age;
            this.dateOfStudy = dateOfStudy;
            // Initialisierung der physiologischen Parameter mit Standardwerten
            ecg = new ECG();
            emg  = new EMG();
            eeg  = new EEG();
            resp = new Resp();
        }
        // Eigenschaft zum Abrufen von MR-Bildern, erstellt ein neues Objekt, falls null
        public MRImages MRImages {
            get
            {
                if (mRImages == null)
                {
                    mRImages = new MRImages();
                }
                return mRImages;
            }
        }

        // Eigenschaften zur Steuerung und Abfrage der physiologischen Parameter

        // ECG-Parameter
        public double ECGAmplitude { set {ecg.Amplitude=value; } get { return ecg.Amplitude; } }
        public double ECGFrequency { set {ecg.Frequency=value; } get { return ecg.Frequency; } }
        public int ECGHarmonics { set {ecg.Harmonics=value; } get { return ecg.Harmonics; } }
        public double ECGHighAlarm { get { return ecg.HighAlarm; } set { ecg.HighAlarm = value; } }
        public double ECGLowAlarm { get { return ecg.LowAlarm; } set { ecg.LowAlarm = value; } }
        public string ECGLowAlarmString { get { return ecg.LowAlarmString; } }
        public string ECGHighAlarmString { get { return ecg.HighAlarmString; } }


        // EMG-Parameter
        public double EMGAmplitude { set { emg.Amplitude = value; } get { return emg.Amplitude; } }
        public double EMGFrequency { set { emg.Frequency = value; } get { return emg.Frequency; } }
        public int EMGHarmonics { set { emg.Harmonics = value; } get { return emg.Harmonics; } }
        public double EMGHighAlarm { get { return emg.HighAlarm; } set { emg.HighAlarm = value; } }
        public double EMGLowAlarm { get { return emg.LowAlarm; } set { emg.LowAlarm = value; } }
        public string EMGLowAlarmString { get { return emg.LowAlarmString; }}
        public string EMGHighAlarmString { get { return emg.HighAlarmString; } }


        // EEG-Parameter
        public double EEGAmplitude { set { eeg.Amplitude = value; } get { return eeg.Amplitude; } }
        public double EEGFrequency { set { eeg.Frequency = value; } get { return eeg.Frequency; } }
        public int EEGHarmonics { set { eeg.Harmonics = value; } get { return eeg.Harmonics; } }
        public double EEGHighAlarm { get { return eeg.HighAlarm; } set { eeg.HighAlarm = value; } }
        public double EEGLowAlarm { get { return eeg.LowAlarm; } set { eeg.LowAlarm = value; } }
        public string EEGLowAlarmString { get { return eeg.LowAlarmString; } }
        public string EEGHighAlarmString { get { return eeg.HighAlarmString; } }


        // Resp-Parameter
        public double RespAmplitude { set { resp.Amplitude = value; } get { return resp.Amplitude; } }
        public double RespFrequency { set { resp.Frequency = value; } get { return resp.Frequency; } }
        public int RespHarmonics { set { resp.Harmonics = value; } get { return resp.Harmonics; } }
        public double RespHighAlarm { get { return resp.HighAlarm; } set { resp.HighAlarm = value; } }
        public double RespLowAlarm { get { return resp.LowAlarm; } set { resp.LowAlarm = value; } }
        public string RespiLowAlarmString { get { return resp.LowAlarmString; } }
        public string RespiHighAlarmString { get { return resp.HighAlarmString; } }
        // Eigenschaft zum Abrufen der Liste von Samples
        public List<double> SampleList
        {
            get { return sampleList; }
        }
        // Eigenschaft für die Klinikzugehörigkeit
        public MonitorConstants.clinic Clinic
        {
            get { return clinic; }
            set { clinic = value; }
        }



        // Methode zur Ermittlung des nächsten Samples basierend auf dem angegebenen Parameter
        public double NextSample(double timeIndex, MonitorConstants.Parameter parameter)
        {
            double nextSample = 0.0;
            //nextSample = this.ecg.NextSample(timeIndex)
            // Auswahl des Parameters, für den das nächste Sample berechnet werden soll
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
            // Entferne das älteste Sample, wenn die maximale Anzahl erreicht wurde
            if (sampleList.Count >= maxSamples)
            {
                sampleList.RemoveAt(0); // Remove the oldest sample
            }
            sampleList.Add(nextSample); // Füge das neue Sample zur Liste hinzu

            return (nextSample);
        }


    }
}
