/// <summary>
/// Die Klasse 'EMG' repräsentiert einen physiologischen Parameter zur Simulation von EMG-Signalen.
/// Sie erbt von der abstrakten Klasse 'PhysioParameter' und implementiert das Interface 'IPhysioFunctions'.
/// Die Klasse generiert ein rechteckförmiges EMG-Signal mit einstellbarer Amplitude und Frequenz.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    /// <summary>
    /// Die Klasse 'EMG' simuliert ein rechteckförmiges EMG-Signal basierend auf der Amplitude und Frequenz.
    /// </summary>
    class EMG : PhysioParameter, IPhysioFunctions
    {
        /// <summary>
        /// Standardkonstruktor, initialisiert das EMG-Objekt mit Standardwerten.
        /// </summary>
        public EMG() : base() { }
        /// <summary>
        /// Überladener Konstruktor, initialisiert das EMG-Objekt mit den angegebenen Parametern.
        /// </summary>
        /// <param name="amplitude">Amplitude des EMG-Signals.</param>
        /// <param name="frequency">Frequenz des EMG-Signals.</param>
        /// <param name="harmonics">Anzahl der Harmonischen des EMG-Signals.</param>
        public EMG(double amplitude, double frequency, int harmonics) : base(amplitude, frequency, harmonics) { }
        /// <summary>
        /// Berechnet das nächste Sample des EMG-Signals basierend auf dem Zeitindex.
        /// Das Signal ist rechteckförmig mit Amplitudenwerten von +1 oder -1.
        /// </summary>
        /// <param name="timeIndex">Der Zeitindex für das Sample.</param>
        /// <returns>Das berechnete Sample des EMG-Signals.</returns>
        public override double NextSample(double timeIndex)
        {
            double sample       = 0.0;
            double stepIndex    = 0.0;
            double signalLength = 1.0;

            timeIndex=timeIndex/6000;// Zeitindex normalisieren

            signalLength = (double)(1.0 /Frequency);// Berechnung der Signallänge basierend auf der Frequenz
            stepIndex = (double)(timeIndex % signalLength); // Schritt innerhalb der Signallänge berechnen
            if (stepIndex > (signalLength / 2.0)) // Rechtecksignal generieren: +1 in der zweiten Hälfte, -1 in der ersten Hälfte

            {
                sample = 1;
                Console.Write("sample=1");
            }
            else
            {
                sample = -1;
                Console.Write("sample=-1");
            }
            sample *= Amplitude; // Amplitude anwenden
            return (sample);
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
