using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace PatientMonitor
{
    class Database
    {
        // Maximale Anzahl aktiver Patienten
        const int maxActivePatients = 100;

        // Liste zur Speicherung der Patienten
        //private List<Patient> data = new List<Patient>();
        private ObservableCollection<Patient> data = new ObservableCollection<Patient>();

        public ObservableCollection<Patient> Data
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

            return data.ToList();
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
            int patientCount = data.Count;

            using (Stream ausgabe = File.Create(dataPath))
            {
                BinaryWriter writer = new BinaryWriter(ausgabe);
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
                        Stationary stationary = patient as Stationary;
                        writer.Write(stationary.RoomNumber);
                    }
                }
            }
        }
        public void OpenData(string dataPath)
        {
            using (Stream eingabe = File.OpenRead(dataPath))
            {
                BinaryReader reader = new BinaryReader(eingabe);
                data.Clear(); // ObservableCollection wird geleert
                int patientCount = reader.ReadInt32();

                for (int i = 0; i < patientCount; i++)
                {
                    bool isStationary = reader.ReadBoolean();
                    string patientName = reader.ReadString();
                    int age = reader.ReadInt32();
                    DateTime dateOfStudy = DateTime.Parse(reader.ReadString());
                    MonitorConstants.clinic clinic = (MonitorConstants.clinic)reader.ReadInt32();

                    double ecgAmplitude = reader.ReadDouble();
                    double ecgFrequency = reader.ReadDouble();
                    double ecgHighAlarm = reader.ReadDouble();
                    double ecgLowAlarm = reader.ReadDouble();
                    int ecgHarmonics = reader.ReadInt32();

                    double eegAmplitude = reader.ReadDouble();
                    double eegFrequency = reader.ReadDouble();
                    double eegHighAlarm = reader.ReadDouble();
                    double eegLowAlarm = reader.ReadDouble();
                    int eegHarmonics = reader.ReadInt32();

                    double emgAmplitude = reader.ReadDouble();
                    double emgFrequency = reader.ReadDouble();
                    double emgHighAlarm = reader.ReadDouble();
                    double emgLowAlarm = reader.ReadDouble();
                    int emgHarmonics = reader.ReadInt32();

                    double respAmplitude = reader.ReadDouble();
                    double respFrequency = reader.ReadDouble();
                    double respHighAlarm = reader.ReadDouble();
                    double respLowAlarm = reader.ReadDouble();
                    int respHarmonics = reader.ReadInt32();

                    Patient patient;
                    if (isStationary)
                    {
                        int roomNum = reader.ReadInt32();
                        Stationary stationaryPatient = new Stationary(
                            patientName,
                            age,
                            dateOfStudy,
                            ecgAmplitude,
                            ecgFrequency,
                            ecgHarmonics,
                            clinic,
                            roomNum
                        )
                        {
                            ECGLowAlarm = ecgLowAlarm,
                            ECGHighAlarm = ecgHighAlarm,
                            EEGAmplitude = eegAmplitude,
                            EEGFrequency = eegFrequency,
                            EEGLowAlarm = eegLowAlarm,
                            EEGHighAlarm = eegHighAlarm,
                            EMGAmplitude = emgAmplitude,
                            EMGFrequency = emgFrequency,
                            EMGLowAlarm = emgLowAlarm,
                            EMGHighAlarm = emgHighAlarm,
                            RespAmplitude = respAmplitude,
                            RespFrequency = respFrequency,
                            RespLowAlarm = respLowAlarm,
                            RespHighAlarm = respHighAlarm,
                            Stationary = true,
                            Ambulatory = false,
                            Type = "Stationary"
                        };

                        patient = stationaryPatient;
                    }
                    else
                    {
                        Patient ambPatient = new Patient(
                            patientName,
                            age,
                            dateOfStudy,
                            ecgAmplitude,
                            ecgFrequency,
                            ecgHarmonics,
                            clinic
                        )
                        {
                            ECGLowAlarm = ecgLowAlarm,
                            ECGHighAlarm = ecgHighAlarm,
                            EEGAmplitude = eegAmplitude,
                            EEGFrequency = eegFrequency,
                            EEGLowAlarm = eegLowAlarm,
                            EEGHighAlarm = eegHighAlarm,
                            EMGAmplitude = emgAmplitude,
                            EMGFrequency = emgFrequency,
                            EMGLowAlarm = emgLowAlarm,
                            EMGHighAlarm = emgHighAlarm,
                            RespAmplitude = respAmplitude,
                            RespFrequency = respFrequency,
                            RespLowAlarm = respLowAlarm,
                            RespHighAlarm = respHighAlarm,
                            Ambulatory = true,
                            Stationary = false,
                            Type = "Ambulatory"
                        };
                        patient = ambPatient;
                    }
                    // Patient hinzufügen
                    data.Add(patient);
                }
                // Debugging: Anzahl der geladenen Patienten
                System.Diagnostics.Debug.WriteLine($"Loaded {patientCount} patients.");
            }
        }
    }
}
