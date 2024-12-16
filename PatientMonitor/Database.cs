using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class Database
    {
        // Maximale Anzahl aktiver Patienten
        const int maxActivePatients = 100;

        // Liste zur Speicherung der Patienten
        private List<Patient> data = new List<Patient>();

        public List<Patient> Data
        {
            get { return data; }
        }

        // Methode zum Hinzufügen eines Patienten
        public void AddPatient(Patient patient)
        {
            if (data.Count < maxActivePatients)
            {
                data.Add(patient);
            }
            else
            {
                throw new InvalidOperationException("The database has reached its maximum capacity of active patients.");
            }
        }

        // Methode zum Abrufen aller Patienten
        public List<Patient> GetPatients()
        {
            return new List<Patient>(data); // Gibt eine Kopie der Liste zurück
        }

        // Optionale Methode: Patientenanzahl abrufen
        public int PatientCount()
        {
            return data.Count;
        }

        // Optionale Methode: Entfernen eines Patienten
        public void RemovePatient(Patient patient)
        {
            if (data.Contains(patient))
            {
                data.Remove(patient);
            }
            else
            {
                throw new ArgumentException("The specified patient does not exist in the database.");
            }
        }
        public void SaveData(string dataPath)
        {
            int patientCount = 0;

            using (Stream ausgabe = File.Create(dataPath))
            {
                BinaryWriter writer = new BinaryWriter(ausgabe);
                patientCount = data.Count;
                writer.Write(patientCount);
                foreach (Patient patient in data)
                {
                    if (patient is Stationary)
                        writer.Write(true);
                    else
                        writer.Write(false);
                    writer.Write(patient.PatientName);
                    writer.Write(patient.Age);
                    writer.Write(patient.DateOfStudy.ToString());
                    writer.Write((int)patient.Clinic);
                    writer.Write(patient.ECGAmplitude);
                    writer.Write(patient.ECGFrequency);
                    writer.Write(patient.ECGHighAlarm);
                    writer.Write(patient.ECGLowAlarm);
                    writer.Write(patient.ECGHarmonics);
                    writer.Write(patient.EEGAmplitude);
                    writer.Write(patient.EEGFrequency);
                    writer.Write(patient.EEGHighAlarm);
                    writer.Write(patient.EEGLowAlarm);
                    writer.Write(patient.EEGHarmonics);
                    writer.Write(patient.EMGAmplitude);
                    writer.Write(patient.EMGFrequency);
                    writer.Write(patient.EMGHighAlarm);
                    writer.Write(patient.EMGLowAlarm);
                    writer.Write(patient.EMGHarmonics);
                    writer.Write(patient.RespAmplitude);
                    writer.Write(patient.RespFrequency);
                    writer.Write(patient.RespHighAlarm);
                    writer.Write(patient.RespLowAlarm);
                    writer.Write(patient.RespHarmonics);
                    if (patient is Stationary)
                    {
                        Stationary stationary;
                        stationary = patient as Stationary;
                        writer.Write(stationary.RoomNumber);
                    }
                }
            }
        }
        public void OpenData(string dataPath)
        {
            BinaryReader reader;
            int patientCount = 0;
            // Deklaration vorab, damit wir sie nach dem if/else verwenden können
            Patient patient;

            string patientName = "";
            int age = 0;
            string dateOfStudyString;  // erst als String
            MonitorConstants.clinic clinic;
            double ecgAmplitude;
            double ecgFrequency;
            double ecgHighAlarm;
            double ecgLowAlarm;
            double eegAmplitude;
            double eegFrequency;
            double eegHighAlarm;
            double eegLowAlarm;
            double emgAmplitude;
            double emgFrequency;
            double emgHighAlarm;
            double emgLowAlarm;
            double respAmplitude;
            double respFrequency;
            double respHighAlarm;
            double respLowAlarm;
            int ecgHarmonics;
            int eegHarmonics;
            int emgHarmonics;
            int respHarmonics;
            bool isStationary = false;
            int roomNum;

            using (Stream eingabe = File.OpenRead(dataPath))
            {
                reader = new BinaryReader(eingabe);
                data.Clear();
                patientCount = reader.ReadInt32();
                for (int i = 0; i < patientCount; i++)
                {
                    isStationary = reader.ReadBoolean();
                    patientName = reader.ReadString();
                    age = reader.ReadInt32();

                    // DateTime parsen
                    dateOfStudyString = reader.ReadString();
                    DateTime dateOfStudy = DateTime.Parse(dateOfStudyString);

                    clinic = (MonitorConstants.clinic)reader.ReadInt32();
                    ecgAmplitude = reader.ReadDouble();
                    ecgFrequency = reader.ReadDouble();
                    ecgHighAlarm = reader.ReadDouble();
                    ecgLowAlarm = reader.ReadDouble();
                    ecgHarmonics = reader.ReadInt32();

                    eegAmplitude = reader.ReadDouble();
                    eegFrequency = reader.ReadDouble();
                    eegHighAlarm = reader.ReadDouble();
                    eegLowAlarm = reader.ReadDouble();
                    eegHarmonics = reader.ReadInt32();

                    emgAmplitude = reader.ReadDouble();
                    emgFrequency = reader.ReadDouble();
                    emgHighAlarm = reader.ReadDouble();
                    emgLowAlarm = reader.ReadDouble();
                    emgHarmonics = reader.ReadInt32();

                    respAmplitude = reader.ReadDouble();
                    respFrequency = reader.ReadDouble();
                    respHighAlarm = reader.ReadDouble();
                    respLowAlarm = reader.ReadDouble();
                    respHarmonics = reader.ReadInt32();

                    if (isStationary)
                    {
                        roomNum = reader.ReadInt32();

                        // Stationary-Objekt erstellen
                        Stationary stationaryPatient = new Stationary(
                            patientName,
                            age,
                            dateOfStudy,   
                            ecgAmplitude,
                            ecgFrequency,
                            ecgHarmonics,
                            clinic,
                            roomNum
                        );

                        // Eigenschaften setzen
                        stationaryPatient.ECGLowAlarm = ecgLowAlarm;
                        stationaryPatient.ECGHighAlarm = ecgHighAlarm;
                        stationaryPatient.EEGAmplitude = eegAmplitude;
                        stationaryPatient.EEGFrequency = eegFrequency;
                        stationaryPatient.EEGLowAlarm = eegLowAlarm;
                        stationaryPatient.EEGHighAlarm = eegHighAlarm;
                        stationaryPatient.EMGAmplitude = emgAmplitude;
                        stationaryPatient.EMGFrequency = emgFrequency;
                        stationaryPatient.EMGLowAlarm = emgLowAlarm;
                        stationaryPatient.EMGHighAlarm = emgHighAlarm;
                        stationaryPatient.RespAmplitude = respAmplitude;
                        stationaryPatient.RespFrequency = respFrequency;
                        stationaryPatient.RespLowAlarm = respLowAlarm;
                        stationaryPatient.RespHighAlarm = respHighAlarm;
                        stationaryPatient.Stationary = true;
                        stationaryPatient.Ambulatory = false;
                        stationaryPatient.Type = "Stationary";

                        // Zu patient zuweisen
                        patient = stationaryPatient;
                    }
                    else
                    {
                        // Ambulanter Patient
                        Patient ambPatient = new Patient(
                            patientName,
                            age,
                            dateOfStudy,    
                            ecgAmplitude,
                            ecgFrequency,
                            ecgHarmonics,
                            clinic
                        );

                        // Eigenschaften setzen
                        ambPatient.ECGLowAlarm = ecgLowAlarm;
                        ambPatient.ECGHighAlarm = ecgHighAlarm;
                        ambPatient.EEGAmplitude = eegAmplitude;
                        ambPatient.EEGFrequency = eegFrequency;
                        ambPatient.EEGLowAlarm = eegLowAlarm;
                        ambPatient.EEGHighAlarm = eegHighAlarm;
                        ambPatient.EMGAmplitude = emgAmplitude;
                        ambPatient.EMGFrequency = emgFrequency;
                        ambPatient.EMGLowAlarm = emgLowAlarm;
                        ambPatient.EMGHighAlarm = emgHighAlarm;
                        ambPatient.RespAmplitude = respAmplitude;
                        ambPatient.RespFrequency = respFrequency;
                        ambPatient.RespLowAlarm = respLowAlarm;
                        ambPatient.RespHighAlarm = respHighAlarm;
                        ambPatient.Ambulatory = true;
                        ambPatient.Stationary = false;
                        ambPatient.Type = "Ambulatory";

                        // Zu patient zuweisen
                        patient = ambPatient;
                    }

                    // Objekt zur Datenbank hinzufügen
                    data.Add(patient);
                }
            }
        }
    }
}
