/// <summary>
/// Die Klasse 'Database' verwaltet eine Liste von Patienten mithilfe einer ObservableCollection. 
/// Sie bietet Funktionen zum Hinzufügen, Entfernen, Abrufen, Speichern und Laden von Patienten.
/// Beim Speichern werden alle wichtigen Patientendaten, einschließlich physiologischer Parameter 
/// und Patiententypen (stationär oder ambulant), in einer Binärdatei gespeichert. 
/// Beim Laden werden diese Daten rekonstruiert und in die Datenbank eingetragen. 
/// Es wird sichergestellt, dass die maximale Anzahl von 100 aktiven Patienten nicht überschritten wird.
/// </summary>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace PatientMonitor
{
    /// <summary>
    /// Die Klasse 'Database' verwaltet eine Sammlung von Patienten 
    /// und bietet Methoden zum Hinzufügen, Entfernen, Speichern und Laden der Patienten.
    /// </summary>
    class Database
    {
        // Maximale Anzahl aktiver Patienten, die gespeichert werden können
        const int maxActivePatients = 100;

        // Liste zur Speicherung der Patienten
        //private List<Patient> data = new List<Patient>();
        // ObservableCollection zur Speicherung der Patienten, um Änderungen automatisch im UI anzuzeigen

        private ObservableCollection<Patient> data = new ObservableCollection<Patient>();
        /// <summary>
        /// Öffentliche Eigenschaft zum Zugriff auf die Patientenliste.
        /// </summary>
        public ObservableCollection<Patient> Data
        {
            get { return data; }
        }

        /// <summary>
        /// Fügt einen neuen Patienten zur Datenbank hinzu, sofern das Maximum nicht erreicht ist.
        /// </summary>
        /// <param name="patient">Der hinzuzufügende Patient.</param>
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
        /// <summary>
        /// Gibt eine Liste aller Patienten zurück.
        /// </summary>
        /// <returns>Liste der Patienten.</returns>
        // Methode zum Abrufen aller Patienten
        public List<Patient> GetPatients()
        {

            return data.ToList();
        }
        /// <summary>
        /// Gibt die aktuelle Anzahl der Patienten in der Datenbank zurück.
        /// </summary>
        /// <returns>Anzahl der Patienten.</returns>
        // Optionale Methode: Patientenanzahl abrufen
        public int PatientCount()
        {
            return data.Count;
        }
        /// <summary>
        /// Entfernt einen Patienten aus der Datenbank.
        /// </summary>
        /// <param name="patient">Der zu entfernende Patient.</param>
        /// <exception cref="ArgumentException">Wird ausgelöst, wenn der Patient nicht in der Datenbank vorhanden ist.</exception>

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
        /// <summary>
        /// Speichert die Patientenliste in einer Datei.
        /// </summary>
        /// <param name="dataPath">Der Pfad zur Datei, in der die Daten gespeichert werden.</param>

        public void SaveData(string dataPath)
        {
            int patientCount = data.Count;

            using (Stream ausgabe = File.Create(dataPath))
            {
                BinaryWriter writer = new BinaryWriter(ausgabe);
                writer.Write(patientCount); // Anzahl der Patienten schreiben
                foreach (Patient patient in data)
                {
                    if (patient is Stationary)
                        writer.Write(true); // Schreibe den Patiententyp (stationär/ambulant)
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
                    if (patient is Stationary)  // Falls der Patient stationär ist, Raumnummer speichern
                    {
                        Stationary stationary = patient as Stationary;
                        writer.Write(stationary.RoomNumber);
                    }
                }
            }
        }
        /// <summary>
        /// Lädt die Patientenliste aus einer Datei und aktualisiert die Datenbank.
        /// </summary>
        /// <param name="dataPath">Der Pfad zur Datei, aus der die Daten geladen werden.</param>

        public void OpenData(string dataPath)
        {
            using (Stream eingabe = File.OpenRead(dataPath))
            {
                BinaryReader reader = new BinaryReader(eingabe);
                data.Clear(); // Bestehende Daten löschen
                int patientCount = reader.ReadInt32();  // Anzahl der Patienten lesen

                for (int i = 0; i < patientCount; i++)
                {
                    // Lese Patiententyp (stationär/ambulant)
                    bool isStationary = reader.ReadBoolean();
                    string patientName = reader.ReadString();
                    int age = reader.ReadInt32();
                    DateTime dateOfStudy = DateTime.Parse(reader.ReadString());
                    MonitorConstants.clinic clinic = (MonitorConstants.clinic)reader.ReadInt32();
                    // Lese die physiologischen Parameter
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
                    // Erstelle den Patient anhand des Typs
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
                    // Patient zur Datenbank hinzufügen
                    data.Add(patient);
                }
                // Debugging: Anzahl der geladenen Patienten
                System.Diagnostics.Debug.WriteLine($"Loaded {patientCount} patients.");
            }
        }
    }
}
