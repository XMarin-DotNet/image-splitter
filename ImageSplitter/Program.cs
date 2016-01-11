using System.Drawing;

namespace ImageSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = Image.FromFile(args[0]);
            var sizeX = int.Parse(args[1]);
            var sizeY = int.Parse(args[2]);
            var format = args[3];

            using (var splitter = new ImageSplitter(source, format, sizeX, sizeY, 32, 14, Color.OrangeRed, Brushes.Black))
            {
                splitter.Split();
            }
        }
    }
}
