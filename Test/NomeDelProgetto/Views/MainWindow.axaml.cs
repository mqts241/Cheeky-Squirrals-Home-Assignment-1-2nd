using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
//using MessageBox.Avalonia;
//using MessageBox.Avalonia.Enums;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Avalonia.Interactivity;

namespace NomeDelProgetto.Views;


public partial class MainWindow : Window
{
    //Creating Attributes of an B2Image
    public int d_Height;
    public int d_Width;
    public int[,] Pixels;
    //public Grid ImageGrid { get; private set; }

    public MainWindow()
    {
        InitializeComponent();
        Pixels = new int[0, 0];
        //ImageGrid = new Grid();
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

            // Initialize Pixels with the correct dimensions
            Pixels = new int[d_Height, d_Width];

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
    public string ImageToFile(Streamreader image)
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

    public async Task SafeImage_Click(object? sender, RoutedEventArgs e)
    {
        if (Pixels == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No image to save!");
            Console.ResetColor();
            return;
        }

        var file = await StorageProvider.SafeFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Image",
            Filter = new[] {new FilePickerFileType("Text Files", new[] {"txt"})}
            DefaultExtension = "txt"
        });

        if (file != null)
        {
            try
            {
                await using var stream = await file.OpenWriteAsync();
                using var writer = new StreamWriter(stream);
                ImageToFile(writer);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error saving image: {e.Message}");
                Console.ResetColor();
            }
        }
    }


        public void DisplayImage()
        {
            if (Pixels == null) return; // Check if there is an image to display
            ImageGrid.Children.Clear(); // Clear previous image
            ImageGrid.Columns = d_Width; // Set columns
            ImageGrid.Rows = d_Height; // Set rows

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