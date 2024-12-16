using System;
using System.Collections.Generic;

namespace PatientMonitor
{
    class PatientComparer : IComparer<Patient>
    {
        MonitorConstants.compareAfter ca;
        public MonitorConstants.compareAfter CA
        {
            set { ca = value; }
            get { return ca; }
        }

        public int Compare(Patient x, Patient y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int result = 0;

            switch (ca)
            {
                case MonitorConstants.compareAfter.Age:
                    result = x.Age.CompareTo(y.Age);
                    break;

                case MonitorConstants.compareAfter.Name:
                    result = string.Compare(x.PatientName, y.PatientName, StringComparison.Ordinal);
                    break;

                case MonitorConstants.compareAfter.Clinic:
                    result = string.Compare(x.Clinic.ToString(), y.Clinic.ToString(), StringComparison.Ordinal);
                    break;

                case MonitorConstants.compareAfter.Ambulatory:
                   
                    result = (x is Stationary).CompareTo(y is Stationary);
                    break;

                case MonitorConstants.compareAfter.Stationary:
                    result = (y is Stationary).CompareTo(x is Stationary);
                    break;

                default:
                    result = 0;
                    break;
            }

            return result;
        }
    }
}
