using System;
using System.Collections.Generic;

namespace PatientMonitor
{
    class Database
    {
        // Maximale Anzahl aktiver Patienten
        const int maxActivePatients = 100;

        // Liste zur Speicherung der Patienten
        public List<Patient> data = new List<Patient>();

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
    }
}
