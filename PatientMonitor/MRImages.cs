/// <summary>
/// Die Klasse 'MRImages' verwaltet die Anzeige und Navigation von medizinischen Bildern (MR-Bildern).
/// Sie ermöglicht das Laden, Vor- und Zurückblättern sowie das Festlegen der maximalen Anzahl von Bildern.
/// </summary>

using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace PatientMonitor
{
    /// <summary>
    /// Die Klasse 'MRImages' dient zum Verwalten und Anzeigen einer Reihe von MR-Bildern.
    /// </summary>
    public class MRImages
    {
        // Maximale Anzahl von Bildern
        private int maxImages = 0;
        private int currentImageIndex = 0;   // Aktueller Bildindex
        private string stringBase = "";// Basisverzeichnis für die Bilder
        private BitmapImage anImage; // Aktuelles Bild als BitmapImage
        /// <summary>
        /// Gibt das aktuell geladene Bild zurück.
        /// </summary>
        public BitmapImage AnImage
        {
            get { return anImage; }
            private set { anImage = value; }
        }

        /// <summary>
        /// Lädt Bilder basierend auf dem angegebenen Dateipfad.
        /// </summary>
        public void LoadImages(string imageFile)
        {
            if (string.IsNullOrEmpty(imageFile))
            {
                throw new ArgumentException("File path cannot be null or empty.");
            }

            // Basisverzeichnis bestimmen
            stringBase = Path.GetDirectoryName(imageFile) + "\\";

            // Prüfen, ob der Dateiname im richtigen Format ist
            string fileName = Path.GetFileNameWithoutExtension(imageFile);
            if (!fileName.StartsWith("Base", StringComparison.OrdinalIgnoreCase) ||
                !int.TryParse(fileName.Substring(4, 2), out int selectedImageNumber))
            {
                throw new Exception("Invalid file name format. Expected: Base**.bmp");
            }


            // Maximalen Index bestimmen
            maxImages = selectedImageNumber;

            if (maxImages <= 0)
            {
                throw new Exception("No valid images found.");
            }

            // Starte immer mit Base01.bmp
            currentImageIndex = 0;
            LoadCurrentImage();
        }
        /// <summary>
        /// Lädt das aktuelle Bild basierend auf dem aktuellen Bildindex.
        /// </summary>
        private void LoadCurrentImage()
        {
            string currentFileName = Path.Combine(stringBase, $"Base{(currentImageIndex + 1):D2}.bmp");

            if (File.Exists(currentFileName))
            {
                anImage = LoadImage(currentFileName);
            }
            else
            {
                throw new FileNotFoundException($"File not found: {currentFileName}");
            }
        }
        /// <summary>
        /// Lädt ein Bild von einem angegebenen Pfad und gibt es als BitmapImage zurück.
        /// </summary>
        /// <param name="filePath">Der Pfad des Bildes.</param>
        /// <returns>Das geladene Bild als BitmapImage.</returns>
        public BitmapImage LoadImage(string filePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(filePath, UriKind.Absolute);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }
        /// <summary>
        /// Gibt den aktuellen Bildindex und die maximale Anzahl der Bilder als Text zurück.
        /// </summary>
        /// <returns>Ein String im Format "aktuellerIndex/maxImages".</returns>
        public string GetImageIndexText()
        {
            return $"{currentImageIndex + 1}/{maxImages}";
        }
        /// <summary>
        /// Blättert zum nächsten Bild vor. Falls das letzte Bild erreicht ist, wird zum ersten Bild gewechselt.
        /// </summary>
        public void ForwardImage()
        {
            if (currentImageIndex + 1 < maxImages) // Prüfen, ob es ein weiteres Bild gibt
            {
                currentImageIndex++;
                LoadCurrentImage(); // Lade das nächste Bild
            }
            else
            {
                currentImageIndex = 0; // Zurück zum ersten Bild
                LoadCurrentImage();
            }
        }
        /// <summary>
        /// Blättert zum vorherigen Bild zurück. Falls das erste Bild erreicht ist, wird zum letzten Bild gewechselt.
        /// </summary>
        public void BackwardImage()
        {
            if (currentImageIndex > 0) // Prüfen, ob es ein vorheriges Bild gibt
            {
                currentImageIndex--;
                LoadCurrentImage(); // Lade das vorherige Bild
            }
            else
            {
                currentImageIndex = maxImages - 1;
                LoadCurrentImage();
            }
        }
        /// <summary>
        /// Legt die maximale Anzahl der anzuzeigenden Bilder fest und lädt das erste Bild.
        /// </summary>
        /// <param name="newMaxImages">Die neue maximale Anzahl der Bilder.</param>
        public void SetMaxImages(int newMaxImages)
        {
            if (newMaxImages <= 0)
            {
                throw new ArgumentException("Max images must be greater than 0.");
            }

            maxImages = newMaxImages;
            currentImageIndex = 0; // Springe zurück zum ersten Bild
            LoadCurrentImage(); // Lade das erste Bild mit der neuen Begrenzung
        }
        /// <summary>
        /// Aktualisiert die maximale Anzahl der Bilder basierend auf der Benutzereingabe.
        /// </summary>
        /// <param name="input">Die Eingabe des Benutzers als String.</param>
        public void UpdateMaxImagesFromInput(string input)
        {
            // Versuche, die Eingabe in eine Zahl zu konvertieren
            if (int.TryParse(input, out int newMaxImages) && newMaxImages > 0)
            {
                // Setze die neue maximale Anzahl von Bildern
                maxImages = newMaxImages;

                // Springe zum ersten Bild
                currentImageIndex = 0;

                // Lade das erste Bild
                LoadCurrentImage();
            }
            else
            {
                throw new ArgumentException("Invalid input. Please enter a positive integer.");
            }
        }
        /// <summary>
        /// Gibt die maximale Anzahl der geladenen Bilder zurück.
        /// </summary>
        public int MaxImages
        {
            get { return maxImages; }
        }




    }

}
