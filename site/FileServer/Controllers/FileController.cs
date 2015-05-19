using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileServer.Code;
using LComplete.Framework.Cache;
using LComplete.Framework.Common;

namespace FileServer.Controllers
{
    public class FileController : Controller
    {
        private UploadFileRepository _repository;

        public FileController()
        {
            _repository = new UploadFileRepository();
        }

        public ActionResult Img(string uniqueKey, string thumb)
        {
            int width = 0;
            int height = 0;
            if (!string.IsNullOrWhiteSpace(thumb))
            {
                string[] args = thumb.Split('_');
                if (args.Length >= 1)
                {
                    width = ValueConverter.Parse<int>(args[0]);
                }
                if (args.Length == 2)
                {
                    height = ValueConverter.Parse<int>(args[1]);
                }
                if (width <= 0 || args.Length > 2)
                    return HttpNotFound();
            }

            UploadFile file = GetUploadFile(uniqueKey);
            if (file == null)
                return HttpNotFound();

            string filePath = width > 0
                ? PathUtils.GetThumbPath(file.RelativePath, width, height > 0 ? (int?)height : null)
                : PathUtils.GetFilePath(file.RelativePath);

            return File(filePath, file.ContentType);
        }

        private UploadFile GetUploadFile(string uniqueKey)
        {
            string cacheKey = "file:" + uniqueKey;
            return CacheManager.Get<UploadFile>(cacheKey, () =>
            {
                return _repository.FirstOrDefault(t => t.UniqueKey == uniqueKey);
            }, TimeSpan.FromHours(4));
        }

        public ActionResult Static(string uniqueKey)
        {
            UploadFile file = GetUploadFile(uniqueKey);
            if (file == null)
                return HttpNotFound();

            return File(PathUtils.GetFilePath(file.RelativePath), file.ContentType);
        }
    }
}
