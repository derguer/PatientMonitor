using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
     class Stationary : Patient 
    {
        private int roomNumber;

        // Property for roomNumber
        public int RoomNumber
        {
            get { return roomNumber; }
            set { roomNumber = value; }
        }

        // Constructor
        // Constructor: Extends the Patient constructor
        public Stationary(string patientName, int age, DateTime dateOfStudy, double amplitude, double frequency, int harmonics, MonitorConstants.clinic clinic, int roomNumber)
            : base(patientName, age, dateOfStudy, amplitude, frequency, harmonics, clinic) // Call base class constructor
        {
            this.roomNumber = roomNumber; // Initialize additional field
        }

        // Overloaded constructor for cases with fewer parameters
        public Stationary(string patientName, int age, DateTime dateOfStudy, int roomNumber)
            : base(patientName, age, dateOfStudy) // Call base class constructor
        {
            this.roomNumber = roomNumber; // Initialize additional field
        }

    }
}
