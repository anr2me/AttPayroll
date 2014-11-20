using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Interface.Service;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Data.Repository;
using Service.Service;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;
using Core.DomainModel;
using System.Collections;
using System.Globalization;
using Core.Constants;
using System.IO;

namespace WebView.Controllers
{
    public class UploadifyController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public dynamic Upload(/*HttpPostedFileBase file*/)
        {
            try
            {
                HttpPostedFileBase file = Request.Files["Filedata"];
                string tempPath = System.Configuration.ConfigurationManager.AppSettings["FolderPath"];
                string savepath = Server.MapPath(tempPath);
                // Process the file here.
                //
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (!Directory.Exists(savepath)) Directory.CreateDirectory(savepath);
                    file.SaveAs(Path.Combine(savepath, fileName));

                    // Process start

                    // Process End

                    string ret = tempPath + "/" + fileName;
                    //Response.Write(ret);
                    Response.StatusCode = 200;
                    return ret;
                }
                //return "Upload processed. filename=" + file.FileName;
            }
            catch (Exception ex)
            {
                string ret = "Error: " + ex.Message;
                //Response.Write(ret);
                return ret;
            }
            return Response;
        }
    }
}
