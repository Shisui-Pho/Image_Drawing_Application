using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public static class CRenderImage
{
    public static void SaveImageToFile(string filename, FrameworkElement objectToBerenderd)
    {
        using(FileStream fs = new FileStream(filename, FileMode.Create))
        {
            RenderTargetBitmap rnd = new RenderTargetBitmap(
            (int)objectToBerenderd.ActualWidth,
            (int)objectToBerenderd.ActualHeight,
            96d,
            96d,
            PixelFormats.Default);
            rnd.Render(objectToBerenderd);

            BitmapEncoder encoder;
            if (Path.GetExtension(filename) == ".png")
                encoder = new PngBitmapEncoder();
            else
                encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rnd));
            encoder.Save(fs);
        }//using
    }//SaveImageToFile method
}//CRenderImage