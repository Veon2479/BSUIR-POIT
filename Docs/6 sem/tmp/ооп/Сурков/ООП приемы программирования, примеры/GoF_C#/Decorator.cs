using System;

namespace GoF.Decorator
{
    public interface IPhoto
    {
        void Draw();
    }

    public class Photo : IPhoto
    {
        public void Draw()
        {
            Console.WriteLine("This is photo");
        }
    }

    public class TaggedPhoto : IPhoto
    {
        private readonly IPhoto _source;
        private readonly string _tag;

        public TaggedPhoto(IPhoto source, string tag)
        {
            _source = source;
            _tag = tag;
        }

        public void Draw()
        {
            _source.Draw();
            Console.WriteLine("Tagged by {0}", _tag);
        }
    }

    public static class Sample
    {
        public static void Start()
        {
            var photo = new Photo();
            Console.WriteLine("-- Photo ------------");
            photo.Draw();

            var taggedPhoto = new TaggedPhoto(photo, "the first tag");
            Console.WriteLine("\n-- Tagged photo -----");
            taggedPhoto.Draw();
            
            var doublyTagged = new TaggedPhoto(taggedPhoto, "the second tag");
            Console.WriteLine("\n-- Doubly tagged ----");
            doublyTagged.Draw();
        }
    }
}
