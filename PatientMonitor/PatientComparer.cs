/// <summary>
/// Diese Klasse implementiert einen Vergleichsmechanismus für Patienten anhand verschiedener Kriterien.
/// Die Kriterien werden über das Enum 'MonitorConstants.compareAfter' festgelegt und können folgende sein:
/// - Name des Patienten
/// - Alter des Patienten
/// - Klinik, in der der Patient behandelt wird
/// - Ob der Patient ambulant oder stationär behandelt wird
///
/// Hauptfunktionen:
/// - Vergleich von zwei Patientenobjekten nach dem ausgewählten Kriterium.
/// - Unterstützung der Sortierung von Patientenlisten anhand der definierten Vergleichskriterien.
/// </summary>

using System;
using System.Collections.Generic;

namespace PatientMonitor
{
    // Klasse zum Vergleichen von Patienten anhand eines bestimmten Kriteriums
    class PatientComparer : IComparer<Patient>
    {
        // Vergleichskriterium (z. B. Name, Alter, Klinik)
        MonitorConstants.compareAfter ca;
        // Eigenschaft zum Festlegen und Abrufen des Vergleichskriteriums
        public MonitorConstants.compareAfter CA
        {
            set { ca = value; }
            get { return ca; }
        }
        // Methode zum Vergleichen von zwei Patienten
        public int Compare(Patient x, Patient y)
        {
            if (x == null && y == null) return 0; // Beide Patienten sind null, daher gleich
            if (x == null) return -1;            // Nur x ist null, daher ist x kleiner
            if (y == null) return 1;            // Nur y ist null, daher ist y kleiner

            int result = 0; // Ergebnis des Vergleichs
            // Vergleich basierend auf dem gesetzten Kriterium
            switch (ca)
            {
                case MonitorConstants.compareAfter.Age:
                    result = x.Age.CompareTo(y.Age); // Vergleiche das Alter der Patienten
                    break;

                case MonitorConstants.compareAfter.Name:
                    result = string.Compare(x.PatientName, y.PatientName, StringComparison.Ordinal);  // Vergleiche die Namen der Patienten
                    break;

                case MonitorConstants.compareAfter.Clinic:
                    result = string.Compare(x.Clinic.ToString(), y.Clinic.ToString(), StringComparison.Ordinal);  // Vergleiche die Klinikzugehörigkeit der Patienten
                    break;

                case MonitorConstants.compareAfter.Ambulatory:
                   
                    result = (x is Stationary).CompareTo(y is Stationary); // Prüfe, ob Patienten ambulant sind, und vergleiche
                    break;

                case MonitorConstants.compareAfter.Stationary:
                    result = (y is Stationary).CompareTo(x is Stationary);  // Prüfe, ob Patienten stationär sind, und vergleiche
                    break;

                default:
                    result = 0;  // Kein Vergleich notwendig
                    break;
            }

            return result; // Rückgabe des Vergleichsergebnisses
        }
    }
}
