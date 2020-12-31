using System;
using System.Drawing;
using System.Windows.Forms;

using ImageMatrix.Source.Common;

namespace ImageMatrix.Source.Core
{
    public class ImageEditor
    {
        private Bitmap image;
        private ProgressBar bar;

        public Matrix redMatrix;
        public Matrix greenMatrix;
        public Matrix blueMatrix;


        public ImageEditor(Bitmap image, ref ProgressBar bar)
        {
            this.image = image;
            this.bar = bar;

            redMatrix = GetRedValues();
            greenMatrix = GetGreenValues();
            blueMatrix = GetBlueValues();
        }

        public Matrix GetRedValues()
        {
            redMatrix = new Matrix(image.Width, image.Height);
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                    redMatrix[x, y] = image.GetPixel(x, y).R;
            }

            return redMatrix;
        }

        public Matrix GetGreenValues()
        {
            greenMatrix = new Matrix(image.Width, image.Height);
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                    greenMatrix[x, y] = image.GetPixel(x, y).G;
            }

            return greenMatrix;
        }

        public Matrix GetBlueValues()
        {
            blueMatrix = new Matrix(image.Width, image.Height);
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                    blueMatrix[x, y] = image.GetPixel(x, y).B;
            }

            return blueMatrix;
        }

        private void ChangeColor(ref Matrix color, Matrix kernel)
        {
            Matrix changedMatrix = new Matrix(color.Width, color.Height);
            int kernelX = 0, kernelY = 0;
            int sum = 0;
            float pixelsChanged = 0;

            int kWidth = kernel.Width;
            int kHeight = kernel.Height;
            int cWidth = color.Width;
            int cHeight = color.Height;

            int colorProgress = 0;

            while (kernelX + kWidth < cWidth)
            {
                while (kernelY + kHeight < cHeight)
                {
                    //Multiply kernel by selected section of color
                    for (int x = 0; x < kWidth; x++)
                    {
                        for (int y = 0; y < kHeight; y++)
                        {
                            int colorX = x + kernelX;
                            int colorY = y + kernelY;
                            sum += kernel[x, y] * color[colorX, colorY];
                        }
                    }

                    if (sum < 0)
                        sum = 0;
                    else if (sum > 255)
                        sum = 255;

                    changedMatrix[kernelX + 1, kernelY + 1] = sum;
                    pixelsChanged++;
                    float percentage = 100 * (pixelsChanged / ((float)cWidth * (float)cHeight));
                    if (percentage >= colorProgress + 1)
                    {
                        colorProgress++;
                        bar.BeginInvoke((MethodInvoker)delegate
                        {
                            bar.Value++;
                            bar.Refresh();
                        });
                    }
                    sum = 0;
                    kernelY++;
                }

                kernelY = 0;
                kernelX++;
            }

            color = changedMatrix;
            bar.BeginInvoke((MethodInvoker)delegate
            {
                bar.Value++;
                bar.Refresh();
            });
        }

        public void ChangeRed(Matrix kernel)
        {
            ChangeColor(ref redMatrix, kernel);
        }

        public void ChangeGreen(Matrix kernel)
        {
            ChangeColor(ref greenMatrix, kernel);
        }

        public void ChangeBlue(Matrix kernel)
        {
            ChangeColor(ref blueMatrix, kernel);
        }

        public Image MatrixColorsToImage()
        {
            Bitmap image = new Bitmap(redMatrix.Width, redMatrix.Height);
            for (int x = 0; x < redMatrix.Width; x++)
            {
                for (int y = 0; y < redMatrix.Height; y++)
                {
                    int r = redMatrix[x, y];
                    int g = greenMatrix[x, y];
                    int b = blueMatrix[x, y];

                    image.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return image;
        }
    }
}
