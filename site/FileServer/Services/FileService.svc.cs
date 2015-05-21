using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using FileServer.Code;
using LComplete.Framework.Common;
using LComplete.Framework.Tools;
using WcfContract.Messages;
using WcfContract.Results;
using WcfContract.Services;

namespace FileServer.Services
{
    public class FileService : IFileService
    {
        public UploadResult Upload(UploadMessage uploadMessage)
        {
            string rootPath = uploadMessage.IsImage ? "img" : "static";
            string extension = Path.GetExtension(uploadMessage.FileName).TrimStart('.');
            string uniqueKey = RandomUtils.GetRandomFileName();
            string fileName = uniqueKey + (string.IsNullOrWhiteSpace(extension) ? "" : "." + extension);
            string relativePath = rootPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + fileName;
            string savePath = PathUtils.GetFilePath(relativePath);

            UploadResult result = new UploadResult();
            try
            {
                long fileLength;
                FileUtils.SaveNetworkStream(uploadMessage.FileStream, savePath, out fileLength,autoCreateDir: true);
                int width = 0, height = 0;
                if (uploadMessage.IsImage)
                {
                    FileStream fileStream = File.OpenRead(savePath);
                    if (ImageUtils.IsImageStream(fileStream) == false)
                        throw new Exception("上传的文件不是正确的图片格式。");

                    using (ImageTool imageTool = new ImageTool(fileStream))
                    {
                        Size size = imageTool.GetSize();
                        width = size.Width;
                        height = size.Height;
                        foreach (var thumbSize in uploadMessage.ThumbSizes)
                        {
                            string thumbPath = PathUtils.GetThumbPath(relativePath, thumbSize.Width, thumbSize.Height);
                            imageTool.Save(thumbPath, thumbSize.Width, thumbSize.Height);
                        }
                    }
                }

                UploadFile upload = new UploadFile()
                {
                    ContentType = uploadMessage.ContentType,
                    CreateDate = DateTime.Now,
                    FileExtension = extension,
                    FileSize = fileLength,
                    OriginalName = uploadMessage.FileName,
                    RelativePath = relativePath,
                    UploadAdminId = uploadMessage.UploadAdminId,
                    UploadUserId = uploadMessage.UploadUserId,
                    UniqueKey = uniqueKey,
                    Width = width,
                    Height = height
                };
                var fileRepo = new UploadFileRepository();
                fileRepo.Add(upload);

                result = new UploadResult()
                {
                    FileId = upload.Id,
                    IsSuccess = true,
                    HttpPath = rootPath + "/" + uniqueKey,
                    UniqueKey = upload.UniqueKey
                };
            }
            catch (Exception ex)
            {
                if (File.Exists(savePath))
                    File.Delete(savePath);

                result.IsSuccess = false;
                result.Message = ex.ToString();
            }

            return result;
        }
    }
}
