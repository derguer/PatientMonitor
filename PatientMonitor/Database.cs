using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    class Database
    {
        const int maxActivePatients = 100;
        public List<Patient> data = new List<Patient>();
    }
}
