using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PatientMonitor
{
    internal class MRImages
    {
        private Bitmap anImage;
        public MRImages() { }
        public void loadImages(string imageFile)
        {
            anImage = new Bitmap(imageFile);
        }

        public Bitmap AnImage { get {return anImage; } set { anImage = value; } }
    }
}
