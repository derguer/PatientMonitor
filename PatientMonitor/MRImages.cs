using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace PatientMonitor
{
    public class MRImages
    {
        private int maxImages = 0;
        private int currentImageIndex = 0;
        private string stringBase = "";
        private BitmapImage anImage;

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

        public BitmapImage LoadImage(string filePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(filePath, UriKind.Absolute);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public string GetImageIndexText()
        {
            return $"{currentImageIndex + 1}/{maxImages}";
        }
        public void ForwardImage()
        {
            if (currentImageIndex + 1 < maxImages) // Prüfen, ob es ein weiteres Bild gibt
            {
                currentImageIndex++;
                LoadCurrentImage(); // Lade das nächste Bild
            }
            else
            {
                currentImageIndex = 0;
                LoadCurrentImage();
            }
        }

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
        public int MaxImages
        {
            get { return maxImages; }
        }




    }

}
