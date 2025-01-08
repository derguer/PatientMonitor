using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    /// <summary>
    /// Abstrakte Klasse 'PhysioParameter', die allgemeine Eigenschaften und Methoden 
    /// für physiologische Parameter definiert.
    /// </summary>
    abstract class PhysioParameter
    {
        // Private Felder für Amplitude, Frequenz und Harmonische
        private double amplitude = 0.0; // Amplitude des Signals
        private double frequency = 0.0; // Frequenz des Signals
        private int harmonics = 1; // Anzahl der Harmonischen

        // Private Felder für Alarmgrenzen und Alarmnachrichten
       
        private double lowAlarm = 0; // Untere Alarmgrenze
        private double highAlarm = 0; // Obere Alarmgrenze
        private string lowAlarmString = " ";  // Nachricht für niedrigen Alarm
        private string highAlarmString = " "; // Nachricht für hohen Alarm
        // Öffentliche Eigenschaften zur Steuerung der Parameter
        public double Amplitude { get { return amplitude; } set { amplitude = value; } }
        // Eigenschaft für Frequenz mit Überprüfung der Alarme bei Änderung
        public double Frequency { get { return frequency; } set
            {
                frequency = value;
                CheckAlarms(); // Alarme prüfen, wenn die Frequenz geändert wird
            }
        }

        public int Harmonics { get { return harmonics; } set { harmonics = value; } }
        /// <summary>
        /// Konstruktor zur Initialisierung eines PhysioParameter-Objekts mit den angegebenen Werten.
        /// </summary>
        /// <param name="amplitude">Die Amplitude des Signals.</param>
        /// <param name="frequency">Die Frequenz des Signals.</param>
        /// <param name="harmonics">Die Anzahl der Harmonischen.</param>
        /// 
        public PhysioParameter(double amplitude, double frequency, int harmonics)
        {
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.harmonics = harmonics;
        }
        // Nur lesbare Eigenschaften für die Alarmnachrichten
        public string LowAlarmString
        {
            get { return lowAlarmString; }
        }
        public string HighAlarmString
        {
            get { return highAlarmString; }
        }
        // Eigenschaften für die Alarmgrenzen mit Überprüfung der Alarme bei Änderung
        public double LowAlarm
        {
            get { return lowAlarm; }
            set
            {
                lowAlarm = value;
                CheckAlarms(); // Prüft Alarme, wenn die untere Alarmgrenze geändert wird
            }
        }

        public double HighAlarm
        {
            get { return highAlarm; }
            set
            {
                highAlarm = value;
                CheckAlarms(); // Prüft Alarme, wenn die obere Alarmgrenze geändert wird
            }
        }
        /// <summary>
        /// Prüft die aktuellen Alarmbedingungen basierend auf der Frequenz
        /// und den gesetzten Alarmgrenzen.
        /// </summary>
        private void CheckAlarms()
        {
            if (frequency >= highAlarm)
            {
                highAlarmString = "HIGH ALARM: " + frequency;
                lowAlarmString = " "; // Vorrang für High Alarm
            }
            else if (frequency <= lowAlarm)
            {
                lowAlarmString = "LOW ALARM: " + frequency;
                highAlarmString = " ";
            }
            else
            {
                // Bestehende Alarme beibehalten, falls sie aktuell aktiv sind
                highAlarmString = highAlarmString.Contains("HIGH ALARM") ? highAlarmString : " ";
                lowAlarmString = lowAlarmString.Contains("LOW ALARM") ? lowAlarmString : " ";
            }
        }

        /// <summary>
        /// Standardkonstruktor.
        /// </summary>
        public PhysioParameter() { }
        /// <summary>
        /// Zeigt die Nachricht für den hohen Alarm an, wenn die Frequenz
        /// die obere Alarmgrenze überschreitet.
        /// </summary>
        /// <param name="frequency">Die aktuelle Frequenz.</param>
        /// <param name="alarmHigh">Die obere Alarmgrenze.</param>
        public void displayHighAlarm(double frequency, double alarmHigh)
        {
            if (frequency >= alarmHigh)
            {
                highAlarmString = "HIGH ALARM:" + frequency;
            }
            else
                highAlarmString = " ";
        }
        /// <summary>
        /// Zeigt die Nachricht für den niedrigen Alarm an, wenn die Frequenz
        /// die untere Alarmgrenze unterschreitet.
        /// </summary>
        /// <param name="frequency">Die aktuelle Frequenz.</param>
        /// <param name="alarmLow">Die untere Alarmgrenze.</param>
        public void displayLowAlarm(double frequency, double alarmLow)
        {
            if (frequency <= alarmLow)
            {
                lowAlarmString = "LOW ALARM:" + frequency;
            }
            else
                lowAlarmString = " ";
        }
        /// <summary>
        /// Abstrakte Methode zur Berechnung der nächsten Signalprobe.
        /// Muss von abgeleiteten Klassen implementiert werden.
        /// </summary>
        /// <param name="timeIndex">Der Zeitindex, zu dem die Probe berechnet werden soll.</param>
        /// <returns>Die berechnete Signalprobe als Double-Wert.</returns>
        public abstract double NextSample(double timeIndex);

    }
}