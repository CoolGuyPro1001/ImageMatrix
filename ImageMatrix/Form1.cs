using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Threading;

using ImageMatrix.Source.Common;
using ImageMatrix.Source.Core;

namespace ImageMatrix
{
    public partial class ImageMatrix : Form
    {
        private static Matrix redKernel; 
        private static Matrix greenKernel;
        private static Matrix blueKernel;

        private static ImageEditor editor;

        private Image originalImage;

        public ImageMatrix()
        {
            InitializeComponent();

            redKernel = new Matrix(3, 3);
            greenKernel = new Matrix(3, 3);
            blueKernel = new Matrix(3, 3);
        }

        private static void ChangeRedColor()
        {
            editor.ChangeRed(redKernel);
        }

        private static void ChangeGreenColor()
        {
            editor.ChangeGreen(greenKernel);
        }

        private static void ChangeBlueColor()
        {
            editor.ChangeBlue(blueKernel);
        }

        private void image_Click(object sender, EventArgs e)
        {

            //Select image file path
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            string imagePath = dialog.FileName;

            //Set Image from file path
            originalImage = Image.FromFile(imagePath);
            image.Image = originalImage;

            editor = new ImageEditor((Bitmap)image.Image, ref Progress);
        }

        private void MatrixButton_Click(object sender, EventArgs e)
        {
            //Setup Kernels
            redKernel.PrintAllContents();
            greenKernel.PrintAllContents();
            blueKernel.PrintAllContents();
            Progress.Value = 0;

            //ChangeImage
            Thread threadR = new Thread(() => ChangeRedColor());
            Thread threadG = new Thread(() => ChangeGreenColor());
            Thread threadB = new Thread(() => ChangeBlueColor());

            Console.WriteLine("Changing Image");

            threadR.Start();
            threadG.Start();
            threadB.Start();

            threadR.Join();
            Console.WriteLine("Red Complete");
            threadG.Join();
            Console.WriteLine("Green Complete");
            threadB.Join();
            Console.WriteLine("Blue Complete");
            Console.WriteLine(Progress.Value);


            image.Image = editor.MatrixColorsToImage();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure You Want To Save This Image?\n",
                "This will Not Replace The Original Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                image.Image.Save("MatrixedImage.png");
            }
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown spin_box = (NumericUpDown)sender;
            if (spin_box.Name == "RedValue00") redKernel[0, 0] = (int) spin_box.Value;
            if (spin_box.Name == "RedValue10") redKernel[1, 0] = (int)spin_box.Value;
            if (spin_box.Name == "RedValue20") redKernel[2, 0] = (int)spin_box.Value;
            if (spin_box.Name == "RedValue01") redKernel[0, 1] = (int)spin_box.Value;
            if (spin_box.Name == "RedValue11") redKernel[1, 1] = (int)spin_box.Value;
            if (spin_box.Name == "RedValue21") redKernel[2, 1] = (int)spin_box.Value;
            if (spin_box.Name == "RedValue02") redKernel[0, 2] = (int)spin_box.Value;
            if (spin_box.Name == "RedValue12") redKernel[1, 2] = (int)spin_box.Value;
            if (spin_box.Name == "RedValue22") redKernel[2, 2] = (int)spin_box.Value;
            if (spin_box.Name == "GreenValue00") greenKernel[0, 0] = (int)spin_box.Value;
            if (spin_box.Name == "GreenValue10") greenKernel[1, 0] = (int)spin_box.Value;
            if (spin_box.Name == "GreenValue20") greenKernel[2, 0] = (int)spin_box.Value;
            if (spin_box.Name == "GreenValue01") greenKernel[0, 1] = (int)spin_box.Value;
            if (spin_box.Name == "GreenValue11") greenKernel[1, 1] = (int)spin_box.Value;
            if (spin_box.Name == "GreenValue21") greenKernel[2, 1] = (int)spin_box.Value;
            if (spin_box.Name == "GreenValue02") greenKernel[0, 2] = (int)spin_box.Value;
            if (spin_box.Name == "GreenValue12") greenKernel[1, 2] = (int)spin_box.Value;
            if (spin_box.Name == "GreenValue22") greenKernel[2, 2] = (int)spin_box.Value;
            if (spin_box.Name == "BlueValue00") blueKernel[0, 0] = (int)spin_box.Value;
            if (spin_box.Name == "BlueValue10") blueKernel[1, 0] = (int)spin_box.Value;
            if (spin_box.Name == "BlueValue20") blueKernel[2, 0] = (int)spin_box.Value;
            if (spin_box.Name == "BlueValue01") blueKernel[0, 1] = (int)spin_box.Value;
            if (spin_box.Name == "BlueValue11") blueKernel[1, 1] = (int)spin_box.Value;
            if (spin_box.Name == "BlueValue21") blueKernel[2, 1] = (int)spin_box.Value;
            if (spin_box.Name == "BlueValue02") blueKernel[0, 2] = (int)spin_box.Value;
            if (spin_box.Name == "BlueValue12") blueKernel[1, 2] = (int)spin_box.Value;
            if (spin_box.Name == "BlueValue22") blueKernel[2, 2] = (int)spin_box.Value;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            image.Image = originalImage;
        }
    }
}
