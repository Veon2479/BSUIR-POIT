using System;

namespace GoF.Bridge
{
    public class Painter
    {
        private readonly IPaintDevice _device;

        public Painter(IPaintDevice device)
        {
            _device = device;
        }

        public void DrawCircle(int x, int y, int radius)
        {
            _device.DrawCircleImp(x, y, radius);
        }

        // All quality independed methods are implemented in this class, while quality depended ones are delegated

        public void DrawText(string text)
        {
            Console.WriteLine("Text ({0})", text);
        }
    }

    public interface IPaintDevice 
    {
        void DrawCircleImp(int x, int y, int radius);
        // ...other quality/platform depended methods
    }

    public class LowQualityPaintDevice : IPaintDevice
    {
        public void DrawCircleImp(int x, int y, int radius)
        {
            Console.WriteLine("Circle (x: {0}, y: {1}, radius: {2}) [low quality]", x, y, radius);
        }
    }

    public class HighQualityPaintDevice : IPaintDevice
    {
        public void DrawCircleImp(int x, int y, int radius)
        {
            Console.WriteLine("Circle (x: {0}, y: {1}, radius: {2}) [high quality]", x, y, radius);
        }
    }

    public static class Sample
    {
        public static void Start()
        {
            // [1]
            {
                Console.WriteLine("-- Using Low Quality Painter ----");
                var painter = new Painter(new LowQualityPaintDevice());
                painter.DrawText("just text");
                painter.DrawCircle(10, 10, 5);
            }

            // [2]
            {
                Console.WriteLine("\n-- Using High Quality Painter ---");
                var painter = new Painter(new HighQualityPaintDevice());
                painter.DrawText("just text");
                painter.DrawCircle(10, 10, 5);
            }
        }
    }
}
