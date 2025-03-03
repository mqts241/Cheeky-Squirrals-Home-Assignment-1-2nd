using Avalonia.Controls;
using System;
using System.IO;

namespace NomeDelProgetto.Views;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Modifying.DisplayImage();
        B2Image.LoadImage();
        B2Image.SaveImage();
    }
}