using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    /// <summary>
    /// Die Klasse 'ECG' repräsentiert einen physiologischen Parameter zur Simulation von EKG-Signalen.
    /// Sie erbt von der abstrakten Klasse 'PhysioParameter' und implementiert das Interface 'IPhysioFunctions'.
    /// Die Klasse bietet Methoden zur Berechnung der nächsten Signalprobe basierend auf Amplitude, Frequenz 
    /// und Harmonischen.
    /// </summary>
    class ECG : PhysioParameter, IPhysioFunctions
    {
        /// <summary>
        /// Standardkonstruktor, der den Basis-Konstruktor von 'PhysioParameter' aufruft.
        /// </summary>
        public ECG() : base() { }
        /// <summary>
        /// Konstruktor zur Initialisierung eines ECG-Objekts mit den angegebenen Parametern.
        /// </summary>
        /// /// <param name="amplitude">Die Amplitude des EKG-Signals.</param>
        /// <param name="frequency">Die Frequenz des EKG-Signals.</param>
        /// <param name="harmonics">Die Anzahl der zu berücksichtigenden Harmonischen.</param>

        public ECG(double amplitude, double frequency, int harmonics) : base(amplitude, frequency, harmonics) { }

        /// <summary>
        /// Berechnet die nächste Probe des EKG-Signals basierend auf Zeitindex, Frequenz, Amplitude 
        /// und der Anzahl der Harmonischen.
        /// </summary>
        /// /// <param name="timeIndex">Der Zeitindex, zu dem die Probe berechnet werden soll.</param>
        /// <returns>Die berechnete Signalprobe als Double-Wert.</returns>

        public override double NextSample(double timeIndex)
        {
            const double HzToBeatsPerMin = 6000.0; // Konstante zur Umrechnung von Hertz in Schläge pro Minute

            double sample;

            //sample = Math.Cos(2 * Math.PI * (frequency / HzToBeatsPerMin) * timeIndex);
            //sample *= amplitude;
            // Berechnung der EKG-Probe ohne Harmonische (nur Grundfrequenz)
            if (Harmonics == 0)
            {
                sample = Math.Cos(2 * Math.PI * (Frequency / HzToBeatsPerMin) * timeIndex);
                sample *= Amplitude;

                return(sample);
            }else if(Harmonics == 1) // Berechnung der EKG-Probe mit einer Harmonischen
            {
                sample =  Amplitude * Math.Cos(2 * Math.PI * (Frequency / HzToBeatsPerMin) * timeIndex);
                sample += Amplitude/2 * Math.Cos(2 * Math.PI * (2*Frequency / HzToBeatsPerMin) * timeIndex);
                return (sample);
            } else if(Harmonics == 2) // Berechnung der EKG-Probe mit zwei Harmonischen
            {
                sample =   Amplitude * Math.Cos(2 * Math.PI * (Frequency / HzToBeatsPerMin) * timeIndex);
                sample +=  Amplitude/2 * Math.Cos(2 * Math.PI * (2 * Frequency / HzToBeatsPerMin) * timeIndex);
                sample +=  Amplitude/3 * Math.Cos(2 * Math.PI * (3 * Frequency / HzToBeatsPerMin) * timeIndex);
                return (sample);
            }
            else // Standardfall: Berechnung der EKG-Probe ohne Harmonische
            {
                sample = Math.Cos(2 * Math.PI * (Frequency / HzToBeatsPerMin) * timeIndex);
                sample *= Amplitude;
                return (sample);
            }
        }
        /// <summary>
        /// Überschreibt die LowAlarmString-Eigenschaft der Basisklasse.
        /// </summary>
        public new string LowAlarmString => base.LowAlarmString;
        /// <summary>
        /// Überschreibt die HighAlarmString-Eigenschaft der Basisklasse.
        /// </summary>
        public new string HighAlarmString => base.HighAlarmString;
        /// <summary>
        /// Überschreibt die LowAlarm-Eigenschaft der Basisklasse.
        /// </summary>
        public new double LowAlarm
        {
            get => base.LowAlarm;
            set => base.LowAlarm = value;
        }
        /// <summary>
        /// Überschreibt die HighAlarm-Eigenschaft der Basisklasse.
        /// </summary>
        public new double HighAlarm
        {
            get => base.HighAlarm;
            set => base.HighAlarm = value;
        }
       

    }
}

