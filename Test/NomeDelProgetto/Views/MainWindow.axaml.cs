using Avalonia.Controls;
using System;
using System.IO;
using System.Text;

namespace NomeDelProgetto.Views;


public partial class MainWindow : Window
{
    //Creating Attributes of an B2Image
    public int d_Height;
    public int d_Width;
    public int[,] Pixels;
    public MainWindow()
    {
        InitializeComponent();
    }


    public void LoadImage()
    {
        //safe Filepath, test for existence and right file format
        string filePath = "smile.b2img.txt";
        if (File.Exists(filePath))
        {
            //Load lines into array
            string[] lines = File.ReadAllLines(filePath);
            if(lines.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid file format!");
                Console.ResetColor();
                return;
            }

            //When test approved split and store Data for B2Image

            //Extract the dimensions
            string[] dimensions = lines[0].Split(' ');
            d_Height = int.Parse(dimensions[0]);
            Width = int.Parse(dimensions[1]);

            //Read the Pixel Data
            string Data = lines[1];

            //Test if amount of pixels fit the dimensions
            if (Data.Length != d_Height * Width)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The amount of pixels don not fit in the set dimensions!");
                Console.ResetColor();
                return;
            }

            int index = 0;
            for (int i = 0; i < d_Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Pixels[i, j] = Data[index] - 0; //converts the chars into int
                    index++;
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Image loaded successfully!"); 
            Console.ResetColor();
        }
    }

    // Convert image to string format for saving
    public string ImageToFile(MainWindow image)
    {
        string dimensions = $"{image.d_Height} {image.Width}";
        StringBuilder Pixels = new StringBuilder();
        for (int i = 0; i < image.d_Height; i++)
        {
            for (int j = 0; j < image.Width; j++)
            {
                Pixels.Append(image.Pixels[i, j]);
            }
        }
        return $"{dimensions}\n{Pixels}";
    }

    public void SafeImage(MainWindow image)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Filter = "B2 Image Files (*.b2img.txt)|*.b2img.txt",
            Title = "Save Image File"
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(saveFileDialog.FileName, image.ToFileFormat());
            MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}