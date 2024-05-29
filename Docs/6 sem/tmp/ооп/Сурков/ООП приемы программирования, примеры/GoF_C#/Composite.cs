using System;

namespace GoF.Composite
{
    public class Image
    {
        public string Data { get; set; }
    }

    public interface IFilter
    {
        void Apply(Image image);
    }

    public class SepiaFilter : IFilter
    {
        public void Apply(Image image)
        {
            image.Data = string.Format("<sepia>{0}</sepia>", image.Data);
        }
    }

    public class GrayscaleFilter : IFilter
    {
        public void Apply(Image image)
        {
            image.Data = string.Format("<grayscale>{0}</grayscale>", image.Data);
        }
    }

    public class SmoothFilter : IFilter
    {
        public void Apply(Image image)
        {
            image.Data = string.Format("<smooth>{0}</smooth>", image.Data);
        }
    }

    public class GroupFilter : IFilter // Composite filter
    {
        private readonly IFilter[] _filters;

        public GroupFilter(params IFilter[] filters)
        {
            _filters = filters;
        }

        public void Apply(Image image)
        {
            foreach (var filter in _filters)
                filter.Apply(image);
        }
    }

    public static class Sample
    {
        public static void UserLogic(IFilter filter)
        {
            var image = new Image() { Data = "my image" };
            filter.Apply(image);
            Console.WriteLine("Filtered image: {0}", image.Data);
        }

        public static void Start()
        {
            // [1]
            Console.WriteLine("-- Sepia filter ---------");
            UserLogic(new SepiaFilter());

            // [2]
            Console.WriteLine("\n-- Grayscale filter -----");
            UserLogic(new GrayscaleFilter());

            // [3]
            Console.WriteLine("\n-- Smooth filter --------");
            UserLogic(new SmoothFilter());

            // [4]
            Console.WriteLine("\n-- Group filter ---------");
            UserLogic(new GroupFilter(new SepiaFilter(), new GrayscaleFilter(), new SmoothFilter()));
        }
    }
}
