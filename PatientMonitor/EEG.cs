/// <summary>
/// Die Klasse 'EEG' repräsentiert einen physiologischen Parameter zur Simulation von EEG-Signalen.
/// Sie erbt von 'PhysioParameter' und implementiert das Interface 'IPhysioFunctions'.
/// Die Klasse berechnet das EEG-Signal basierend auf einer exponentiellen Kurvenfunktion.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    /// <summary>
    /// Die Klasse 'EEG' repräsentiert den physiologischen Parameter EEG und 
    /// enthält Methoden zur Simulation eines exponentiellen EEG-Signals.
    /// </summary>
    class EEG : PhysioParameter, IPhysioFunctions
    {
        /// <summary>
        /// Standardkonstruktor, initialisiert das EEG-Objekt mit Standardwerten.
        /// </summary>
        public EEG() : base() { }
        /// <summary>
        /// Überladener Konstruktor, initialisiert das EEG-Objekt mit den angegebenen Werten.
        /// </summary>
        /// <param name="amplitude">Amplitude des EEG-Signals.</param>
        /// <param name="frequency">Frequenz des EEG-Signals.</param>
        /// <param name="harmonics">Anzahl der Harmonischen des EEG-Signals.</param>
        public EEG(double amplitude, double frequency, int harmonics) : base(amplitude, frequency, harmonics) { }
        /// <summary>
        /// Berechnet das nächste Sample des EEG-Signals basierend auf dem Zeitindex.
        /// </summary>
        /// <param name="timeIndex">Der Zeitindex für das Sample.</param>
        /// <returns>Das berechnete Sample des EEG-Signals.</returns>

        public override double NextSample(double timeIndex)
        {
            // Normalisierung des Zeitindex auf die Einheit Sekunden
            timeIndex = timeIndex / 6000;

            double sample = 0.0;
            double signalLength = 1.0 / Frequency; // Berechnung der Signallänge basierend auf der Frequenz
            double halfSignalLength = signalLength / 2;
            double stepIndex = timeIndex % signalLength; // Berechnung des Phasenschritts innerhalb der Periode

            // Konstanten für die exponentielle Funktion
            double alpha = 5.0; // Steilheit der exponentiellen Kurve, anpassbar

            if (stepIndex <= halfSignalLength)
            {
                // Exponentieller Anstieg von -Amplitude bis +Amplitude in der ersten Hälfte der Periode
                sample = -this.Amplitude + (2 * this.Amplitude * (1 - Math.Exp(-alpha * (stepIndex / halfSignalLength))));
            }
            else
            {
                // Exponentieller Abfall von +Amplitude bis -Amplitude in der zweiten Hälfte der Periode
                sample = this.Amplitude - (2 * this.Amplitude * (1 - Math.Exp(-alpha * ((stepIndex - halfSignalLength) / halfSignalLength))));
            }

            return sample;
        }
        /// <summary>
        /// Gibt den aktuellen Low-Alarm-String zurück.
        /// </summary>
        public new string LowAlarmString => base.LowAlarmString;
        /// <summary>
        /// Gibt den aktuellen High-Alarm-String zurück.
        /// </summary>
        public new string HighAlarmString => base.HighAlarmString;
        /// <summary>
        /// Eigenschaft zur Verwaltung des unteren Alarmwerts.
        /// </summary>
        public new double LowAlarm
        {
            get => base.LowAlarm;
            set => base.LowAlarm = value;
        }
        /// <summary>
        /// Eigenschaft zur Verwaltung des oberen Alarmwerts.
        /// </summary>
        public new double HighAlarm
        {
            get => base.HighAlarm;
            set => base.HighAlarm = value;
        }
    }
}
