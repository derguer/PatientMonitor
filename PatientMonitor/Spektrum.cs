/// <summary>
/// Die Klasse 'Spektrum' implementiert die Fast Fourier Transformation (FFT)
/// zur Frequenzanalyse von diskreten Signalen. Sie bietet Methoden zur 
/// Umwandlung von Zeitbereichsdaten in den Frequenzbereich sowie zur 
/// Verwaltung der Abtastrate.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMonitor
{
    /// <summary>
    /// Die Klasse 'Spektrum' dient der Berechnung der Fast Fourier Transformation (FFT).
    /// </summary>
    public class Spektrum
    {
        public int samples = 128; // Anzahl der Samples (Abtastpunkte)
        /// <summary>
        /// Struktur zur Darstellung komplexer Zahlen.
        /// </summary>
        public struct komplex
        {
            public double re;// Realteil
            public double im;// Imaginärteil
        }
        public komplex[] data; // Array zur Speicherung der komplexen Daten
        /// <summary>
        /// Konstruktor, der die Dimension der FFT und das Datenarray initialisiert.
        /// </summary>
        /// <param name="dimension">Anzahl der Abtastpunkte.</param>
        public Spektrum(int dimension)
        {
            samples = dimension;
            data = new komplex[samples];
        }
        /// <summary>
        /// Berechnet die FFT für ein gegebenes Signal und gibt das Spektrum zurück.
        /// </summary>
        /// <param name="daten">Das Eingabesignal im Zeitbereich.</param>
        /// <param name="nSamples">Anzahl der Abtastpunkte.</param>
        /// <returns>Das berechnete Spektrum im Frequenzbereich.</returns>
        public double[] FFT(double[] daten, int nSamples)
        {
            data = new komplex[nSamples];

            for (int i = 0; i < nSamples; i++)
            {
                data[i].re = daten[i];
                data[i].im = 0.0;
            }

            FFT();

            double[] OutPut = new double[nSamples / 2];

            for (int i = 0; i < nSamples / 2; i++) // Initialisiere das komplexe Array mit den Eingabewerten
            {
                OutPut[i] = Math.Sqrt(data[i].re * data[i].re + data[i].im * data[i].im);
            }
            return OutPut;
        }

        /// <summary>
        /// Methode FFT aus dem Buch :
        /// 
        /// "Praktische Informationstechnik mit C#", Oliver Kluge
        /// Springer Verlag
        /// 
        /// </summary>
        public void FFT()
        {
            double tempr = 0; // für Tausch bei Bit-Umkehr
            double tempi = 0; // für Tausch bei Bit-Umkehr
            double wreal = 0; // Drehfaktor (Realteil)
            double wimag = 0; // Drehfaktor (Imaginärteil)
            double real1 = 0; // Hilfsvariable
            double imag1 = 0; // Hilfsvariable
            double real2 = 0; // Hilfsvariable
            double imag2 = 0; // Hilfsvariable
            int i = 0;
            int j = 0;
            int k = 0;
            int stufen = 0;
            int sprung = 0;
            int schritt = 0;
            int element = 0;
            // Bit-Umkehr
            for (j = 0; j < samples - 1; j++)
            {
                if (j < i)
                {
                    tempr = data[j].re;
                    tempi = data[j].im;
                    data[j].re = data[i].re;
                    data[j].im = data[i].im;
                    data[i].re = tempr;
                    data[i].im = tempi;
                }
                k = samples / 2;
                while (k <= i)
                {
                    i -= k;
                    k /= 2;
                }
                i += k;
            }
            // Iterationsstufen berechnen
            stufen = (int)(Math.Log10((double)samples) /
            Math.Log10((double)2));
            sprung = 2;
            for (i = 0; i < stufen; i++)
            {
                // jede Iterationsstufe startet mit dem 1. Wert
                element = 0;
                for (j = samples / sprung; j >= 1; j--)
                {
                    schritt = sprung / 2;
                    for (k = 0; k < schritt; k++)
                    {
                        // 1. Zweig des Butterfly
                        wreal = Math.Cos(k * 2.0 * Math.PI / sprung);
                        wimag = Math.Sin(k * 2.0 * Math.PI / sprung);
                        real1 = data[element + k].re + (wreal * data[element + k + schritt].re - wimag * data[element + k + schritt].im);
                        imag1 = data[element + k].im + (wreal * data[element + k + schritt].im + wimag * data[element + k + schritt].re);
                        // 2. Zweig des Butterfly
                        wreal *= -1.0;
                        wimag *= -1.0;
                        real2 = data[element + k].re + (wreal * data[element + k + schritt].re - wimag * data[element + k + schritt].im);
                        imag2 = data[element + k].im + (wreal * data[element + k + schritt].im + wimag * data[element + k + schritt].re);
                        // Ergebnisse übernehemen
                        data[element + k].re = real1;
                        data[element + k].im = imag1;
                        data[element + k + schritt].re = real2;
                        data[element + k + schritt].im = imag2;
                    }
                    element += sprung;
                }
                sprung *= 2;
            }
        }
        /// <summary>
        /// Eigenschaft zur Verwaltung der Abtastrate.
        /// </summary>
        public int Abtastrate
        {
            get
            {
                return samples;
            }
            set
            {
                samples = value;
            }
        }
    }
}
