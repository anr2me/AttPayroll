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

namespace WebView.Controllers
{
    public class UploadifyController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string Upload(HttpPostedFileBase file)
        {
            // Process the file here.
            //

            return "Upload processed. filename=" + file.FileName;
        }
    }
}
