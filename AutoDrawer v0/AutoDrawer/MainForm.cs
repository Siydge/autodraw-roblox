﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace AutoDrawer
{
    public partial class MainForm : Form
    {
        Bitmap imageFile;
        Bitmap image;
        Bitmap processedImage;
        public static Bitmap imagePreview;
        bool PreviosulyDrawn = false;
        bool LockedLast = false;
        bool FreeDraw2 = false;
        bool MDown = false;
        int LastX;
        int LastY;
        bool CursorOffset = false;
        int xOffset = 0;
        int yOffset = 0;
        int width;
        int height;
        int interval;
        int clickdelay;
        int blackThreshold;
        int transparencyThreshold;
        int[,] pixelArray;
        ArrayList stack;
        PathIntegers horizontal = new PathIntegers { name = "horizontal - 12345678", path = 12345678 };
        PathIntegers vertical = new PathIntegers { name = "vertical - 14627358", path = 14627358 };
        PathIntegers diagonal = new PathIntegers { name = "diagonal - 26573481", path = 26573481 };
        int pathInt;
        bool finished;

        public void SetCursorPos(int posX, int posY)
        {
            SimWinInput.SimMouse.Act(SimWinInput.SimMouse.Action.MoveOnly, posX + xOffset, posY + yOffset);
        }
        public void LeftClick(SimWinInput.SimMouse.Action Action,int posX,int posY)
        {
            SimWinInput.SimMouse.Act(Action, posX + xOffset, posY + yOffset);
        }
        public const SimWinInput.SimMouse.Action MOUSEEVENTF_LEFTDOWN = SimWinInput.SimMouse.Action.LeftButtonDown;
        public const SimWinInput.SimMouse.Action MOUSEEVENTF_LEFTUP = SimWinInput.SimMouse.Action.LeftButtonUp;

        public MainForm()
        {
            InitializeComponent();
            blackThreshold = (int)blackThreshNumeric.Value;
            transparencyThreshold = (int)transThreshNumeric.Value;
            interval = Convert.ToInt32(intervalInput.Text);
            interval = int.Parse(intervalInput.Text);
            clickdelay = Convert.ToInt32(clickdelayInput.Text);
            clickdelay = int.Parse(clickdelayInput.Text);
            pathList.Items.Add(horizontal);
            pathList.Items.Add(vertical);
            pathList.Items.Add(diagonal);
            pathList.SetSelected(2, true);
            pathInt = ((PathIntegers)pathList.SelectedItem).path;
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            upload();                        
        }

        private void upload()
        {
            // Show the Open File dialog. If the user clicks OK, load the
            // picture that the user chose.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imageFile = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = imageFile;
                width = imageFile.Width;
                height = imageFile.Height;
                widthInput.Text = width.ToString();
                heightInput.Text = height.ToString();
                image = imageFile;
                imagePreview = image;
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void clear()
        {
            // Clear the picture.
            pictureBox1.Image = null;
            imageFile = null;
            image = null;
            imagePreview = null;
        }

        private void widthInput_TextChanged(object sender, EventArgs e)
        {
            // Change in width of image
            try
            {
                width = Convert.ToInt32(widthInput.Text);
                width = int.Parse(widthInput.Text);
                image = resizeImage(imageFile, new Size(width, height));
            }
            catch (Exception)
            {
                width = image.Width;
            }
            pictureBox1.Image = image;
            imagePreview = image;
        }

        private void heightInput_TextChanged(object sender, EventArgs e)
        {
            // Change in height of image
            try
            {
                height = Convert.ToInt32(heightInput.Text);
                height = int.Parse(heightInput.Text);
                image = resizeImage(imageFile, new Size(width, height));
            }
            catch (Exception)
            {
                height = image.Height;
            }
            pictureBox1.Image = image;
            imagePreview = image;
        }

        private void blackThreshNumeric_ValueChanged(object sender, EventArgs e)
        {
            // Threshold at which a pixel is deemed black
            blackThreshold = (int)blackThreshNumeric.Value;
        }

        private void transThreshNumeric_ValueChanged(object sender, EventArgs e)
        {
            // Threshold at which a pixel is deemed transparent
            transparencyThreshold = (int)transThreshNumeric.Value;
        }

        private void intervalInput_TextChanged(object sender, EventArgs e)
        {
            // Delay interval between the drawing of each pixel
            try
            {
                interval = Convert.ToInt32(intervalInput.Text);
                interval = int.Parse(intervalInput.Text);
            }
            catch (Exception)
            {
                interval = 0;
            }
        }

        private void clickdelayInput_TextChanged(object sender, EventArgs e)
        {
            // Delay interval between the drawing of each pixel
            try
            {
                clickdelay = Convert.ToInt32(clickdelayInput.Text);
                clickdelay = int.Parse(clickdelayInput.Text);
            }
            catch (Exception)
            {
                clickdelay = 0;
            }
        }

        private void pathList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pathInt = ((PathIntegers)pathList.SelectedItem).path;
            }
            catch (Exception)
            {
            }
        }

        private void customButton_Click(object sender, EventArgs e)
        {
            PathSeqForm m = new PathSeqForm();
            m.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm m = new SettingsForm();
            m.Show();
            m.FormClosed += new FormClosedEventHandler(Settings_Closed);
        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            pathList.ClearSelected();
            pathInt = PathSeqForm.pathInt;
        }

        void Settings_Closed(object sender, FormClosedEventArgs e)
        {
            CursorOffset = SettingsForm.CursorOffset;
            FreeDraw2 = SettingsForm.FreeDraw;
            if (CursorOffset)
            {
                xOffset = SettingsForm.xOffset;
                yOffset = SettingsForm.yOffset;
            }
            else
            {
                xOffset = 0;
                yOffset = 0;
            }
        }

        private void processButton_Click(object sender, EventArgs e)
        {
            // Converts image to grayscale and applies thresholds
            try
            {
                Bitmap greyImage = MakeGrayscale3(image);
                processedImage = scan(greyImage, blackThreshold, transparencyThreshold);
                pictureBox1.Image = processedImage;
                imagePreview = processedImage;
            }
            catch (Exception)
            {
                MessageBox.Show(new Form() { TopMost = true }, "No image was found", "Process Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            // Starts drawing
            try
            {
                int imageTest = imagePreview.Width;
                PreviewForm m = new PreviewForm();
                m.Show();
                WindowState = FormWindowState.Minimized;
                while (true)
                {
                    Application.DoEvents();
                    if (ModifierKeys == Keys.Alt)
                    {
                        m.Close();
                        break;
                    }
                    if (ModifierKeys == Keys.Shift)
                    {
                        PreviosulyDrawn = true;
                        if (!LockedLast)
                        {
                            LastX = Cursor.Position.X;
                            LastY = Cursor.Position.Y;
                        }
                        m.Close();
                        start();
                        break;
                    }
                    if (ModifierKeys == Keys.Control)
                    {
                        if (PreviosulyDrawn)
                        {
                            LockedLast = !LockedLast;
                            NOP(1000000);
                        }
                    }
                    if (LockedLast)
                    {
                        m.Location = new Point(LastX - m.Width / 2, LastY - m.Height / 2);
                    }
                    else
                    {
                        int xpos = Cursor.Position.X - m.Width / 2;
                        int ypos = Cursor.Position.Y - m.Height / 2;
                        m.Location = new Point(xpos, ypos);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(new Form() { TopMost = true }, "No image was found", "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }        
        }

        private void start()
        {
            try
            {
                finished = draw();
                if (finished == true)
                    MessageBox.Show(new Form() { TopMost = true }, "Drawing Complete", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                else
                    MessageBox.Show(new Form() { TopMost = true }, "Drawing halted", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception)
            {
                MessageBox.Show(new Form() { TopMost = true }, "No image was found", "Drawing Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //Console.WriteLine("path: {0}", pathInt);
            }
        }

        private Bitmap resizeImage(Bitmap imgToResize, Size size)
        {
            // Resizes image
            return new Bitmap(imgToResize, size);
        }

        private Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
            new float[][]
            {
                new float[] {.3f, .3f, .3f, 0, 0},
                new float[] {.59f, .59f, .59f, 0, 0},
                new float[] {.11f, .11f, .11f, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1}
            });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        private Bitmap scan(Bitmap image, int blackThreshold, int transparencyThreshold)
        {
            // Scans each pixel in image and gives it a black or white value
            pixelArray = new int[image.Width, image.Height];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);
                    if ((pixel.R < blackThreshold | pixel.G < blackThreshold | pixel.B < blackThreshold) && pixel.A > transparencyThreshold)
                    {
                        image.SetPixel(x, y, Color.Black);
                        pixelArray[x, y] = 1;
                    }
                    else
                    {
                        image.SetPixel(x, y, Color.White);
                        pixelArray[x, y] = 0;
                    }
                }
            }
            return image;
        }

        private bool draw()
        {
            Point mouseCenter = Cursor.Position;
            int xorigin = mouseCenter.X - (image.Width / 2);
            int yorigin = mouseCenter.Y - (image.Height / 2);
            if (LockedLast)
            {
                xorigin = LastX - (image.Width / 2);
                yorigin = LastY - (image.Height / 2);
            }
            int xpos = xorigin;
            int ypos = yorigin;
            bool cont = true;

            // Draw all the areas
            stack = new ArrayList();
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Application.DoEvents();
                    if (ModifierKeys == Keys.Alt)
                        return false;
                    if (pixelArray[x, y] == 1)
                    {
                        xpos = xorigin + x;
                        ypos = yorigin + y;
                        SetCursorPos(xpos, ypos);
                        NOP(clickdelay * 10000);
                        SetCursorPos(xpos, ypos+1);
                        LeftClick(MOUSEEVENTF_LEFTDOWN, xpos, ypos);
                        cont = drawArea(stack, x, y, xorigin, yorigin);
                        LeftClick(MOUSEEVENTF_LEFTUP, xpos, ypos);
                        if (ModifierKeys == Keys.Alt)
                            return false;
                    }
                    if (!cont)
                    {
                        resetPixels();
                        return false;
                    }
                }
            }
            resetPixels();            
            return true;
        }

        private bool drawArea(ArrayList stack, int x, int y, int xorigin, int yorigin)
        {
            bool cont;
            var path = pathInt.ToString().Select(t => int.Parse(t.ToString())).ToArray();

            while (true)
            {
                Application.DoEvents();
                if (ModifierKeys == Keys.Alt)
                    return false;
                NOP(interval);
                SetCursorPos(xorigin + x, yorigin + y);
                pixelArray[x, y] = 2;
                /*
		        +---+---+---+
		        | 1 | 2 | 3 |
		        +---+---+---+
		        | 4 |   | 5 |
		        +---+---+---+
		        | 6 | 7 | 8 |
		        +---+---+---+
                */
                cont = false;
                foreach (int i in Enumerable.Range(0, 7))
                {
                    switch (path[i])
                    {
                        case 1:
                            if ((x > 0) && (y > 0))
                            {
                                if (pixelArray[x - 1, y - 1] == 1)
                                {
                                    push(stack, x, y);
                                    x -= 1;
                                    y -= 1;
                                    cont = true;
                                }
                            }
                            break;
                        case 2:
                            if (y > 0)
                            {
                                if (pixelArray[x, y - 1] == 1)
                                {
                                    push(stack, x, y);
                                    y -= 1;
                                    cont = true;
                                }
                            }
                            break;
                        case 3:
                            if ((x > 0) && (y < 0))
                            {
                                if (pixelArray[x + 1, y - 1] == 1)
                                {
                                    push(stack, x, y);
                                    x += 1;
                                    y += 1;
                                    cont = true;
                                }
                            }
                            break;
                        case 4:
                            if (x > 0)
                            {
                                if (pixelArray[x - 1, y] == 1)
                                {
                                    push(stack, x, y);
                                    x -= 1;
                                    cont = true;
                                }
                            }
                            break;
                        case 5:
                            if (x < (image.Width - 1))
                            {
                                if (pixelArray[x + 1, y] == 1)
                                {
                                    push(stack, x, y);
                                    x += 1;
                                    cont = true;
                                }
                            }
                            break;
                        case 6:
                            if ((x < 0) && (y > 0))
                            {
                                if (pixelArray[x - 1, y + 1] == 1)
                                {
                                    push(stack, x, y);
                                    x -= 1;
                                    y -= 1;
                                    cont = true;
                                }
                            }
                            break;
                        case 7:
                            if (y < (image.Height - 1))
                            {
                                if (pixelArray[x, y + 1] == 1)
                                {
                                    push(stack, x, y);
                                    y += 1;
                                    cont = true;
                                }
                            }
                            break;
                        case 8:
                            if ((x < (image.Width - 1)) && (y < (image.Height - 1)))
                            {
                                if (pixelArray[x + 1, y + 1] == 1)
                                {
                                    push(stack, x, y);
                                    x += 1;
                                    y += 1;
                                    cont = true;
                                }
                            }
                            break;
                    }
                }
                if (cont)
                    continue;
                if (!pop(stack, ref x, ref y))
                    break;
            }
            return true;
        }

        private void push(ArrayList stack, int x, int y)
        {
            stack.Add(new Position { x = x, y = y });
        }

        private bool pop(ArrayList stack, ref int x, ref int y)
        {
            if (stack.Count < 1)
                return false;

            Position pos = (Position)stack[stack.Count - 1];

            x = pos.x;
            y = pos.y;

            stack.Remove(pos);
            return true;
        }

        private void resetPixels()
        {
            // Reset the pixels statuses
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    if (pixelArray[x, y] == 2)
                        pixelArray[x, y] = 1;
                }
            }
        }

        private static void NOP(long durationTicks)
        {
            // Static method to initialize and start stopwatch
            var sw = Stopwatch.StartNew();

            while (sw.ElapsedTicks < durationTicks)
            {

            }
        }

        public static void Print2DArray(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var hb = new HatchBrush(HatchStyle.Percent50, this.TransparencyKey);

            e.Graphics.FillRectangle(hb, this.DisplayRectangle);
        }

        private void InfoButton_Click(object sender, EventArgs e)
        {
            InfoForm m = new InfoForm();
            m.Show();
        }
    }

    class Position
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    class PathIntegers
    {
        public string name { get; set; }
        public int path { get; set; }
    }
}
