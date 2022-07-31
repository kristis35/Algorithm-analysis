using System;
using System.Drawing;
using System.Windows;

namespace bandom
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            DrawTriangle(false, 5, 20, 50, 40);
        }
        0p;;

        private Bitmap MyImage;
        public void ShowMyImage(String fileToDisplay, int xSize, int ySize)
        {
            // Sets up an image object to be displayed.
            if (MyImage != null)
            {
                MyImage.Dispose();
            }

            // Stretches the image to fit the pictureBox.
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            MyImage = new Bitmap(fileToDisplay);
            pictureBox1.ClientSize = new Size(xSize, ySize);
            pictureBox1.Image = (Image)MyImage;
        }
    }
}
