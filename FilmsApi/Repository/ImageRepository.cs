using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace FilmsApi.Repository
{
    public class ImageRepository
    {
        public void SaveImage(byte[] imgBytes,string path, string fileName)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var image = Image.Load(imgBytes);
            image.Mutate(m =>
                m.Resize(
                    new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(512, 512)
                    }
                 )
            );

            image.Save($"{path}/{fileName}.jpg");
        }

        public byte[] GetImage(string path)
        {
            if (File.Exists($"{path}.jpg"))
                return File.ReadAllBytes($"{path}.jpg");
            else
                return null;
        }

        public void DeleteImage(string path)
        {
            if (File.Exists($"{path}.jpg"))
                File.Delete($"{path}.jpg");
        }
    }
}
