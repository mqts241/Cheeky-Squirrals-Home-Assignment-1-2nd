namespace Smile;
public class B2Image
{
    //Creating Attributes of an B2Image
    public int Height { get; private set; }
    public int Width { get; private set; }
    public int[,] Pixels { get; private set; }

    public B2Image()
    {
        //Setting a base case
        Height = 0;
        Width = 0;
        Pixels = new int[0, 0];
    }

    public void LoadFile()
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
            Height = int.Parse(dimensions[0]);
            Width = int.Parse(dimensions[1]);

            //Read the Pixel Data
            string Data = lines[1];

            //Test if amount of pixels fit the dimensions
            if (Data.Length != Height * Width)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The amount of pixels don not fit in the set dimensions!");
                Console.ResetColor();
                return;
            }

            int index = 0;
            for (int i = 0; i < Height; i++)
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
}