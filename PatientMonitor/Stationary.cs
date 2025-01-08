/// <summary>
/// Die Klasse 'Stationary' erweitert die Basisklasse 'Patient' und repräsentiert stationäre Patienten.
/// Sie fügt die Eigenschaft 'RoomNumber' hinzu, die die Zimmernummer des stationären Patienten speichert.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    /// <summary>
    /// Repräsentiert einen stationären Patienten, der in einem bestimmten Zimmer untergebracht ist.
    /// </summary>
    class Stationary : Patient
    {
        public int RoomNumber { get; set; }// Eigenschaft zur Speicherung der Zimmernummer

        /// <summary>
        /// Konstruktor zur Initialisierung eines stationären Patienten mit vollständigen Parametern.
        /// </summary>
        /// <param name="patientName">Der Name des Patienten.</param>
        /// <param name="age">Das Alter des Patienten.</param>
        /// <param name="dateOfStudy">Das Datum der Untersuchung.</param>
        /// <param name="amplitude">Die Amplitude des Signals.</param>
        /// <param name="frequency">Die Frequenz des Signals.</param>
        /// <param name="harmonics">Die Anzahl der Harmonischen des Signals.</param>
        /// <param name="clinic">Die Klinik, in der der Patient behandelt wird.</param>
        /// <param name="roomNumber">Die Zimmernummer des Patienten.</param>
        public Stationary(string patientName, int age, DateTime dateOfStudy, double amplitude, double frequency, int harmonics, MonitorConstants.clinic clinic, int roomNumber)
            : base(patientName, age, dateOfStudy, amplitude, frequency, harmonics, clinic)
        {
            this.RoomNumber = roomNumber;
            this.Room = roomNumber.ToString(); // nutzt die vererbte Property "Room" aus Patient
        }
        /// <summary>
        /// Überladener Konstruktor zur Initialisierung eines stationären Patienten mit weniger Parametern.
        /// </summary>
        /// <param name="patientName">Der Name des Patienten.</param>
        /// <param name="age">Das Alter des Patienten.</param>
        /// <param name="dateOfStudy">Das Datum der Untersuchung.</param>
        /// <param name="roomNumber">Die Zimmernummer des Patienten.</param>
        // Überladener Konstruktor für weniger Parameter, z.B. ohne Amplitude/Frequency/Harmonics
        public Stationary(string patientName, int age, DateTime dateOfStudy, int roomNumber)
            : base(patientName, age, dateOfStudy)
        {
            this.RoomNumber = roomNumber; // Setzt die Zimmernummer
            // Room kann hier ebenfalls gesetzt werden, wenn gewünscht:
            this.Room = roomNumber.ToString();  // Setzt die vererbte Eigenschaft 'Room' auf die Zimmernummer
        }
    
    }
}
