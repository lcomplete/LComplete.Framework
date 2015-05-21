using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LComplete.Framework.Wcf;
using WcfContract.Messages;
using WcfContract.Results;
using WcfContract.Services;

namespace NewProject.Site.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FileTest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FileTest(string file)
        {
            WcfClientManager<IFileService> fileClient=new WcfClientManager<IFileService>();
            HttpPostedFileBase postedFile = Request.Files[0];
            using (fileClient)
            {
                var result= fileClient.Channel.Upload(new UploadMessage()
                {
                    ContentType = postedFile.ContentType,
                    FileName = postedFile.FileName,
                    FileStream = postedFile.InputStream,
                    IsImage = postedFile.ContentType=="image/jpeg",
                    ThumbSizes = new ThumbSize[]{new ThumbSize(){Width = 600},new ThumbSize(){Width = 200,Height = 200},  },
                    UploadAdminId = 1,
                    UploadUserId = 1
                });

                return Json(result);
            }
        }
    }
}
