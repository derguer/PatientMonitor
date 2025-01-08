/// <summary>
/// Die Klasse 'Resp' repräsentiert ein physiologisches Atemsignal (Respiration).
/// Sie erbt von 'PhysioParameter' und implementiert das Interface 'IPhysioFunctions'.
/// Die Klasse bietet Funktionen zur Berechnung von Samples basierend auf Frequenz,
/// Amplitude und Harmonischen sowie zur Verwaltung von Alarmgrenzen.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    /// <summary>
    /// Die Klasse 'Resp' implementiert die Darstellung eines Atemsignals.
    /// Sie erbt von 'PhysioParameter' und implementiert das Interface 'IPhysioFunctions'.
    /// </summary>
    class Resp : PhysioParameter, IPhysioFunctions
    {
        /// <summary>
        /// Standardkonstruktor, initialisiert das Atemsignal mit Standardwerten.
        /// </summary>
        public Resp() : base() { }
        /// <summary>
        /// Konstruktor, der das Atemsignal mit angegebenen Werten initialisiert.
        /// </summary>
        /// <param name="amplitude">Die Amplitude des Signals.</param>
        /// <param name="frequency">Die Frequenz des Signals.</param>
        /// <param name="harmonics">Die Anzahl der Harmonischen des Signals.</param>
        public Resp(double amplitude, double frequency, int harmonics) : base(amplitude, frequency, harmonics) { }
        /// <summary>
        /// Berechnet das nächste Sample des Atemsignals basierend auf der Zeit.
        /// </summary>
        /// <param name="timeIndex">Der aktuelle Zeitindex in Millisekunden.</param>
        /// <returns>Das berechnete Sample des Atemsignals.</returns>
        public override double NextSample(double timeIndex)
        {

            timeIndex = timeIndex / 6000;// Zeitindex wird auf Sekundenbasis umgerechnet

            double sample = 0.0;
            double signalLength = 1.0 / Frequency; // Dauer eines Signals
            double stepIndex = timeIndex % signalLength; // Zeit innerhalb eines Signalzyklus

            // Normiere auf den Bereich von -1 bis 1
            sample = (2.0 * (stepIndex / signalLength)) - 1.0;

            // Skaliere mit der Amplitude
            sample *= Amplitude;

            return sample;
        }
        /// <summary>
        /// Gibt den aktuellen Low-Alarm-Status als Text zurück.
        /// </summary>
        public new string LowAlarmString => base.LowAlarmString;
        /// <summary>
        /// Gibt den aktuellen High-Alarm-Status als Text zurück.
        /// </summary>
        public new string HighAlarmString => base.HighAlarmString;
        /// <summary>
        /// Legt die Grenze für den Low-Alarm fest oder gibt sie zurück.
        /// </summary>

        public new double LowAlarm
        {
            get => base.LowAlarm;
            set => base.LowAlarm = value;
        }
        /// <summary>
        /// Legt die Grenze für den High-Alarm fest oder gibt sie zurück.
        /// </summary>
        public new double HighAlarm
        {
            get => base.HighAlarm;
            set => base.HighAlarm = value;
        }
    }
}
