/// <summary>
/// Die Klasse 'MonitorConstants' enthält Enumerationen, die im Projekt 'PatientMonitor' verwendet werden.
/// Dazu gehören Parameterarten (z. B. ECG, EEG), Kliniktypen und Kriterien für die Patientensortierung.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    /// <summary>
    /// Statische Klasse, die verschiedene Enumerationen bereitstellt, die für das Monitoring und die Verwaltung
    /// von Patienten verwendet werden.
    /// </summary>
    internal static class MonitorConstants
    {
        /// <summary>
        /// Enumeration der verschiedenen Parameterarten, die von den Patienten überwacht werden können.
        /// </summary>
        public enum Parameter
        {
            ECG,// Elektrokardiogramm
            EEG,// Elektroenzephalogramm
            EMG,// Elektromyogramm
            Resp // Atmung
        }
        /// <summary>
        /// Enumeration der Klinikarten, in denen Patienten behandelt werden können.
        /// </summary>
        public enum clinic
        {
            Cardiology = 0, // Kardiologie
            Neurology = 1, // Neurologie
            Orthopedics = 2,// Orthopädie
            Surgery = 3,
            Dermatology = 4,
            Radiology = 5,
            Oftalmology = 6,
            Pediatrics = 7,// Pädiatrie
        }
        /// <summary>
        /// Enumeration der Sortierkriterien für Patienten in der Datenbank.
        /// </summary>
        public enum compareAfter
        {
            Name = 0, // Sortierung nach Name
            Age = 1,
            Clinic = 2,
            Ambulatory = 3,
            Stationary = 4,// Sortierung nach stationären Patienten
        }
    }
}
