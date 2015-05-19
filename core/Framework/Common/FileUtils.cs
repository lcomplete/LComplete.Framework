using System.IO;

namespace LComplete.Framework.Common
{
    public static class FileUtils
    {
        public static void SaveMemoryStream(Stream stream, string savePath,bool autoCreateDir=false)
        {
            if(autoCreateDir)
                AutoCreateDir(savePath);

            using (FileStream fs = File.Create(savePath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fs);
            }
        }

        public static void AutoCreateDir(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);

            if (dir != null && Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);
        }

        public static void SaveNetworkStream(Stream networkStream, string savePath,out long fileLength,bool autoCreateDir=false)
        {
            if(autoCreateDir)
                AutoCreateDir(savePath);

            using (FileStream fs=File.Create(savePath))
            {
                const int bufferLen = 4096;
                byte[] buffer = new byte[bufferLen];
                int count;
                while ((count = networkStream.Read(buffer, 0, bufferLen)) > 0)
                {
                    fs.Write(buffer, 0, count);
                }
                fileLength = fs.Length;
            }
        }
    }
}