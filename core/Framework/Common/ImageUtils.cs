using System.Collections.Generic;
using System.IO;

namespace LComplete.Framework.Common
{
    public static class ImageUtils
    {
        public static bool IsImageStream(Stream stream)
        {
            if (stream != null && stream.Length > 0)
            {
                int buffer = stream.ReadByte();
                string fileClass = buffer.ToString();
                buffer = stream.ReadByte();
                fileClass += buffer.ToString();

                stream.Position = 0;

                //jpg || gif ||bmp ||png
                if (fileClass == "255216" || fileClass == "7173" || fileClass == "6677" || fileClass == "13780")
                    return true;
            }

            return false;
        }

        public static bool IsImageExtentions(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            string extensions = Path.GetExtension(fileName).ToLower();
            return (new List<string> { ".jpg", ".jpeg", ".bmp", ".png", ".gif" }).Contains(extensions);
        }
    }
}