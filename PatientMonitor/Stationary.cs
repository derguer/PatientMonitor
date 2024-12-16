using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class Stationary : Patient
    {
        public int RoomNumber { get; set; }

        // Voller Konstruktor mit DateTime
        public Stationary(string patientName, int age, DateTime dateOfStudy, double amplitude, double frequency, int harmonics, MonitorConstants.clinic clinic, int roomNumber)
            : base(patientName, age, dateOfStudy, amplitude, frequency, harmonics, clinic)
        {
            this.RoomNumber = roomNumber;
            this.Room = roomNumber.ToString(); // nutzt die vererbte Property "Room" aus Patient
        }

        // Überladener Konstruktor für weniger Parameter, z.B. ohne Amplitude/Frequency/Harmonics
        public Stationary(string patientName, int age, DateTime dateOfStudy, int roomNumber)
            : base(patientName, age, dateOfStudy)
        {
            this.RoomNumber = roomNumber;
            // Room kann hier ebenfalls gesetzt werden, wenn gewünscht:
            this.Room = roomNumber.ToString();
        }
    }
}
