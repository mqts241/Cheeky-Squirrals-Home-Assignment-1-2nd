using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NomeDelProgetto.Views;

public class Modifying
{

    public int Height { get; private set; }
    public int Width { get; private set; }
    public int[,] Pixels { get; private set; }
    public Grid ImageGrid { get; private set; }

    public void DisplayImage()
    {
        if (Pixels == null) return; // Check if there is an image to display
            ImageGrid.Children.Clear(); // Clear previous image
            ImageGrid.Columns = Width; // Set columns
            ImageGrid.Rows = Height; // Set rows

        for (int i = 0; i < Height; i++) // Iterate over each pixel (i is the rows and j is the columns)
        {
            for (int j = 0; j < Width; j++) 
            {
                var button = new Button // Create a button for each pixel where 1 is black and 0 is white
                {
                    Content = Pixels[i, j] == 1 ? "⬛" : "⬜",
                    Tag = (i, j) // Store position
                };
                button.Click += PixelClick;
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
            button.Content = Pixels[i, j] == 1 ? "⬛" : "⬜";
        }
    }
}