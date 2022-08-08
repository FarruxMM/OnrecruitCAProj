using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Online_recruitment_system.Utils
{
    public static class Extensions
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image/");
        }
        public static bool IsValidImage(this IFormFile file, int kb)
        {
            return file.Length / 1024 < kb;
        }

        public static async Task<string> UpLoad(this IFormFile file, string root, string folder)
        {
            string fileName = Guid.NewGuid().ToString() + "" + file.FileName;
            string filePath = Path.Combine(root, folder, fileName);
            FileStream stream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream);
            return (fileName);
        }
    }
}
