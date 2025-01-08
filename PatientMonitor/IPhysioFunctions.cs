/// <summary>
/// Das Interface 'IPhysioFunctions' definiert grundlegende Funktionen und Eigenschaften, 
/// die von physiologischen Parametern wie ECG, EEG, EMG und Resp implementiert werden.
/// Es enthält Methoden zur Berechnung von Mustern (NextSample) sowie zur Anzeige von Alarmen.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    /// <summary>
    /// Das Interface 'IPhysioFunctions' legt fest, welche Funktionen physiologische Parameter implementieren müssen.
    /// </summary>
    interface IPhysioFunctions
    {
        /// <summary>
        /// Eigenschaft zur Verwaltung der Amplitude des Signals.
        /// </summary>
        double Amplitude { get; set; }
        /// <summary>
        /// Eigenschaft zur Verwaltung der Frequenz des Signals.
        /// </summary>
        double Frequency { get; set; }
        /// <summary>
        /// Eigenschaft zur Verwaltung der Anzahl der Harmonischen.
        /// </summary>
        int Harmonics { get; set; }
        /// <summary>
        /// Berechnet das nächste Sample des Signals basierend auf dem übergebenen Zeitindex.
        /// </summary>
        /// <param name="timerIndex">Der Zeitindex, für den das Sample berechnet wird.</param>
        /// <returns>Das berechnete Sample.</returns>
        double NextSample(double timerIndex);
        /// <summary>
        /// Zeigt den Low-Alarm an, wenn die Frequenz unter dem festgelegten unteren Alarmwert liegt.
        /// </summary>
        /// <param name="frequency">Aktuelle Frequenz des Signals.</param>
        /// <param name="alarmLow">Niedriger Alarmwert.</param>
        void displayLowAlarm(double frequency, double alarmLow);
        /// <summary>
        /// Zeigt den High-Alarm an, wenn die Frequenz den festgelegten oberen Alarmwert überschreitet.
        /// </summary>
        /// <param name="frequency">Aktuelle Frequenz des Signals.</param>
        /// <param name="alarmHigh">Hoher Alarmwert.</param>
        void displayHighAlarm(double frequency, double alarmHigh);
    }
}
