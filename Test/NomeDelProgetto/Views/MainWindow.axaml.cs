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

    private class Modifying
    {

        public int Height { get; private set; }
        public int Width { get; private set; }
        public int[,] Pixels { get; private set; }
        public Grid ImageGrid { get; private set; }

        public Modifying()
        {
            Pixels = new int[0, 0];
            ImageGrid = new Grid();
        }

        public void DisplayImage()
        {
            if (Pixels == null) return; // Check if there is an image to display
            ImageGrid.Children.Clear(); // Clear previous image
            ImageGrid.RowDefinitions.Clear(); // Clear previous row definitions
            ImageGrid.ColumnDefinitions.Clear(); // Clear previous column definitions
            
            for (int i = 0; i < Height; i++) // Set rows i
            {
                ImageGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int j = 0; j < Width; j++) // Set columns j
            {
                ImageGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < Height; i++) // Iterate over each pixel (i is the rows and j is the columns)
            {
                for (int j = 0; j < Width; j++) 
                {
                    var button = new Button // Create a button for each pixel where 1 is black and 0 is white
                    {
                        Content = Pixels[i, j] == 1 ? "⬛" : "⬜", //could use Brushes.Black and Brushes.White
                        Tag = (i, j) // Store position
                    };

                    int row = i; // Store row
                    int column = j; // Store column
                    button.Click += PixelClick;
                    Grid.SetRow(button, row); // Set row for button
                    Grid.SetColumn(button, column); // Set column for button
                    ImageGrid.Children.Add(button);
                }
            }
        }

        private void PixelClick(object? sender, RoutedEventArgs e) 
        {
            if (sender is Button button && button.Tag is (int i, int j)) // Check if the sender is a button and if it has a tag
            {
                // Toggle value
                Pixels[i, j] = Pixels[i, j] == 0 ? 1 : 0;
                button.Content = Pixels[i, j] == 1 ? "⬛" : "⬜"; //could use Brushes.Black and Brushes.White
            }
        }
    }
}