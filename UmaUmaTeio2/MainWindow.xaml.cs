using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tesseract;
using UmaUmaTeio2.Drawing;

namespace UmaUmaTeio2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var value = Text.JaroWinklerDistance.Similarity("CRATE", "CRATE");

            var list = new List<Drawing.Rect>();

            list.Add(new Drawing.Rect( 45, 540, 40, 16));
            list.Add(new Drawing.Rect(118, 540, 40, 16));
            list.Add(new Drawing.Rect(188, 540, 40, 16));
            list.Add(new Drawing.Rect(259, 540, 40, 16));
            list.Add(new Drawing.Rect(330, 540, 40, 16));

            var bitmap = new Bitmap(@"pix\sample.png");

            using var resizeSample = BitmapBuilder.Resize(bitmap, 2);

            using var tesseract = Managers.OcrManager.Instance.DigitEngine;

            for (int i = 0; i < list.Count; i++)
            {
                using var cutSample = BitmapBuilder.Cut(bitmap, list[i]);
                using var scaleSample = BitmapBuilder.Resize(cutSample, 2);
                using var marginSample = BitmapBuilder.Margin(scaleSample, 20, System.Drawing.Color.White);
                using var negaSample = BitmapBuilder.Negative(marginSample);

                using var page = tesseract.Process(marginSample, PageSegMode.SingleBlock);

                Debug.WriteLine($"no.{i + 1} = " + page.GetText().Trim('\r').Trim('\n'));

                negaSample.Save($"pix\\cut_{i+1}.png", System.Drawing.Imaging.ImageFormat.Png);
            }

            resizeSample.Save("pix\\resize.png", System.Drawing.Imaging.ImageFormat.Png);

        }

        

    }
}
