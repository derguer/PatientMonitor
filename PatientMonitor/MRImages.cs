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
        
        int maxImages = 10; //this number can be changed
        private int currentImageIndex = 0; string stringBase = "";
        string stringImage = "";

        private Bitmap anImage;
        public MRImages() { }
        public void loadImages(string imageFile)
        {
            anImage = new Bitmap(imageFile);
        }

        public Bitmap AnImage { get {return anImage; } set { anImage = value; } }
        
        // New Methods in the Class for Delivery 7
        public void forwardImages()
        {
            // called by the “next” button, you set the image to visualize one index further
        }
        public void backImages()
        {
            // called by the “prev” button, you set the image to visualize one index behind
        }
    }
}
