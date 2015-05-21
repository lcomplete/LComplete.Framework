using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using LComplete.Framework.Common;

namespace LComplete.Framework.Tools
{
    public class ImageTool : IDisposable
    {
        public enum ZoomMode
        {
            Fixed,
            GeometricProportion
        }

        private Stream _stream;

        private ZoomMode _zoomMode;

        public ImageTool(Stream stream, ZoomMode zoomMode = ZoomMode.GeometricProportion)
        {
            _stream = stream;
            _zoomMode = zoomMode;
        }

        public void Save(string savePath)
        {
            using (FileStream fileStream = File.OpenWrite(savePath))
            {
                int bytesLength = (int)_stream.Length;
                byte[] rawData = new byte[bytesLength];
                _stream.Read(rawData, 0, bytesLength);
                fileStream.Write(rawData, 0, bytesLength);
                _stream.Position = 0;
            }
        }

        public Size GetSize()
        {
            using (Image img = Image.FromStream(_stream))
            {
                return img.Size;
            }
        }

        /// <summary>
        /// 保存缩略图
        /// </summary>
        /// <param name="savePath">保存路径</param>
        /// <param name="thumbWidth">缩略图限制宽度</param>
        /// <param name="thumbHeight">缩略图限制高度（为null时 表示不限制高度）</param>
        public void Save(string savePath, int thumbWidth, int? thumbHeight)
        {
            using (Image originalImg = Image.FromStream(_stream))
            {
                Size size = originalImg.Size;
                switch (_zoomMode)
                {
                    case ZoomMode.GeometricProportion:
                        if (size.Width > thumbWidth || size.Height > thumbHeight)
                        {
                            float scale;
                            if (thumbHeight.HasValue)
                            {
                                scale = Math.Min((float)thumbWidth / size.Width, (float)thumbHeight / size.Height);
                            }
                            else
                            {
                                scale = (float)thumbWidth / size.Width;
                            }
                            size = new Size((int)(scale * size.Width), (int)(scale * size.Height));
                        }
                        break;
                    case ZoomMode.Fixed:
                        if (thumbHeight.HasValue == false)
                            throw new ArgumentNullException("thumbHeight", "固定缩放模式下必须指定高度");

                        size = new Size(thumbWidth, thumbHeight.Value);
                        break;
                }

                using (Bitmap bmp = new Bitmap(size.Width, size.Height))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.Clear(Color.Transparent);
                    g.DrawImage(originalImg, 0, 0, size.Width, size.Height);

                    FileUtils.AutoCreateDir(savePath);
                    bmp.Save(savePath, ImageFormat.Jpeg);
                }
            }
        }

        public virtual void Close()
        {
            Dispose(true);
        }

        public virtual void Dispose()
        {
            Close();
        }

        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                if (_stream != null)
                    _stream.Close();
            }
            _stream = null;
        }
    }
}
